#Requires -Version 5.1
<#
.SYNOPSIS
    Instalador alterno de NamiSoft para Windows sin firma digital
    (evita el WinError 4551 de Smart App Control) + configuracion de SQL Server.

.DESCRIPTION
    Por que funciona:
      SAC bloquea la EJECUCION de archivos PE (exe) sin firma ni reputacion.
      El instalador oficial (Inno Setup -> NamiSoft.exe) cae ahi.
      Este script NO ejecuta ningun binario sin firma: usa "dotnet.exe"
      (firmado por Microsoft) como host y corre la aplicacion como DLL.

    Modos de payload (por defecto: -FromRelease):
      (nada)         baja el ZIP framework-dependent de la ultima release de GitHub
      -FromRelease   idem, explicito
      -Zip <ruta>    usa un paquete NamiSoft-*-win-x64-fdd.zip local
      -FromSource    compila desde el repo con el SDK de .NET 10 (si no hay asset)

    Base de datos (con SQL Server ya instalado en el equipo):
      -DbServer "localhost,1433" -DbName NamiSoft   (pide la password de sa)
      -DbPassword "..."                             (sin prompt; queda en el historial)
      -DbConnection "cadena completa"               (prioridad; p.ej. Trusted_Connection)
      -NoDb                                         (no toca nada, se configura despues)

    No requiere permisos de administrador: la app va a %LOCALAPPDATA%\NamiSoft
    y la configuracion a %USERPROFILE%\.config\namisoft.

.PARAMETER Test
    Lanza la app 8 segundos para verificar que arranca y la cierra.

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File .\instalar-namisoft.ps1 -Zip .\NamiSoft-FDD-win-x64.zip -DbServer "localhost,1433"

.EXAMPLE
    powershell -ExecutionPolicy Bypass -File .\instalar-namisoft.ps1 -Zip .\NamiSoft-FDD-win-x64.zip -DbConnection "Server=localhost;Database=NamiSoft;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True"
#>
[CmdletBinding()]
param(
    [string]$Zip = "",
    [switch]$FromSource,
    [switch]$FromRelease,
    [string]$InstallDir = (Join-Path $env:LOCALAPPDATA "NamiSoft"),
    [string]$DbServer,
    [string]$DbName = "NamiSoft",
    [string]$DbPassword,
    [string]$DbConnection,
    [switch]$NoDb,
    [switch]$NoShortcut,
    [switch]$Test
)

$ErrorActionPreference = "Stop"
$RepoUrl   = "https://codeload.github.com/Elnegraso23/NamiSoft/zip/refs/heads/main"
$DotnetUrl = "https://dot.net/v1/dotnet-install.ps1"
$perfil    = if ($env:USERPROFILE) { $env:USERPROFILE } else { $HOME }

function Info($m) { Write-Host "[i]  $m" -ForegroundColor Cyan }
function Ok($m)   { Write-Host "[OK] $m" -ForegroundColor Green }
function Warn($m) { Write-Host "[!]  $m" -ForegroundColor Yellow }
function Die($m)  { Write-Host "[X]  $m" -ForegroundColor Red; exit 1 }

function Read-Password([string]$prompt) {
    $seg = Read-Host $prompt -AsSecureString
    if (-not $seg -or $seg.Length -eq 0) { return $null }
    $bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($seg)
    try { return [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr) }
    finally { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr) }
}

Write-Host ""
Write-Host "  NamiSoft - instalador sin firma (compatible con SAC)" -ForegroundColor White
Write-Host "  ----------------------------------------------------" -ForegroundColor DarkGray

# ------------------------------------------------------------------ 1. SAC
Info "Revisando Control inteligente de aplicaciones (SAC)..."
try {
    $state = (Get-ItemProperty -Path "HKLM:\SYSTEM\CurrentControlSet\Control\CI\Policy" `
                -Name VerifiedAndReputablePolicyState -ErrorAction Stop).VerifiedAndReputablePolicyState
    switch ($state) {
        1       { Warn "SAC esta ACTIVO (enforcing). Es la causa del WinError 4551; este script lo evita." }
        2       { Warn "SAC esta en modo evaluacion; igual conviene este metodo." }
        0       { Ok   "SAC desactivado en este equipo." }
        default { Info "Valor de politica: $state" }
    }
} catch {
    Info "SAC no configurado en este equipo (no aplica)."
}

# --------------------------------------------------------------- 2. runtime
New-Item -ItemType Directory -Force -Path $InstallDir | Out-Null
$localDotnet = Join-Path $InstallDir "dotnet\dotnet.exe"
$dotnet = $null

function Get-Runtimes([string]$exe) {
    try { return (& $exe --list-runtimes 2>$null) } catch { return @() }
}

$onPath = Get-Command dotnet -ErrorAction SilentlyContinue
if ($onPath) {
    if ((Get-Runtimes $onPath.Source) -match "Microsoft\.NETCore\.App 10\.") { $dotnet = $onPath.Source }
}

if (-not $dotnet -and (Test-Path $localDotnet)) {
    if ((Get-Runtimes $localDotnet) -match "Microsoft\.NETCore\.App 10\.") { $dotnet = $localDotnet }
}

if (-not $dotnet) {
    Info "No hay runtime .NET 10 disponible. Instalando copia local (sin admin)..."
    $tmp = Join-Path $env:TEMP ("namisoft-" + [guid]::NewGuid().ToString("N"))
    New-Item -ItemType Directory -Force -Path $tmp | Out-Null
    $script = Join-Path $tmp "dotnet-install.ps1"
    try {
        [Net.ServicePointManager]::SecurityProtocol = [Net.SecurityProtocolType]::Tls12
        Invoke-WebRequest -Uri $DotnetUrl -OutFile $script -UseBasicParsing
    } catch { Die "No pude descargar dotnet-install.ps1: $($_.Exception.Message)" }

    & powershell -NoProfile -ExecutionPolicy Bypass -File $script `
        -Channel "10.0" -Runtime "dotnet" -InstallDir (Join-Path $InstallDir "dotnet") -NoPath
    if (-not (Test-Path $localDotnet)) { Die "Fallo la instalacion del runtime .NET 10." }
    $dotnet = $localDotnet
    Ok "Runtime .NET 10 instalado en $InstallDir\dotnet"
} else {
    Ok "Runtime .NET 10 encontrado: $dotnet"
}

# --------------------------------------------------------------- 3. payload
$appDir = Join-Path $InstallDir "app"
if (Test-Path $appDir) { Remove-Item $appDir -Recurse -Force }
New-Item -ItemType Directory -Force -Path $appDir | Out-Null

function Build-FromSource {
    Info "Modo -FromSource: compilando desde GitHub..."
    $sdkOk = $false
    if ($dotnet) { try { $sdkOk = [bool]((& $dotnet --list-sdks) -match "^10\.") } catch { $sdkOk = $false } }

    if (-not $sdkOk) {
        Info "Instalando SDK .NET 10 local (necesario para compilar)..."
        $tmp = Join-Path $env:TEMP ("namisoft-sdk-" + [guid]::NewGuid().ToString("N"))
        New-Item -ItemType Directory -Force -Path $tmp | Out-Null
        $script = Join-Path $tmp "dotnet-install.ps1"
        Invoke-WebRequest -Uri $DotnetUrl -OutFile $script -UseBasicParsing
        & powershell -NoProfile -ExecutionPolicy Bypass -File $script `
            -Channel "10.0" -InstallDir (Join-Path $InstallDir "dotnet") -NoPath
        $dotnet = $localDotnet
        Ok "SDK .NET 10 instalado."
    }

    $srcZip = Join-Path $env:TEMP ("namisoft-src-" + [guid]::NewGuid().ToString("N") + ".zip")
    Info "Descargando codigo fuente..."
    Invoke-WebRequest -Uri $RepoUrl -OutFile $srcZip -UseBasicParsing
    $expanded = Join-Path $env:TEMP ("namisoft-src-" + [guid]::NewGuid().ToString("N"))
    Expand-Archive -Path $srcZip -DestinationPath $expanded -Force
    $proj = (Get-ChildItem -Path $expanded -Recurse -Filter "NamiSoft.csproj" | Select-Object -First 1)
    if (-not $proj) { Die "No encontre NamiSoft.csproj en el codigo fuente." }

    Info "Compilando (framework-dependent, sin apphost firmado)..."
    & $dotnet publish $proj.FullName -c Release -r win-x64 --no-self-contained `
        -p:PublishSingleFile=false -o $appDir
    if ($LASTEXITCODE -ne 0) { Die "Fallo la compilacion (exit $LASTEXITCODE)." }

    # helpers del instalador oficial (script de BD, icono, SQL, LEEME)
    $raiz = Split-Path $proj.FullName -Parent
    foreach ($f in @("instalador\Configurar-BD.ps1", "instalador\Configurar-BD.cmd", "instalador\NamiSoft.ico", "README.md")) {
        $origen = Join-Path $raiz $f
        if (Test-Path $origen) {
            $destino = if ($f -eq "README.md") { Join-Path $appDir "LEEME.md" } else { Join-Path $appDir (Split-Path $f -Leaf) }
            Copy-Item $origen $destino -Force
        }
    }
    $sqlOrigen = Join-Path $raiz "sql\NamiSoft_SQLServer.sql"
    if (Test-Path $sqlOrigen) {
        New-Item -ItemType Directory -Force -Path (Join-Path $appDir "sql") | Out-Null
        Copy-Item $sqlOrigen (Join-Path $appDir "sql\NamiSoft_SQLServer.sql") -Force
    }
    Ok "Compilado desde fuente."
}

function Get-ReleaseAsset {
    # Devuelve la URL del ZIP FDD de la ultima release, o $null.
    $api = "https://api.github.com/repos/Elnegraso23/NamiSoft/releases/latest"
    try {
        $rel = Invoke-RestMethod -Uri $api -Headers @{ "User-Agent" = "namisoft-installer" } -UseBasicParsing
        $asset = $rel.assets | Where-Object { $_.name -like "*-win-x64-fdd.zip" } | Select-Object -First 1
        if ($asset) { return $asset.browser_download_url }
    } catch { Warn "No pude consultar la API de GitHub: $($_.Exception.Message)" }
    return $null
}

if ($FromSource) {
    Build-FromSource
}
elseif ($Zip) {
    if (-not (Test-Path $Zip)) { Die "No existe el archivo: $Zip" }
    Info "Extrayendo $Zip ..."
    Expand-Archive -Path $Zip -DestinationPath $appDir -Force
    Ok "Payload extraido en $appDir"
}
else {
    Info "Buscando el paquete FDD en la ultima release de GitHub..."
    $url = Get-ReleaseAsset
    if ($url) {
        $destino = Join-Path $env:TEMP ("namisoft-fdd-" + [guid]::NewGuid().ToString("N") + ".zip")
        Info "Descargando $url"
        Invoke-WebRequest -Uri $url -OutFile $destino -UseBasicParsing
        Expand-Archive -Path $destino -DestinationPath $appDir -Force
        Remove-Item $destino -Force -ErrorAction SilentlyContinue
        Ok "Payload descargado y extraido."
    } else {
        Warn "La release no trae asset *-win-x64-fdd.zip; compilando desde fuente."
        Build-FromSource
    }
}

$dll = Join-Path $appDir "NamiSoft.dll"
if (-not (Test-Path $dll)) { Die "No encontre NamiSoft.dll en el payload. Paquete incorrecto." }

# --------------------------------------------------------- 4. base de datos
# NamiSoft resuelve la conexion en este orden:
#   1) NAMISOFT_DB_CONNECTION (cadena completa, tal cual)
#   2) NAMISOFT_DB_SERVER / NAMISOFT_DB_NAME + password de mssql.pass
#   3) sin nada: localhost,1433 / NamiSoft
$passFile = Join-Path (Join-Path $perfil ".config\namisoft") "mssql.pass"

function Set-UserEnv([string]$nombre, [string]$valor) {
    try { [Environment]::SetEnvironmentVariable($nombre, $valor, "User") }
    catch {
        [Environment]::SetEnvironmentVariable($nombre, $valor, "Process")
        Warn "No pude guardar $nombre a nivel usuario; queda solo en esta sesion."
    }
}

if ($NoDb) {
    Info "Base de datos: sin cambios (-NoDb)."
}
elseif ($DbConnection) {
    Set-UserEnv "NAMISOFT_DB_CONNECTION" $DbConnection
    Ok "Cadena de conexion guardada en NAMISOFT_DB_CONNECTION (variables de usuario)."
}
else {
    $srv = if ($DbServer) { $DbServer } else { "localhost,1433" }
    $pass = $DbPassword
    if (-not $pass) {
        $pass = Read-Password "Password del usuario sa de SQL Server [$srv] (Enter = configurar despues)"
    }

    if ($pass) {
        $carpeta = Join-Path $perfil ".config\namisoft"
        New-Item -ItemType Directory -Force -Path $carpeta | Out-Null
        Set-Content -Path $passFile -Value $pass.Trim() -NoNewline -Encoding ASCII
        try {
            $acl = Get-Acl $passFile
            $acl.SetAccessRuleProtection($true, $false)
            $regla = New-Object System.Security.AccessControl.FileSystemAccessRule(
                "$env:USERDOMAIN\$env:USERNAME", "FullControl", "Allow")
            $acl.SetAccessRule($regla)
            Set-Acl -Path $passFile -AclObject $acl
        } catch { Warn "No pude restringir permisos de mssql.pass (no es critico)." }
        Set-UserEnv "NAMISOFT_DB_SERVER" $srv
        Set-UserEnv "NAMISOFT_DB_NAME" $DbName
        Ok "BD configurada: servidor $srv / base $DbName"
        Info "Password guardada en $passFile (solo tu usuario puede leerla)."
    } else {
        Warn "Sin password: usa 'NamiSoft - Configurar base de datos' del menu Inicio antes de abrir la app."
    }
}

# --------------------------------------------------------------- 5. lanzador
$launcher = Join-Path $InstallDir "NamiSoft.cmd"
$rel = if ($dotnet -eq $localDotnet) { "%~dp0dotnet\dotnet.exe" } else { "dotnet" }
$cmdBody = @"
@echo off
rem Lanzador NamiSoft - ejecuta la app via dotnet (host firmado, no dispara SAC)
rem Si algun dia ves WinError 4551 aqui, revisa que dotnet.exe sea el de Microsoft.
setlocal
start "" /min "$rel" "%~dp0app\NamiSoft.dll" %*
"@
Set-Content -Path $launcher -Value $cmdBody -Encoding ASCII
Ok "Lanzador creado: $launcher"

# --------------------------------------------------------------- 6. accesos
$icono = Join-Path $appDir "NamiSoft.ico"
if (-not (Test-Path $icono)) { $icono = Join-Path $appDir "NamiSoft.exe" }

function New-Acceso([string]$rutaLnk, [string]$destino, [string]$desc) {
    try {
        $ws = New-Object -ComObject WScript.Shell
        $lnk = $ws.CreateShortcut($rutaLnk)
        $lnk.TargetPath = $destino
        $lnk.WorkingDirectory = $InstallDir
        $lnk.IconLocation = "$icono,0"
        $lnk.Description = $desc
        $lnk.Save()
        return $true
    } catch { return $false }
}

if (-not $NoShortcut) {
    $startMenu = Join-Path $env:APPDATA "Microsoft\Windows\Start Menu\Programs"
    $cfgCmd = Join-Path $appDir "Configurar-BD.cmd"

    if (New-Acceso (Join-Path $startMenu "NamiSoft.lnk") $launcher "NamiSoft - control de asistencia y nomina") {
        Ok "Acceso directo en el menu Inicio: NamiSoft"
    } else { Warn "No pude crear el acceso del menu Inicio." }

    if (Test-Path $cfgCmd) {
        if (New-Acceso (Join-Path $startMenu "NamiSoft - Configurar base de datos.lnk") $cfgCmd "Apunta NamiSoft a tu SQL Server") {
            Ok "Acceso directo: NamiSoft - Configurar base de datos"
        }
    }

    $desktop = [Environment]::GetFolderPath("Desktop")
    if ($desktop) { [void](New-Acceso (Join-Path $desktop "NamiSoft.lnk") $launcher "NamiSoft") }
}

# --------------------------------------------------------------- 7. prueba
if ($Test) {
    Info "Probando arranque (8 s)..."
    $antes = @(Get-Process dotnet -ErrorAction SilentlyContinue | Select-Object -ExpandProperty Id)
    Start-Process -FilePath $launcher -WindowStyle Minimized | Out-Null
    Start-Sleep -Seconds 8
    $nuevos = @(Get-Process dotnet -ErrorAction SilentlyContinue | Where-Object { $antes -notcontains $_.Id })
    if ($nuevos.Count -gt 0) {
        Ok "La app arranco correctamente (proceso dotnet PID $($nuevos[0].Id))."
        $nuevos | Stop-Process -Force -ErrorAction SilentlyContinue
    } else {
        Warn "No detecte proceso nuevo. Ejecuta $launcher desde una consola para ver el error."
    }
}

Write-Host ""
Ok "Instalacion terminada en: $InstallDir"
Write-Host ""
Write-Host "  Como abrirla:" -ForegroundColor White
Write-Host "    - Menu Inicio -> NamiSoft"
Write-Host "    - o doble clic en $launcher"
Write-Host ""
Write-Host "  Base de datos:" -ForegroundColor White
Write-Host "    La primera vez que abras NamiSoft crea la base y las tablas solo"
Write-Host "    (migraciones EF). El usuario sa necesita permiso dbcreator/sysadmin."
Write-Host "    Para cambiar servidor o password: menu Inicio -> NamiSoft - Configurar base de datos"
Write-Host ""
Write-Host "  Importante:" -ForegroundColor White
Write-Host "    NO ejecutes app\NamiSoft.exe directamente: ese si es un binario sin"
Write-Host "    firma y SAC lo volveria a bloquear. Usa el lanzador NamiSoft.cmd."
Write-Host ""
Write-Host "  Para desinstalar: borra la carpeta $InstallDir y los accesos directos."
Write-Host ""

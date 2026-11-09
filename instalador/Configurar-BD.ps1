param(
    [string]$Servidor,
    [string]$BaseDatos,
    [string]$PasswordPlano,
    [switch]$Silencioso
)

$ErrorActionPreference = "Stop"
$DefaultServidor = "localhost,1433"
$DefaultBase = "NamiSoft"

function Escribir($texto, $color = "Gray") { Write-Host $texto -ForegroundColor $color }

Escribir ""
Escribir "==============================================" "Cyan"
Escribir "  NamiSoft - Configuración de base de datos" "Cyan"
Escribir "==============================================" "Cyan"
Escribir ""

if ([string]::IsNullOrWhiteSpace($Servidor)) {
    $Servidor = Read-Host "Servidor SQL (host,puerto) [$DefaultServidor]"
    if ([string]::IsNullOrWhiteSpace($Servidor)) { $Servidor = $DefaultServidor }
}
if ([string]::IsNullOrWhiteSpace($BaseDatos)) {
    $BaseDatos = Read-Host "Nombre de la base [$DefaultBase]"
    if ([string]::IsNullOrWhiteSpace($BaseDatos)) { $BaseDatos = $DefaultBase }
}

$password = $PasswordPlano
if ([string]::IsNullOrWhiteSpace($password)) {
    $seguro = Read-Host "Contraseña del usuario sa" -AsSecureString
    $bstr = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($seguro)
    try { $password = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($bstr) }
    finally { [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($bstr) }
}

if ([string]::IsNullOrWhiteSpace($password)) {
    Escribir "No se capturó ninguna contraseña. Cancelado." "Red"
    exit 1
}

# --- 1) Comprobar que el puerto TCP responde -------------------------------
$partes = $Servidor.Split(",")
$host_ = $partes[0].Trim()
$puerto = 1433
if ($partes.Count -gt 1) { [void][int]::TryParse($partes[1].Trim(), [ref]$puerto) }

Escribir ""
Escribir "Probando conexión TCP a $host_`:$puerto ..." "Yellow"
$tcp = New-Object System.Net.Sockets.TcpClient
try {
    $ok = $tcp.BeginConnect($host_, $puerto, $null, $null).AsyncWaitHandle.WaitOne(5000, $false)
    if (-not $ok -or -not $tcp.Connected) { throw "sin respuesta" }
    Escribir "  Puerto abierto." "Green"
}
catch {
    Escribir "  ADVERTENCIA: no se pudo abrir el puerto $puerto en $host_." "Red"
    Escribir "  Verifica que SQL Server esté encendido y que el firewall permita el puerto." "Red"
}
finally { $tcp.Close() }

# --- 2) Validar credenciales con una conexion real --------------------------
$cadena = "Server=$Servidor;Database=$BaseDatos;User Id=sa;Password=$password;" +
          "Encrypt=True;TrustServerCertificate=True;Connect Timeout=15"
$credencialesOk = $false
try {
    $cn = New-Object System.Data.SqlClient.SqlConnection $cadena
    $cn.Open()
    $cn.Close()
    $credencialesOk = $true
    Escribir "  Credenciales correctas y base '$BaseDatos' accesible." "Green"
}
catch {
    $msg = $_.Exception.Message
    if ($msg -match "Cannot open database|no puede abrir la base|nombre de base de datos") {
        Escribir "  Credenciales correctas; la base '$BaseDatos' aún no existe." "Yellow"
        Escribir "  Se creará sola la primera vez que abras NamiSoft." "Yellow"
        $credencialesOk = $true
    }
    else {
        Escribir "  ADVERTENCIA: no se pudo validar el inicio de sesión:" "Red"
        Escribir "  $msg" "Red"
        Escribir "  Se guardará la configuración de todas formas." "Yellow"
    }
}

# --- 3) Guardar configuracion ----------------------------------------------
$carpeta = Join-Path $env:USERPROFILE ".config\namisoft"
if (-not (Test-Path $carpeta)) { New-Item -ItemType Directory -Path $carpeta -Force | Out-Null }

$rutaPass = Join-Path $carpeta "mssql.pass"
Set-Content -Path $rutaPass -Value $password -NoNewline -Encoding ASCII
try {
    $acl = Get-Acl $rutaPass
    $acl.SetAccessRuleProtection($true, $false)
    $regla = New-Object System.Security.AccessControl.FileSystemAccessRule(
        "$env:USERDOMAIN\$env:USERNAME", "FullControl", "Allow")
    $acl.SetAccessRule($regla)
    Set-Acl -Path $rutaPass -AclObject $acl
}
catch { Escribir "  (No se pudieron restringir permisos del archivo; no es crítico.)" "DarkYellow" }

[Environment]::SetEnvironmentVariable("NAMISOFT_DB_SERVER", $Servidor, "User")
[Environment]::SetEnvironmentVariable("NAMISOFT_DB_NAME", $BaseDatos, "User")

Escribir ""
Escribir "Configuración guardada:" "Green"
Escribir "  Servidor : $Servidor"
Escribir "  Base     : $BaseDatos"
Escribir "  Archivo  : $rutaPass"
Escribir ""
Escribir "Abre NamiSoft desde el menú Inicio. La base y las tablas se crean solas." "Cyan"
Escribir "Si la app ya estaba abierta, ciérrala y vuelve a abrirla para tomar los cambios." "Cyan"
Escribir ""

if (-not $Silencioso) { Read-Host "Presiona Enter para cerrar" | Out-Null }
if (-not $credencialesOk) { exit 2 }
exit 0

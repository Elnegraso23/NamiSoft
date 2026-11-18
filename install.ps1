$ErrorActionPreference = "Stop"

$Repo = "Elnegraso23/NamiSoft"
$AppName = "NamiSoft"
$AssetName = "NamiSoft-win-x64.zip"
$Version = if ($env:NAMISOFT_VERSION) { $env:NAMISOFT_VERSION } else { "latest" }
$InstallDir = if ($env:NAMISOFT_INSTALL_DIR) { $env:NAMISOFT_INSTALL_DIR } else { Join-Path $env:LOCALAPPDATA $AppName }
$Token = $env:GITHUB_TOKEN

Write-Host "==> Descargando $AppName ($Version) desde GitHub..." -ForegroundColor Cyan

$TempDir = Join-Path ([System.IO.Path]::GetTempPath()) ("namisoft-install-" + [Guid]::NewGuid().ToString("N"))
New-Item -ItemType Directory -Path $TempDir -Force | Out-Null
$ZipPath = Join-Path $TempDir $AssetName

try {
    if ($Token) {
        $ApiUrl = if ($Version -eq "latest") {
            "https://api.github.com/repos/$Repo/releases/latest"
        } else {
            "https://api.github.com/repos/$Repo/releases/tags/$Version"
        }
        $Headers = @{
            Authorization = "Bearer $Token"
            Accept        = "application/vnd.github+json"
        }
        $Release = Invoke-RestMethod -Uri $ApiUrl -Headers $Headers
        $Asset = $Release.assets | Where-Object { $_.name -eq $AssetName } | Select-Object -First 1
        if (-not $Asset) {
            throw "No se encontró el archivo $AssetName en el release $Version."
        }
        Invoke-WebRequest -Uri $Asset.url -Headers @{
            Authorization = "Bearer $Token"
            Accept        = "application/octet-stream"
        } -OutFile $ZipPath
    } else {
        $DownloadUrl = if ($Version -eq "latest") {
            "https://github.com/$Repo/releases/latest/download/$AssetName"
        } else {
            "https://github.com/$Repo/releases/download/$Version/$AssetName"
        }
        Invoke-WebRequest -Uri $DownloadUrl -OutFile $ZipPath
    }

    Write-Host "==> Instalando en $InstallDir..." -ForegroundColor Cyan
    New-Item -ItemType Directory -Path $InstallDir -Force | Out-Null
    Expand-Archive -Path $ZipPath -DestinationPath $InstallDir -Force

    $ExePath = Join-Path $InstallDir "$AppName.exe"
    $WshShell = New-Object -ComObject WScript.Shell

    $StartMenuDir = Join-Path $env:APPDATA "Microsoft\Windows\Start Menu\Programs"
    $DesktopDir = [Environment]::GetFolderPath("Desktop")

    foreach ($Dir in @($StartMenuDir, $DesktopDir)) {
        $Shortcut = $WshShell.CreateShortcut((Join-Path $Dir "$AppName.lnk"))
        $Shortcut.TargetPath = $ExePath
        $Shortcut.WorkingDirectory = $InstallDir
        $Shortcut.Description = "Control de asistencia y nomina semanal"
        $Shortcut.Save()
    }

    Write-Host "==> $AppName instalado correctamente." -ForegroundColor Green
    Write-Host "    Ubicación: $InstallDir"
    Write-Host "    Accesos directos creados en Escritorio y Menú Inicio."
}
finally {
    if (Test-Path $TempDir) {
        Remove-Item -Path $TempDir -Recurse -Force -ErrorAction SilentlyContinue
    }
}

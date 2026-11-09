@echo off
chcp 65001 >nul
setlocal EnableExtensions
title NamiSoft - Configurar base de datos
powershell.exe -NoProfile -ExecutionPolicy Bypass -File "%~dp0Configurar-BD.ps1"
if errorlevel 2 (
  echo.
  echo Revisa la configuracion e intenta de nuevo con "Configurar base de datos".
  pause
)
endlocal

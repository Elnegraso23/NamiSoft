$ErrorActionPreference = "Stop"
Set-Location $PSScriptRoot
dotnet restore
dotnet build
dotnet run

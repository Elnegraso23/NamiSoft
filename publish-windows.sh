#!/usr/bin/env bash
set -euo pipefail
cd "$(dirname "$0")"
dotnet publish NamiSoft.csproj -c Release -r win-x64 --self-contained true -o ./publish/win-x64
printf '\nPublicación Windows creada en: %s\n' "$(pwd)/publish/win-x64"

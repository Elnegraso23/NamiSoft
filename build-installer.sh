#!/usr/bin/env bash
# Genera los artefactos de distribucion de NamiSoft.
#
#   ./build-installer.sh [version]
#
# - Siempre: publica self-contained win-x64 y crea el ZIP portable.
# - Si estas en Windows con Inno Setup 6 (ISCC.exe), tambien compila el
#   instalador .exe con asistente.
# - En Linux el .exe se compila en el runner de GitHub Actions:
#       gh workflow run instalador.yml -f version=1.0.0
#       gh run download --name NamiSoft-Setup
set -euo pipefail
cd "$(dirname "$0")"

export PATH="${HOME}/.local/bin:${HOME}/.dotnet:${PATH}"
export DOTNET_ROOT="${DOTNET_ROOT:-${HOME}/.dotnet}"

VERSION="${1:-${NAMISOFT_VERSION:-1.0.0}}"
PUBLISH_DIR="./publish/win-x64"
SALIDA_DIR="./publish/instalador"
ZIP="${SALIDA_DIR}/NamiSoft-portable-win-x64.zip"

mkdir -p "$SALIDA_DIR"

echo "==> 1/3 Publicando self-contained win-x64 (v${VERSION})"
if [[ "${FORCE_PUBLISH:-0}" == "1" || ! -f "${PUBLISH_DIR}/NamiSoft.exe" ]]; then
  dotnet publish NamiSoft.csproj -c Release -r win-x64 --self-contained true -o "$PUBLISH_DIR"
else
  echo "    ya existe ${PUBLISH_DIR}/NamiSoft.exe (usa FORCE_PUBLISH=1 para regenerar)"
fi

echo "==> 2/3 Empaquetando ZIP portable"
rm -f "$ZIP"
python3 -m zipfile -c "$ZIP" "$PUBLISH_DIR"

echo "==> 3/3 Instalador .exe (Inno Setup)"
ISCC=""
for candidato in \
  "iscc" \
  "/c/Program Files (x86)/Inno Setup 6/ISCC.exe" \
  "/mnt/c/Program Files (x86)/Inno Setup 6/ISCC.exe"; do
  if command -v "$candidato" >/dev/null 2>&1 || [[ -f "$candidato" ]]; then ISCC="$candidato"; break; fi
done

if [[ -n "$ISCC" ]]; then
  "$ISCC" "/DMyAppVersion=${VERSION}" "instalador/NamiSoft.iss"
else
  echo "    ISCC no disponible en este equipo. El .exe se compila en Windows o en CI:"
  echo "      gh workflow run instalador.yml -f version=${VERSION}"
  echo "      gh run download --name NamiSoft-Setup"
fi

echo
echo "======================================================"
echo " Artefactos"
echo "======================================================"
for f in "$SALIDA_DIR"/*.exe "$ZIP"; do
  [[ -f "$f" ]] || continue
  printf ' %s\n   tamano: %s\n   sha256: %s\n\n' \
    "$(realpath "$f")" \
    "$(du -h "$f" | cut -f1)" \
    "$(sha256sum "$f" | cut -d' ' -f1)"
done

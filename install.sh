#!/usr/bin/env bash
set -euo pipefail

REPO="Elnegraso23/NamiSoft"
APP_NAME="NamiSoft"
BIN_NAME="namisoft"
INSTALL_DIR="${NAMISOFT_INSTALL_DIR:-$HOME/.local/share/NamiSoft}"
BIN_DIR="$HOME/.local/bin"
DESKTOP_DIR="$HOME/.local/share/applications"
VERSION="${NAMISOFT_VERSION:-latest}"
ASSET_NAME="NamiSoft-linux-x64.tar.gz"

info()  { printf '\033[1;34m==>\033[0m %s\n' "$*"; }
ok()    { printf '\033[1;32m==>\033[0m %s\n' "$*"; }
warn()  { printf '\033[1;33m[aviso]\033[0m %s\n' "$*"; }
fail()  { printf '\033[1;31m[error]\033[0m %s\n' "$*" >&2; exit 1; }

command -v curl >/dev/null 2>&1 || fail "Se requiere 'curl' para instalar $APP_NAME."
command -v tar  >/dev/null 2>&1 || fail "Se requiere 'tar' para extraer $APP_NAME."

OS="$(uname -s)"
ARCH="$(uname -m)"
[ "$OS" = "Linux" ] || fail "Este script es para Linux (detectado: $OS). En Windows usa install.ps1."
case "$ARCH" in
  x86_64|amd64) ;;
  *) fail "Arquitectura no soportada: $ARCH (solo x86_64/amd64)." ;;
esac

# Token opcional para repositorios privados (GITHUB_TOKEN o gh CLI)
TOKEN="${GITHUB_TOKEN:-}"
if [ -z "$TOKEN" ] && command -v gh >/dev/null 2>&1; then
  TOKEN="$(gh auth token 2>/dev/null || true)"
fi

TMP_DIR="$(mktemp -d)"
trap 'rm -rf "$TMP_DIR"' EXIT
ARCHIVE_PATH="$TMP_DIR/$ASSET_NAME"

info "Descargando $APP_NAME ($VERSION) desde GitHub..."

download_public() {
  local url
  if [ "$VERSION" = "latest" ]; then
    url="https://github.com/$REPO/releases/latest/download/$ASSET_NAME"
  else
    url="https://github.com/$REPO/releases/download/$VERSION/$ASSET_NAME"
  fi
  curl -fsSL "$url" -o "$ARCHIVE_PATH"
}

download_with_token() {
  local api_url asset_id
  if [ "$VERSION" = "latest" ]; then
    api_url="https://api.github.com/repos/$REPO/releases/latest"
  else
    api_url="https://api.github.com/repos/$REPO/releases/tags/$VERSION"
  fi

  asset_id="$(curl -fsSL \
    -H "Authorization: Bearer $TOKEN" \
    -H "Accept: application/vnd.github+json" \
    "$api_url" \
    | tr ',' '\n' \
    | grep -B5 "\"name\": *\"$ASSET_NAME\"" \
    | grep '"id":' \
    | head -n1 \
    | sed 's/[^0-9]//g')"

  [ -n "$asset_id" ] || return 1

  curl -fsSL \
    -H "Authorization: Bearer $TOKEN" \
    -H "Accept: application/octet-stream" \
    "https://api.github.com/repos/$REPO/releases/assets/$asset_id" \
    -o "$ARCHIVE_PATH"
}

if [ -n "$TOKEN" ]; then
  download_with_token || download_public || fail "No se pudo descargar $ASSET_NAME desde $REPO. Verifica que exista un Release publicado."
else
  download_public || fail "No se pudo descargar $ASSET_NAME. Si el repositorio es privado, exporta GITHUB_TOKEN o inicia sesión con 'gh auth login'."
fi

info "Instalando en $INSTALL_DIR..."
mkdir -p "$INSTALL_DIR" "$BIN_DIR" "$DESKTOP_DIR"

# Extraer sin tocar archivos de base de datos existentes (*.db*)
tar -xzf "$ARCHIVE_PATH" -C "$INSTALL_DIR"
chmod +x "$INSTALL_DIR/$APP_NAME"

# Wrapper en ~/.local/bin para asegurar el directorio de trabajo (NamiSoft.db)
cat > "$BIN_DIR/$BIN_NAME" <<EOF
#!/usr/bin/env bash
cd "$INSTALL_DIR"
exec "$INSTALL_DIR/$APP_NAME" "\$@"
EOF
chmod +x "$BIN_DIR/$BIN_NAME"

# Acceso directo en el menú de aplicaciones
cat > "$DESKTOP_DIR/namisoft.desktop" <<EOF
[Desktop Entry]
Type=Application
Name=NamiSoft
Comment=Control de asistencia y nómina semanal
Exec=$BIN_DIR/$BIN_NAME
Path=$INSTALL_DIR
Terminal=false
Categories=Office;Utility;
EOF

ok "$APP_NAME instalado correctamente."
printf '  - Ejecutable: %s/%s\n' "$BIN_DIR" "$BIN_NAME"
printf '  - Datos/App:  %s\n' "$INSTALL_DIR"
printf '  - Menú:       NamiSoft (ya disponible en tu lanzador de aplicaciones)\n'

case ":$PATH:" in
  *":$BIN_DIR:"*) ;;
  *)
    warn "$BIN_DIR no está en tu PATH. Para ejecutar '$BIN_NAME' desde terminal, agrega:"
    printf '  export PATH="$HOME/.local/bin:$PATH"\n'
    ;;
esac

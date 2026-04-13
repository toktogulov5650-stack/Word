#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"
APP_PORT="8080"

cd "${ROOT_DIR}"

if [[ ! -f ".env" ]]; then
  echo "Missing .env. Create it from .env.example first."
  exit 1
fi

if [[ ! -f "word.API/appsettings.Production.json" ]]; then
  echo "Missing word.API/appsettings.Production.json. Create it from the example first."
  exit 1
fi

if grep -q 'YOUR_' "word.API/appsettings.Production.json"; then
  echo "word.API/appsettings.Production.json still contains placeholder values."
  echo "Set a real DefaultConnection before building the container."
  exit 1
fi

if grep -q '^APP_PORT=' ".env"; then
  APP_PORT="$(grep '^APP_PORT=' ".env" | tail -n 1 | cut -d '=' -f 2-)"
fi

docker compose -f docker-compose.prod.yml up -d --build
docker compose -f docker-compose.prod.yml ps

echo
echo "If the container is running, verify the API with:"
echo "curl http://127.0.0.1:${APP_PORT}/health"

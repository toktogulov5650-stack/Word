#!/usr/bin/env bash
set -euo pipefail

ROOT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")/../.." && pwd)"

cd "${ROOT_DIR}"

if [[ ! -f ".env" ]]; then
  echo "Missing .env. Create it from .env.example first."
  exit 1
fi

if [[ ! -f "word.API/appsettings.Production.json" ]]; then
  echo "Missing word.API/appsettings.Production.json. Create it from the example first."
  exit 1
fi

docker compose -f docker-compose.prod.yml up -d --build
docker compose -f docker-compose.prod.yml ps

echo
echo "If the container is running, verify the API with:"
echo "curl http://127.0.0.1:8080/health"

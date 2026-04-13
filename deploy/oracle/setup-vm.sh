#!/usr/bin/env bash
set -euo pipefail

if [[ "${EUID}" -eq 0 ]]; then
  echo "Run this script as a regular user with sudo access, not as root."
  exit 1
fi

sudo apt update
sudo apt install -y docker.io docker-compose-plugin nginx certbot python3-certbot-nginx curl
sudo systemctl enable --now docker

if ! id -nG "${USER}" | grep -qw docker; then
  sudo usermod -aG docker "${USER}"
  echo
  echo "Docker group added for ${USER}."
  echo "Disconnect and reconnect SSH before running deploy/oracle/deploy-api.sh."
else
  echo
  echo "User ${USER} is already in the docker group."
fi

echo
echo "Base VM setup is complete."
echo "Next steps:"
echo "1. Reconnect to SSH if docker group was just added."
echo "2. Create .env and word.API/appsettings.Production.json."
echo "3. Run deploy/oracle/deploy-api.sh."

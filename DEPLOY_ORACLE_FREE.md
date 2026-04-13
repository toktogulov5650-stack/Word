# Oracle Cloud Always Free deployment

This project can run cheaply on an Oracle Cloud Always Free VM if we keep the VM focused on the API and use an external PostgreSQL database.

## What this setup uses

- Ubuntu VM in Oracle Cloud Always Free
- Docker + Docker Compose on the VM
- Host Nginx as reverse proxy
- External PostgreSQL, for example Neon, Supabase, or another managed Postgres

This is the lightest production setup for this repository because the VM does not also need to run Postgres.

## 1. Prepare the VM

Open these ports in Oracle Cloud:

- `80` for HTTP
- `443` for HTTPS
- `22` for SSH

Connect over SSH and install Docker, Compose, Nginx, and Certbot:

```bash
sudo apt update
sudo apt install -y docker.io docker-compose-plugin nginx certbot python3-certbot-nginx curl
sudo systemctl enable --now docker
sudo usermod -aG docker $USER
```

Reconnect to SSH after adding your user to the `docker` group.

## 2. Copy the project and create environment variables

Clone the repository on the VM and go to the project folder:

```bash
git clone <YOUR_REPOSITORY_URL> word
cd word
```

Create the production env file from the template:

```bash
cp .env.example .env
nano .env
```

Fill in at least these values:

- `APP_PORT=8080`
- `CORS_ALLOWED_ORIGINS=https://your-frontend-domain.com`
- `DEFAULT_CONNECTION=<your managed postgres connection string>`

For local development on your own machine, you can also copy `word.API/appsettings.Local.example.json` to `word.API/appsettings.Local.json`.

## 3. Start the API container

```bash
docker compose -f docker-compose.prod.yml up -d --build
docker compose -f docker-compose.prod.yml ps
curl http://127.0.0.1:8080/health
```

If the health endpoint returns `Healthy`, the API is up.

## 4. Configure Nginx

Copy the sample config:

```bash
sudo cp deploy/oracle/nginx-word-api.conf /etc/nginx/sites-available/word-api
sudo nano /etc/nginx/sites-available/word-api
```

Replace `api.your-domain.com` with your real API domain, then enable the site:

```bash
sudo ln -s /etc/nginx/sites-available/word-api /etc/nginx/sites-enabled/word-api
sudo nginx -t
sudo systemctl reload nginx
```

## 5. Enable HTTPS

Point your domain DNS to the VM public IP, then run:

```bash
sudo certbot --nginx -d api.your-domain.com
```

Certbot will update the Nginx config and install the certificate.

## 6. Updating the app

```bash
git pull
docker compose -f docker-compose.prod.yml up -d --build
```

## Notes

- The repository no longer stores the real production database password in `appsettings.json`. Use `.env` on the VM instead.
- Because the current application seeds the database on startup, make sure the configured database user has permission to create and update schema.
- On an Always Free VM, external managed Postgres is usually a better trade-off than running Postgres on the same small instance.

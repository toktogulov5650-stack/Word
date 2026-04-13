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

Connect over SSH to the VM.

## 2. Copy the project to the VM

Clone the repository on the VM and go to the project folder:

```bash
git clone <YOUR_REPOSITORY_URL> word
cd word
```

Make the helper scripts executable:

```bash
chmod +x deploy/oracle/setup-vm.sh
chmod +x deploy/oracle/deploy-api.sh
```

## 3. Install Docker, Nginx, and Certbot

Run the VM setup helper:

```bash
./deploy/oracle/setup-vm.sh
```

Reconnect to SSH after adding your user to the `docker` group.

## 4. Create runtime files

Create the small runtime env file from the template:

```bash
cp .env.example .env
nano .env
```

These values are enough:

- `APP_PORT=8080`
- `ASPNETCORE_ENVIRONMENT=Production`
- `SWAGGER_ENABLED=false`
- keep database and CORS settings in `word.API/appsettings.Production.json`, not in `.env`

Then create the production appsettings file:

```bash
cp word.API/appsettings.Production.example.json word.API/appsettings.Production.json
nano word.API/appsettings.Production.json
```

Fill in:

- real `ConnectionStrings:DefaultConnection`
- real frontend domain in `Cors:AllowedOrigins`

## 5. Start the API container

```bash
./deploy/oracle/deploy-api.sh
curl http://127.0.0.1:<APP_PORT>/health
```

If the health endpoint returns `Healthy`, the API is up.

## 6. Configure Nginx

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

## 7. Enable HTTPS

Point your domain DNS to the VM public IP, then run:

```bash
sudo certbot --nginx -d api.your-domain.com
```

Certbot will update the Nginx config and install the certificate.

## 8. Updating the app

```bash
git pull
./deploy/oracle/deploy-api.sh
```

## Notes

- Keep real production secrets in `word.API/appsettings.Production.json` on the VM, not in git.
- `.env.example` only contains variables that are actually passed into the production container.
- Because the current application seeds the database on startup, make sure the configured database user has permission to create and update schema.
- On an Always Free VM, external managed Postgres is usually a better trade-off than running Postgres on the same small instance.
- `.env` and `word.API/appsettings.Production.json` are already ignored by git in this repository.

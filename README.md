# Word API

ASP.NET Core API for a vocabulary-learning project with PostgreSQL storage.

## Before making the repository public

This project used a hardcoded local database password in `appsettings.json`. That has been removed.

Before publishing:

1. Create a new database password in your hosting provider.
2. Configure the database using an environment variable instead of committing secrets.
3. Set allowed frontend domains with environment variables.

## Recommended free public setup

For a fully free demo version, the best fit for this API is:

- Koyeb for the public API
- Neon for PostgreSQL

Why this stack:

- Koyeb currently offers a free instance with 512 MB RAM, 0.1 vCPU, and 2 GB SSD.
- Neon currently has a `$0/month` free plan for prototypes and side projects.
- Railway is not fully free anymore; its Hobby plan is `$5/month`.
- Render can work for demos, but its free Postgres expires after 30 days and free web services spin down after 15 minutes of inactivity.

## Environment variables

Set one of these:

- `ConnectionStrings__DefaultConnection=Host=...;Port=5432;Database=...;Username=...;Password=...;Ssl Mode=Require;Trust Server Certificate=true`
- `DATABASE_URL=postgresql://user:password@host/database?sslmode=require`

Set CORS origins:

- `Cors__AllowedOrigins__0=https://your-frontend-domain.com`
- `Cors__AllowedOrigins__1=http://localhost:3000`

Optional:

- `ASPNETCORE_ENVIRONMENT=Production`

## Docker deploy

This repository now includes a `Dockerfile`, so you can deploy it on platforms that support Docker-based services.

Local build:

```bash
docker build -t word-api .
docker run -p 8080:8080 \
  -e DATABASE_URL="postgresql://user:password@host/database?sslmode=require" \
  -e Cors__AllowedOrigins__0="http://localhost:3000" \
  word-api
```

Health check:

```text
/health
```

## Quick publish path

1. Push this project to a public GitHub repository.
2. Create a free Neon Postgres database.
3. Create a Koyeb Web Service from this repository or from the Dockerfile.
4. Add `DATABASE_URL` from Neon.
5. Add your frontend domain to `Cors__AllowedOrigins__0`.
6. Deploy and test `/health`.

## Notes

- This repository currently contains the backend API only.
- If you also need a free public frontend later, Vercel or Netlify are good options for a static frontend.

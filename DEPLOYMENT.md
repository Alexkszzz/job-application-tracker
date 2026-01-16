# Deployment Guide

## Pre-Deployment Checklist

### ✅ Completed

- [x] JWT secret moved to environment variables
- [x] CORS configured for production URLs
- [x] Environment variable examples created
- [x] .gitignore updated

### ⚠️ Before Deploying

1. **Generate Strong JWT Secret**

   ```bash
   # Run this command and copy the output
   openssl rand -base64 64
   ```

2. **Set Environment Variables**

   **Backend (Railway/Render/Azure):**

   ```
   JwtSettings__Secret=<your-generated-secret>
   AllowedOrigins__0=https://your-frontend.vercel.app
   ASPNETCORE_ENVIRONMENT=Production
   ```

   **Frontend (Vercel/Netlify):**

   ```
   NEXT_PUBLIC_API_URL=https://your-api.railway.app/api
   ```

---

## Quick Deploy (SQLite - Demo/Portfolio)

### Frontend → Vercel (Free)

1. Push code to GitHub
2. Go to [vercel.com](https://vercel.com) → Import Project
3. Select your repository
4. Set environment variable:
   - `NEXT_PUBLIC_API_URL` = your backend URL (add after backend deployed)
5. Deploy!

### Backend → Railway (Free tier)

1. Go to [railway.app](https://railway.app)
2. New Project → Deploy from GitHub
3. Select `apps/api/JobTracker.Api` as root directory
4. Add environment variables:
   ```
   JwtSettings__Secret=<your-secret-from-openssl>
   AllowedOrigins__0=https://your-app.vercel.app
   ```
5. Railway will auto-detect .NET and deploy

### Configure CORS After Deploy

1. Get your Vercel URL (e.g., `https://job-tracker-abc123.vercel.app`)
2. Update Railway env var: `AllowedOrigins__0=https://job-tracker-abc123.vercel.app`
3. Update Vercel env var: `NEXT_PUBLIC_API_URL=https://your-api.railway.app/api`
4. Redeploy both

---

## SQLite Limitations on Railway

⚠️ **Data Persistence:**

- Railway has persistent volumes, but SQLite database can be lost on some operations
- For demo/portfolio: Acceptable risk
- For real use: Migrate to PostgreSQL (see below)

⚠️ **What Resets Database:**

- Changing deployment settings
- Scaling to multiple instances
- Some platform maintenance

---

## Production Deploy (PostgreSQL - Recommended)

### 1. Setup PostgreSQL Database

**Option A: Railway (Easiest)**

- Add PostgreSQL plugin to your Railway project
- Automatic connection string in env vars

**Option B: Supabase (Free tier)**

- Create project at [supabase.com](https://supabase.com)
- Copy connection string

**Option C: Azure SQL Database**

- Create database in Azure Portal
- Copy connection string

### 2. Update Backend Connection

```bash
# Set in Railway/Azure env vars
ConnectionStrings__DefaultConnection=<your-postgresql-connection-string>
```

### 3. Install PostgreSQL Package

```bash
cd apps/api/JobTracker.Api
dotnet add package Npgsql.EntityFrameworkCore.PostgreSQL
```

### 4. Update Program.cs

Replace SQLite with PostgreSQL:

```csharp
// Change from:
options.UseSqlite("Data Source=jobtracker.db")

// To:
options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
```

### 5. Run Migrations

```bash
# Generate migration for PostgreSQL
dotnet ef migrations add InitialCreate -o Migrations

# Apply to database (run from Railway console or locally with prod connection string)
dotnet ef database update
```

---

## Post-Deployment Testing

1. ✅ Visit frontend URL - Should load
2. ✅ Register new account - Should create user
3. ✅ Login - Should get JWT token
4. ✅ Create application - Should save to database
5. ✅ Logout and login again - Should restore session
6. ✅ Open browser console - No CORS errors

---

## Monitoring

### Check Logs

**Railway:**

- Click deployment → View Logs

**Vercel:**

- Project → Logs tab

**Common Issues:**

- `JWT Secret not configured` → Set JwtSettings\_\_Secret env var
- `CORS error` → Add frontend URL to AllowedOrigins
- `Cannot connect to database` → Check connection string

---

## Rollback Plan

If deployment fails:

1. Railway: Redeploy previous version
2. Vercel: Revert deployment from dashboard
3. Database: Keep backups if using PostgreSQL

---

## Cost Estimate

### Free Tier (Demo)

- Vercel: Free (hobby projects)
- Railway: $5/month (500 hours free trial)
- Total: ~$0-5/month

### Production (PostgreSQL)

- Vercel: Free
- Railway: ~$10-20/month (compute + database)
- OR Azure App Service: ~$13/month + Database $5/month
- Total: ~$10-25/month

---

## Security Reminders

✅ **DO:**

- Use environment variables for secrets
- Generate strong JWT secret (64+ characters)
- Enable HTTPS (automatic on Vercel/Railway)
- Regularly update dependencies

❌ **DON'T:**

- Commit .env files
- Use same JWT secret as development
- Expose database credentials
- Disable CORS completely

---

## Need Help?

Check platform docs:

- [Railway Docs](https://docs.railway.app)
- [Vercel Docs](https://vercel.com/docs)
- [Azure App Service](https://learn.microsoft.com/azure/app-service/)

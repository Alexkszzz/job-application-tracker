using JobTracker.Api.Data;
using JobTracker.Api.Models;
using JobTracker.Api.Services.Implementation;
using JobTracker.Api.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Threading.RateLimiting;

try
{
    Console.WriteLine("=== Application Starting ===");
    
    var builder = WebApplication.CreateBuilder(args);
    Console.WriteLine("✓ WebApplication builder created");

    // Configure Kestrel to listen on Railway's PORT environment variable
    var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
    Console.WriteLine($"✓ Configuring to listen on port: {port}");
    builder.WebHost.ConfigureKestrel(options =>
    {
        options.ListenAnyIP(int.Parse(port));
    });
    Console.WriteLine("✓ Kestrel configured");

    // Add services to the container.
    builder.Services.AddControllers();
    Console.WriteLine("✓ Controllers added");

// Add rate limiting
builder.Services.AddRateLimiter(options =>
{
    options.AddFixedWindowLimiter("auth", limiterOptions =>
    {
        limiterOptions.PermitLimit = 10; // Increased for testing
        limiterOptions.Window = TimeSpan.FromMinutes(1);
        limiterOptions.QueueLimit = 0;
    });
    
    // Return proper error message when rate limit exceeded
    options.OnRejected = async (context, cancellationToken) =>
    {
        context.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        await context.HttpContext.Response.WriteAsJsonAsync(new
        {
            message = "Too many requests. Please try again later.",
            retryAfter = "60 seconds"
        }, cancellationToken);
    };
});

// Add Swagger services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        var allowedOrigins = builder.Configuration.GetSection("AllowedOrigins").Get<string[]>();
        
        if (allowedOrigins == null || allowedOrigins.Length == 0)
        {
            // Default to allowing all origins in development/testing
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            policy.WithOrigins(allowedOrigins)
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

// Add DbContext
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<JobTrackerDbContext>(options =>
{
    if (builder.Environment.IsProduction() && !string.IsNullOrEmpty(connectionString) && connectionString != "This will be override by user secrets")
    {
        // Use PostgreSQL in production
        options.UseNpgsql(connectionString);
    }
    else if (builder.Environment.IsDevelopment())
    {
        // Use SQLite file for local development
        options.UseSqlite("Data Source=jobtracker.db");
        Console.WriteLine("Using SQLite database for development: jobtracker.db");
    }
    else
    {
        // Use SQLite - Railway has writable /tmp directory
        var tmpDir = "/tmp";
        if (!Directory.Exists(tmpDir))
        {
            tmpDir = Path.GetTempPath(); // Fallback to system temp
        }
        var dbPath = Path.Combine(tmpDir, "jobtracker.db");
        Console.WriteLine($"Using SQLite database at: {dbPath}");
        options.UseSqlite($"Data Source={dbPath}");
    }
});

// Add Identity
builder.Services.AddIdentity<ApplicationUser, IdentityRole>(options =>
{
    // Password requirements
    options.Password.RequireDigit = true;
    options.Password.RequireLowercase = true;
    options.Password.RequireUppercase = true;
    options.Password.RequireNonAlphanumeric = true;
    options.Password.RequiredLength = 8;
    options.Password.RequiredUniqueChars = 4;
    
    // Lockout settings (prevent brute force)
    options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15);
    options.Lockout.MaxFailedAccessAttempts = 5;
    options.Lockout.AllowedForNewUsers = true;
    
    // User settings
    options.User.RequireUniqueEmail = true;
})
.AddEntityFrameworkStores<JobTrackerDbContext>()
.AddDefaultTokenProviders();

// Add JWT Authentication
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var secret = jwtSettings["Secret"] ?? throw new InvalidOperationException("JWT Secret not configured");

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings["Issuer"],
        ValidAudience = jwtSettings["Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret)),
        ClockSkew = TimeSpan.Zero // Remove default 5-minute grace period for immediate expiration
    };
});
builder.Services.AddAuthorization();

// Register application services
builder.Services.AddScoped<IApplicationService, ApplicationService>();
builder.Services.AddScoped<IAuthService, AuthService>();

var app = builder.Build();

// Apply migrations and create database on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<JobTrackerDbContext>();
        
        // Ensure database is created (simpler than migrations for SQLite)
        context.Database.EnsureCreated();
    }
    catch (Exception ex)
    {
        // Log to console since logger might not be available
        Console.WriteLine($"Database initialization error: {ex.Message}");
        Console.WriteLine($"Stack trace: {ex.StackTrace}");
        
        // Don't crash the app - let it start without database
        // (will fail on first API call but at least we'll see the error)
    }
}

app.UseCors("AllowFrontend");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHttpsRedirection();
}
else
{
    // Enable Swagger in production for testing (remove in real production)
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseRateLimiter();
    Console.WriteLine("✓ Rate limiter configured");

    app.UseAuthentication();
    Console.WriteLine("✓ Authentication configured");
    
    app.UseAuthorization();
    Console.WriteLine("✓ Authorization configured");

    app.MapControllers();
    Console.WriteLine("✓ Controllers mapped");

    Console.WriteLine($"=== Application ready to start on port {port} ===");
    app.Run();
}
catch (Exception ex)
{
    Console.WriteLine("=== FATAL ERROR ===");
    Console.WriteLine($"Exception Type: {ex.GetType().FullName}");
    Console.WriteLine($"Message: {ex.Message}");
    Console.WriteLine($"Stack Trace: {ex.StackTrace}");
    if (ex.InnerException != null)
    {
        Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");
        Console.WriteLine($"Inner Stack Trace: {ex.InnerException.StackTrace}");
    }
    Console.WriteLine("===================");
    throw;
}

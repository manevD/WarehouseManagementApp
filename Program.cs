using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WarehouseManagement.Components;
using WarehouseManagement.Components.Account;
using WarehouseManagement.Data;
using WarehouseManagement.Services;
using WarehouseManagement.Services.WarehouseManagement.Services;

var builder = WebApplication.CreateBuilder(args);

// =========================================================
// Blazor
// =========================================================

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();


// =========================================================
// Authentication State
// =========================================================

builder.Services.AddCascadingAuthenticationState();

builder.Services.AddScoped<IdentityRedirectManager>();
builder.Services.AddScoped<ExcelExportService>();
builder.Services.AddScoped<EmailService>();
builder.Services.AddScoped<
    AuthenticationStateProvider,
    IdentityRevalidatingAuthenticationStateProvider>();


// =========================================================
// Authentication
// =========================================================

builder.Services.AddAuthentication(options =>
{
    options.DefaultScheme = IdentityConstants.ApplicationScheme;
    options.DefaultSignInScheme = IdentityConstants.ExternalScheme;
})
.AddIdentityCookies();


// =========================================================
// Database
// =========================================================

var connectionString =
    builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException(
        "Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContextFactory<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();


// =========================================================
// ASP.NET Core Identity
// =========================================================

builder.Services.AddIdentityCore<ApplicationUser>(options =>
{
    options.SignIn.RequireConfirmedAccount = true;

    options.Stores.SchemaVersion =
        IdentitySchemaVersions.Version3;
})
.AddRoles<IdentityRole>()
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddSignInManager()
.AddDefaultTokenProviders();


// =========================================================
// Email
// =========================================================

builder.Services.AddSingleton<
    IEmailSender<ApplicationUser>,
    IdentityNoOpEmailSender>();


var app = builder.Build();


// =========================================================
// Seed Database / Roles
// =========================================================

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;

    await DbInitializer.InitializeAsync(services);
}


// =========================================================
// HTTP Request Pipeline
// =========================================================

if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler(
        "/Error",
        createScopeForErrors: true);

    app.UseHsts();
}

app.UseStatusCodePagesWithReExecute(
    "/not-found",
    createScopeForStatusCodePages: true);

app.UseHttpsRedirection();

app.UseAntiforgery();


// =========================================================
// Blazor Endpoints
// =========================================================

app.MapStaticAssets();

app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();


// =========================================================
// Identity Endpoints
// =========================================================

app.MapAdditionalIdentityEndpoints();


// =========================================================
// Run
// =========================================================

app.Run();
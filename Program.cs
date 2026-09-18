using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using System.Globalization;
using WarehouseManagement.Components;
using WarehouseManagement.Components.Account;
using WarehouseManagement.Configuration;
using WarehouseManagement.Data;
using WarehouseManagement.Services;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddLocalization();
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
builder.Services.AddScoped<PurchaseOrderPdfService>();
builder.Services.AddScoped<OrderPdfService>();
QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;

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
builder.Services.Configure<ShopifyOptions>(
    builder.Configuration.GetSection("Shopify"));

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
var supportedCultures = new[]
{
    new CultureInfo("de"),
    new CultureInfo("mk"),
    new CultureInfo("hr"),
    new CultureInfo("en")

};
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("de"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};
app.UseRequestLocalization(localizationOptions);
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

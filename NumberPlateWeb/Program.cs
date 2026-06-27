using NumberPlateWeb.Modules.Auth.Repositories;
using NumberPlateWeb.Modules.Auth.Services;
using NumberPlateWeb.Modules.Database;
using NumberPlateWeb.Modules.ExternalSystems.AdminAlerts;
using NumberPlateWeb.Modules.ExternalSystems.InternetLocation;
using NumberPlateWeb.Modules.ExternalSystems.PlateRecognition;
using NumberPlateWeb.Modules.Notifications.Repositories;
using NumberPlateWeb.Modules.Notifications.Services;
using NumberPlateWeb.Modules.PoliceManagement.Repositories;
using NumberPlateWeb.Modules.PoliceManagement.Services;
using NumberPlateWeb.Modules.Scanning.Repositories;
using NumberPlateWeb.Modules.Scanning.Services;
using NumberPlateWeb.Modules.VehicleLists.Repositories;
using NumberPlateWeb.Modules.VehicleLists.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});

builder.Services.AddSingleton<SqliteConnectionFactory>();
builder.Services.AddSingleton<SqliteDatabaseInitializer>();
builder.Services.AddSingleton<IAuthRepository, SqliteAuthRepository>();
builder.Services.AddSingleton<IPoliceRepository, InMemoryPoliceRepository>();
builder.Services.AddSingleton<IVehicleListRepository, SqliteVehicleListRepository>();
builder.Services.AddSingleton<IScanLogRepository, SqliteScanLogRepository>();
builder.Services.AddSingleton<INotificationRepository, SqliteNotificationRepository>();
builder.Services.AddSingleton<IPlateRecognitionGateway, MockPlateRecognitionGateway>();
builder.Services.AddSingleton<IInternetLocationService, MockInternetLocationService>();
builder.Services.AddSingleton<IAdminAlertGateway, MockAdminAlertGateway>();

builder.Services.AddScoped<AuthService>();
builder.Services.AddScoped<PoliceService>();
builder.Services.AddScoped<VehicleListService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<ScanService>();

var app = builder.Build();

app.Services.GetRequiredService<SqliteDatabaseInitializer>().Initialize();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseRouting();
app.UseSession();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();

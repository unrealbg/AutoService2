namespace AutoService2
{
    using Microsoft.Extensions.Logging;
    using Microsoft.EntityFrameworkCore;
    using AutoService2.Services;
    using AutoService2.Data;
    using AutoService2.Data.Repositories;
    using AutoService2.Models;

    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

            // Регистриране на DbContext
            builder.Services.AddDbContext<AutoServiceDbContext>(options =>
            {
                var dbPath = Path.Combine(FileSystem.AppDataDirectory, "autoservice.db");
                options.UseSqlite($"Filename={dbPath}");
            });

            // Регистриране на repositories
            builder.Services.AddScoped<VehicleRepository>();
            builder.Services.AddScoped<CustomerRepository>();
            builder.Services.AddScoped<WorkOrderRepository>();
            builder.Services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            builder.Services.AddScoped<Repository<Part>>();
            builder.Services.AddScoped<Repository<Appointment>>();
            builder.Services.AddScoped<Repository<Invoice>>();
            builder.Services.AddScoped<Repository<Settings>>();
            builder.Services.AddScoped<Repository<ServiceType>>();
            builder.Services.AddScoped<Repository<MaintenanceSchedule>>();
            builder.Services.AddScoped<Repository<DiagnosticReport>>();

            // Регистриране на сервизи
            builder.Services.AddScoped<DashboardService>();
            builder.Services.AddScoped<VehicleService>();
            builder.Services.AddScoped<CustomerService>();
            builder.Services.AddScoped<WorkOrderService>();
            builder.Services.AddScoped<ServiceTypeService>();
            builder.Services.AddScoped<MaintenanceService>();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif

            var app = builder.Build();

            // Инициализирай базата данни
            Task.Run(async () =>
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<AutoServiceDbContext>();
                await DatabaseInitializer.InitializeAsync(context);
            }).Wait();

            return app;
        }
    }
}

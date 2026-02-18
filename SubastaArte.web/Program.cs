using SubastaArte.Infraestructure.Repository.Interfaces;
using SubastaArte.Application.Profiles;
using SubastaArte.Application.Services.Implementations;
using SubastaArte.Application.Services.Interfaces;
using SubastaArte.Infraestructure.Data;
using SubastaArte.Infraestructure.Repository.Implementations;
using SubastaArte.web.Middleware;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using System.Text;



//***********
// =======================
// Configurar Serilog
// =======================
// Crear carpeta Logs automáticamente (evita errores si no existe)
Directory.CreateDirectory("Logs");

// Configuración Serilog
var logger = new LoggerConfiguration()
    // Nivel mínimo global (recomendado: Information)
    .MinimumLevel.Information()

    // Reducir ruido de logs internos de Microsoft
    .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    //Mostrar SQL ejecutado por EF Core
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore.Database.Command", LogEventLevel.Information)

    // Enriquecer logs con contexto (RequestId, etc.)
    .Enrich.FromLogContext()

    // Consola: útil para depurar en Visual Studio
    .WriteTo.Console()

    // Archivos separados por nivel (rolling diario)
    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Information)
        .WriteTo.File(@"Logs\Info-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Warning)
        .WriteTo.File(@"Logs\Warning-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Error)
        .WriteTo.File(@"Logs\Error-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .WriteTo.Logger(l => l
        .Filter.ByIncludingOnly(e => e.Level == LogEventLevel.Fatal)
        .WriteTo.File(@"Logs\Fatal-.log",
            shared: true,
            encoding: Encoding.UTF8,
            rollingInterval: RollingInterval.Day))

    .CreateLogger();

// Paso obligatorio ANTES de crear builder
Log.Logger = logger;

var builder = WebApplication.CreateBuilder(args);

// Integrar Serilog al host
builder.Host.UseSerilog(Log.Logger);

// Add services to the container.
builder.Services.AddControllersWithViews();
//***********
// =======================
// Configurar Dependency Injection
// =======================
//*** Repositories
builder.Services.AddTransient<IRepositoryUsuario, RepositoryUsuario>();
builder.Services.AddTransient<IRepositoryRol, RepositoryRol>();
builder.Services.AddTransient<IRepositoryEstadoUsuario, RepositoryEstadoUsuario>();



//*** Services
builder.Services.AddTransient<IServiceUsuario, ServiceUsuario>();
builder.Services.AddTransient<IServiceEstadoUsuario, ServiceEstadoUsuario>();
builder.Services.AddTransient<IServiceRol, ServiceRol>();


// =======================
// Configurar AutoMapper
// =======================
builder.Services.AddAutoMapper(config =>
{
    //*** Profiles

    config.AddProfile<UsuarioProfile>();
    config.AddProfile<RolProfile>();
    config.AddProfile<EstadoUsuarioProfile>();
});

// =======================
// Configurar SQL Server DbContext
// =======================
var connectionString = builder.Configuration.GetConnectionString("SqlServerDataBase");
if (string.IsNullOrWhiteSpace(connectionString))
{
    throw new InvalidOperationException(
        "No se encontró la cadena de conexión 'SqlServerDataBase' en appsettings.json / appsettings.Development.json.");
}

builder.Services.AddDbContext<SubastasContext>(options =>
{
    options.UseSqlServer(connectionString, sqlOptions =>
    {
        // Reintentos ante fallos transitorios (recomendado)
        sqlOptions.EnableRetryOnFailure();
    });

    if (builder.Environment.IsDevelopment())
        options.EnableSensitiveDataLogging();
});


var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    // Middleware personalizado
    app.UseMiddleware<ErrorHandlingMiddleware>();
}



app.UseHttpsRedirection();
app.UseRouting();

app.UseSerilogRequestLogging();

app.UseAuthorization();

app.MapStaticAssets();

app.UseAntiforgery();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();


app.Run();

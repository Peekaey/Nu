using Microsoft.EntityFrameworkCore;
using Nu_Apis.Helpers;
using Nu_Apis.Interfaces;
using Nu_BusinessService.Interfaces;
using Nu_BusinessService.Services;
using Nu_DataService;
using Nu_DataService.Interfaces;
using Nu_DataService.Repositories;
using Nu_DataService.Services;

namespace Nu_Apis;

public class Program
{
    public static void Main(string[] args)
    {

        var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        if (string.IsNullOrEmpty(postgresConnectionString))
        {
            Console.Error.WriteLineAsync("POSTGRES_CONNECTION_STRING environment variable is not set.");
            throw new InvalidOperationException("POSTGRES_CONNECTION_STRING environment variable is not set.");
        }

        var builder = WebApplication.CreateBuilder(args);
        // Validates scopes and services
        // IE - new service added but not registered
        builder.Host.UseDefaultServiceProvider(options =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });
        ConfigureHostServices(builder.Services);
        ConfigureDatabaseService(builder.Services, postgresConnectionString);
        var app = builder.Build();
        
        // Runs Pending Migrations
        InitialiseDatabase(app);
        
        ConfigureWebApp(app, builder);
        app.Run();
    }
    

    private static void ConfigureWebApp(WebApplication app, WebApplicationBuilder builder)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        // Disable to fix cors redirection for local
        app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");

        app.UseAuthorization();

        app.MapControllers();
    }

    private static void ConfigureHostServices(IServiceCollection services)
    {
        // Logging
        services.AddLogging(logging =>
        {
            logging.ClearProviders();
            logging.AddConsole();
            logging.AddDebug();
            logging.SetMinimumLevel(LogLevel.Trace);
        });

        services.AddControllers();

        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        services.AddSingleton<IRequestValidationHelpers, RequestValidationHelpers>();
        services.AddSingleton<IUserProfileService, UserProfileService>();
        services.AddSingleton<IUserProfilePictureService, UserProfilePictureService>();
        
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IUserProfilePictureRepository, UserProfilePictureRepository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IAccountBusinessService, AccountBusinessService>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins("http://localhost:5174")
                    .AllowAnyHeader()
                    .AllowAnyMethod();
            });
        });
        
        // Treats all controllers like services and validates their dependencies
        services.AddControllers().AddControllersAsServices();
    }

    private static void ConfigureDatabaseService(IServiceCollection services, string postgresConnectionString)
    {
        services.AddDbContext<DataContext>(options =>
        {
            options.UseNpgsql(postgresConnectionString, npgsqlOptions =>
            {
                npgsqlOptions.MigrationsAssembly("Nu-DataService");
            });
            options.UseNpgsql(postgresConnectionString)
                .LogTo(Console.WriteLine, LogLevel.Information);
        });
    }

    private static void InitialiseDatabase(IHost host)
    {
        using (var scope = host.Services.CreateScope())
        {
            try
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>();
                if (!dbContext.Database.CanConnect())
                {
                    throw new ApplicationException("Unable to connect to database.");
                }

                dbContext.Database.Migrate();
                Console.WriteLine("Database initialisation complete.");
            }
            catch (Exception e)
            {
                Console.WriteLine("Error occurred while initialising database: " + e.Message);
                throw;
            }
        }
    }
}
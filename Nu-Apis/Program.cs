using System.Text;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;
using Microsoft.IdentityModel.Tokens;
using Nu_Apis.Helpers;
using Nu_Apis.Interfaces;
using Nu_BusinessService;
using Nu_BusinessService.Interfaces;
using Nu_BusinessService.Services;
using Nu_Cache.Interfaces;
using Nu_Cache.Services;
using Nu_DataService;
using Nu_DataService.Interfaces;
using Nu_DataService.Repositories;
using Nu_DataService.Services;
using Nu_Models;

namespace Nu_Apis;

public class Program
{
    public static void Main(string[] args)
    {

        var postgresConnectionString = Environment.GetEnvironmentVariable("POSTGRES_CONNECTION_STRING");
        var rootFolderPath = "";
        if (string.IsNullOrEmpty(postgresConnectionString))
        {
            Console.Error.WriteLineAsync("POSTGRES_CONNECTION_STRING environment variable is not set.");
            throw new InvalidOperationException("POSTGRES_CONNECTION_STRING environment variable is not set.");
        }
        
        if (string.IsNullOrEmpty(rootFolderPath))
        {
            Console.Error.WriteLineAsync("Root folder path is not set.");
            throw new InvalidOperationException("Root folder path is not set.");
        }
        

        var builder = WebApplication.CreateBuilder(args);
        
        // Debug Local Environment Only
        builder.WebHost.ConfigureKestrel(options =>
        {
            options.ListenAnyIP(5000); 
        });
        var configuration = builder.Configuration;

        // Validates scopes and services
        // IE - new service added but not registered
        builder.Host.UseDefaultServiceProvider(options =>
        {
            options.ValidateScopes = true;
            options.ValidateOnBuild = true;
        });
        ConfigureHostServices(builder.Services, configuration,rootFolderPath);
        ConfigureAuthentication(builder.Services, configuration);
        ConfigureDatabaseService(builder.Services, postgresConnectionString);
        var app = builder.Build();
        
        // Expose Files on File System
        // TODO look at protecting files with authentication if auth enabled.
        app.UseStaticFiles(new StaticFileOptions
        {
            FileProvider = new PhysicalFileProvider(rootFolderPath),
            RequestPath = "/" + app.Services.GetRequiredService<ApplicationConfigurationSettings>().LastFolderName,
            OnPrepareResponse = ctx =>
                ctx.Context.Response.Headers.Append("Cache-Control", "public, max-age=604800")
        });
        
        // Runs Pending Migrations
        InitialiseDatabase(app);
        
        ConfigureWebApp(app, builder, rootFolderPath);
        app.Run();
    }
    

    private static void ConfigureWebApp(WebApplication app, WebApplicationBuilder builder, string rootFolderPath)
    {
        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        // Disable to fix cors redirection for local
        // app.UseHttpsRedirection();

        app.UseCors("AllowFrontend");

        app.UseAuthorization();

        app.MapControllers();

        var lifetime = app.Services.GetRequiredService<IHostApplicationLifetime>();
        lifetime.ApplicationStarted.Register(() =>

            // TODO Look at optimising and moving to IBackgroundService
            _ = Task.Run(async () =>
            {
                try
                {
                    // Indexing Metadata Process
                    using (var scope = app.Services.CreateScope())
                    {
                        var backgroundService =
                            scope.ServiceProvider.GetRequiredService<IBackgroundOrchestratorService>();
                        Console.WriteLine("Starting Library Indexing Process...");
                        var indexResult = await backgroundService.IndexLibraryContents(rootFolderPath);
                        if (indexResult.Success)
                        {
                            Console.WriteLine("Library Indexing Complete");
                        }
                        else
                        {
                            Console.WriteLine($"Library Indexing Failed: {indexResult.Error}");
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(
                        $"Unable to Index Library During Application Startup: {e.Message} | {e.StackTrace} ");
                }
            }));
    }

    private static void ConfigureAuthentication(IServiceCollection services, IConfiguration configuration)
    {
        // Configure cookie authentication
        services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                // Look for the token in the cookie "Nu_JWToken"
                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        if (context.Request.Cookies.ContainsKey("Nu_JWToken"))
                        {
                            context.Token = context.Request.Cookies["Nu_JWToken"];
                        }

                        return Task.CompletedTask;
                    }
                };
            });
        
        services.AddCors(options =>
        {
            // options.AddPolicy("AllowFrontend", policy =>
            // {
            //     policy.WithOrigins("http://localhost:5173")
            //         .AllowAnyHeader()
            //         .AllowCredentials()
            //         .AllowAnyMethod();
            // });
            // Debug Local Environment Only
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy
                    .SetIsOriginAllowed(origin => true)
                    .WithExposedHeaders("Content-Disposition")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
        });

        services.AddAuthorization();
    }

    private static void ConfigureHostServices(IServiceCollection services, IConfiguration configuration, string rootFolderPath)
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
        
        services.AddSingleton<IApiRequestValidationHelpers, ApiApiRequestValidationHelpers>();
        services.AddSingleton<IUserProfileService, UserProfileService>();
        services.AddSingleton<IUserProfilePictureService, UserProfilePictureService>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddSingleton<IAuthenticationBusinessService, AuthenticationBusinessService>();
        services.AddSingleton<IBackgroundFolderService, BackgroundFolderService>();
        services.AddSingleton<IBackgroundFileService, BackgroundFileService>();
        services.AddSingleton<ILibraryBusinessFileService, LibraryBusinessFileService>();
        services.AddSingleton<FilePathExtensions>();
        services.AddSingleton<IFilePathExtensions, FilePathExtensions>();
        services.AddSingleton<IBackgroundResizeService, BackgroundResizeService>();
        services.AddSingleton<IMappingHelpers, MappingHelpers>();
        
        services.AddSingleton<JwtConfig>(sp =>
        {
            var jwtConfig = new JwtConfig();
            configuration.GetSection("JwtConfig").Bind(jwtConfig);
            return jwtConfig;
        });

        services.AddSingleton<ApplicationConfigurationSettings>(acs =>
        {
            var applicationConfig = new ApplicationConfigurationSettings();
            applicationConfig.RootFolderPath = rootFolderPath;
            return applicationConfig;
        });

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<IAccountRepository, AccountRepository>();
        services.AddScoped<IUserProfileRepository, UserProfileRepository>();
        services.AddScoped<IUserProfilePictureRepository, UserProfilePictureRepository>();
        services.AddScoped<ILibraryFolderIndexRepository, LibraryFolderIndexRepository>();
        services.AddScoped<ILibraryFileIndexRepository, LibraryFileIndexRepository>();
        services.AddScoped<ILibraryPreviewThumbnailIndexRepository, LibraryPreviewThumbnailIndexRepository>();
        services.AddScoped<IAccountService, AccountService>();
        services.AddScoped<IIndexingService, IndexingService>();
        services.AddScoped<IBackgroundOrchestratorService, BackgroundOrchestratorService>();
        services.AddScoped<IAccountBusinessService, AccountBusinessService>();
        services.AddScoped<ILibraryBusinessService, LibraryBusinessService>();
        services.AddScoped<ILibraryService, LibraryService>();

        
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
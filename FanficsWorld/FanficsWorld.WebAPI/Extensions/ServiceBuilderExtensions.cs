using System.Text;
using FanficsWorld.DataAccess;
using FanficsWorld.DataAccess.Entities;
using FanficsWorld.DataAccess.Interfaces;
using FanficsWorld.DataAccess.Repositories;
using FanficsWorld.Services.Interfaces;
using FanficsWorld.Services.Jobs;
using FanficsWorld.Services.Services;
using FanficsWorld.WebAPI.Validators;
using FluentValidation;
using Ganss.XSS;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Quartz;

namespace FanficsWorld.WebAPI.Extensions;

public static class ServiceBuilderExtensions
{
    public static void ConfigureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<FanficsDbContext>(x =>
            x.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IUnitOfWork, UnitOfWork>();
        
        services.AddScoped<IUserRepository, UserRepository>();
        services.AddScoped<IFanficRepository, FanficRepository>();
        services.AddScoped<IFandomRepository, FandomRepository>();
        services.AddScoped<ITagRepository, TagRepository>();

        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IFanficService, FanficService>();
        services.AddScoped<IFandomService, FandomService>();
        services.AddScoped<ITagService, TagService>();

        services.AddValidatorsFromAssemblyContaining<LoginUserDtoValidator>();
        services.AddSingleton<IHtmlSanitizer>(_ => new HtmlSanitizer());
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.AddRouting(options => options.LowercaseUrls = true);
        
        services.AddQuartz(q =>
        {
            q.UseMicrosoftDependencyInjectionJobFactory();

            q.AddJobAndTrigger<FanficStatusUpdatingJob>(configuration);
        });

        services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
    }

    public static void ConfigureIdentity(this IServiceCollection services)
    {
        services.AddIdentity<User, IdentityRole>(opts =>
            {
                opts.Password.RequiredLength = 8;
                opts.Password.RequireNonAlphanumeric = false;
                opts.User.RequireUniqueEmail = true;
            })
            .AddEntityFrameworkStores<FanficsDbContext>()
            .AddDefaultTokenProviders();
    }
    
    public static void ConfigureJwt(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtConfiguration = configuration.GetSection("JwtConfig");
        var secretKey = jwtConfiguration["Secret"];
        services.AddAuthentication(options =>
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
                    ValidIssuer = jwtConfiguration["ValidIssuer"],
                    ValidAudience = jwtConfiguration["ValidAudience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey))
                };
            });
    }
    
    public static void ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "Fanfics World API",
                Version = "v1",
                Description = "Fanfics World API Services."
            });
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Name = "Authorization",
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "JWT Authorization header using the Bearer scheme."
            });
            
            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });
    }
}
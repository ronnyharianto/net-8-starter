using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using NET.Starter.API.Controllers.V1;
using NET.Starter.API.Controllers.V1.Security;
using NET.Starter.API.Extensions.StartupExtensions;
using System.Reflection;
using System.Text;

namespace NET.Starter.API.Extensions.StartupExtensions
{
    public class CustomControllerFeatureProvider : ControllerFeatureProvider
    {
        protected override bool IsController(TypeInfo typeInfo)
        {
            var isCustomController = !typeInfo.IsAbstract && typeof(BaseController).IsAssignableFrom(typeInfo);
            return isCustomController || base.IsController(typeInfo);
        }
    }

    public static class WebApplicationBuilderExtensions
    {
        public static WebApplicationBuilder AddController(this WebApplicationBuilder builder)
        {
            builder.Services.AddAntiforgery(options =>
            {
                options.SuppressXFrameOptionsHeader = true;
            });

            builder.Services.AddControllers(
                options =>
                {
                    //options.Filters.Add<AuthorizationFilter>();
                    //options.Filters.Add<TransactionFilter<ApplicationDbContext>>();
                })
                .ConfigureApplicationPartManager(manager => manager.FeatureProviders.Add(new CustomControllerFeatureProvider()))
                .AddNewtonsoftJson(x => x.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);

            return builder;
        }

        public static WebApplicationBuilder AddSwaggerGen(this WebApplicationBuilder builder)
        {
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "HGC.API", Version = "v1" });
                c.AddSecurityDefinition("Bearer",
                    new OpenApiSecurityScheme
                    {
                        Type = SecuritySchemeType.Http,
                        BearerFormat = "JWT",
                        Scheme = "Bearer",
                        In = ParameterLocation.Header,
                        Name = Microsoft.Net.Http.Headers.HeaderNames.Authorization,
                        Description = "JWT Authorization header using the Bearer scheme (Example: 'Bearer 12345abcdef')"
                    }
                );
                c.AddSecurityRequirement(
                    new OpenApiSecurityRequirement
                    {
                            {
                                new OpenApiSecurityScheme
                                {
                                    Reference = new OpenApiReference
                                    {
                                        Type = ReferenceType.SecurityScheme,
                                        Id = "Bearer"
                                    },
                                },
                                Array.Empty<string>()
                            }
                    });
                c.EnableAnnotations();
            });

            return builder;
        }

        public static WebApplicationBuilder AddCors(this WebApplicationBuilder builder)
        {
            var allowedHosts = builder.Configuration.GetValue<string>("AllowedHosts") ?? "*";

            builder.Services.AddCors(options =>
            {
                options.AddDefaultPolicy(builder =>
                {
                    builder
                        .AllowAnyOrigin()
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                });
            });

            return builder;
        }

        public static WebApplicationBuilder AddAuthentication(this WebApplicationBuilder builder)
        {
            var securityConfig = builder.Configuration.GetSection("SecurityConfig");

            builder.Services
                .AddAuthentication(options =>
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
                        ValidIssuer = securityConfig.GetValue<string>("Issuer"),
                        ValidAudience = securityConfig.GetValue<string>("Audience"),
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityConfig.GetValue<string>("SecretKey") ?? string.Empty)),
                        ClockSkew = TimeSpan.Zero
                    };
                });

            builder.Services.AddAuthorization();

            return builder;
        }
    }
}

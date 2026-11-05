

using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using SurveyBasket.API.Contracts.Authentication;
using System.Text;

namespace SurveyBasket.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddAuthConfig();
            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
           throw new InvalidOperationException("Connection String 'DefaultConnection' not found !");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddControllers();
            services.AddOpenApiServices()
                    .AddMapsterConfig()
                    .AddFluentValidationConfig();

            services.AddScoped<IPollService, PollService>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IJwtProvider, JwtProvider>();

            return services;
        }

        //-----------------------------------
        private static IServiceCollection AddMapsterConfig(this IServiceCollection services)

        {
            // Add Mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));
            return services;
        }
       private static IServiceCollection AddOpenApiServices(this IServiceCollection services)
        {

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
            return services;
        }
       private static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
                  services
                 .AddFluentValidationAutoValidation()
                 .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            return services;
        }
        private static IServiceCollection AddAuthConfig(this IServiceCollection services)
        {
            services.AddIdentity<ApplicationUser, IdentityRole>().
                 AddEntityFrameworkStores<ApplicationDbContext>();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }
            ).AddJwtBearer(o =>
            {
                o.SaveToken = true;
                o.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("kfdfdf")),
                    ValidIssuer = "SurveyBasketApp",
                    ValidAudience = "SurveyBasketUsers"
                };
            });
               

            return services;
        }
    }



}

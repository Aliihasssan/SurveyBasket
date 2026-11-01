

namespace SurveyBasket.API
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("DefaultConnection") ??
           throw new InvalidOperationException("Connection String 'DefaultConnection' not found !");

            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            services.AddControllers();
            services.AddOpenApi()
                    .AddMapsterConfig()
                    .AddFluentValidationConfig();

            services.AddScoped<IPollService, PollService>();

            return services;
        }

        //-----------------------------------
        public static IServiceCollection AddMapsterConfig(this IServiceCollection services)

        {
            // Add Mapster
            var mappingConfig = TypeAdapterConfig.GlobalSettings;
            mappingConfig.Scan(Assembly.GetExecutingAssembly());
            services.AddSingleton<IMapper>(new Mapper(mappingConfig));
            return services;
        }
        public static IServiceCollection AddOpenApiServices(this IServiceCollection services)
        {

            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            services.AddOpenApi();
            return services;
        }
        public static IServiceCollection AddFluentValidationConfig(this IServiceCollection services)
        {
                  services
                 .AddFluentValidationAutoValidation()
                 .AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            
            return services;
        }
    }



}



using Microsoft.EntityFrameworkCore;

namespace SurveyBasket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            //builder.Services.AddIdentityApiEndpoints<ApplicationUser>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>();

            var config = builder.Configuration["Key"];
            builder.Services.AddDependencies(builder.Configuration);
            var app = builder.Build();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.MapOpenApi();
                app.UseSwaggerUI(option => option.SwaggerEndpoint("/openapi/v1.json", "vvv"));
            }

            app.UseHttpsRedirection();

            app.UseCors(); // must be before Authorization

            app.UseAuthorization();

            //app.MapIdentityApi<ApplicationUser>();

            app.MapControllers();
            app.UseExceptionHandler();

            app.Run();
        }
    }
}

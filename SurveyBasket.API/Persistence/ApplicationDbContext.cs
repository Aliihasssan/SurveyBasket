using Microsoft.EntityFrameworkCore;
using SurveyBasket.API.Persistence.EntitiesConfiguration;


namespace SurveyBasket.API.Persistence
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options)
    {
        public DbSet<Poll> Polls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //modelBuilder.ApplyConfiguration(new PollConfiguration());

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
                
            base.OnModelCreating(modelBuilder);
        }

    }
}

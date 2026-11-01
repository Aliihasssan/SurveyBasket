
namespace SurveyBasket.API.Persistence.EntitiesConfiguration
{
    public class PollConfiguration : IEntityTypeConfiguration<Poll>
    {
        public void Configure(EntityTypeBuilder<Poll> builder)
        {
            builder.HasIndex(x => x.Title);
            builder.Property(x =>x.Title).IsRequired().HasMaxLength(100);
            builder.Property(x => x.Summary).IsRequired().HasMaxLength(1500);
        }
    }
}

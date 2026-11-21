namespace SurveyBasket.API.Entities
{
    public class Question : AuditableEntity
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;

        public int PollId { get; set; }
        public bool IsActive { get; set; }

        public Poll Poll { get; set; } = default!;
        public ICollection<Answer> Answers { get; set; } = [];

    }
}

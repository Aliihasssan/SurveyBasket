namespace SurveyBasket.API.Abstractions.cs
{
    public record Error(string Code, string Describtion)
    {
        public static readonly Error None = new Error(string.Empty,string.Empty);
    }
}

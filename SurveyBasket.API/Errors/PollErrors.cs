namespace SurveyBasket.API.Errors.cs
{
    public static class PollErrors
    {
        public static Error PollNotFound =
                new Error("Poll.NotFound", "No Poll was found with the given ID");
    }
}

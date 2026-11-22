namespace SurveyBasket.API.Errors
{
    public static class QuestionErrors
    {
        public static Error QuestionNotFound =
            new Error("Question.NotFound", "No Question was found with the given ID");
        public static Error DuplicatedQuestion =
            new Error("Question.DuplicatedContent", "Another question with the same content exists");
    }
}

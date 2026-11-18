namespace SurveyBasket.API.Errors
{
    public static class UserErrors
    {
        public static Error InvalidCredentials = new Error("User.InvalidCredentials", "Invalid email/password");
        public static Error UserNotFound = new Error("User.NotFound", "This user is not found");
        public static Error RefreshTokenNotFound = new Error("RT.NotFound", "This RF is not found");
    }
}

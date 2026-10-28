using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Contracts.Requests
{
    public record CreatePollRequest
        (
        int Id,
        string Name, 
        string Description,
        DateTime? BirthDate
        );
 
}

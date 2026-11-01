using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Contracts.Requests
{
    public record PollRequest
        (
        int Id,
        string Title, 
        string Summary,
        bool IsPublished,
        DateOnly StartsAt,
        DateOnly EndsAt
        );
 
}

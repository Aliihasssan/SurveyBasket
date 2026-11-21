using System.ComponentModel.DataAnnotations;

namespace SurveyBasket.API.Contracts.Polls
{
    public record PollRequest
        (
        //int Id,
        string Title, 
        string Summary,
        DateOnly StartsAt,
        DateOnly EndsAt
        );
 
}

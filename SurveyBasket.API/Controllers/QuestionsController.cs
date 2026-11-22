using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Questions;

namespace SurveyBasket.API.Controllers
{
    [Route("api/polls/{pollId}/[controller]")]
    [ApiController]
    [Authorize]
    public class QuestionsController (IQuestionService questionService): ControllerBase
    {
        private readonly IQuestionService _questionService = questionService;

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById()
        {
            return Ok();
        }

        [HttpPost("")]
        public async Task<IActionResult> Add([FromRoute] int pollId, [FromBody] QuestionRequest request,CancellationToken cancellation)
        {
            var result = await _questionService.AddAsync(pollId, request, cancellation);

            if (result.IsSuccess)
                return CreatedAtAction(nameof(GetById), new { pollId, result.Value.Id }, result.Value);

            return result.Error.Code == QuestionErrors.DuplicatedQuestion.Code
                ? Problem(statusCode: StatusCodes.Status409Conflict, detail: result.Error.Describtion, title: result.Error.Code)
                : Problem(statusCode: StatusCodes.Status404NotFound, detail: result.Error.Describtion, title: result.Error.Code) ;
;
            //Problem(statusCode: StatusCodes.Status404NotFound, detail: result.Error.Describtion, title: result.Error.Code,)
        }

    }
}

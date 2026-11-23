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
        //===================================================================

        [HttpGet]
        public async Task<IActionResult> GetAll([FromRoute] int pollId,CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAllAsync(pollId, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : Problem(statusCode: StatusCodes.Status404NotFound,title:result.Error.Code,detail:result.Error.Describtion);
        }
        //------------
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int pollId,[FromRoute]int id,CancellationToken cancellationToken)
        {
            var result = await _questionService.GetAsync(pollId, id, cancellationToken);

            return result.IsSuccess ? Ok(result.Value) : Problem(statusCode:StatusCodes.Status404NotFound,title:result.Error.Code,detail:result.Error.Describtion);
        }
        //-------
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
        //----------

        [HttpPut("{id}")]
        public async Task<IActionResult> ToggleStatus([FromRoute] int pollId, [FromRoute] int id ,CancellationToken cancellationToken)
        {
            var result =await _questionService.ToggleStatusAsync(pollId, id , cancellationToken);
            return result.IsSuccess ? NoContent() : Problem(statusCode: StatusCodes.Status404NotFound, title: result.Error.Code, detail: result.Error.Describtion);
        }
    }
}

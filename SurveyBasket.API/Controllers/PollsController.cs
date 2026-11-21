using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Polls;
using SurveyBasket.API.Services;
using System.Threading.Tasks;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]

    public class PollsController(IPollService pollService) : ControllerBase
    {
        // Primary Constructor
        private readonly IPollService _pollService = pollService;
        
        // -----------------------------------------

        [HttpGet("")]
        public async Task<IActionResult> GetAll (CancellationToken cancellationToken)
        {
            var polls =await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls.Value);
        }
        //========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            var result = await _pollService.GetAsync(id, cancellationToken);
            return result.IsFailure ?
                Problem(statusCode:StatusCodes.Status404NotFound,
                title:"Not Found",
                extensions: new Dictionary<string, object?>
                {
                    {
                        "errors" ,new[] {result.Error}
                    }
                } 
                ):
                Ok(result.Value);
        }
        //========================
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PollRequest req,CancellationToken cancellationToken=default)
        {
            var newPoll = await _pollService.AddAsync(req,cancellationToken);
            return newPoll.IsFailure ?  Problem(statusCode: StatusCodes.Status409Conflict, detail: newPoll.Error.Describtion, title: newPoll.Error.Code):
             CreatedAtAction(nameof(GetById), new { id = newPoll.Value.Id }, newPoll.Value);
        }
        //========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest req,CancellationToken cancellationToken)
        {
            var isUpdated = await _pollService.UpdateAsync(id, req,cancellationToken);

            if (isUpdated.IsSuccess)
                return NoContent();

            return NotFound(isUpdated.Error);
        }
        //========================
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute]int id,CancellationToken cancellationToken)
        {
            var isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

            if (isDeleted.IsSuccess)
                return NoContent();

            return NotFound(PollErrors.PollNotFound);

        }

        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, [FromBody] PollRequest req, CancellationToken cancellationToken)
        {
            var isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

            if (isUpdated.IsSuccess)
                return NoContent();

            return NotFound(isUpdated.Error);
            //return NotFound(PollErrors.PollNotFound);
        }




    }
}

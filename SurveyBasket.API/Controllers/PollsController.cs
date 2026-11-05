using Mapster;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Polls;
using SurveyBasket.API.Services;
using System.Threading.Tasks;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController(IPollService pollService) : ControllerBase
    {
        // Primary Constructor
        private readonly IPollService _pollService = pollService;
        
        // -----------------------------------------

        [HttpGet("")]
        [Authorize]
        public async Task<IActionResult> GetAll (CancellationToken cancellationToken)
        {
            var polls =await _pollService.GetAllAsync(cancellationToken);
            return Ok(polls.Adapt<List<PollResponse>>());
        }
        //========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id, CancellationToken cancellationToken)
        {
            Poll? poll = await _pollService.GetAsync(id, cancellationToken);
            var poly = poll.Adapt<PollResponse>();
            return poll is null ? NotFound() : Ok(poly);
        }
        //========================
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PollRequest req,CancellationToken cancellationToken=default)
        {
            Poll response = req.Adapt<Poll>();
            var newPoll = await _pollService.AddAsync(response,cancellationToken);
            return CreatedAtAction(nameof(GetById), new { id = newPoll.Id }, newPoll);
        }
        //========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest req,CancellationToken cancellationToken)
        {
            Poll response = req.Adapt<Poll>();
            bool isUpdated = await _pollService.UpdateAsync(id, response,cancellationToken);

            if (isUpdated)
                return NoContent();

            return NotFound();
        }
        //========================
        [HttpDelete]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            bool isDeleted = await _pollService.DeleteAsync(id, cancellationToken);

            if (isDeleted)
                return NoContent();

            return NotFound();
        }

        [HttpPut("{id}/togglePublish")]
        public async Task<IActionResult> TogglePublishStatus([FromRoute] int id, [FromBody] PollRequest req, CancellationToken cancellationToken)
        {
            Poll response = req.Adapt<Poll>();
            bool isUpdated = await _pollService.TogglePublishStatusAsync(id, cancellationToken);

            if (isUpdated)
                return NoContent();

            return NotFound();
        }




    }
}

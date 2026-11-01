using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Requests;
using SurveyBasket.API.Contracts.Responses;
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
        public IActionResult GetAll ()
        {
            var polls = _pollService.GetAllAsync();
            return Ok(polls.Adapt<List<PollResponse>>());
        }
        //========================
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            Poll? poll = await _pollService.GetAsync(id);
            var poly = poll.Adapt<PollResponse>();
            return poll is null ? NotFound() : Ok(poly);
        }
        //========================
        [HttpPost]
        public async Task<IActionResult> AddAsync([FromBody] PollRequest req)
        {
            Poll response = req.Adapt<Poll>();
            var newPoll = await _pollService.AddAsync(response);
            return CreatedAtAction(nameof(GetById), new { id = newPoll.Id }, newPoll);
        }
        //========================
        [HttpPut("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] PollRequest req)
        {
            Poll response = req.Adapt<Poll>();
            bool isUpdated = await _pollService.UpdateAsync(id, response);

            if (isUpdated)
                return NoContent();

            return NotFound();
        }
        //========================
        [HttpDelete]
        public async Task<IActionResult> Delete(int id)
        {
            bool isDeleted = await _pollService.DeleteAsync(id);

            if (isDeleted)
                return NoContent();

            return NotFound();
        }
      



    }
}

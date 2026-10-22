using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Models;
using SurveyBasket.API.Services;

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
            return Ok(_pollService.GetAll());
        }

        [HttpGet("{id}")]
        public IActionResult GetById (int id)
        {
            Poll ?poll = _pollService.Get(id);
            return poll is null ? NotFound() : Ok(poll);
        }

        [HttpPost]
        public IActionResult Add(Poll poll)
        {
            var newPoll  = _pollService.Add(poll);
            return CreatedAtAction(nameof(GetById),new {id =newPoll.Id},newPoll);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id ,Poll poll)
        {
            bool isUpdated = _pollService.Update(id,poll);

            if (isUpdated)
                return NoContent();

            return NotFound();
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool isDeleted = _pollService.Delete(id);

            if (isDeleted )
               return NoContent();

            return NotFound();
        }
       
    }
}

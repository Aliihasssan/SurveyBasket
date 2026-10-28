using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Contracts.Requests;
using SurveyBasket.API.Contracts.Responses;
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
            var polls = _pollService.GetAll();
            return Ok(polls.Adapt<List<PollResponse>>());
        }
        //========================
        [HttpGet("{id}")]
        public IActionResult GetById ([FromRoute]int id)
        {
            Poll? poll = _pollService.Get(id);
            var poly = poll.Adapt<PollResponse>();  
            return poll is null ? NotFound() : Ok(poly);
        }
        //========================
        [HttpPost]
        public IActionResult Add([FromBody] CreatePollRequest req)
        {
            Poll response = req.Adapt<Poll>();
            var newPoll  = _pollService.Add(response);
            return CreatedAtAction(nameof(GetById),new {id =newPoll.Id},newPoll);
        }
        //========================
        [HttpPut("{id}")]
        public IActionResult Update([FromRoute]int id ,[FromBody]CreatePollRequest req )
        {
            Poll response = req.Adapt<Poll>();
            bool isUpdated = _pollService.Update(id,response);

            if (isUpdated)
                return NoContent();

            return NotFound();
        }
        //========================
        [HttpDelete]
        public IActionResult Delete(int id)
        {
            bool isDeleted = _pollService.Delete(id);

            if (isDeleted ) 
               return NoContent();

            return NotFound();
        }
        //======================== Test
        [HttpGet("test")]
        public IActionResult Get([FromHeader(Name = "fdfd")] string ?id)
        {
            return Ok(id);
        }


       
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SurveyBasket.API.Models;

namespace SurveyBasket.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PollsController : ControllerBase
    {
        private readonly List<Poll> _polls = new List<Poll>();

        [HttpGet]
        public IActionResult GetAll ()
        {
            return Ok(_polls);
        }
    }
}

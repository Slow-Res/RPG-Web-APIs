using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Project.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<string> events = new();
        private static List<Character> _characters = new()
        {
            new (),
            new () { Id = 1, Name= "Sam"},
            new () { Id = 2, Name= "Gandalf"},
            new () { Id = 3, Name= "Aragon"},

        };

        [HttpGet]
        [Route("/[controller]s")]
        public ActionResult<List<Character>> GetAll() 
        { 
            events.Add($"Fetching All Characters @ {DateTime.Now}");
            return Ok(_characters);
        }

        [HttpGet]
        [Route("/[controller]/events")]
        public ActionResult<List<string>> Events()
        {
            return Ok(events);
        }

        [HttpGet]
        [Route("/[controller]/{id}")]
        public ActionResult<Character> GetOne(int id) {
            events.Add($"Fetching a single Character with id: #{id} @ {DateTime.Now}");
            var charcter = _characters.FirstOrDefault<Character>(  c => c.Id == id);
            return Ok(charcter);
        }

        [HttpPost]
        [Route("/[controller]")]
        public ActionResult<Character> AddOne(Character chr) {
            events.Add($"Added a  new single Character with id: #{chr.Id} @ {DateTime.Now}");
            _characters.Add(chr);
            return Ok(chr);
        }
    }
}
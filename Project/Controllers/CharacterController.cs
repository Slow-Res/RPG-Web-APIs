using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Project.Controllers
{
    [ApiController]
    [Route("/[controller]")]
    public class CharacterController : ControllerBase
    {
        
        private static List<Character> _characters = new()
        {
            new (),
            new () { Id = 1, Name= "Sam"},
            new () { Id = 2, Name= "Gandalf"},
            new () { Id = 3, Name= "Aragon"},

        };

        [HttpGet]
        [Route("/[controller]s")]
        public ActionResult<List<Character>> GetAll() => Ok(_characters);

        [HttpGet]        
        [Route("/[controller]/{id}")]
        public ActionResult<Character> GetOne(int id) {

            var charcter = _characters.FirstOrDefault<Character>(  c => c.Id == id);
            return Ok(charcter);

        } 

    }
}
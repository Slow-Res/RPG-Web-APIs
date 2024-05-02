using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Project.DTOs.Character;
using Project.Models;
using Project.Services.CharacterServices;

namespace Project.Controllers
{
    [Authorize]
    [ApiController]
    [Route("/[controller]")]
    public class CharacterController : ControllerBase
    {
        private static List<string> events = new();
        ICharacterServices _characterServices;

        public CharacterController(ICharacterServices characterServices)
        {
            _characterServices = characterServices;
        }

        [HttpGet]
        [Route("/[controller]s")]
        public async Task<ActionResult<ServiceResponse<List<GetCharacterDTO>>>> GetAll()
        { 
            
            events.Add($"Fetching All Characters @ {DateTime.Now}");
            return Ok( await _characterServices.GetAll());
        }

        [HttpGet]
        [Route("/[controller]/events")]
        public  ActionResult<List<string>> Events()
        {
            return Ok(events);
        }

        [HttpGet]
        [Route("/[controller]/{id}")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> GetOne(int id) {
            events.Add($"Fetching a single Character with id: #{id} @ {DateTime.Now}");
            var results = await _characterServices.GetOne(id);
            if(results.Data is null) return NotFound(results);
            return Ok(results);
        }

        [HttpPost]
        [Route("/[controller]")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddOne(AddCharacterDTO chr) {
            events.Add($"Added a  new single Character with name: #{chr.Name} @ {DateTime.Now}");
            return Ok( await _characterServices.AddOne(chr));
        }

        [HttpPut]
        [Route("/[controller]")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> UpdateOne(UpdateCharacterDTO chr) {
            events.Add($"Added a  new single Character with name: #{chr.Name} @ {DateTime.Now}");
            var results = await _characterServices.UpdateOne(chr);
            if(results.Data is null) return NotFound(results);
            return Ok(results);
        }

        [HttpDelete]
        [Route("/[controller]")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> DeleteOne(int id) {
            var results = await _characterServices.DeleteOne(id);
            if(results.Data is null) return NotFound(results);
            return Ok(results);
        }

        [HttpPost]
        [Route("Skill")]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddCharacterSkill(AddCharacterSkillDTO characterSkill)
        {
            return Ok(await _characterServices.AddCharacterSkill(characterSkill));
        }

    }
}
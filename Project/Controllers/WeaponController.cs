using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Project.DTOs.WeaponDTOs;
using Project.Services.WeaponServices;

namespace Project.Controllers
{
    [Route("/[controller]")]
    [ApiController]
    [Authorize]
    public class WeaponController : ControllerBase
    {
        public readonly IWeaponService _weaponService;

        public WeaponController(IWeaponService weaponService)
        {
            _weaponService = weaponService;
        }

        [HttpPost]
        public async Task<ActionResult<ServiceResponse<GetCharacterDTO>>> AddWeapon(AddWeaponDTO weapon)
        {
            return Ok(await _weaponService.AddWeapon(weapon));
        }
    }
}

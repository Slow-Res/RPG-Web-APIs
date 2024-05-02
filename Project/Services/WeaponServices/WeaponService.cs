
using Project.DTOs.WeaponDTOs;
using System.Security.Claims;

namespace Project.Services.WeaponServices
{
    public class WeaponService : IWeaponService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public WeaponService(IMapper mapper, DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
             
        }

        private async Task<User> GetUser()
        {
            int id = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO weapon)
        {
            var user = await GetUser();
            var ch = await _context.Characters
                .Include(ch => ch.Weapon)
                .FirstOrDefaultAsync(  c => c.Id == weapon.CharacterId && c.User!.Id == user.Id );
            if (ch == null)
                return new ServiceResponse<GetCharacterDTO> 
                    { Message = $"Could not find any character with id {weapon.CharacterId}", Success = false };
            
            if(ch.Weapon != null)
                return new ServiceResponse<GetCharacterDTO>
                    { Message = $"{ch.Name} Already have a weapon", Success = false };

            var newWeapon = new Weapon { Name = weapon.Name, Damage = weapon.Damage, Character = ch };
            _context.Weapons.Add(newWeapon);
            await _context.SaveChangesAsync();
            return new ServiceResponse<GetCharacterDTO>
                { Data = _mapper.Map<GetCharacterDTO>(ch), Message = "New Weapon Added Sucessfully" };

        }
    }
}

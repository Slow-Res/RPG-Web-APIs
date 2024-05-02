
using Project.Controllers;
using Project.Migrations;
using System.Diagnostics;
using System.Security.Claims;

namespace Project.Services.CharacterServices
{
    public class CharacterService : ICharacterServices
    {

        private static List<Character> _characters = new()
        {
            new (),
            new () { Id = 1, Name= "Sam"},
            new () { Id = 2, Name= "Gandalf"},
            new () { Id = 3, Name= "Aragon"},

        };

        public User CurrentUser { get; set; }
        private readonly IMapper _mapper;
        private readonly DataContext _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        

        public CharacterService(IMapper mapper , DataContext context, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _context = context;
            _mapper = mapper;
            CurrentUser = GetUser().GetAwaiter().GetResult();
        }

        private async Task<User> GetUser()
        {
            int id = int.Parse(_httpContextAccessor.HttpContext!.User.FindFirstValue(ClaimTypes.NameIdentifier));
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddOne(AddCharacterDTO character)
        {
            var newCharacter = _mapper.Map<Character>(character);
            newCharacter.User = await GetUser();
            _context.Add(newCharacter);
            await _context.SaveChangesAsync();
            return new ServiceResponse<GetCharacterDTO>() { Data = _mapper.Map<GetCharacterDTO>(newCharacter), Message="Successfully added a new character"};
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteOne(int id)
        {
            var myCharacter = _context.Characters.FirstOrDefault( c => c.Id == id);
            if( myCharacter is null)
                return new ServiceResponse<List<GetCharacterDTO>>() { Data = null, Message=$"Can't find a character with id: {id}" , Success = false };
            _context.Characters.Remove(myCharacter);
            await _context.SaveChangesAsync();
            return new ServiceResponse<List<GetCharacterDTO>>()
            {
                Data =  (await _context.Characters.ToListAsync()).Select( c => _mapper.Map<GetCharacterDTO>(c)).ToList()
            };

        }

        public async  Task<ServiceResponse<List<GetCharacterDTO>>> GetAll()
        {
            var user = await GetUser();
            var chars = await _context.Characters.Where( c => c.User!.Id == user.Id).Include(c => c.Weapon).ToListAsync();
            var response = chars.Select(c => _mapper.Map<GetCharacterDTO>(c)).ToList();
            return new ServiceResponse<List<GetCharacterDTO>>() { Data = response };
        }
         
        public async Task<ServiceResponse<GetCharacterDTO>> GetOne(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(  c => c.Id == id && GetUser().Id == c.User!.Id );
            return character is null ?
                new ServiceResponse<GetCharacterDTO>() { Data = null, Message=$"Can't find a character with id: {id}" , Success = false } :
                new ServiceResponse<GetCharacterDTO>() { Data = _mapper.Map<GetCharacterDTO>(character) };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateOne(UpdateCharacterDTO character)
        {
            var user = await GetUser();
            var currentCharacter = _context.Characters.Include(c => c.User)
                                                      .FirstOrDefault(c => c.Id == character.Id && c.User!.Id == user.Id);

            if (currentCharacter is null || user.Id != currentCharacter.User!.Id)
                return new ServiceResponse<GetCharacterDTO>()
                    { Message = $"Can't find a character with id: {character.Id}", Success = false };

            currentCharacter.Name = character.Name;
            currentCharacter.HitPoints = character.HitPoints;
            currentCharacter.Defense = character.Defense;
            currentCharacter.Strength = character.Strength;
            currentCharacter.Intelligence = character.Intelligence;
            currentCharacter.Class = character.Class;

            _context.Characters.Update(currentCharacter);
            await _context.SaveChangesAsync();
            return new ServiceResponse<GetCharacterDTO>() 
                { Data = _mapper.Map<GetCharacterDTO>(character), Message="Your Character got updated successfully" };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDTO characterSkill)
        {   
            var character = await _context.Characters.Include(c => c.Weapon)
                                               .Include(c => c.Skills)
                                               .FirstOrDefaultAsync(c => c.Id == characterSkill.CharacterId && c.User.Id == CurrentUser.Id);

            if (character == null) return new ServiceResponse<GetCharacterDTO> { Success=false, Message = "Character Not Found." };

            var skill = await _context.Skills.FirstOrDefaultAsync(s => s.Id == characterSkill.SkillId);

            if (skill == null) return new ServiceResponse<GetCharacterDTO> { Success = false, Message = "Skill Not Found." };

            character.Skills!.Add(skill);
            await _context.SaveChangesAsync();
            return new ServiceResponse<GetCharacterDTO>
                {
                    Data = _mapper.Map<GetCharacterDTO>(character),
                    Message = "Skill Added Sucessfully"
                };


        }
    }
}

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

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public CharacterService(IMapper mapper , DataContext context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddOne(AddCharacterDTO character)
        {
            var newCharacter = _mapper.Map<Character>(character);
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
            return new ServiceResponse<List<GetCharacterDTO>>() { Data =  (await _context.Characters.ToListAsync()).Select( c => _mapper.Map<GetCharacterDTO>(c)).ToList() };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetOne(int id)
        {
            var character = await _context.Characters.FirstOrDefaultAsync(  c => c.Id == id);
            return character is null ?
                new ServiceResponse<GetCharacterDTO>() { Data = null, Message=$"Can't find a character with id: {id}" , Success = false } :
                new ServiceResponse<GetCharacterDTO>() { Data = _mapper.Map<GetCharacterDTO>(character) };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateOne(UpdateCharacterDTO character)
        {
            _context.Characters.Update(_mapper.Map<Character>(character));
            await _context.SaveChangesAsync();
            return new ServiceResponse<GetCharacterDTO>() { Data = _mapper.Map<GetCharacterDTO>(character), Message="Your Character got updated successfully" };
        }
    }
}
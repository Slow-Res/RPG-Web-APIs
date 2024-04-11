using System.ComponentModel;
using System.Reflection.Metadata.Ecma335;
using AutoMapper;
using Project.Controllers;
using Project.DTOs.Character;
using Project.Models;
using Project.Services.CharacterServices;

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

        public CharacterService(IMapper mapper)
        {
            this._mapper = mapper;
        }

        public async Task<ServiceResponse<GetCharacterDTO>> AddOne(AddCharacterDTO character)
        {
            var newCharacter = _mapper.Map<Character>(character);
            newCharacter.Id = _characters.Max( c => c.Id ) + 1;
             _characters.Add(newCharacter);
             return new ServiceResponse<GetCharacterDTO>() { Data = _mapper.Map<GetCharacterDTO>(newCharacter), Message="Successfully added a new character"};
        }

        public async Task<ServiceResponse<List<GetCharacterDTO>>> DeleteOne(int id)
        {
            var myCharacter = _characters.Find( c => c.Id == id );
            if( myCharacter is null)
                return new ServiceResponse<List<GetCharacterDTO>>() { Data = null, Message=$"Can't find a character with id: {id}" , Success = false };
            _characters.Remove(myCharacter);
            return new ServiceResponse<List<GetCharacterDTO>>() { Data = _characters.Select( c => _mapper.Map<GetCharacterDTO>(c)).ToList() };
        }


        public async  Task<ServiceResponse<List<GetCharacterDTO>>> GetAll()
        {
            return new ServiceResponse<List<GetCharacterDTO>>() { Data = _characters.Select( c => _mapper.Map<GetCharacterDTO>(c)).ToList() };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> GetOne(int id)
        {
            var character = _characters.FirstOrDefault<Character>(  c => c.Id == id);
            return character is null ?
                new ServiceResponse<GetCharacterDTO>() { Data = null, Message=$"Can't find a character with id: {id}" , Success = false } :
                new ServiceResponse<GetCharacterDTO>() { Data = _mapper.Map<GetCharacterDTO>(character) };
        }

        public async Task<ServiceResponse<GetCharacterDTO>> UpdateOne(UpdateCharacterDTO character)
        {
            var myCharacter = _characters.Find( c => c.Id == character.Id );
            if( myCharacter is null)
                return new ServiceResponse<GetCharacterDTO>() { Data = null, Message=$"Can't find a character with id: {character.Id}" , Success = false };
            myCharacter.Name = character.Name;
            myCharacter.HitPoints = character.HitPoints;
            myCharacter.Defense = character.Defense;
            myCharacter.Intelligence = character.Intelligence;
            myCharacter.Class = character.Class;
            myCharacter.Strength = character.Strength;
            return new ServiceResponse<GetCharacterDTO>() { Data = _mapper.Map<GetCharacterDTO>(myCharacter), Message="Your Character got updated successfully" };
        }
    }
}
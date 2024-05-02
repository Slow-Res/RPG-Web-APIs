using Project.Controllers;
using Project.DTOs.Character;
using Project.Models;

namespace Project.Services.CharacterServices
{
    public interface ICharacterServices
    {
        Task<ServiceResponse<GetCharacterDTO>> GetOne(int id);
        Task<ServiceResponse<List<GetCharacterDTO>>> GetAll();
        Task<ServiceResponse<GetCharacterDTO>> AddOne(AddCharacterDTO character);
        Task<ServiceResponse<GetCharacterDTO>> UpdateOne(UpdateCharacterDTO character);
        Task<ServiceResponse<List<GetCharacterDTO>>> DeleteOne(int id);

        Task<ServiceResponse<GetCharacterDTO>> AddCharacterSkill(AddCharacterSkillDTO skill);

    }
}
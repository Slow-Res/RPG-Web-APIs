using AutoMapper;
using Project.Controllers;
using Project.DTOs.Character;
using Project.DTOs.Skill;
using Project.DTOs.WeaponDTOs;

namespace Project
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character,GetCharacterDTO>();
            CreateMap<AddCharacterDTO,Character>();
            CreateMap<UpdateCharacterDTO,Character>();
            CreateMap<UpdateCharacterDTO,GetCharacterDTO>();
            CreateMap<Weapon, GetWeaponDTO>();
            CreateMap<AddWeaponDTO,Weapon>();
            CreateMap<Skill, GetSkillDTO>();

        }
    }
}
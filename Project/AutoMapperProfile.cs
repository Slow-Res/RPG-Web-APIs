using AutoMapper;
using Project.Controllers;
using Project.DTOs.Character;

namespace Project
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character,GetCharacterDTO>();
            CreateMap<AddCharacterDTO,Character>();
            CreateMap<UpdateCharacterDTO,Character>();
        }
    }
}
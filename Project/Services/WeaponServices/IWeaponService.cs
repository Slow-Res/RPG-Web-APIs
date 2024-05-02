using Project.DTOs.WeaponDTOs;

namespace Project.Services.WeaponServices
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO weapon);
    }
}

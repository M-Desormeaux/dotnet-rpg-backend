using System.Threading.Tasks;
using NetRPG.DTOs.Character;
using NetRPG.DTOs.Weapon;
using NetRPG.Models;

namespace NetRPG.Services.WeaponService
{
    public interface IWeaponService
    {
        Task<ServiceResponse<GetCharacterDTO>> AddWeapon(AddWeaponDTO newWeapon);
    }
}
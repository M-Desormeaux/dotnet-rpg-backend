using AutoMapper;
using NetRPG.DTOs.Character;
using NetRPG.DTOs.Fight;
using NetRPG.DTOs.Weapon;
using NetRPG.Models;

namespace NetRPG
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Character, GetCharacterDTO>();
            CreateMap<PostCharacterDTO, Character>();
            CreateMap<Weapon, GetWeaponDTO>();
            CreateMap<Skill, GetSkillDTO>();
            CreateMap<Character, HighScoreDTO>();
        }
    }
}
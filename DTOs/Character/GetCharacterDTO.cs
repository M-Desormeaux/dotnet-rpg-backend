using System.Collections.Generic;
using NetRPG.DTOs.Weapon;
using NetRPG.Models;

namespace NetRPG.DTOs.Character
{
    public class GetCharacterDTO
    {
        public int ID { get; set; }
        public string Name { get; set; } = "Player";
        public int HitPoints { get; set; } = 15;
        public int Str { get; set; } = 10;
        public int Con { get; set; } = 10;
        public int Dex { get; set; } = 10;
        public int Int { get; set; } = 10;
        public int Wis { get; set; } = 10;
        public int Cha { get; set; } = 10;
        public ClassTypes ClassType { get; set; } = ClassTypes.Knight;
        public GetWeaponDTO Weapon { get; set; }
        public List<GetSkillDTO> Skills { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
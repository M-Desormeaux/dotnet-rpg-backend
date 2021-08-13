using NetRPG.Models;

namespace NetRPG.DTOs.Character
{
    public class PostCharacterDTO
    {
        public string Name { get; set; } = "Player";
        public int HitPoints { get; set; } = 15;
        public int Str { get; set; } = 10;
        public int Con { get; set; } = 10;
        public int Dex { get; set; } = 10;
        public int Int { get; set; } = 10;
        public int Wis { get; set; } = 10;
        public int Cha { get; set; } = 10;
        public ClassTypes ClassType { get; set; } = ClassTypes.Knight;
    }
}
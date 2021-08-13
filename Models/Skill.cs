using System.Collections.Generic;

namespace NetRPG.Models
{
    public class Skill
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Damage { get; set; }
        public List<Character> Characters { get; set; }
    }
}
namespace NetRPG.DTOs.Fight
{
    public class HighScoreDTO
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int Fights { get; set; }
        public int Victories { get; set; }
        public int Defeats { get; set; }
    }
}
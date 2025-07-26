namespace SimpleEcommerce_BackEnd.Models.Dtos.Talent
{
    public class TalentResponseDto
    {
        public Guid TalentID { get; set; }
        public required string TalentName { get; set; }
        public required string TalentGeneration { get; set; }
    }
}
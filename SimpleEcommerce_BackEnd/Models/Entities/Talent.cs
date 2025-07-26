namespace SimpleEcommerce_BackEnd.Models.Entities
{
    public class Talent
    {
        public Guid TalentID { get; set; }
        public required string TalentName { get; set; }
        public required string TalentGeneration { get; set; }

        public ICollection<Product> Products { get; set; } = new List<Product>();
        //Cú pháp rút gọn: public ICollection<Product> Products { get; set; } = [];
    }
}
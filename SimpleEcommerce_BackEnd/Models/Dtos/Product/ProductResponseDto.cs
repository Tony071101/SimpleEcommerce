namespace SimpleEcommerce.Models.Dtos.Product
{
    public class ProductResponseDto
    {
        public Guid ProductID { get; set; } // ID sẽ có khi trả về
        public required string ProductName { get; set; }
        public string? ProductDescription { get; set; }
        public decimal ProductPrice { get; set; }
        public int ProductStock { get; set; }
        public string? ProductImageUrl { get; set; }
        
        public Guid CategoryID { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public Guid TalentID { get; set; }
        public string TalentName { get; set; } = string.Empty;
    }
}
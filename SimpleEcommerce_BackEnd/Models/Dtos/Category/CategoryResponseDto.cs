namespace SimpleEcommerce_BackEnd.Models.Dtos.Category
{
    public class CategoryResponseDto
    {
        public Guid CategoryID { get; set; } // Bao gồm CategoryID cho phản hồi
        public required string CategoryName { get; set; }
        public string? Description { get; set; }
    }
}
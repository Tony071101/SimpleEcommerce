using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce_BackEnd.Models.Dtos.Product
{
    public class CreateProductRequestDto
    {
        [Required(ErrorMessage = "Tên sản phẩm là bắt buộc.")]
        [StringLength(200, ErrorMessage = "Tên sản phẩm không được vượt quá 200 ký tự.")]
        public string ProductName { get; set; } = string.Empty;

        public string? ProductDescription { get; set; }

        [Required(ErrorMessage = "Giá sản phẩm là bắt buộc.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Giá sản phẩm phải lớn hơn 0.")]
        public decimal ProductPrice { get; set; }

        [Required(ErrorMessage = "Số lượng tồn kho là bắt buộc.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng tồn kho không hợp lệ.")]
        public int ProductStock { get; set; }

        public string? ProductImageUrl { get; set; }

        [Required(ErrorMessage = "ID danh mục là bắt buộc.")]
        public Guid CategoryID { get; set; }

        [Required(ErrorMessage = "ID người tài là bắt buộc.")]
        public Guid TalentID { get; set; }
    }
}
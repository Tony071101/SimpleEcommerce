using System.ComponentModel.DataAnnotations;

namespace SimpleEcommerce_BackEnd.Models.Dtos.Talent
{
    public class CreateTalentDto
    {
        [Required(ErrorMessage = "Talent name is required.")]
        [StringLength(100, ErrorMessage = "Talent name cannot exceed 100 characters.")]
        public required string TalentName { get; set; }

        [Required(ErrorMessage = "Talent Generation is required.")]
        [StringLength(500, ErrorMessage = "Talent Generation cannot exceed 100 characters.")]
        public required string TalentGeneration { get; set; }
    }
}
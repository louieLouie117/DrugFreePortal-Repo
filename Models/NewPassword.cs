using System.ComponentModel.DataAnnotations;

namespace DrugFreePortal.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public required string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public required string ConfirmPassword { get; set; }

        public required string Token { get; set; }
    }

}
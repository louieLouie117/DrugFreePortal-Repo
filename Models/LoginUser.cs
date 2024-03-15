using System;
using System.ComponentModel.DataAnnotations;

namespace DrugFreePortal.Models
{
    public class LoginUser
    {
        // No other fields!
        public int UserId { get; set; }
        public string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        public string Password { get; set; }
    }
}
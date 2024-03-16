using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrugFreePortal.Models
{
    public enum AccountType { Admin = 0, Dean = 1, Student = 2, Evaluator = 3 }

    public enum SubscriptionStatus { Active = 1, Suspended = 2, Canceled = 3 }

    public class User
    {
        [Key]
        public int UserId { get; set; }

        [Column(TypeName = "nvarchar(24)")]
        [EnumDataType(typeof(AccountType))]
        public AccountType AccountType { get; set; }

        // boolean for checking in 
        public bool CheckedIn { get; set; }

        public required string School { get; set; }

        public required string StudentId { get; set; }


        [Display(Name = "First Name")]
        [MinLength(2, ErrorMessage = "First name is too short")]
        public required string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [MinLength(2, ErrorMessage = "Last name is too short")]
        public required string LastName { get; set; }

        [EmailAddress]
        [Required]
        public required string Email { get; set; }

        [DataType(DataType.Password)]
        [Required]
        [MinLength(8, ErrorMessage = "Password must be 8 characters or longer!")]
        public required string Password { get; set; }

        public required string PhoneNumber { get; set; }

        public required string StripeCustomerId { get; set; }

        public required bool AcceptedTerms { get; set; }



        [Column(TypeName = "nvarchar(24)")]
        [EnumDataType(typeof(SubscriptionStatus))]
        public SubscriptionStatus SubscriptionStatus { get; set; }

        // [Display(Name = "Profile Picture")]
        // public string ProfilePic { get; set; }

        // [NotMapped]
        // public IFormFile files { get; set; }

        public required string ReleaseVersion { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;


    }
}
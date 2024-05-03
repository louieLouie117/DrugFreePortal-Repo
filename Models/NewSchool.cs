using System.ComponentModel.DataAnnotations;

namespace DrugFreePortal.Models
{
    public class NewSchool
    {
        [Key]
        public int SchoolId { get; set; }

        public required string Dean { get; set; }
        public required string Name { get; set; }

        public required string Address { get; set; }

        public required string City { get; set; }

        public required string State { get; set; }

        public required string ZipCode { get; set; }

        public required string OnSiteDate { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;



    }
}
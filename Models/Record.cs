using System.ComponentModel.DataAnnotations;

namespace DrugFreePortal.Models
{
    public class Record
    {
        [Key]
        public int RecordId { get; set; }

        public required int UserId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string StudentId { get; set; }

        public required string SchoolName { get; set; }

        public required int SchoolId { get; set; }

        public required string ComplicationType { get; set; }

        public required string Status { get; set; }

        public required string StatusColor { get; set; }

        public required string Semester { get; set; }

        public required string Notes { get; set; }


        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public DateTime UpdatedAt { get; set; } = DateTime.Now;





    }
}
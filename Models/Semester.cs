using System.ComponentModel.DataAnnotations;

namespace DrugFreePortal.Models
{
    public class Semester
    {
        [Key]
        public int SemesterId { get; set; }

        public required string Title { get; set; }
        public required string Tracker { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;



    }
}
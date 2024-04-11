
using System.ComponentModel.DataAnnotations;

namespace DrugFreePortal.Models
{
    public class ComplianceType
    {
        [Key]
        public int ComplianceTypeId { get; set; }
        public required string Name { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;



    }
}
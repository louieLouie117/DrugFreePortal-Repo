using System.ComponentModel.DataAnnotations;

namespace DrugFreePortal.Models
{
    public class Queue
    {
        [Key]
        public int QueueId { get; set; }

        public required string SchoolId { get; set; }

        public required string SchoolName { get; set; }

        public required string StudentId { get; set; }

        public required string StudentUserId { get; set; }

        public required string FirstName { get; set; }

        public required string LastName { get; set; }

        public required string Email { get; set; }

        public required string PhoneNumber { get; set; }


    }
}
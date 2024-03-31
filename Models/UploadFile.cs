
using System.ComponentModel.DataAnnotations;



namespace DrugFreePortal.Models
{
    public class UploadFile
    {
        [Key]
        public int UoloadFileId { get; set; }
        public required string FileName { get; set; }
        public required string FilePath { get; set; }
        public required string FileType { get; set; }
        public long FileSize { get; set; }

        // [NotMapped]
        public required IFormFile files { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;

        // one to many relationship key to the user
        public int UserId { get; set; }
        // navP
        public User? User { get; set; }


    }
}
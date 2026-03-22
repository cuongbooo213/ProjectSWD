using SWD_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD_Project.Models.Entities
{
    
    public class Request
    {
        public int Id { get; set; }

        public int MenteeId { get; set; }

        [Required]
        public string Title { get; set; }

        public string Content { get; set; }

        public RequestStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("MenteeId")]
        public User Mentee { get; set; }
        public int? MentorId { get; set; }

        [ForeignKey("MentorId")]
        public User Mentor { get; set; }
    }
}

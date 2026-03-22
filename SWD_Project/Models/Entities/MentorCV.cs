using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD_Project.Models.Entities
{
    public class MentorCV
    {
        public int Id { get; set; }

        public int MentorId { get; set; }

        public string Bio { get; set; }

        public string Experience { get; set; }

        public ICollection<Skill> Skills { get; set; }

        [ForeignKey("MentorId")]
        public User Mentor { get; set; }
    }
}

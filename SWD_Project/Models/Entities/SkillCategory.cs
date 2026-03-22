using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SWD_Project.Models.Entities
{
    public class SkillCategory
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        public string Description { get; set; }

        public ICollection<Skill> Skills { get; set; }
    }
}

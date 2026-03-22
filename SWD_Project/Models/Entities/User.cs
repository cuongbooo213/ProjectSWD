using SWD_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;

namespace SWD_Project.Models.Entities
{
    

    public class User
    {
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        public string PasswordHash { get; set; }

        public Role Role { get; set; }

        public string FullName { get; set; }

        public string Email { get; set; }

        public bool IsBlocked { get; set; } = false;

        public ICollection<Request> Requests { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD_Project.Models.Entities
{
    public class ChatBotLog
    {
        public int Id { get; set; }

        public int? UserId { get; set; }

        public string UserPrompt { get; set; }

        public string GeminiResponse { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

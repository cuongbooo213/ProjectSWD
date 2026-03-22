using SWD_Project.Models.Enums;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SWD_Project.Models.Entities
{
    public class Transaction
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        public decimal Amount { get; set; }

        public string VnPayTranId { get; set; }

        public TransactionStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        [ForeignKey("UserId")]
        public User User { get; set; }
    }
}

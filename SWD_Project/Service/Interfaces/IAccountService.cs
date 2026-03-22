using SWD_Project.Models.Entities;
using System.Threading.Tasks;

namespace SWD_Project.Service.Interfaces
{
    public interface IAccountService
    {
        Task<User> AuthenticateAsync(string username, string password);
        Task<bool> RegisterAsync(User user, string password);
        bool IsUsernameDuplicate(string username);
        bool IsEmailDuplicate(string email);
    }
}

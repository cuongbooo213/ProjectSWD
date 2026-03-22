using System;
using Microsoft.EntityFrameworkCore;
using SWD_Project.Data;
using SWD_Project.Models.Entities;
using SWD_Project.Service.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace SWD_Project.Service.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly ApplicationDbContext _context;

        public AccountService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<User> AuthenticateAsync(string username, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Username == username);
            if (user == null) return null;

            if (VerifyPassword(password, user.PasswordHash))
            {
                return user;
            }
            return null;
        }

        public async Task<bool> RegisterAsync(User user, string password)
        {
            try
            {
                user.PasswordHash = HashPassword(password);
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool IsUsernameDuplicate(string username)
        {
            return _context.Users.Any(u => u.Username == username);
        }

        public bool IsEmailDuplicate(string email)
        {
            return _context.Users.Any(u => u.Email == email);
        }

        private string HashPassword(string password)
        {
            // Simple SHA256 for demo purposes. BCrypt is recommended in production.
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        private bool VerifyPassword(string password, string hash)
        {
            // The DB currently has "123456" as unhashed password from seed data,
            // so we support direct match for seed data, otherwise hash comparison.
            if (hash == password) return true;
            return HashPassword(password) == hash;
        }
    }
}

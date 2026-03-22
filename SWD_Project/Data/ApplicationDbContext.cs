using Microsoft.EntityFrameworkCore;
using SWD_Project.Models.Entities;

namespace SWD_Project.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<MentorCV> MentorCVs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Skill> Skills { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Relationship
            modelBuilder.Entity<Request>()
                .HasOne(r => r.Mentee)
                .WithMany(u => u.Requests)
                .HasForeignKey(r => r.MenteeId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Request>()
                .HasOne(r => r.Mentor)
                .WithMany()
                .HasForeignKey(r => r.MentorId)
                .OnDelete(DeleteBehavior.Restrict);

            // ===== SEED USERS =====
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    Id = 1,
                    Username = "mentee1",
                    PasswordHash = "123456",
                    FullName = "Nguyen Van A",
                    Role = Role.Mentee,
                    Email = "mentee1@gmail.com"
                },
                new User
                {
                    Id = 2,
                    Username = "mentor1",
                    PasswordHash = "123456",
                    FullName = "Tran Van B",
                    Role = Role.Mentor,
                    Email = "mentor@gmail.com"
                }
            );

            // ===== SEED REQUEST =====
            modelBuilder.Entity<Request>().HasData(
                new Request
                {
                    Id = 1,
                    MenteeId = 1,
                    MentorId = 2,
                    Title = "Learn ASP.NET Core",
                    Content = "I want to learn MVC",
                    Status = RequestStatus.Pending,
                    CreatedAt = DateTime.Now
                },
                new Request
                {
                    Id = 2,
                    MenteeId = 1,
                    MentorId = 2,
                    Title = "Learn React",
                    Content = "Need frontend mentor",
                    Status = RequestStatus.Accepted,
                    CreatedAt = DateTime.Now
                },
                new Request
                {
                    Id = 3,
                    MenteeId = 1,
                    MentorId = 2,
                    Title = "Learn SQL",
                    Content = "Database optimization",
                    Status = RequestStatus.Completed,
                    CreatedAt = DateTime.Now
                }
            );

            // ===== SEED MENTOR CV =====
            modelBuilder.Entity<MentorCV>().HasData(
                new MentorCV
                {
                    Id = 1,
                    MentorId = 2,
                    Bio = "Senior .NET Developer with 5 years of experience.",
                    Experience = "FPT Software (2020-2023), VNG (2023-Present)"
                }
            );

            // ===== SEED CATEGORY & SKILLS =====
            modelBuilder.Entity<Category>().HasData(
                new Category { Id = 1, Name = "Programming Languages", Description = "Core programming languages" },
                new Category { Id = 2, Name = "Frameworks", Description = "Web and App Frameworks" },
                new Category { Id = 3, Name = "Databases", Description = "Relational and NoSQL DBs" }
            );

            modelBuilder.Entity<Skill>().HasData(
                new Skill { Id = 1, Name = "C#", CategoryId = 1 },
                new Skill { Id = 2, Name = "Java", CategoryId = 1 },
                new Skill { Id = 3, Name = "ASP.NET Core", CategoryId = 2 },
                new Skill { Id = 4, Name = "React", CategoryId = 2 },
                new Skill { Id = 5, Name = "SQL Server", CategoryId = 3 }
            );

            // Seed Many-to-Many
            modelBuilder.Entity("MentorCVSkill").HasData(
                new { MentorCVsId = 1, SkillsId = 1 },
                new { MentorCVsId = 1, SkillsId = 3 },
                new { MentorCVsId = 1, SkillsId = 5 }
            );
        }
    }
}
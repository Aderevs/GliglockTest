using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using System.Diagnostics;

namespace GliglockTest.DbLogic
{
    public class TestsDbContext : DbContext
    {
        private readonly string _connectionString;

        public TestsDbContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        public DbSet<Student> Students { get; set; }
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Test> Tests { get; set; }
        public DbSet<PassedTest> PassedTests { get; set; }
        public DbSet<Question> Questions { get; set; }
        public DbSet<AnswerOption> AnswerOptions { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<Student>()
                .HasMany(s => s.PassedTests)
                .WithOne(pt => pt.Student)
                .HasForeignKey(pt => pt.StudentId);

            modelBuilder.Entity<Teacher>()
                .HasMany(t => t.Tests)
                .WithOne(t => t.Teacher)
                .HasForeignKey(t => t.TeacherId);

            modelBuilder.Entity<Test>()
                .HasMany(t => t.Questions)
                .WithOne(q => q.Test)
                .HasForeignKey(q => q.TestId);

            modelBuilder.Entity<Question>()
                .HasMany(q => q.AnswerOptions)
                .WithOne(ao => ao.Question)
                .HasForeignKey(ao => ao.QuestionId);

            modelBuilder.Entity<Test>()
                .HasMany(t => t.Results)
                .WithOne(r => r.Test)
                .HasForeignKey(r => r.TestId);

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder
                .UseSqlServer(_connectionString)
                .LogTo(e => Debug.WriteLine(e));
        }
    }
}

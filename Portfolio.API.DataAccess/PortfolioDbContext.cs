using Microsoft.EntityFrameworkCore;
using Portfolio.API.DataAccess.Entities;
using Portfolio.API.DataAccess.EntityConfigurations;

namespace Portfolio.API.DataAccess
{
    public class PortfolioDbContext(DbContextOptions options) : DbContext(options)
    {
        public DbSet<ApiTracker> ApiTrackers { get; set; }

        public DbSet<Password> Passwords { get; set; }

        public DbSet<CurriculumVitae> CurriculumVitae { get; set; }

        public DbSet<Link> Links { get; set; }

        public DbSet<WorkExperience> WorkExperiences { get; set; }

        public DbSet<PersonalProject> PersonalProjects { get; set; }

        public DbSet<Education> EducationHistory { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            new ApiTrackerConfiguration(Database).Configure(modelBuilder.Entity<ApiTracker>());
            new CurriculumVitaeConfiguration().Configure(modelBuilder.Entity<CurriculumVitae>());
            new WorkExperienceConfiguration().Configure(modelBuilder.Entity<WorkExperience>());
        }
    }
}

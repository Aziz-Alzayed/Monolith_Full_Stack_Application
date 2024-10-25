using FSTD.DataCore.Models.ProductivityModels;
using FSTD.DataCore.Models.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Configuration;

namespace FSTD.DataCore.Models
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
    {
        public DbContextOptions<ApplicationDbContext> Options { get; }
        public DbSet<TasksModel> Tasks { get; set; }
        public ApplicationDbContext()
        {
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
            Options = options;
            if (Database.IsRelational())
            {
                this.Database.SetCommandTimeout(180); // Only apply this for relational databases
            }
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<TasksModel>(entity =>
            {
                entity.ToTable("Tasks", schema: "productivity");

                // Generate the GUID automatically when inserting
                entity.Property(e => e.Id)
                      .ValueGeneratedOnAdd()
                      .HasDefaultValueSql("NEWID()");

                // Set maximum length for Name and Description
                entity.Property(e => e.Name)
                    .HasMaxLength(100)
                    .IsRequired();

                entity.Property(e => e.Description)
                    .HasMaxLength(500);

                // Configure the one-to-many relationship with ApplicationUser
                entity.HasOne(e => e.ApplicationUser)
                    .WithMany(u => u.Tasks) // ApplicationUser can have many Tasks
                    .HasForeignKey(e => e.ApplicationUserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });


        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var connString = ConfigurationManager.ConnectionStrings["ApplicationDbContext"]?.ConnectionString ?? "test";
                optionsBuilder.UseSqlServer(connString);
                optionsBuilder.EnableSensitiveDataLogging();
            }
        }
    }
}

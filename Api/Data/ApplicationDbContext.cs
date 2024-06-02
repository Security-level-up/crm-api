using Microsoft.EntityFrameworkCore;
using Models;

namespace Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<SalesOpportunity> SalesOpportunities { get; set; }
        public DbSet<PipelineStage> PipelineStages { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().ToTable("User");
            modelBuilder.Entity<Role>().ToTable("Role");
            modelBuilder.Entity<SalesOpportunity>().ToTable("SalesOpportunity");
            modelBuilder.Entity<PipelineStage>().ToTable("PipelineStage");

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.UserID);
                entity.HasOne(u => u.Role)
                      .WithMany(r => r.Users)
                      .HasForeignKey(u => u.RoleID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(r => r.RoleID);
                entity.HasMany(r => r.Users)
                      .WithOne(u => u.Role)
                      .HasForeignKey(u => u.RoleID)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<SalesOpportunity>(entity =>
            {
                entity.HasKey(so => so.OpportunityID);
                entity.HasOne(so => so.AssignedUser)
                      .WithMany()
                      .HasForeignKey(so => so.AssignedTo)
                      .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(so => so.PipelineStage)
                      .WithMany(ps => ps.SalesOpportunities)
                      .HasForeignKey(so => so.Stage)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<PipelineStage>(entity =>
            {
                entity.HasKey(ps => ps.StageID);
                entity.HasMany(ps => ps.SalesOpportunities)
                      .WithOne(so => so.PipelineStage)
                      .HasForeignKey(so => so.Stage)
                      .OnDelete(DeleteBehavior.Restrict);
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}

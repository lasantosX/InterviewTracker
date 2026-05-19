using Microsoft.EntityFrameworkCore;

using InterviewTracker.Domain.Models;

namespace InterviewTracker.Data;

public class InterviewTrackerDbContext : DbContext
{
    public InterviewTrackerDbContext(DbContextOptions<InterviewTrackerDbContext> options)
        : base(options)
    {
    }

    public DbSet<Company> Companies => Set<Company>();
    public DbSet<Recruiter> Recruiters => Set<Recruiter>();
    public DbSet<Interview> Interviews => Set<Interview>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Name)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Website)
                .HasMaxLength(300);

            entity.Property(x => x.Location)
                .HasMaxLength(150);
        });

        modelBuilder.Entity<Recruiter>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.FullName)
                .HasMaxLength(150)
                .IsRequired();

            entity.Property(x => x.Email)
                .HasMaxLength(200);

            entity.Property(x => x.LinkedInUrl)
                .HasMaxLength(300);
        });

        modelBuilder.Entity<Interview>(entity =>
        {
            entity.HasKey(x => x.Id);

            entity.Property(x => x.RoleTitle)
                .HasMaxLength(200)
                .IsRequired();

            entity.Property(x => x.Status)
                .HasMaxLength(50)
                .IsRequired();

            entity.Property(x => x.Notes)
                .HasMaxLength(2000);

            entity.Property(x => x.ExpectedSalary)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(x => x.Company)
                .WithMany(x => x.Interviews)
                .HasForeignKey(x => x.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Recruiter)
                .WithMany(x => x.Interviews)
                .HasForeignKey(x => x.RecruiterId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }
}
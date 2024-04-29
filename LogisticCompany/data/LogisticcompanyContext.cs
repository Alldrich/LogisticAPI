using LogisticCompany.models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace LogisticCompany.data;

public partial class LogisticcompanyContext : IdentityDbContext<IdentityUser>
{
    public LogisticcompanyContext()
    {
    }

    public LogisticcompanyContext(DbContextOptions<LogisticcompanyContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Company> Companies { get; set; }
    
    public virtual DbSet<Employee> Employees { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseNpgsql(
            $" Host={Environment.GetEnvironmentVariable("HOST")};" +
            $" Database={Environment.GetEnvironmentVariable("DATABASE")};" +
            $" Username={Environment.GetEnvironmentVariable("USER")};" +
            $" Password={Environment.GetEnvironmentVariable("PASSWORD")};" +
            $" SearchPath={Environment.GetEnvironmentVariable("SEARCHPATH")};");
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("company_pk");
        
            entity.ToTable("company", "logistic_company" , t => t.ExcludeFromMigrations());
        
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CreationDate)
                .HasDefaultValueSql("CURRENT_DATE")
                .HasColumnName("creation_date");
            entity.Property(e => e.Name)
                .HasMaxLength(90)
                .HasColumnName("name");
        });
        
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("employee_pk");
        
            entity.ToTable("employee", "logistic_company" , t => t.ExcludeFromMigrations());
        
            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.CompanyId).HasColumnName("company_id");
            entity.Property(e => e.Name)
                .HasMaxLength(30)
                .HasColumnName("name");
        
            entity.HasOne(d => d.Company).WithMany(p => p.Employees)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("employee_company_id_fk");
        });
        
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using DoCert.Model;
using Havit.Data.EntityFrameworkCore;
using Havit.Data.EntityFrameworkCore.Migrations.Internal;
using Microsoft.EntityFrameworkCore;


namespace DoCert.Entity;

public class DoCertDbContext(DbContextOptions options) : Havit.Data.EntityFrameworkCore.DbContext(options)
{
    public DbSet<Donor> Donors { get; init; }
    public DbSet<Donate> Donates { get; init; }
    public DbSet<Agenda> Agendas { get; init; }
    public DbSet<MailAccount> MailAccounts { get; init; }
    public DbSet<Certificate> Certificates { get; init; }

    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //base.OnConfiguring(optionsBuilder);
        //optionsBuilder.UseSqlite("Data Source=..\\Docert\\Assets\\docert.db");
    }
    
    protected override void ModelCreatingCompleting(ModelBuilder modelBuilder)
    {
        base.ModelCreatingCompleting(modelBuilder);
        modelBuilder.Entity<Certificate>()
            .HasOne(e => e.Donor)
            .WithOne(e => e.Certificate)
            .HasForeignKey<Donor>(e => e.CertificateId)
            .HasPrincipalKey<Certificate>(e => e.Id);
        
        modelBuilder.Entity<MailAccount>().Ignore(p => p.ClearPassword);
    }
}
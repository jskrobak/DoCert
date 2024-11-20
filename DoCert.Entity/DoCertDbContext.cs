using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;
using System.Reflection;
using DoCert.Model;
using Microsoft.EntityFrameworkCore;

namespace DoCert.Entity;

public class DoCertDbContext(DbContextOptions options) : Havit.Data.EntityFrameworkCore.DbContext(options)
{
    public DbSet<BankAccount> BankAccounts { get; init; }
    public DbSet<Donor> Donors { get; init; }
    public DbSet<Donate> Donates { get; init; }

    /*
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        //base.OnConfiguring(optionsBuilder);
        optionsBuilder.UseSqlite("Data Source=..\\..\\..\\data\\docert.db");
    }*/
}
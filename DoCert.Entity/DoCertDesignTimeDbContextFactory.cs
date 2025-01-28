using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace DoCert.Entity;

public class DoCertDesignTimeDbContextFactory:IDesignTimeDbContextFactory<DoCertDbContext>
{
    public DoCertDbContext CreateDbContext(string[] args)
    {
        var builder = new DbContextOptionsBuilder<DoCertDbContext>();
        builder.UseSqlite("Data Source=..\\Docert\\Assets\\docert.db");
        
        return new DoCertDbContext(builder.Options);
    }
}
using Microsoft.EntityFrameworkCore;
using PersonaApp.Models;

namespace PersonaApp.Data;

public class AppDbContext:DbContext
{
    public DbSet<Persona> Personas { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var connectionString = "";
        optionsBuilder.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString));
    }
}
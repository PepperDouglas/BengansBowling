using BengansBowling.Models;
using Microsoft.EntityFrameworkCore;

public class BowlingAlleyContext : DbContext
{
    public DbSet<Member> Members { get; set; }
    public DbSet<Competition> Competitions { get; set; }
    public DbSet<Track> Tracks { get; set; }
    public DbSet<Match> Matches { get; set; } // Assuming you have a Match model

    public BowlingAlleyContext() { }

    public BowlingAlleyContext(DbContextOptions<BowlingAlleyContext> options) : base(options) {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) {
        if (!optionsBuilder.IsConfigured) {
            optionsBuilder.UseSqlServer(@"Server=DESKTOP-HUERL9P;Database=BengansBowling;Trusted_Connection=True;TrustServerCertificate=True;");
        }
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        modelBuilder.Entity<Match>()
            .HasOne<Member>(m => m.PlayerOne)
            .WithMany()
            .HasForeignKey(m => m.PlayerOneId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        modelBuilder.Entity<Match>()
            .HasOne<Member>(m => m.PlayerTwo)
            .WithMany()
            .HasForeignKey(m => m.PlayerTwoId)
            .OnDelete(DeleteBehavior.Restrict); // Prevent cascading delete

        modelBuilder.Entity<Member>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedOnAdd(); // Explicitly setting Id as an identity column
        });
        
        modelBuilder.Entity<Competition>()
        .Property(c => c.Id)
        .ValueGeneratedOnAdd(); // This ensures Id is auto-generated


        // Other configurations...
    }


    // Use Fluent API configurations here if needed
}

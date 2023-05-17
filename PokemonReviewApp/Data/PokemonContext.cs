using Microsoft.EntityFrameworkCore;
using Pokemon_Review_System.Models;

namespace Pokemon_Review_System.Data
{
    public class PokemonContext : DbContext
    {
        public PokemonContext(DbContextOptions<PokemonContext> option ) : base(option)
        { }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<Owner> Owners { get; set; }
        public DbSet<Pokemon> Pokemon { get; set; }
        public DbSet<PokemonCategory> PokemonCategories { get; set; }
        public DbSet<PokemonOwner> PokemonOwners { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Reviewer> Reviewers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<PokemonCategory>()
                //en 3ando el PK Composite from PokemonId and CategoryId
                .HasKey(pc => new { pc.PokemonId, pc.CategoryId });

            modelBuilder.Entity<PokemonCategory>()
                     .HasOne(P => P.Pokemon)
                     .WithMany(PC => PC.PokemonCategories)
                     .HasForeignKey(C => C.CategoryId);
            modelBuilder.Entity<PokemonCategory>()
                        .HasOne(p => p.Category)
                        .WithMany(pc => pc.PokemonCategories)
                        .HasForeignKey(c => c.CategoryId);

            modelBuilder.Entity<PokemonOwner>()
      .HasKey(pc => new { pc.PokemonId, pc.OwnerId });

            modelBuilder.Entity<PokemonOwner>()
                     .HasOne(P => P.Pokemon)
                     .WithMany(PC => PC.PokemonOwners)
                     .HasForeignKey(C => C.OwnerId);
            modelBuilder.Entity<PokemonOwner>()
                        .HasOne(p => p.Owner)
                        .WithMany(pc => pc.PokemonOwners)
                        .HasForeignKey(c => c.OwnerId);
        }









    }
}

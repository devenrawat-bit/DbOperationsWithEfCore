using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEfCoreApp.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //data seeding for Currency table 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency { Id = 1, Title = "INR", Description = "Indian INR" },
                new Currency { Id = 2, Title = "Dollar", Description = "Dollar" },
                new Currency { Id = 3, Title = "Euro", Description = "Euro" },
                new Currency { Id = 4, Title = "Dinar", Description = "Dinar" }
                );
            //data seeding for the Language table

            modelBuilder.Entity<Language>().HasData(
                new Language { Id = 1, Title = "Hindi", Description = "Hindi" },
                new Language { Id = 2, Title = "Tamil", Description = "Tamil" },
                new Language { Id = 3, Title = "Punjabi", Description = "Punjabi" },
                new Language { Id = 4, Title = "Urdu", Description = "Urdu" }
                );
        }


        public DbSet<Book> Books { get; set; } //this books table will be created in the database
        public DbSet<Language> Languages { get; set; }
        public DbSet<BookPrice> BookPrices { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}

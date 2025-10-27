using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEfCoreApp.Data
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }
        //now creating a master table 
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Currency>().HasData(
                new Currency { id = 1, Title = "INR", description = "Indian INR" },
                new Currency { id = 2, Title = "Dollar", description = "Dollar" },
                new Currency { id = 3, Title = "Euro", description = "Euro" },
                new Currency { id = 4, Title = "Dinar", description = "Dinar" }
                );
        }
        public DbSet<Book> Books { get; set; } //this books table will be created in the database
        public DbSet<Language> Languages { get; set; }
        public DbSet<BookPrice> BookPrices { get; set; }
        public DbSet<Currency> Currencies { get; set; }
    }
}

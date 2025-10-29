namespace DbOperationsWithEfCoreApp.Data
{
    public class BookPrice
    {
        public int Id { get; set; }//primary key
        public int BookId { get; set; } //foreign key
        public int CurrencyId { get; set; } //foreign key
        public int Amount { get; set; } 

        public virtual Book Book { get; set; } //navigation property
        public virtual Currency Currency { get; set; } //navigation property
    }
}

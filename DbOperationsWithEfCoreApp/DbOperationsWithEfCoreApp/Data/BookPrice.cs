namespace DbOperationsWithEfCoreApp.Data
{
    public class BookPrice
    {
        public int id { get; set; }//primary key
        public int BookId { get; set; } //foreign key
        public int CurrencyId { get; set; } //foreign key
        public int Amount { get; set; } 

        public Book Book { get; set; } //navigation property
        public Currency Currency { get; set; } //navigation property
    }
}

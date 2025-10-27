namespace DbOperationsWithEfCoreApp.Data
{
    public class Currency
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public ICollection<BookPrice> BookPrices { get; set; } //one to many relationship, here the Currency is parent and BookPrice is child, because one currency can be used for multiple book prices
        //currency is one and the book prices are many
    }
}

namespace DbOperationsWithEfCoreApp.Data
{
    public class Author
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }


        //public int BookPriceId { get; set; }
        //remeber not to do this, as this will make a loop in the relationship the book has a foreign key to author and author has foreign key to bookprice and bookprice has foreign key to book this is creating a loop in the relationships the error will be thrown by ef core while updating the database 
        //public BookPrice? BookPrice { get; set; }

        //public int? CurrencyId { get; set; }
        //public Currency? Currency { get; set; }
    }
}


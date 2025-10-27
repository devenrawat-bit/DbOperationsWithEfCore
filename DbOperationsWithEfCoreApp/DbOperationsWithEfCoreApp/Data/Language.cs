namespace DbOperationsWithEfCoreApp.Data
{
    public class Language
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }

        public ICollection<Book> Books { get; set; } //one to many relationship
        //book here is many and language is one
        //one language can be used for multiple books
    }
}

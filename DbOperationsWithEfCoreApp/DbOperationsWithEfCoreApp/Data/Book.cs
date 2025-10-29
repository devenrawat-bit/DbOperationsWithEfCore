namespace DbOperationsWithEfCoreApp.Data
{
    public class Book
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public int NoOfPages { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedOn { get; set; }
        //creating a foreign key relationship with Language table
        public int LanguageId { get; set; } //this column will act as foreign key
        public virtual Language? Language { get; set; } //one to many relationship
        //ismein pehla Language (left side wala) type hai —mean class type
        //aur doosra Language (right side wala) property ka naam hai.
        public int AuthorId { get; set; } //foreign key property will be created in the book table
        public virtual Author? Author { get; set; } //navigation property
        //here the first one is the author class type and the second one is object of the author class
        //here the right side author is the property name
    }
}

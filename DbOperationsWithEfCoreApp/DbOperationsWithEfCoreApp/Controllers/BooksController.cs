using DbOperationsWithEfCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace DbOperationsWithEfCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController(AppDbContext appDbContext) : ControllerBase
    {
        //using raw sql 
        [HttpGet("rawSql")]
        public async Task<IActionResult> GetBooksUsingRawSqlAsync()
        {
            var books = await appDbContext.Books.FromSql($"SELECT top 1 * FROM Books").ToListAsync();//we can also use the linq extension method here like .where() for the filtration 
            return Ok(books);
            //here we are using the fromsqlraw method to execute the raw sql query and get the data from the database
            //this is useful when we want to execute complex queries that are not supported by linq or ef core
        }





        //lazy loading example
        [HttpGet("lazyLoading")]
        public async Task<IActionResult> GetBooksLazyLoadingAsync()
        {
            var books = await appDbContext.Books.FirstAsync();//first we get the books without the related data
            //now we will access the related data which will be loaded automatically by ef core because of lazy loading
            var authorName = books.Author?.Name; //accessing the author navigation property to trigger lazy loading
            var language = books.Language; //accessing the language navigation property to trigger lazy loading
            return Ok(books);
            //in lazy loading the book table data will first be loaded in a single query and then the author and language data will be loaded in separate queries for each book when we access the navigation property
            //the navigational properties should be virtual to use the lazy laoding 
        }









        //explicit loading example
        [HttpGet("explicitLoading")]
        public async Task<IActionResult> GetBooksExplicitLoadingAsync()
        {
            var books = await appDbContext.Books.FirstAsync();//first we get the books without the related data
            //now we will load the related data explicitly for each book
          
                //loading the author data explicitly
                await appDbContext.Entry(books).Reference(b => b.Author).LoadAsync();//reference method is used to load the reference navigation property
                //loading the language data explicitly
           //here the book table should have the foreign key relationship with the author table and language table
                await appDbContext.Entry(books).Reference(b => b.Language).LoadAsync();//reference method is used to load the reference navigation property 
            return Ok(books);
            //in explicit loading the book table data will first be loaded in a single query and then the author and language data will be loaded in separate queries for each book 
        }
        //the entry method always accepts a single entity object and then we can use the reference method to load the reference navigation property or collection method to load the collection navigation property






        //eager loading example we have to use the include method to include the related data
        //[HttpGet("eagerLoading")]
        //public async Task<IActionResult> GetBooksAsync()
        //{
        //    var result = await appDbContext.Books.Include(x => x.Author).ThenInclude(x => x.Currency).ToListAsync();//in include method we have to pass the navigation property name which is a class
        //    return Ok(result);
        //}
        //with the use of the eager loading we dont need to use the navigation property to get the related data  
        //the related data is the data that is present in the related table with the help of foreign key relationship like in the book table the related data is the author table and the language table
        //but here we still use the navigation property to include the related data in behind the scenes the ef core will create the join query to get the related data from the related table






        //to get a single book record with our custom response using anonyomous object
        [HttpGet("")]
        public async Task<IActionResult> GetBookAsync()
        {
            var result = await appDbContext.Books.Select(x => new
            {
                Title = x.Title,
                Description = x.Description,
                NoOfPages = x.NoOfPages,
                AuthorName = x.Author != null ? x.Author.Name : "NA" // Use a string property for author name
            }).ToListAsync();
            return Ok(result); 
        }
        [HttpPost("")]
        public async Task<IActionResult> AddNewBookAsync([FromBody] Book book)//book here is the Book class object and now we can access all the properties of the book class
        {

            //to hardcore the author data 
            //var author= new Author
            //{

            // Name = "Doe",
            // Email = "nextarget@testyopmail.com"
            //};
            //book.Author = author; //assigning the author object to the book's author property
            //after doing this when we insert a book record the author record will also be inserted automatically because of the relationship established between book and author through the navigation property


            //here also before inserting we can add the validation 
            //book.CreatedOn = DateTime.Now;
            appDbContext.Books.Add(book);//add here is the method 
            await appDbContext.SaveChangesAsync();//to save the changes to the database
            return Ok(book);//return the inserted book object as a response
        }

        //task to get the data 
        //[HttpGet("getData")]
        //public async Task<IActionResult> GetAllBooksAsync()
        //{
        //    var result = await (from books in appDbContext.Books
        //                        select books).ToListAsync();
        //    return Ok(result);
        //}



        //note the add method is used for only one record insertion to insert multiple records use addrange method
        //url is api/books/bulkAdd
        //and the rest is same as above
        [HttpPost("bulkAdd")]
        public async Task<IActionResult> AddBooksAsync([FromBody] List<Book> book)
        {
            appDbContext.Books.AddRange(book);//to add multiple records use addrangemethod
            await appDbContext.SaveChangesAsync();
            return Ok(book);
        }//if you use post then the attribute should be frombody otherwise it will give error



        //updating a book record
        //we require the route parameter to identify which book to update and the updated book object in the request body which contains the new values for the book properties

        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBookAsync([FromRoute] int id, [FromBody] Book updatedBook)
        {
            var result = await appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);//taking the value 
            if (result == null) //checking if the book with the given id exists
            {
                return NotFound($"Book with id {id} not found");
            }
            //here we are hitting the db for the first time to get the book record to be updated

            //updating the properties of the retrieved book record with the values from the updatedBook object
            result.Title = updatedBook.Title;
            result.Description = updatedBook.Description;
            result.NoOfPages = updatedBook.NoOfPages;
            //result.IsActive = updatedBook.IsActive;
            //result.LanguageId = updatedBook.LanguageId;
            //result.AuthorId = updatedBook.AuthorId;
            //these operations are in memory operations and not hitting in the database 


            await appDbContext.SaveChangesAsync();
            //here we are hitting the db for the second time to save the changes
            return Ok(result);
            //in this we are hitting the database two times which is not good the solution to this is below 

        }




        //updating a book record with single hit
        [HttpPut("")]
        public async Task<IActionResult> UpdateBookWithSingleHitAsync([FromBody] Book updatedBook)
        {
            appDbContext.Books.Update(updatedBook);//here this will not create a first hit to the database instead it will track the entity directly

            await appDbContext.SaveChangesAsync();//this will create the only one hit to the database to save the changes
            return Ok(updatedBook);
            //in this we are hitting the database only once which is good the drawback of this approach is we need to send all the properties of the book object even if we want to update only one property

        }




        //updating the books in bulk 
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBookInBulkAsync()
        {
            //await appDbContext.Books.ExecuteUpdateAsync(x=>x.SetProperty(p=>p.Title,"abdevillers").SetProperty(p=>p.NoOfPages,101).SetProperty(p=>p.Description,p=>p.Description+"dj bravo"));//to add the data to the existing one 


            //adding the where condition
            await appDbContext.Books.Where(x => x.NoOfPages > 100).ExecuteUpdateAsync(x => x.SetProperty(p => p.Title, "abdevillers is the best").SetProperty(p => p.NoOfPages, 101).SetProperty(p => p.Description, p => p.Description + "dj bravo"));//to add the data to the existing one 
            return Ok();
            //in this we are hitting the database only once 

        }



        //deleting operation
        //deleting the data in two hits of the database
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBookAsync([FromRoute] int id)
        {
            var book = await appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);//the first hit to find the book
            if (book == null)
            {
                return NotFound($"Book with id {id} not found");
            }
            appDbContext.Books.Remove(book);//remove is the method to delete the record

            await appDbContext.SaveChangesAsync();//second hit to delete the book
            return Ok($"Book with id {id} deleted successfully");
            //this will hit the database two times first to find and then to delete

        }



        //now deleting with single hit
        [HttpDelete("singleHit/{id:int}")]
        public async Task<IActionResult> DeleteBookWithSingleHitAsync([FromRoute] int id)
        {
            //if (!await appDbContext.Books.AnyAsync(x => x.Id == id))
            //  {
            //      return NotFound($"Book with id {id} not found");
            //  } to check either the data exist or not before deleting but this will again hit the database once more so we can skip this check

            await appDbContext.Books.Where(x => x.Id == id).ExecuteDeleteAsync();
            return Ok($"Book with id {id} deleted successfully");
            //this will hit the database only once to delete the record
        }




        //now for bulk delete in single hit
        [HttpDelete("bulkDelete/{id:int}")]
        public async Task<IActionResult> BulkDeleteBooksAsync([FromRoute] int id)
        {
            await appDbContext.Books.Where(x => x.Id < id).ExecuteDeleteAsync();
            //this method is used where we want to delete multiple records based on a condition and it is more effecient than the metod removerange as remove range delete each record individually through ef tracking 
            //It executes(executedeleteasync) one SQL DELETE statement for all matching rows.

            return Ok($"Books with Id less than {id} deleted successfully");
            //this will hit the database only once to delete the records

            //ExecutedeleteAsync(); is available from EF Core 7.0 onwards and it can delete the data in bulk or which ever the mathcing condition is either its only one data matching with the condition of the where with single hit to the database
        }
    }
}

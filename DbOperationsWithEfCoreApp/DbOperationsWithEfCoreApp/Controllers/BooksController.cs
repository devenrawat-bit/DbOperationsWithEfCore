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
        [HttpPost("")]
        public async Task<IActionResult> AddNewBookAsync([FromBody] Book book)//book here is the class object and now we can access all the properties of the book class
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
            var result = await appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (result == null)
            {
                return NotFound($"Book with id {id} not found");
            }
            //here we are hitting the db for the first time to get the book record to be updated

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
            var book = await appDbContext.Books.FirstOrDefaultAsync(x => x.Id == id);
            if (book == null)
            {
                return NotFound($"Book with id {id} not found");
            }
            appDbContext.Books.Remove(book);
            await appDbContext.SaveChangesAsync();
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
        [HttpDelete("bulkDelete/{pages:int}")]
        public async Task<IActionResult> BulkDeleteBooksAsync([FromRoute] int pages)
        {
            await appDbContext.Books.Where(x => x.NoOfPages < pages).ExecuteDeleteAsync();
            return Ok($"Books with pages less than {pages} deleted successfully");
            //this will hit the database only once to delete the records
        }
    }
}

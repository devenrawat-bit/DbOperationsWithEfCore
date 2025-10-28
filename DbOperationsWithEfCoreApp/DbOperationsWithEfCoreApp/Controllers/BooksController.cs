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

            result.Title = updatedBook.Title;
            result.Description = updatedBook.Description;
            result.NoOfPages = updatedBook.NoOfPages;
            //result.IsActive = updatedBook.IsActive;
            //result.LanguageId = updatedBook.LanguageId;
            //result.AuthorId = updatedBook.AuthorId;


            await appDbContext.SaveChangesAsync();
            return Ok(result);
            //in this we are hitting the database two times which is not good the solution to this is below 

        }




        //updating a book record with single hit
        [HttpPut("")]
        public async Task<IActionResult> UpdateBookWithSingleHitAsync([FromBody] Book updatedBook)
        {
            appDbContext.Books.Update(updatedBook);

            await appDbContext.SaveChangesAsync();
            return Ok(updatedBook);
            //in this we are hitting the database two times which is not good the solution to this is below 

        }



        //updating the books in bulk 
        [HttpPut("bulk")]
        public async Task<IActionResult> UpdateBookInBulkAsync()
        {
            //await appDbContext.Books.ExecuteUpdateAsync(x=>x.SetProperty(p=>p.Title,"abdevillers").SetProperty(p=>p.NoOfPages,101).SetProperty(p=>p.Description,p=>p.Description+"dj bravo"));//to add the data to the existing one 


            //adding the where condition
            await appDbContext.Books.Where(x => x.NoOfPages > 100).ExecuteUpdateAsync(x => x.SetProperty(p => p.Title, "abdevillers is the best").SetProperty(p => p.NoOfPages, 101).SetProperty(p => p.Description, p => p.Description + "dj bravo"));//to add the data to the existing one 

            await appDbContext.SaveChangesAsync();
            return Ok();
            //in this we are hitting the database two times which is not good the solution to this is below 

        }


    }
}

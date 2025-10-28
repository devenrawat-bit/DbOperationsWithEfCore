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
        public async Task<IActionResult> AddBooksAsync([FromBody] List<Book> book )
        {
            appDbContext.Books.AddRange(book);//to add multiple records use addrangemethod
            await appDbContext.SaveChangesAsync();
          return Ok(book);
        }//if you use post then the attribute should be frombody otherwise it will give error
    }
}

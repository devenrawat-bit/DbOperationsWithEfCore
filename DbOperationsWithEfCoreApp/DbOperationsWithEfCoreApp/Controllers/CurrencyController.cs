using DbOperationsWithEfCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace DbOperationsWithEfCoreApp.Controllers
{
    [Route("api/Currencies")]
    [ApiController]
    public class CurrencyController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public CurrencyController(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
            //the right one is of the constructor parameter and the left one is of the private field
        }
        //creating an action method to get all the currencies from the database
        [HttpGet("")]
        public async Task<IActionResult> GetAllCurrencies()//use the async keyword while dealing with database operations always
        {
            //var result = _appDbContext.Currencies.ToList();//to list here is the part of the linq and is the extension method, this is the mehtod syntax

            //var result = from currencies in _appDbContext.Currencies
            //             select currencies; or we can do like below


            //var result = (from currencies in _appDbContext.Currencies
            //             select currencies).ToList();
            //this is the query syntax


            var result = await (from currencies in _appDbContext.Currencies
                                select currencies).ToListAsync(); //combining both query and method syntax
            //return Ok(result.ToList()); //200 OK status code
            return Ok(result);
        }

        //to get the data via id(primary key)
        [HttpGet("{id:int}")]//if both the action method route is same then the swagger will give ambiguity error to fix this we will define the data type in the route 
        public async Task<IActionResult> GetAllCurrenciesByIdAsync([FromRoute] int id)//use the async keyword while dealing with database operations always
        {
            //var result=await _appDbContext.Currencies.FindAsync(2); //instead of hardcoding use id and the FromRoute attribute
            var result = await _appDbContext.Currencies.FindAsync(id);
            return Ok(result);
        }

        //if you dont have a primary key then how to get the data 
        //[HttpGet("{name}")]
        //public async Task<IActionResult> GetAllCurrenciesByNameAsync([FromRoute] string name)//use the async keyword while dealing with database operations always
        //{
        //    //var result=await _appDbContext.Currencies.FindAsync(2); //instead of hardcoding use id and the FromRoute attribute
        //    //var result = await _appDbContext.Currencies.Where(x => x.Title == name).FirstOrDefaultAsync();
        //    //to increase the performance we will use the linq in the firstordefault async
        //    var result = await _appDbContext.Currencies.FirstOrDefaultAsync(x => x.Title == name);

        //    return Ok(result);
        //}



        //getting one record using multiple parameter (route)
        //[HttpGet("{name}/{description}")]
        //public async Task<IActionResult> GetAllCurrenciesByNameAsync([FromRoute] string name, [FromRoute] string description)//use the async keyword while dealing with database operations always
        //{
        //    var result = await _appDbContext.Currencies.FirstOrDefaultAsync(x => x.Title == name && x.Description == description);
        //    return Ok(result);
        //}
        //means:
        //👉 “Go to the Currencies table, and find the first record where the Title column matches the name value from the route AND the Description column matches the description value from the route.”
        //If EF finds it → it returns that record.
        //If it doesn’t find any matching record → it returns null



        //now to get multiple records using a condition
        [HttpGet("{name}/{data}")] //here the name and the data should match the below method parameter always when the route parameter is variable and not fixed only then it will work if its fixed then we can give any name
        public async Task<IActionResult> GetAllCurrenciesByNameAsync([FromRoute] string name, [FromRoute] string data)//use the async keyword while dealing with database operations always
        {
            var result = await _appDbContext.Currencies.Where(x => x.Title == name && x.Description == data).ToListAsync();
            return Ok(result);
        }



        //getting multiple records using a list of ids
        //[HttpGet("all")]
        //public async Task<IActionResult> GetAllCurrenciesUsingToListAsync()
        //{
        //    var ids =new List<int> {1,2,3};  
        //    var result = await _appDbContext.Currencies.Where(x=> ids.Contains(x.Id)).ToListAsync();
        //    return Ok(result);
        //}

        //to get data dynamically using query string
        //[HttpPost("all")]
        //public async Task<IActionResult> GetAllCurrenciesUsingToListAsync([FromBody] List<int> ids)
        //{
        //    //var ids =new List<int> {1,2,3};  
        //    var result = await _appDbContext.Currencies.Where(x=> ids.Contains(x.Id)).ToListAsync();
        //    return Ok(result);
        //}



        //now selecting a specific column 
        [HttpPost("all")]
        public async Task<IActionResult> GetAllCurrenciesUsingToListAsync([FromBody] List<int> ids)
        {
            //var ids =new List<int> {1,2,3};  
            var result = await _appDbContext.Currencies.Where(x => ids.Contains(x.Id)).Select(
                //x => new Currency
                //{
                //    Id = x.Id,
                //    Title = x.Title
                //}).ToListAsync();

            //if there is anonyomous type required then we can do like below
            x => new 
                {
                    yoId = x.Id, //the answer will be in the yoid =1, yotitle=inr like this the one you defined here but if you use the class name then you can not use the yoid yotitle here you have to use the property that are defined in that or the column name defined in the database 
                    yoTitle = x.Title
                }).ToListAsync();
            //if there is no where statement then we can also use the table name directly like _appDbContext.Currencies.Select
            //id=currencies.id and title=currencies.title like this also works 
            return Ok(result);
        }
    }
}

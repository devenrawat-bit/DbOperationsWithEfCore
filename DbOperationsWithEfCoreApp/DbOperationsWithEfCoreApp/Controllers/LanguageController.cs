using DbOperationsWithEfCoreApp.Data;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace DbOperationsWithEfCoreApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LanguageController : ControllerBase
    {
        private readonly AppDbContext _appDbContext;
        public LanguageController(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;
        }
        [HttpGet("")]
        public async Task<IActionResult> GetAllLanguages()
        //now our application can handle multiple requests simultaneously without blocking the main thread
        {//simultaneous means at the same time 

            var result = await(from languages in _appDbContext.Languages
                          select languages).ToListAsync(); //query and method syntax combined
            return Ok(result);
        }
    }
}

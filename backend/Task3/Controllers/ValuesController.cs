using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Task3.Models;

namespace Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyDbContext _db;
        private readonly ILogger<ValuesController> _logger;


        public ValuesController (MyDbContext db , ILogger<ValuesController>  logger) 
        {
            _db = db;
            _logger = logger;
        }
        // Get all products
        [HttpGet("All")]
        public IActionResult GetAllProducts()
        {
            var data = _db.Products.ToList();
            _logger.LogInformation("esraa  {jjj}",data.Count);

            return Ok(data);

        }
    }
}

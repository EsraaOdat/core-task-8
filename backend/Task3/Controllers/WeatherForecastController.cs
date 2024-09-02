using Microsoft.AspNetCore.Mvc;

namespace Task3.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        }; 

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "GetWeatherForecast")]
        public IEnumerable<WeatherForecast> Get()
        {
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                TemperatureC = Random.Shared.Next(-20, 55),
                Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            })
            .ToArray();
        }

        /*  [HttpGet("Sum/{expression}")]
          public IActionResult SumNumbers(string expression)
          {
              var parts = expression.Split(' ');
              var x = (parts[1]);

              // Your existing logic
              if (parts.Length == 2)
              {
                  int number1 = int.Parse(parts[0]);
                  int number2 = int.Parse(parts[2]);

                  int result = number1 + number2;

                  return Ok(result.ToString());
              }
              else
              {
                  return BadRequest("Invalid expression format. Please use the format 'number+number'.");
              }
          }*/
        // Example: GET api/math/check?num1=20&num2=10
        [HttpGet("check")]
        public IActionResult CheckIntegers([FromQuery] int num1, [FromQuery] int num2)
        {
            bool result = (num1 == 30 || num2 == 30 || (num1 + num2) == 30);
            return Ok(result);
        }

        [HttpGet("checkMultiple")]
        public IActionResult CheckMultiple([FromQuery] int number)
        {
            if (number <= 0)
            {
                return BadRequest("The number must be positive.");
            }

            bool result = (number % 3 == 0 || number % 7 == 0);
            return Ok(new { rusult = result});
        }

    }


}


using Task3.Models;

namespace Task3.DTOs
{
    public class ProductRequestDTO
    {



        public string ProductName { get; set; } = null!;

        public string? Description { get; set; }

        public decimal Price { get; set; }

        public int? CategoryId { get; set; }

        public IFormFile? Image { get; set; }


       










    }
}

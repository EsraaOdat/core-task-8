using Task3.Models;

namespace Task3.DTOs
{
    public class CartItemsResponsive
    {
        public int CartItemId { get; set; }

        public int? CartId { get; set; }


        public int Quantity { get; set; }

        public  ProductDto Product { get; set; }
    
}



    public class ProductDto
    {

        public string ProductName { get; set; } = null!;

        public string Image { get; set; }

        public decimal Price { get; set; }


    }

}

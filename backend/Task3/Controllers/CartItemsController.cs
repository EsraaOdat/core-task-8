using Microsoft.AspNetCore.Mvc;
using System.Reflection.Metadata.Ecma335;
using Task3.DTOs;
using Task3.Models;

namespace Task3.Controllers
{
    public class CartItemsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        private readonly MyDbContext _db;

        public CartItemsController(MyDbContext db)
        {
            _db = db;
        }

        // Get all Items

        // Get all Cart Items
        [HttpGet]
        [Route("AllItems")]
        public IActionResult GetAllCartItems()
        {
            var CartItems = _db.CartItems.Select(x =>
            
            new CartItemsResponsive
            {
                CartId = x.CartId,
                CartItemId = x.CartItemId,
                Quantity = x.Quantity,
                Product = new ProductDto
                {
                    ProductName = x.Product.ProductName, 
                    Price = x.Product.Price ,
                    Image=x.Product.Image
                }
            });

            return Ok(CartItems);
        }


        [HttpPost]
        [Route("AddItem")]
        public IActionResult AddCartItem([FromBody] AddCartItemsDto newItem)
        {
            

            // Create a new cart item
            var cartItem = new CartItem
            {
                CartId = newItem.CartId,
                ProductId = newItem.ProductId,
                Quantity = newItem.Quantity
            };

            // Add the cart item to the database
            _db.CartItems.Add(cartItem);
            _db.SaveChanges();

            return Ok(new { Message = "Item added to cart successfully.", CartItem = cartItem });
        }







        [HttpPut("{id}")]
        public IActionResult UpdateCartItem(int id, [FromBody] UpdateCartController cartItem)
        {
            var existingCart = _db.CartItems.Find(id);
            if (existingCart == null)
            {
                return NotFound(new { message = "Cart not found" });
            }

            existingCart.CartId = cartItem.CartId ?? existingCart.CartId;
            existingCart.Quantity = cartItem.Quantity;


            _db.CartItems.Update(existingCart);
            _db.SaveChanges();

            return Ok(new { message = "Cart updated successfully", CartItem = existingCart });
        }



        [HttpDelete("{cartItemId}")]
        public IActionResult DeleteCartItem(int cartItemId)
        {
            // Assuming the correct property name is 'CartItemId' in your CartItem entity class
            var cartItemToDelete = _db.CartItems.FirstOrDefault(x => x.CartItemId == cartItemId);

            if (cartItemToDelete == null)
            {
                return NotFound(new { message = "Cart item not found" });
            }

            _db.CartItems.Remove(cartItemToDelete);
            _db.SaveChanges();

            return Ok(new { message = "Cart item deleted successfully", DeletedItem = cartItemToDelete });
        }
        //-------------------------------------------------------------------------------------------------------------------------
        //-------------------------------------------------------------------------------------------------------------------------

        /*
                [HttpGet("CheckProduct/{productId}")]
                public IActionResult CheckProductInCart(int productId)
                {
                    var cartItem = _db.CartItems.FirstOrDefault(c => c.ProductId == productId && c.CartId == 1); // Assuming CartId = 1
                    if (cartItem != null)
                    {
                        return Ok(new { exists = true, quantity = cartItem.Quantity });
                    }
                    return Ok(new { exists = false });
                }*/

       


        //--------------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------------

/*

        [HttpGet("CheckProductInCart/{cartId}/{productId}")]
        public IActionResult CheckProductInCart(int cartId, int productId )
        {
            var existingCartItem = _db.CartItems
                .FirstOrDefault(ci => ci.CartId == cartId && ci.ProductId == productId);

            if (existingCartItem != null)
            {
                return Ok(new { exists = true, quantity = existingCartItem.Quantity, cartItemId = existingCartItem.CartItemId });
            }

            return Ok(new { exists = false });
        }


*/








    }

}

using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Task3.DTOs;
using Task3.Models;

namespace Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly MyDbContext _db;

        public ProductsController(MyDbContext db)
        {
            _db = db;
        }

        // Get all products
        [HttpGet("AllProducts")]
        public IActionResult GetAllProducts()
        {
            var data = _db.Products.ToList();
            return Ok(data);
        }




        // Get Last5 INDEX products
        [HttpGet("GetLast5Products")]
        public IActionResult GetLast5Products()
        {
           
            var data = _db.Products.OrderBy(p => p.ProductName).ToList();
            var lastFiveProducts = data.TakeLast(5).ToList();

            return Ok(lastFiveProducts);

        }








        // Get products by category ID
        [HttpGet("ProductsByCategoryId/{categoryId}")]
        public IActionResult GetProductsByCategoryId(int categoryId)
        {
            var products = _db.Products.Where(x => x.CategoryId == categoryId).ToList();
            return Ok(products);
        }


        // Get a single product by ID
        [HttpGet("Product/{id}")]
        public IActionResult GetProductById(int id)
        {
            var data = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }

        // Get product by ID with a name filter
        [HttpGet("ProductByName/{id:int:max(10)}")]
        public IActionResult GetProductByIdAndName(int id, [FromQuery] string name)
        {
            var data = _db.Products.FirstOrDefault(c => c.ProductId == id && c.ProductName == name);
            if (data == null)
            {
                return NotFound();
            }
            return Ok(data);
        }




        /*        ----------------------------------------------------------------------------------------------------
        *//*        ----------------------------------------------------------------------------------------------------
        */

        [HttpPost]
        [Route("AddProduct")]
        public IActionResult CreateProduct([FromForm] ProductRequestDTO productDto)
        {
            // Ensure the "Product" directory exists
            var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Product");
            if (!Directory.Exists(uploadedFolder))
            {
                Directory.CreateDirectory(uploadedFolder);
            }

            // Save the uploaded image file
            var fileImage = Path.Combine(uploadedFolder, productDto.Image.FileName);
            using (var stream = new FileStream(fileImage, FileMode.Create))
            {
                productDto.Image.CopyTo(stream);
            }

            // Prepare the data to be saved in the database as a new Product
            var product = new Product
            {
                ProductName = productDto.ProductName,
                Description = productDto.Description,
                Price = productDto.Price,
                CategoryId = productDto.CategoryId,
                Image = productDto.Image.FileName
            };

            // Add the product to the database and save changes
            _db.Products.Add(product);
            _db.SaveChanges();

            // Return a success response
            return Ok(new { message = "Product added successfully", product });
        }



        /*        ----------------------------------------------------------------------------------------------------
*//*        ----------------------------------------------------------------------------------------------------
*/


        [HttpPut("UpdateProduct/{id}")]
        public IActionResult UpdateProduct(int id, [FromForm] ProductRequestDTO Product)
        {
            // Find the existing product by ID
            var existingProduct = _db.Products.FirstOrDefault(p => p.ProductId == id);
            if (existingProduct == null)
            {
                return NotFound(new { message = "Product not found" });
            }

            // Validate the incoming model
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Ensure the "Product" directory exists
            var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Product");
            if (!Directory.Exists(uploadedFolder))
            {
                Directory.CreateDirectory(uploadedFolder);
            }

            // Save the uploaded image file if provided
            if (Product.Image != null)
            {
                var fileImage = Path.Combine(uploadedFolder, Product.Image.FileName);
                using (var stream = new FileStream(fileImage, FileMode.Create))
                {
                    Product.Image.CopyTo(stream);
                }

                // Update the image path
                existingProduct.Image = Product.Image.FileName;
            }

            // Update the existing product's properties with the new values
            existingProduct.ProductName = Product.ProductName;
            existingProduct.Description = Product.Description;
            existingProduct.Price = Product.Price;

            // Save changes to the database
            _db.Products.Update(existingProduct);
            _db.SaveChanges();

            // Return a success response with the updated product
            return Ok(new { message = "Product updated successfully", product = existingProduct });
        }


        /*        ----------------------------------------------------------------------------------------------------
*/        /*        ----------------------------------------------------------------------------------------------------
*/











        // Delete a product by ID
        [HttpDelete("Delete/{id}")]
        public IActionResult Delete(int id)
        {
            var data = _db.Products.Find(id);
            if (data == null)
            {
                return NotFound();
            }
            _db.Products.Remove(data);
            _db.SaveChanges();
            return Ok(data);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using Task3.DTOs;
using Task3.Models;

namespace Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly MyDbContext _db;

        public CategoriesController(MyDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        [Route("AllCategories")]
        public IActionResult GetAllCategories()
        {
            var data = _db.Categories.ToList();
            return Ok(data);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------


        [HttpGet]
        [Route("Category/{id:int:min(3)}")]
        public IActionResult GetCategoryById(int id)
        {
            var data = _db.Categories.Find(id);
            if (data == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(data);
        }
        //--------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------

        [HttpGet]
        [Route("Category/{name}")]
        public IActionResult GetCategoryByName(string name)
        {
            var data = _db.Categories.FirstOrDefault(c => c.CategoryName == name);
            if (data == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            return Ok(data);
        }

        //--------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------


        // Get products by category ID sorted by price descending
        [HttpGet("ProductsByCategoryId/{categoryId}")]
        public IActionResult GetProductsByCategoryId(int categoryId)
        {
            var products = _db.Products
                              .Where(x => x.CategoryId == categoryId)
                              .OrderByDescending(x => x.Price) // ترتيب المنتجات حسب السعر من الأعلى إلى الأقل
                              .ToList();
            return Ok(products);
        }



        //--------------------------------------------------------------------------------------------------------------------
        //--------------------------------------------------------------------------------------------------------------------
        // Get products by category ID sorted by price ascending
        [HttpGet("ProductsByCategoryId2/{categoryId}")]
        public IActionResult GetProductsByCategoryId2(int categoryId)
        {
            var products = _db.Products
                              .Where(x => x.CategoryId == categoryId)
                              .OrderBy(x => x.Price) // ترتيب المنتجات حسب السعر من الأصغر إلى الأكبر
                              .ToList();
            return Ok(products);
        }

        /*        --------------------------------------------------------------------------------------
        */        /*        ----------------------------------------------------------------------------------------------------
        */


        // AddCategory


        [HttpPost]
        [Route("AddCategory")]
        public IActionResult AddCategory([FromForm] categoryRequestDTO categoryDto)
        {
            //if (!ModelState.IsValid)
            //    return BadRequest(ModelState);

            //var category = new Category
            //{
            //    CategoryName = categoryDto.CategoryName,
            //    Image = categoryDto.Image
            //};

            //_db.Categories.Add(category);
            //_db.SaveChanges();
            var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
            if (!Directory.Exists(uploadedFolder))
            {
                Directory.CreateDirectory(uploadedFolder);
            }
            var fileImage = Path.Combine(uploadedFolder, categoryDto.Image.FileName);
            using (var stream =   new FileStream( fileImage , FileMode.Create ))
            {
                categoryDto.Image.CopyToAsync(stream);

            }

                var dataResponse = new Category
                {
                    Image = categoryDto.Image.FileName,
                    CategoryName = categoryDto.CategoryName
                };

            _db.Categories.Add(dataResponse);
            _db.SaveChanges();

            return Ok(new { message = "Category added successfully", dataResponse });
        }

        /*        --------------------------------------------------------------------------------------
*/
        /*        ----------------------------------------------------------------------------------------------------
*/
        /* [HttpPut]
         [Route("UpdateCategory/{id}")]
         public async Task<IActionResult> UpdateCategory(int id, [FromForm] categoryRequestDTO categoryDto)
         {
             var existingCategory = _db.Categories.Find(id);
             if (existingCategory == null)
             {
                 return NotFound(new { message = "Category not found" });
             }

             if (!ModelState.IsValid)
                 return BadRequest(ModelState);

             // Update the existing category with new values
             existingCategory.CategoryName = categoryDto.CategoryName;

             if (categoryDto.Image != null)
             {
                 try
                 {
                     var folderPath = Path.Combine("wwwroot", "images");
                     if (!Directory.Exists(folderPath))
                     {
                         Directory.CreateDirectory(folderPath);
                     }
                     var filePath = Path.Combine(folderPath, categoryDto.Image.FileName);
                     using (var stream = new FileStream(filePath, FileMode.Create))
                     {
                         await categoryDto.Image.CopyToAsync(stream);
                     }
                     existingCategory.Image = filePath;
                 }
                 catch (Exception ex)
                 {
                     return StatusCode(500, new { message = "File upload failed", error = ex.Message });
                 }
             }


             _db.SaveChanges();

             return Ok(new { message = "Category updated successfully", category = existingCategory });
         }*/

        /*        ----------------------------------------------------------------------------------------------------
*//*        ----------------------------------------------------------------------------------------------------
*/















        //UpdateCategory




        [HttpPut("{id}")]
        public IActionResult UpdateCategory(int id, [FromForm] categoryRequestDTO category)
        {
            // Step 1: Find the existing category by ID
            var existingCategory = _db.Categories.Find(id);
            if (existingCategory == null)
            {
                return NotFound(new { message = "Category not found" });
            }

            // Step 2: Update the category name
            existingCategory.CategoryName = category.CategoryName;

            // Step 3: Handle the image file upload if a new image is provided
            if (category.Image != null)
            {
                var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadsFolderPath))
                {
                    Directory.CreateDirectory(uploadsFolderPath);
                }
                var filePath = Path.Combine(uploadsFolderPath, category.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    category.Image.CopyTo(stream); // Use CopyTo instead of CopyToAsync
                }

                // Update the image path in the database
                existingCategory.Image = category.Image.FileName;
            }

            // Step 4: Save the updated category data to the database
            _db.Categories.Update(existingCategory);
            _db.SaveChanges();

            // Step 5: Return a success response
            return Ok(new { message = "Category updated successfully", category = existingCategory });
        }

        /*        ----------------------------------------------------------------------------------------------------
*//*        ----------------------------------------------------------------------------------------------------
*/

        [HttpDelete]
        [Route("{id:int}")]
        public IActionResult DeleteCategory(int id)
        {
            var data = _db.Categories.Find(id);
            if (data == null)
            {
                return NotFound(new { message = "Category not found" });
            }
            _db.Products.RemoveRange(data.Products);
            _db.Categories.Remove(data);
            _db.SaveChanges();
            return Ok(new { message = "Category deleted", category = data });
        }
    }
}

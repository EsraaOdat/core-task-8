using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Task3.DTOs;
using Task3.Models;

namespace Task3.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly MyDbContext _db;

        public UsersController(MyDbContext db)
        {
            _db = db;
        }


        // ------------------------------------------------------------------------------------------------------------
        //  ------------------------------------------------------------------------------------------------------------

        // First: Get all users
        [HttpGet]
        [Route("GetAllUsers")]
        public IActionResult GetAllUsers()
        {
            var users = _db.Users.ToList();
            return Ok(users);
        }



        // ------------------------------------------------------------------------------------------------------------
        //  ------------------------------------------------------------------------------------------------------------

       // simplified version of the GetUserById


        /*
                // GET: api/users/GetUserById/{id}
                [HttpGet("GetUserById/{id:int:max(3)}")]
                public IActionResult GetUserById(int id)
                {
                    // Attempt to find the user by ID
                    var user = _db.Users.Find(id);

                    // Return the result directly
                    return Ok(user);
                }

            */






        // ------------------------------------------------------------------------------------------------------------
        //  ------------------------------------------------------------------------------------------------------------



        // Second: Get user by ID
        [HttpGet]
        [Route("GetUserById/{id}")]
/*
        [ProducesResponseType(200)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]*/


        public IActionResult GetUserById(int id )
        {
            // Check if the provided ID is valid (e.g., greater than zero)
            if (id <= 0)
            {
                return BadRequest(new { message = "Invalid ID. The ID must be a positive integer." });
            }

            try
            {


                // Attempt to find the user in the database:


                //1. Get the first user with the specified UserId or null if none exists.
               // var user = _db.Users.FirstOrDefault(u => u.UserId == id);

                //2. Get all users with the specified UserId (can return multiple or none).
               // var users = _db.Users.Where(u => u.UserId == id);



                // 3.Find a user by the primary key (UserId) (only works with List<T>).
                 var user = _db.Users.Find(id);

                //4.Get the First User with the Specified UserId Using Where and FirstOrDefault

                //var user = _db.Users.Where(u => u.UserId == id).FirstOrDefault();




                if (user == null)
                {
                    // Return 404 Not Found if the user doesn't exist
                    return NotFound(new { message = $"User with ID {id} not found." });
                }

                // Return 200 OK if the user is found
                return Ok(user);
            }
            catch (Exception ex)
            {
                // Return 500 Internal Server Error for unexpected exceptions
                return StatusCode(500, new { message = "An unexpected error occurred.", details = ex.Message });
            }
        }


        
       // ------------------------------------------------------------------------------------------------------------
      //  ------------------------------------------------------------------------------------------------------------




        // Third: Get user by name
        [HttpGet]
        //Multiple route 
        [Route("hello/{name}")]
        [Route("GetUserByName/{name}")]
        public IActionResult GetUserByName(string name)
        {
            var user = _db.Users.FirstOrDefault(u => u.Username == name);
            if (user == null)
            {
                return NotFound(new { message = $"User with name '{name}' not found." });
            }
            return Ok(user);
        }



        // ------------------------------------------------------------------------------------------------------------
        //  ------------------------------------------------------------------------------------------------------------


        // Fourth: Delete user by ID
        [HttpDelete]
        [Route("DeleteUser/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var user = _db.Users.Find(id);
            if (user == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            _db.Users.Remove(user);
            _db.SaveChanges();
            return Ok(new { message = $"User with ID {id} has been deleted." });
        }



        // ------------------------------------------------------------------------------------------------------------
        //  ------------------------------------------------------------------------------------------------------------


        /* // Fifth: Add a new user
         [HttpPost]
         [Route("AddUser")]
         public IActionResult AddUser([FromForm] UsersRequestDTO newUser)
         {
             var user = new User // User entity from your data model
             {
                 Username = newUser.Username,
                 Email = newUser.Email,
                 Password = newUser.Password,
                 Phone = newUser.Phone,
                 Image = newUser.Image?.FileName // Store the file name in the User entity
             };

             // Handle image file upload
             if (newUser.Image != null)
             {
                 var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                 if (!Directory.Exists(uploadedFolder))
                 {
                     Directory.CreateDirectory(uploadedFolder);
                 }

                 var filePath = Path.Combine(uploadedFolder, newUser.Image.FileName);
                 using (var stream = new FileStream(filePath, FileMode.Create))
                 {
                     newUser.Image.CopyTo(stream);
                 }
             }

             _db.Users.Add(user); // Add the User entity to the database
             _db.SaveChanges();
             return Ok(new { message = "User added successfully.", user });
         }*/

        //----------------------------------------------------------------------------------------------------------------------
        //----------------------------------------------------------------------------------------------------------------------
        // Add a new user with hashed password
        [HttpPost]
        [Route("AddUser")]
        public IActionResult AddUser([FromForm] UsersRequestDTO newUser)
        {
            byte[] passwordHash, passwordSalt;
            PasswordHasherMethod.CreatePasswordHash(newUser.Password, out passwordHash, out passwordSalt);
            User user = new User
            {
                Username = newUser.Username,
                Email = newUser.Email,
                Password=newUser.Password,

                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            // Handle image file upload
            if (newUser.Image != null)

            {
                var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadedFolder))
                {
                    Directory.CreateDirectory(uploadedFolder);
                }

                var filePath = Path.Combine(uploadedFolder, newUser.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    newUser.Image.CopyTo(stream);
                }
            }

            _db.Users.Add(user);
            _db.SaveChanges();
            return Ok(new { message = "User added successfully.", user });
        }

        //-----------------------------------------------------------------------------------
        //-----------------------------------------------------------------------------------








        // Login action
        [HttpPost]
        [Route("Login")]
        public IActionResult Login([FromForm] LoginDTO loginRequest)
        {
            var user = _db.Users.FirstOrDefault(u => u.Email == loginRequest.Email );

            if (user == null)
            {
                return Unauthorized(new { message = "Invalid email ." });
            }

            // Verify the password
            if (!PasswordHasherMethod.VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
            {
                return Unauthorized(new { message = "Invalid password." });
            }

            // Generate a token or return a successful response
            return Ok(new { message = "Login successful." });
        }

        


















    // ------------------------------------------------------------------------------------------------------------
    //  ------------------------------------------------------------------------------------------------------------




    // Sixth: Update an existing user
    [HttpPut]
        [Route("UpdateUser/{id}")]
        public IActionResult UpdateUser(int id, [FromForm] UsersRequestDTO updatedUser)
        {
            var existingUser = _db.Users.Find(id);
            if (existingUser == null)
            {
                return NotFound(new { message = $"User with ID {id} not found." });
            }

            // Update user properties

            // Update user's username if a new one is provided; otherwise, keep the current username
            existingUser.Username = updatedUser.Username ?? existingUser.Username;

            // Update user's email if a new one is provided; otherwise, keep the current email
            existingUser.Email = updatedUser.Email ?? existingUser.Email;

            // Update user's password if a new one is provided; otherwise, keep the current password
/*            existingUser.Password = updatedUser.Password ?? existingUser.Password;
*/
            // Update user's phone number if a new one is provided; otherwise, keep the current phone number
            existingUser.Phone = updatedUser.Phone ?? existingUser.Phone;


            // Handle image file update if a new image is provided
            if (updatedUser.Image != null)
            {
                var uploadedFolder = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");
                if (!Directory.Exists(uploadedFolder))
                {
                    Directory.CreateDirectory(uploadedFolder);
                }

                var filePath = Path.Combine(uploadedFolder, updatedUser.Image.FileName);
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    updatedUser.Image.CopyTo(stream);
                }

                // Update the Image property with the new file name
                existingUser.Image = updatedUser.Image.FileName;
            }

            _db.Users.Update(existingUser);
            _db.SaveChanges();
            return Ok(new { message = "User updated successfully.", user = existingUser });
        }
    }

}

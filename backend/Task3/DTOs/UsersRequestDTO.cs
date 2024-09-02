namespace Task3.DTOs
{
    public class UsersRequestDTO
    {
        public string Username { get; set; } = null!;

        public string? Email { get; set; } = null!;
        public string? Password { get; set; }

       /* public byte[]? PasswordHash { get; set; }

        public byte[]? PasswordSalt { get; set; }*/

        public string? Phone { get; set; }

        public IFormFile? Image { get; set; }
    }
}

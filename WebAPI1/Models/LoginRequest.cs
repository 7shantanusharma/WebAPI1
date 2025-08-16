namespace WebAPI1.Models
{
    public class LoginRequest
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
        public List<Roles>? RoleName { get; set; }
    }

    public class Roles
    {
        public string? RoleName { get; set; }
    }
}

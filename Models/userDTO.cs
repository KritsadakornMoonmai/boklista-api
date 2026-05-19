namespace boklista_api.Models
{
    public class UserDTO
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

namespace boklista_api.Models
{
    public class UserDTOCreate
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}

namespace boklista_api.Models
{
    public class UserDTOLogin
    {
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}

namespace boklista_api.Models
{
    public class UserDTODetailed
    {
        public Guid Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
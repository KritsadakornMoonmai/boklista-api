
namespace boklista_api.Models
{
    public class AccessTokenDto
    {
        public required string Token { get; set; } = string.Empty;
        public required string RenewToken { get; set; } = string.Empty;
        public required UserDTO User { get; set; }
    }
}
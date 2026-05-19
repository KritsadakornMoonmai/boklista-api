
namespace boklista_api.Models
{
    public class RenewTokenRequestDTO
    {
        public Guid UserId { get; set; }
        public required string RenewToken { get; set; }
    }
}
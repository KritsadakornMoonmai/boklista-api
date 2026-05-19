using boklista_api.Models;

namespace boklista_api.Services
{
    public interface IUserService
    {
        Task<User> GetUserByIdAsync(Guid id);
        Task<UserDTO> GetUserByUsernameAsync(string username);
        Task<AccessTokenDto?> LoginAsync(UserDTOLogin userDto);
        Task<User> AddUserAsync(UserDTOCreate userDto);
        Task<User> UpdateUserAsync(Guid id, UserDTODetailed userDto);
        Task<bool> DeleteUserAsync(Guid id);

        Task<AccessTokenDto> RenewTokenAsync(RenewTokenRequestDTO renewTokenRequest);
    }
}
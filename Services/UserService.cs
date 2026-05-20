using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using boklista_api.Data;
using boklista_api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace boklista_api.Services
{
    public class UserService(BookListDbContext context, IConfiguration configuration) : IUserService
    {
        public Task<User> AddUserAsync(UserDTOCreate userDto)
        {
            var get_user = context.Users.SingleOrDefault(u => u.Username == userDto.Username);
            if (get_user != null)
            {
                throw new Exception("User already exists");
            }

            User user = new User();
            var hash_password = new PasswordHasher<User>();

            user.Username = userDto.Username;
            user.Email = userDto.Email;
            user.PasswordHash = hash_password.HashPassword(user, userDto.Password);
            context.Users.Add(user);
            context.SaveChanges();
            return Task.FromResult(user);
        }

        public Task<bool> DeleteUserAsync(Guid id)
        {
            User get_user = context.Users.SingleOrDefault(u => u.Id.Equals(id));
            if (get_user == null)
            {
                throw new Exception("User not found");
            }
            context.Users.Remove(get_user);
            context.SaveChanges();
            return Task.FromResult(true);
        }

        public Task<User> GetUserByIdAsync(Guid id)
        {
            User get_user = context.Users.SingleOrDefault(u => u.Id.Equals(id));
            if (get_user == null)
            {
                throw new Exception("User not found");
            }
            return Task.FromResult(get_user);
        }

        public Task<UserDTO> GetUserByUsernameAsync(string username)
        {
            User get_user = context.Users.FirstOrDefault(u => u.Username == username);
            if (get_user == null)
            {
                throw new Exception("User not found");
            }

            UserDTO fetchedUser = new UserDTO();
            fetchedUser.Username = get_user.Username;
            fetchedUser.Email = get_user.Email;
            return Task.FromResult(fetchedUser);
        }

        public async Task<AccessTokenDto?> LoginAsync(UserDTOLogin userDto)
        {
            if (userDto.Username == null || userDto.Password == null)
            {
                return null;
            }

            User user = context.Users.SingleOrDefault(u => u.Username == userDto.Username);
            if (user == null)
            {
                return null;
            }

            if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, userDto.Password)
            == PasswordVerificationResult.Failed)
            {
                return null;
            }

            return await CreateTokenResponse(user);
        }

        private async Task<AccessTokenDto> CreateTokenResponse(User? user)
        {
            UserDTO getUser = new UserDTO
            {
                Id = user.Id,
                Username = user.Username,
                Email = user.Email
            };

            return new AccessTokenDto
            {
                Token = CreateToken(user),
                RenewToken = await GenerateRenewTokenAsync(user),
                User = getUser,
            };
        }

        public Task<User> UpdateUserAsync(Guid id, UserDTODetailed userDto)
        {
            User get_user = context.Users.SingleOrDefault(u => u.Id.Equals(id));
            if (get_user == null)
            {
                throw new Exception("User not found");
            }

            var hash_password = new PasswordHasher<User>();
            var hashedPassword = hash_password.HashPassword(get_user, userDto.Password);

            get_user.Username = userDto.Username;
            get_user.PasswordHash = hashedPassword;
            get_user.Email = userDto.Email;
            context.SaveChanges();
            return Task.FromResult(get_user);
        }


        private async Task<string> GenerateRenewTokenAsync(User user)
        {
            string renewToken = RenewToken();
            user.Token = renewToken;
            user.TokenExpiration = DateTime.UtcNow.AddDays(7);
            await context.SaveChangesAsync();
            return renewToken;
        }

        private string RenewToken()
        {
            var randNum = new byte[32];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randNum);

            return Convert.ToBase64String(randNum);
        }

        private async Task<User?> ValidateRenewTokenAsync(Guid userId, string renewToken)
        {
            User? user = context.Users.SingleOrDefault(u => u.Id == userId);
            if (user == null || user.TokenExpiration < DateTime.UtcNow || user.Token != renewToken)
            {
                return null;
            }
            return user;
        }

        private string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(
                configuration.GetValue<string>("Appsettings:Token")!));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: configuration.GetValue<string>("Appsettings:Issuer"),
                audience: configuration.GetValue<string>("Appsettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(15),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        public async Task<AccessTokenDto?> RenewTokenAsync(RenewTokenRequestDTO renewTokenRequest)
        {
            var user = await ValidateRenewTokenAsync(renewTokenRequest.UserId, renewTokenRequest.RenewToken);
            if (user == null)
                return null;

            return await CreateTokenResponse(user);
        }
    }
}


using Microsoft.AspNetCore.Mvc;
using boklista_api.Models;
using boklista_api.Services;
using Microsoft.AspNetCore.Authorization;

namespace boklista_api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController(IUserService userService) : ControllerBase
{
    [HttpGet("{id}")]
    public async Task<ActionResult<User>> GetUser(Guid id)
    {
        try
        {
            var user = await userService.GetUserByIdAsync(id);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("username/{username}")]
    public async Task<ActionResult<UserDTO>> GetUserByUsername(string username)
    {
        try
        {
            var user = await userService.GetUserByUsernameAsync(username);
            return Ok(user);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("login")]
    public async Task<ActionResult<AccessTokenDto>> Login(UserDTOLogin userDto)
    {

        AccessTokenDto? credentialsChecking = await userService.LoginAsync(userDto);
        if (credentialsChecking == null)
        {
            return BadRequest("Invalid username or password");
        }
        return Ok(credentialsChecking);

    }

    [HttpPost("register")]
    public async Task<ActionResult<User>> CreateUser(UserDTOCreate userDto)
    {
        try
        {
            var createdUser = await userService.AddUserAsync(userDto);
            return CreatedAtAction(nameof(GetUser), new { id = createdUser.Id }, createdUser);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(Guid id, UserDTODetailed userDto)
    {
        if (id != userDto.Id) return BadRequest();
        try
        {
            var updatedUser = await userService.UpdateUserAsync(id, userDto);
            return Ok(updatedUser);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteUser(Guid id)
    {
        try
        {
            var result = await userService.DeleteUserAsync(id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return NotFound(ex.Message);
        }
    }

    [Authorize]
    [HttpGet]
    public IActionResult AuthenticationEndpoint()
    {
        return Ok("This is a protected endpoint");
    }

    [HttpPost("renew-token")]
    public async Task<ActionResult<AccessTokenDto>> RenewToken(RenewTokenRequestDTO renewTokenRequest)
    {
        var token = await userService.RenewTokenAsync(renewTokenRequest);
        if (token == null || token.Token == null || token.RenewToken == null)
        {
            return Unauthorized("Invalid or expired renew token");
        }
        return Ok(token);
    }
}

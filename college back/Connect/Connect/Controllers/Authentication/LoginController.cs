using api.Data;
using api.models.User;
using Connect.Controllers.Authentication.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

    namespace Connect.Controllers.Authentication
{

    // LoginController.cs

    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IAuthService _authService;

        public LoginController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var (user, token) = await _authService.AuthenticateUserAsync(login);
            if (user != null && token != null)
                return Ok(new { token });

            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto newUser)
        {
            if (await _authService.IsUsernameTakenAsync(newUser.Username))
                return BadRequest("Username already exists.");

            var success = await _authService.RegisterUserAsync(newUser);
            if (!success)
                return BadRequest("Invalid role specified.");

            return Ok("User registered successfully.");
        }
    }

}

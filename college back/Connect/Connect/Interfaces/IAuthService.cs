using Connect.Controllers.Authentication.Dto;
using api.models.User;

public interface IAuthService
{
    Task<(Users user, string token)> AuthenticateUserAsync(LoginDto login);
    Task<string> GenerateTokenAsync(Users user);
    Task<bool> IsUsernameTakenAsync(string username);
    Task<bool> RegisterUserAsync(RegisterDto newUser);

}
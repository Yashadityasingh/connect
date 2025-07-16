using api.Data;
using api.models;
using api.models.User;
using Connect.Controllers.Authentication.Dto;
using Connect.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

public class AuthService : IAuthService
{
    private readonly ConnectDbContext _context;
    private readonly IConfiguration _config;

    public AuthService(ConnectDbContext context, IConfiguration config)
    {
        _context = context;
        _config = config;
    }

    public async Task<(Users user, string token)> AuthenticateUserAsync(LoginDto login)
    {
        var user = await _context.Users
            .Include(u => u.UserRoles)
                .ThenInclude(ur => ur.Role)
            .FirstOrDefaultAsync(u => u.username == login.Username);

        if (user != null && PasswordHasher.VerifyHashedPassword(user.password, login.Password))
        {
            var token = await GenerateTokenAsync(user);
            return (user, token);
        }

        return (null, null);
    }

    public async Task<string> GenerateTokenAsync(Users user)
    {
        // 1) Create signing credentials
        var keyBytes = Encoding.UTF8.GetBytes(_config["Jwt:Key"]);
        var securityKey = new SymmetricSecurityKey(keyBytes);
        var creds = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        // 2) Load this user's roles and select their NAMES
        var roleNames = await _context.UserRoles
            .Where(ur => ur.UserId == user.Id)
            .Include(ur => ur.Role)             // eager‑load Role entity
            .Select(ur => ur.Role.Name)         // pick the Name property
            .ToListAsync();

        // 3) Build the claims list
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name,           user.username),
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
        };

        // add one ClaimTypes.Role for each role name
        foreach (var rn in roleNames)
        {
            claims.Add(new Claim(ClaimTypes.Role, rn));
        }

        // 4) Create the JWT
        var jwt = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(60),
            signingCredentials: creds
        );

        // 5) Return the serialized token
        return new JwtSecurityTokenHandler().WriteToken(jwt);
    }

    public async Task<bool> IsUsernameTakenAsync(string username)
    {
        return await _context.Users.AnyAsync(u => u.username == username);
    }

    public async Task<bool> RegisterUserAsync(RegisterDto newUser)
    {
        {
            if (await IsUsernameTakenAsync(newUser.Username))
                return false;

            var user = new Users
            {
                username = newUser.Username,
                password = PasswordHasher.HashPassword(newUser.Password),
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync(); // generates user.Id

            var roles = await _context.Roles
                .Where(r => newUser.Roles.Contains(r.Name))
                .ToListAsync();

            foreach (var role in roles)
            {
                _context.UserRoles.Add(new UserRole
                {
                    UserId = user.Id,
                    RoleId = role.Id
                });
            }

            await _context.SaveChangesAsync();
            return true;
        }
    }
}

using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace apiC.Services
{
  public class TokenService(UserManager<IdentityUser> userManager, IConfiguration configuration)
  {
    private readonly UserManager<IdentityUser> _userManager = userManager;
    private readonly IConfiguration _configuration = configuration;

    public async Task<string> GenerateToken(string username, string password)
    {
      // Validate user
      var user = await _userManager.FindByNameAsync(username);
      if (user == null || !await _userManager.CheckPasswordAsync(user, password))
      {
        return null;
      }

      // Create claims (user information)
      var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Name, user.UserName),
            // Add additional custom claims as needed
        };

      // configurantion tokens
      var tokenOptions = new TokenValidationParameters
      {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
        ValidateIssuer = true,
        ValidIssuer = _configuration["JWt:Issuer"],
        ValidateAudience = true,
        ValidAudience = _configuration["JWT:Audience"],
        ClockSkew = TimeSpan.Zero
      };

      var tokenHandler = new JwtSecurityTokenHandler();
      var securityToken = tokenHandler.CreateToken(new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(claims),
        Expires = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["JWT:Expire"])),
        SigningCredentials = new SigningCredentials(tokenOptions.IssuerSigningKey, SecurityAlgorithms.HmacSha256)
      });

      return tokenHandler.WriteToken(securityToken);
    }
  }
}
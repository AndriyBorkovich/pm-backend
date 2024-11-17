using Ardalis.Result;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectManager.Core.Entities;
using ProjectManager.Modules.Administration.Contracts.Requests;
using ProjectManager.Modules.Administration.Contracts.Responses;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ProjectManager.Modules.Administration.Features.Commands;

public class JwtHandler
{
    private readonly IConfiguration _configuration;
    private readonly IConfigurationSection _jwtSettings;
    private readonly UserManager<User> _userManager;

    public JwtHandler(IConfiguration configuration, UserManager<User> userManager)
    {
        _configuration = configuration;
        _jwtSettings = _configuration.GetSection("Jwt");
        _userManager = userManager;
    }

    public SigningCredentials GetSigningCredentials()
    {
        var key = Encoding.UTF8.GetBytes(_jwtSettings.GetSection("Key").Value);
        var secret = new SymmetricSecurityKey(key);

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }

    public async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
            {
                new(ClaimTypes.Name, user.UserName)
            };

        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles)
        {
            claims.Add(new Claim(ClaimTypes.Role, role));
        }

        return claims;
    }

    public JwtSecurityToken GenerateTokenOptions(SigningCredentials signingCredentials, List<Claim> claims)
    {
        var tokenOptions = new JwtSecurityToken(
            issuer: _jwtSettings["Issuer"],
            audience: _jwtSettings["Audience"],
            claims: claims,
            expires: DateTime.Now.AddMinutes(Convert.ToDouble(_jwtSettings["ExpiryInMinutes"])),
            signingCredentials: signingCredentials);

        return tokenOptions;
    }
}

public class LoginCommandHandler(UserManager<User> userManager, JwtHandler jwtHandler)
    : IRequestHandler<LoginRequest, Result<LoginResponse>>
{
    public async Task<Result<LoginResponse>> Handle(LoginRequest request, CancellationToken cancellationToken)
    {
        // Find user by username
        var user = await userManager.FindByNameAsync(request.Username);
        if (user == null)
        {
            return Result.NotFound("User not found.");
        }

        // Check password validity
        if (!await userManager.CheckPasswordAsync(user, request.Password))
        {
            return Result.Unauthorized("Invalid credentials.");
        }

        // Generate JWT token
        var signingCredentials = jwtHandler.GetSigningCredentials();
        var claims = await jwtHandler.GetClaims(user);
        var tokenOptions = jwtHandler.GenerateTokenOptions(signingCredentials, claims);
        var token = new JwtSecurityTokenHandler().WriteToken(tokenOptions);

        // Return success result
        return Result.Success(new LoginResponse { Token = token });
    }
}

using System.Threading.Tasks;
using EasyShop.Identity.Application.DTOs;
using EasyShop.Identity.Application.Interfaces;
using EasyShop.Identity.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace EasyShop.Identity.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IJwtTokenGenerator _tokenGenerator;

    public AuthController(UserManager<ApplicationUser> userManager, IJwtTokenGenerator tokenGenerator)
    {
        _userManager = userManager;
        _tokenGenerator = tokenGenerator;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] UserAddDto dto)
    {
        var user = new ApplicationUser
        {
            UserName = dto.Email,
            Email = dto.Email,
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            DateOfBirth = dto.DateOfBirth,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
        {
            return BadRequest(result.Errors);
        }

        var token = _tokenGenerator.GenerateToken(user);

        var response = new AuthResponseDto
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token
        };

        return Ok(response);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] UserLoginDto dto)
    {
        var user = await _userManager.FindByEmailAsync(dto.Email);
        if (user == null)
        {
            return Unauthorized(new { Message = "Credenciales incorrectas." });
        }

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!isPasswordValid)
        {
            return Unauthorized(new { Message = "Credenciales incorrectas." });
        }

        var token = _tokenGenerator.GenerateToken(user);

        var response = new AuthResponseDto
        {
            Id = user.Id.ToString(),
            Email = user.Email,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token
        };

        return Ok(response);
    }
}
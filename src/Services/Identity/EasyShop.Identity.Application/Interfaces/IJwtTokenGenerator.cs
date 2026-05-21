using EasyShop.Identity.Domain.Entities;

namespace EasyShop.Identity.Application.Interfaces;

public interface IJwtTokenGenerator
{
    string GenerateToken(ApplicationUser user);
}
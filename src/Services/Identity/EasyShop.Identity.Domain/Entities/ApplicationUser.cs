using Microsoft.AspNetCore.Identity;
using System;

namespace EasyShop.Identity.Domain.Entities;

public class ApplicationUser : IdentityUser<Guid>
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public DateTime DateOfBirth { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
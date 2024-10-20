using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StayCloudAPI.Core.Domain.Identity
{
    public class AppUser : IdentityUser<Guid>
    {
        [Required]
        [MaxLength(100)]
        public required string FirstName { get; set; }

        [Required]
        [MaxLength(100)]
        public required string LastName { get; set; }

        public bool IsActived { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpireTime { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(500)]
        public string? Avatar { get; set; }
    }
}

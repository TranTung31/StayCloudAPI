using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace StayCloudAPI.Core.Domain.Identity
{
    public class AppRole : IdentityRole<Guid>
    {
        [Required]
        [MaxLength(100)]
        public required string DisplayName { get; set; }
    }
}

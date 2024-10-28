using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StayCloudAPI.Core.Domain.Identity
{
    [Table("AppRoles")]
    public class AppRole : IdentityRole<Guid>
    {
        [Required]
        [MaxLength(100)]
        public required string DisplayName { get; set; }
    }
}

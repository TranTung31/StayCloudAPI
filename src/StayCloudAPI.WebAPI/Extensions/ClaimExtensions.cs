using Microsoft.AspNetCore.Identity;
using StayCloudAPI.Application.DTOs.Content.AuthDto;
using StayCloudAPI.Core.Domain.Identity;
using System.ComponentModel;
using System.Reflection;
using System.Security.Claims;

namespace StayCloudAPI.WebAPI.Extensions
{
    public static class ClaimExtensions
    {
        public static void GetPermissions(this List<RoleClaimsDto> allPermissions, Type policy)
        {
            FieldInfo[] fields = policy.GetFields(BindingFlags.Static | BindingFlags.Public);

            foreach (FieldInfo fi in fields)
            {
                var attribute = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);
                string displayName = fi.GetValue(null).ToString();
                var attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), true);

                if (attributes.Length > 0)
                {
                    var description = (DescriptionAttribute)attributes[0];
                    displayName = description.Description;
                }

                allPermissions.Add(new RoleClaimsDto { Value = fi.GetValue(null).ToString(), Type = "Permissions", DisplayName = displayName });
            }
        }

        public static async Task AddPermissionClaim(this RoleManager<AppRole> roleManager, AppRole role, string permission)
        {
            var allClaims = await roleManager.GetClaimsAsync(role);

            if (!allClaims.Any(a => a.Type == "Permissions" && a.Value == permission))
            {
                await roleManager.AddClaimAsync(role, new Claim("Permission", permission));
            }
        }
    }
}

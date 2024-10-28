using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using StayCloudAPI.Core.Domain.Entities;
using StayCloudAPI.Core.Domain.Identity;

namespace StayCloudAPI.Infrastructure
{
    public class StayCloudAPIDbContext : IdentityDbContext<AppUser, AppRole, Guid>
    {
        public StayCloudAPIDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Room> Rooms { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<IdentityUserClaim<Guid>>().ToTable("AppUserClaims").HasKey(x => x.Id);
            builder.Entity<IdentityRoleClaim<Guid>>().ToTable("AppRoleClaims").HasKey(x => x.Id);
            builder.Entity<IdentityUserLogin<Guid>>().ToTable("AppUserLogins").HasKey(x => x.UserId);
            builder.Entity<IdentityUserRole<Guid>>().ToTable("AppUserRoles").HasKey(x => new { x.RoleId, x.UserId });
            builder.Entity<IdentityUserToken<Guid>>().ToTable("AppUserTokens").HasKey(x => new { x.UserId });
        }

        // Tự động gán giá trị DateCreated,... bằng thời gian hiện tại khi SaveChange
        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            var entries = ChangeTracker
                .Entries()
                .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

            foreach (var entry in entries)
            {
                var dateCreatedProp = entry.Entity.GetType().GetProperty("DateCreated");

                if (entry.State == EntityState.Added && dateCreatedProp != null)
                {
                    dateCreatedProp.SetValue(entry.Entity, DateTime.UtcNow);
                }

                var dateModifiedProp = entry.Entity.GetType().GetProperty("DateModified");

                if (entry.State == EntityState.Modified && dateModifiedProp != null)
                {
                    dateModifiedProp.SetValue(entry.Entity, DateTime.UtcNow);
                }
            }

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}

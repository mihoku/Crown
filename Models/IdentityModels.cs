using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using crown.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace crown.Models
{
    // You can add profile data for the user by adding more properties to your ApplicationUser class, please visit https://go.microsoft.com/fwlink/?LinkID=317594 to learn more.
    public class ApplicationUser : IdentityUser
    {
        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            // Note the authenticationType must match the one defined in CookieAuthenticationOptions.AuthenticationType
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            // Add custom user claims here
            return userIdentity;
        }
        public string CompleteName { get; set; }
        public bool isGuest { get; set; }
        public string ResetPasswordCode { get; set; }
    }

    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext()
            : base("DefaultConnection", throwIfV1Schema: false)
        {
        }

        public DbSet<theme> theme { get; set; }
        public DbSet<subTheme> subTheme { get; set; }
        public DbSet<timelineItem> timelineItem { get; set; }
        public DbSet<archive> archive { get; set; }
        public DbSet<archiveItem> archiveItem { get; set; }
        public DbSet<archiveType> archiveType { get; set; }
        public DbSet<subThemeItem> subThemeItem { get; set; }
        public DbSet<timelineDetails> timelineDetails { get; set; }

        public static ApplicationDbContext Create()
        {
            return new ApplicationDbContext();
        }
    }
}
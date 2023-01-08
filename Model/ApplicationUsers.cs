using Microsoft.AspNetCore.Identity;

namespace VilllaParks.Model
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
    }
}

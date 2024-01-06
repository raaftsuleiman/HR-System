using Microsoft.AspNetCore.Identity;

namespace HR.Data
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }

        public virtual Employee employee { get; set; }
    }
}

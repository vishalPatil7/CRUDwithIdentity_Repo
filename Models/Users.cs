using Microsoft.AspNetCore.Identity;

namespace CRUDwithIdentity.Models
{
    public class Users : IdentityUser<int>
    {
        public string FullName { get; set; }
    }
}

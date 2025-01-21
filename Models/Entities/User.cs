using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Server.Models
{
    // IdentityUser  is the base class provided by ASP.NET Identity that includes default properties like username, email, password ....
    public class User : IdentityUser<string>
    {
        // these are custome properties on top of the default ones
        [Required]
        public int EmployeeNumber { get; set; }
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

    }
}

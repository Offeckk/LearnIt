using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace LearnIt.Data.Entities
{
    // Add profile data for application users by adding properties to the LIUser class
    public class LIUser : IdentityUser<string>
    {
        public List<LearnCourse> Courses { get; set; }
    }
}

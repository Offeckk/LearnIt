using LearnIt.Data.Entities;
using System.Collections.Generic;

namespace LearnIt.Models
{
    public class UserProfileViewModel
    {
        public string Name { get; set; }

        public List<LearnCourse> Courses { get; set; }

        public List<string> Roles { get; set; }

    }
}

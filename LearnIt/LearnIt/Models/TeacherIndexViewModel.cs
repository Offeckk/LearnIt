using LearnIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnIt.Models
{
    public class TeacherIndexViewModel
    {
        public List<LearnCourse> TeacherCourses { get; set; }

        public List<LearnCourse> ForeignCourses { get; set; }


    }
}

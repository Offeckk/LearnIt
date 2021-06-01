using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LearnIt.Models
{
    public class LearnCourseDetailsViewModel : LearnCourseEditViewModel
    {
        public string Teacher { get; set; }
        public List<string> Students { get; set; }

        public bool IsCurrentUserJoined { get; set; }
    }
}

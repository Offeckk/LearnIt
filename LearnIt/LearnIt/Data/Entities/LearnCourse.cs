using System;
using System.Collections.Generic;

namespace LearnIt.Data.Entities
{
    public class LearnCourse
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public CourseStatus Status { get; set; }
        public string StatusId { get; set; }
        public List<LIUser> Users { get; set; }

    }
}

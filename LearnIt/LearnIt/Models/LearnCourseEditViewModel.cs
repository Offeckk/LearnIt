using LearnIt.Data.Entities;
using System.Collections.Generic;

namespace LearnIt.Models
{
    public class LearnCourseEditViewModel : LearnCourseCreateViewModel
    {
        public string Id { get; set; }
        public string Status { get; set; }
        public List<CourseStatus> Statuses { get; set; }
    }
}

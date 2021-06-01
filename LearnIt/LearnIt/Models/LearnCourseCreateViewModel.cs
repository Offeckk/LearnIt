using LearnIt.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace LearnIt.Models
{
    public class LearnCourseCreateViewModel : IValidatableObject
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime EndDate { get; set; }

        IEnumerable<ValidationResult> IValidatableObject.Validate(ValidationContext validationContext)
        {
            List<ValidationResult> res = new List<ValidationResult>();
            if (StartDate < DateTime.Today)
            {
                ValidationResult mss = new ValidationResult("Start date must be today or later");
                res.Add(mss);

            }
            if (StartDate >= EndDate)
            {
                ValidationResult mss = new ValidationResult("Start date must be earlier than end date");
                res.Add(mss);
            }
            return res;
        }
    }
}

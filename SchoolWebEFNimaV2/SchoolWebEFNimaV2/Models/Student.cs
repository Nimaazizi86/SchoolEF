using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebEFNimaV2.Models
{
    public class Student
    {
        [Key]
        public int StudentID { get; set; }
        [Required]
        public string StudentFirstName { get; set; }
        [Required]
        public string StudentLastName { get; set; }

        public int? CourseId { get; set; }

        //public virtual Course courses { get; set; }
        public virtual List<Course> Courses { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SchoolWebEFNimaV2.Models
{
    public class Teacher
    {

        [Key]
        public int TeacherID { get; set; }
        [Required]
        public string TeacherName { get; set; }
        [Required]
        public string TeacherLastName { get; set; }

        public virtual List<Course> Courses { get; set; }
        public virtual List<Assignment> Assignments { get; set; }

    }
}
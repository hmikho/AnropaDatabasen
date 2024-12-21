using System;
using System.Collections.Generic;

namespace Lab3_ORM_SQL.Models;

public partial class Course
{
    public int CourseId { get; set; }
    public string CourseName { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}

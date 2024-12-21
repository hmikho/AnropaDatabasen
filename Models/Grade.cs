using System;
using System.Collections.Generic;

namespace Lab3_ORM_SQL.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int? CourseId { get; set; }

    public int? StudentId { get; set; }

    public int? TeacherId { get; set; }

    public string? Grades { get; set; }

    public DateTime GradeDate { get; set; }

    public virtual Course Course { get; set; }

    public virtual Student Student { get; set; }

    public virtual Staff Teacher { get; set; }
}

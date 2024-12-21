using System;
using System.Collections.Generic;
using System.Security.Claims;

namespace Lab3_ORM_SQL.Models;

public partial class Student
{
    public int StudentId { get; set; }
    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? SocialSecurityNumber { get; set; }

    public string? Class { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}

using System;
using System.Collections.Generic;

namespace Lab3_ORM_SQL.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Position { get; set; }

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}

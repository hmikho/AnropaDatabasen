using Lab3_ORM_SQL.Data;
using Lab3_ORM_SQL.Models;
using Microsoft.EntityFrameworkCore;

namespace Lab3_ORM_SQL
{
    class Program
    {
        private static SchoolContext context;

        static async Task Main(string[] args)
        {
            bool keepRunning = true;

            while (keepRunning)
            {
                Console.WriteLine("\nChoose a function:");
                Console.WriteLine("1. Show staff");
                Console.WriteLine("2. Show all students");
                Console.WriteLine("3. Show all students in one specific class");
                Console.WriteLine("4. Show grades from last month");
                Console.WriteLine("5. Show courses with average, highest, and lowest grades");
                Console.WriteLine("6. Add new student");
                Console.WriteLine("7. Add new staff");
                Console.WriteLine("8. Exit");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        StaffFunctions.FetchStaff(); // Anropa metoden från en annan klass
                        break;
                    case "2":
                        StudentFunctions.FetchAllStudents(); // Anropa metoden från en annan klass
                        break;
                    case "3":
                        StudentFunctions.FetchStudentByClass();
                        break;
                    case "4":
                        GradeFunction.FetchRecentGrades();
                        break;
                    case "5":
                        using (var context = new SchoolContext())
                        {
                            await CourseFunction.GetCourseStats(context);
                        }
                        break;
                    case "6":
                        StudentFunctions.AddNewStudent();
                        break;
                    case "7":
                        StaffFunctions.AddNewStaff();
                        break;
                    case "8":
                        keepRunning = false;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }
    }
    public class StaffFunctions
    {
        public static void FetchStaff()
        {
            using (var context = new SchoolContext())
            {
                Console.WriteLine("\nPick an option: ");
                Console.WriteLine("1. Show all staff");
                Console.WriteLine("2. See staff in a specific category (e.g., Teacher)");
                string choice = Console.ReadLine();

                if (choice == "1")
                {
                    var staffMembers = context.Staffs.ToList();
                    Console.WriteLine("\nAll staff: ");
                    foreach (var staff in staffMembers)
                    {
                        Console.WriteLine($"{staff.FirstName} {staff.LastName} - {staff.Position}");
                    }
                }
                else if (choice == "2")
                {
                    var teachers = context.Staffs
                    .Where(s => s.Position.ToLower() == "teacher")
                    .ToList();


                    if (teachers.Any())
                    {
                        Console.WriteLine("\nTeachers:");
                        foreach (var teacher in teachers)
                        {
                            Console.WriteLine($"{teacher.FirstName} {teacher.LastName} - {teacher.Position}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No teachers found.");
                    }
                }
            }
        }

        public static void AddNewStaff()
        {
            using (var context = new SchoolContext())
            {
                Console.WriteLine("\nEnter firstname:");
                string firstName = Console.ReadLine();

                Console.WriteLine("\nEnter lastname:");
                string lastName = Console.ReadLine();

                Console.WriteLine("\nEnter position:");
                string position = Console.ReadLine();

                var newStaff = new Staff
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Position = position
                };

                context.Staffs.Add(newStaff);
                context.SaveChanges();
                Console.WriteLine("\nNew staff has been added!");
            }
        }
    }

    public class StudentFunctions
    {
        public static void FetchAllStudents()
        {
            using (var context = new SchoolContext())
            {
                Console.WriteLine("\nDo you want to sort students by:");
                Console.WriteLine("1. Firstname");
                Console.WriteLine("2. Lastname");
                string sortChoice = Console.ReadLine();

                Console.WriteLine("\nDo you want to sort:");
                Console.WriteLine("1. Ascending");
                Console.WriteLine("2. Descending");
                string orderChoice = Console.ReadLine();

                var students = context.Students.AsQueryable();

                if (sortChoice == "1")
                    students = orderChoice == "1" ? students.OrderBy(s => s.FirstName) : students.OrderByDescending(s => s.FirstName);
                else if (sortChoice == "2")
                    students = orderChoice == "1" ? students.OrderBy(s => s.LastName) : students.OrderByDescending(s => s.LastName);

                Console.WriteLine("\nStudents:");
                foreach (var student in students.ToList())
                {
                    Console.WriteLine($"{student.FirstName} {student.LastName} - Class: {student.Class}");
                }
            }
        }

        public static void FetchStudentByClass()
        {
            using (var context = new SchoolContext())
            {
                var classes = context.Students.Select(s => s.Class).Distinct().ToList();
                Console.WriteLine("\nAvailable Classes:");
                foreach (var cls in classes)
                {
                    Console.WriteLine(cls);
                }

                Console.WriteLine("\nName a class:");
                string chosenClass = Console.ReadLine();

                var studentsInClass = context.Students
                .Where(s => s.Class.ToLower() == chosenClass.ToLower())
                .Distinct()
                .ToList();


                if (studentsInClass.Any())
                {
                    Console.WriteLine($"\nStudent in class {chosenClass}");
                    foreach (var student in studentsInClass)
                    {
                        Console.WriteLine($"{student.FirstName} {student.LastName}");
                    }
                }
                else
                {
                    Console.WriteLine($"No students where found in class {chosenClass}");
                }
            }
        }

        public static void AddNewStudent()
        {
            Console.WriteLine("\nEnter firstname:");
            string firstName = Console.ReadLine();

            Console.WriteLine("\nEnter lastname:");
            string lastName = Console.ReadLine();

            Console.WriteLine("\nEnter social security number:");
            string ssn = Console.ReadLine();

            Console.WriteLine("\nEnter class: ");
            string cls = Console.ReadLine();

            using (var context = new SchoolContext())
            {
                var newStudent = new Student
                {
                    FirstName = firstName,
                    LastName = lastName,
                    SocialSecurityNumber = ssn,
                    Class = cls
                };

                context.Students.Add(newStudent);
                context.SaveChanges();
                Console.WriteLine("A new student has been added!");
            }
        }
    }


    public class GradeFunction
    {
        public static void FetchRecentGrades()
        {
            using (var context = new SchoolContext())
            {
                var recentGeades = context.Grades
                    .Where(g => g.GradeDate >= DateTime.Now.AddMonths(-1))
                    .ToList();

                Console.WriteLine("\nGrades set last month:");
                foreach (var grade in recentGeades)
                {
                    var student = context.Students.FirstOrDefault(s => s.StudentId == grade.StudentId);
                    var course = context.Courses.FirstOrDefault(c => c.CourseId == grade.CourseId);
                    Console.WriteLine($"Student: {student?.FirstName} {student?.LastName} - Course: {course?.CourseName} - Grade: {grade.Grades}");

                }
            }
        }
    }
    public class CourseFunction
    {
        public static async Task GetCourseStats(SchoolContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context), "The DbContext instance is null.");
            }

            var coursesWithGrades = await context.Courses
                .Include(c => c.Grades)
                .ToListAsync();

            if (!coursesWithGrades.Any())
            {
                Console.WriteLine("No courses found in the database.");
                return;
            }

            var courseStats = coursesWithGrades.Select(course =>
            {
                var numericGrades = course.Grades
                    .Select(g => ConvertGradeToNumeric(g.Grades))
                    .ToList();

                return new CourseStats
                {
                    CourseName = course.CourseName ?? "Unknown Course",
                    AverageGrade = numericGrades.Any() ? numericGrades.Average() : 0,
                    HighestGrade = numericGrades.Any() ? numericGrades.Max() : 0,
                    LowestGrade = numericGrades.Any() ? numericGrades.Min() : 0
                };
            }).ToList();

            foreach (var stat in courseStats)
            {
                Console.WriteLine($"Course: {stat.CourseName}");
                Console.WriteLine($"Average Grade: {stat.AverageGrade:F2}");
                Console.WriteLine($"Highest Grade: {stat.HighestGrade}");
                Console.WriteLine($"Lowest Grade: {stat.LowestGrade}");
                Console.WriteLine();
            }
        }

        public static double ConvertGradeToNumeric(string grade)
        {
            if (string.IsNullOrWhiteSpace(grade))
            {
                return 0;
            }

            switch (grade.ToUpper())
            {
                case "A":
                    return 5;
                case "B":
                    return 4;
                case "C":
                    return 3;
                case "D":
                    return 2;
                case "E":
                    return 1;
                case "F":
                    return 0;
                default:
                    return 0;
            }
        }
    }


}


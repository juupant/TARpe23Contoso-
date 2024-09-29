using Contoso_University.Models;

namespace Contoso_University.Data
{
    public class DbInitializer
    {
        public static void Initialize(SchoolContext context)
        {
            //teeb kindlaks et andmebaas tehakse, või oleks olemas
            context.Database.EnsureCreated();

            //kui õpilaste tabelis juba on õpilasi, väljub funktsioonist
            if (context.Students.Any())
            {
                return;
            }

            //objekt õpilastega, mis lisatakse siis, kui õpilasi sisestatud ei ole
            var students = new Student[]
            {
                new Student{FirstMidName="Skibidi",LastName="Rizz",EnrollmentDate=DateTime.Parse("2069-04-20")},
                new Student{FirstMidName="Meredith",LastName="Alonso",EnrollmentDate=DateTime.Parse("2002-09-01")},
                new Student{FirstMidName="Arturo",LastName="Anand",EnrollmentDate=DateTime.Parse("2003-09-01")},
                new Student{FirstMidName="Gytis",LastName="Barzdukas",EnrollmentDate=DateTime.Parse("2002-09-01")},
                new Student{FirstMidName="Yan",LastName="Li",EnrollmentDate=DateTime.Parse("2002-09-01")},
                new Student{FirstMidName="Peggy",LastName="Justice",EnrollmentDate=DateTime.Parse("2001-09-01")},
                new Student{FirstMidName="Laura",LastName="Norman",EnrollmentDate=DateTime.Parse("2003-09-01")},
                new Student{FirstMidName="Nino",LastName="Olivetto",EnrollmentDate=DateTime.Parse("2005-09-01")},
                new Student{FirstMidName="Carson",LastName="Alexander",EnrollmentDate=DateTime.Parse("2005-09-01")},
                new Student{FirstMidName="Alan",LastName="Unt",EnrollmentDate=DateTime.Parse("2023-08-20")},
            };

            //iga õpilane lisatakse ükshaaval läbi foreach tsükli
            foreach (Student student in students)
            {
                context.Students.Add(student);
            }
            //ja andmebaasi muudatused salvestatakse
            context.SaveChanges();

            //eelnev struktuur, kuid kursustega: \/
            var courses = new Course[]
            {
                new Course{CourseId=1050,Title="Keemia",Credits=3},
                new Course{CourseId=4022,Title="Mikroökonoomika",Credits=3},
                new Course{CourseId=4041,Title="Mikroökonoomika",Credits=3},
                new Course{CourseId=1045,Title="Calculus",Credits=4},
                new Course{CourseId=3141,Title="Trigonomeetria",Credits=4},
                new Course{CourseId=2021,Title="Kompositsioon",Credits=3},
                new Course{CourseId=2042,Title="Kirjandus",Credits=4},
                new Course{CourseId=1069,Title="Kehaline Kasvatus",Credits=1}
            };
            foreach(Course course in courses)
            {
                context.Courses.Add(course);
            }
            context.SaveChanges();

            var enrollments = new Enrollment[]
            {
                new Enrollment{StudentId=1,CourseId=1050,Grade=Grade.A},
                new Enrollment{StudentId=1,CourseId=4022,Grade=Grade.C},
                new Enrollment{StudentId=1,CourseId=4041,Grade=Grade.B},

                new Enrollment{StudentId=2,CourseId=1845,Grade=Grade.B},
                new Enrollment{StudentId=2,CourseId=3141,Grade=Grade.F},
                new Enrollment{StudentId=2,CourseId=2021,Grade=Grade.F},

                new Enrollment{StudentId=3,CourseId=1050},

                new Enrollment{StudentId=4,CourseId=1050},
                new Enrollment{StudentId=4,CourseId=4022,Grade=Grade.F},

                new Enrollment{StudentId=5,CourseId=4041,Grade=Grade.C},

                new Enrollment{StudentId=6,CourseId=1045},

                new Enrollment{StudentId=7,CourseId=3141,Grade=Grade.A},

                new Enrollment{StudentId=10,CourseId=1069,Grade=Grade.A},
            };
            foreach (Enrollment enrollment in enrollments)
            {
                context.Enrollments.Add(enrollment);
            }
            context.SaveChanges();


            if (context.Instructors.Any())
            {
                return;
            }
            var instructors = new Instructor[]
            {
                new Instructor{FirstMidName = "Ying", LastName = "Yang", DaysOff = 0, HireDate = DateTime.Parse("2019-09-01")},
                new Instructor{FirstMidName = "Original", LastName = "Name", DaysOff = 2, HireDate = DateTime.Parse("1776-09-01")},
                new Instructor{FirstMidName = "This", LastName = "One", DaysOff = 9123, HireDate = DateTime.Parse("1776-09-01")},
            };
            context.Instructors.AddRange(instructors);
            context.SaveChanges();


            if (context.Departments.Any())
            {
                return;
            }
            var departments = new Department[]
            {
                new Department{
                    Name="IT",
                    Budget=0,
                    StartDate=DateTime.Parse("1999-09-09"),
                    InstructorID=1,
                    SuperImportantString="ashydasnhdhjanjd shua",
                },
                new Department{
                    Name="Language",
                    Budget=0,
                    StartDate=DateTime.Parse("2000-01-01"),
                    InstructorID=1,
                    SuperImportantString="ggghbnjmkl,aszxdcrfvgbhnjmk,2",
                },
                new Department{
                    Name="Everything else",
                    Budget=0,
                    StartDate=DateTime.Parse("3000-01-01"),
                    SuperImportantString="gfsdfshiASYDWEWiuwyrgctI12387646yiusabgdgy",
                },
            };
            context.Departments.AddRange(departments);
            context.SaveChanges();
        }
    }
}

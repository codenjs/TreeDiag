using System;
using System.Collections.Generic;
using Xunit;

namespace TreeDiag;

public class Employee
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Title { get; set; }
    public List<Employee> DirectReports = [];
}

public class EmployeeTreeDiagnosticWriter : TreeDiagnosticWriter<Employee>
{
    protected override string Format(Employee e) => $"{e.Title} {e.FirstName} {e.LastName}";
    protected override IEnumerable<Employee> GetChildren(Employee e) => e.DirectReports;
}

public class BasicTest
{
    [Fact]
    public void Test()
    {
        var tree = new Employee { FirstName = "Joe", LastName = "Smith", Title = "Executive Director", DirectReports =
            {
                new Employee { FirstName = "Lynelle", LastName = "Hinck", Title = "Executive Assistant" },
                new Employee { FirstName = "Cathy", LastName = "Brown", Title = "Development Director", DirectReports =
                    {
                        new Employee { FirstName = "Lisa", LastName = "Rudolph", Title = "Development Assistant" },
                        new Employee { FirstName = "Mandy", LastName = "Schaff", Title = "Special Events Assistant" }
                    }
                },
                new Employee { FirstName = "Allen", LastName = "Alvarez", Title = "Program Director", DirectReports =
                    {
                        new Employee { FirstName = "Jim", LastName = "Rubino", Title = "Housing Coordinator" },
                        new Employee { FirstName = "Allie", LastName = "Webster", Title = "Workforce Coordinator" },
                        new Employee { FirstName = "Kelly", LastName = "Brock", Title = "Public Assistance Coordinator" }
                    }
                },
                new Employee { FirstName = "Mark", LastName = "Brender", Title = "Finance/HR Director", DirectReports =
                    {
                        new Employee { FirstName = "Jean", LastName = "Hubert", Title = "Finance Assistant" }
                    }
                },
                new Employee { FirstName = "Kim", LastName = "Rudolph", Title = "Volunteer Director", DirectReports =
                    {
                        new Employee { FirstName = "Jamie", LastName = "Martin", Title = "Volunteer Coordinator" }
                    }
                }
            }
        };

        var expected = """
            {0} Executive Director Joe Smith
              {0} Executive Assistant Lynelle Hinck
              {1} Development Director Cathy Brown
                {0} Development Assistant Lisa Rudolph
                {1} Special Events Assistant Mandy Schaff
              {2} Program Director Allen Alvarez
                {0} Housing Coordinator Jim Rubino
                {1} Workforce Coordinator Allie Webster
                {2} Public Assistance Coordinator Kelly Brock
              {3} Finance/HR Director Mark Brender
                {0} Finance Assistant Jean Hubert
              {4} Volunteer Director Kim Rudolph
                {0} Volunteer Coordinator Jamie Martin

            """;
        var actual = new EmployeeTreeDiagnosticWriter().Write(tree);
        Assert.Equal(expected, actual);
    }
}

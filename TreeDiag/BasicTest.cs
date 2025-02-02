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
            Executive Director Joe Smith
              Executive Assistant Lynelle Hinck
              Development Director Cathy Brown
                Development Assistant Lisa Rudolph
                Special Events Assistant Mandy Schaff
              Program Director Allen Alvarez
                Housing Coordinator Jim Rubino
                Workforce Coordinator Allie Webster
                Public Assistance Coordinator Kelly Brock
              Finance/HR Director Mark Brender
                Finance Assistant Jean Hubert
              Volunteer Director Kim Rudolph
                Volunteer Coordinator Jamie Martin

            """;
        var actual = new EmployeeTreeDiagnosticWriter().Write(tree);
        Assert.Equal(expected, actual);
    }
}

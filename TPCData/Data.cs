namespace TPCData;
public static class Data
{
    public static List<Employee> GetEmployees() {
        List<Employee> employees = new List<Employee>();

        Employee employee = new Employee {
            Id = 1,
            FirstName = "Bob",
            LastName = "Jones",
            AnnualSalary = 60000.3m,
            IsManager = true,
            DepartmentId = 1
        };
        employees.Add(employee);
        
        employee = new Employee {
            Id = 2,
            FirstName = "Sarah",
            LastName = "Jameson",
            AnnualSalary = 80000.1m,
            IsManager = true,
            DepartmentId = 2
        };
        employees.Add(employee);

        employee = new Employee {
            Id = 3,
            FirstName = "Douglas",
            LastName = "Roberts",
            AnnualSalary = 40000.2m,
            IsManager = false,
            DepartmentId = 2
        };
        employees.Add(employee);

        employee = new Employee {
            Id = 4,
            FirstName = "Jane",
            LastName = "Stevens",
            AnnualSalary = 30000.2m,
            IsManager = false,
            DepartmentId = 3
        };
        employees.Add(employee);

        return employees;
    }

    public static List<Department> GetDepartments(IEnumerable<Employee> employees){
         List<Department> departments = new List<Department>();

        Department department = new Department {
            Id = 1,
            ShortName = "HR",
            LongName = "Human Resources",
            Employees = from e in employees
                        where e.DepartmentId == 1
                        select e
        };
        departments.Add(department);

        department = new Department {
            Id = 2,
            ShortName = "FN",
            LongName = "Finance",
            Employees = from e in employees
                        where e.DepartmentId == 2
                        select e
        };
        departments.Add(department);
        
        department = new Department {
            Id = 3,
            ShortName = "TE",
            LongName = "Techonology",
            Employees = from e in employees
                        where e.DepartmentId == 3
                        select e
        };
        departments.Add(department);
        
         return departments;
    }
}

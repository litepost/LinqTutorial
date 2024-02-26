// See https://aka.ms/new-console-template for more information
/* 
https://www.youtube.com/watch?v=5l2qA3Pc83M&t=10582s
freecodecamp.org
Gavin Lon 
*/


// ******** Notes ********
/*
Sorting operators
    OrderBy, OrderByDescending
    ThenBy, ThenByDescending
Grouoping operators
    GroupBy 
    ToLookup
Quantifier Operators
    All
    Any
    Contains
Filter Operators
    OfType  look for same data type
    Where
Element Operators
    ElementAt, ElementAtOrDefault   give index, objet is returned, raise error uness use OrDefault
    First, FirstOrDefault
    Last, LastOrDefault
    Single, SingleOrDefault
Join Operators
    Join
    GroupJoin
Equality
    SequenceEqual   values and sorting must be the same
Concatenation Operator
    Concat  append one collection to another collection
Set Operators
    Distinct
    Except
    Intersect
    Union
Generation Operators
    DefaultIfEmpty
    Empty
    Range
    Repeat
Aggregate Operators
    Aggregate
    Average
    Count
    Sum
    Min, Max
Partitionaing Operators
    Skip, SkipWhile
    Take, TakeWhile
Conversion Operators
    ToList
    ToDictionary
    ToArray
Projection Operators
    Select
    SelectMany
Keywords
    Let
    Into
*/  

using System;
using System.Data.Common;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using TPCData;
using TPCExtensions;

List<Employee> employees = Data.GetEmployees();

var filteredEmployees = employees.Filter(emp => emp.IsManager == false);
foreach (var e in filteredEmployees) {
    Console.WriteLine("-------------------------------");
    Console.WriteLine($"Id: {e.Id}");
    Console.WriteLine($"First Name: {e.FirstName}");
    Console.WriteLine($"Last Name: {e.LastName}");
    Console.WriteLine($"Annual Salary: {e.AnnualSalary}");
    Console.WriteLine($"Manager: {e.IsManager}");
    Console.WriteLine($"Dept: {e.DepartmentId}");

}

System.Console.WriteLine();
System.Console.WriteLine();

List<Department> departments = Data.GetDepartments(employees);
var filteredDepts = departments.Filter(dept => dept.ShortName == "HR" || dept.ShortName == "FN");

foreach (var dept in filteredDepts) {
    System.Console.WriteLine("********************");
    System.Console.WriteLine($"Id: {dept.Id}");
    System.Console.WriteLine($"Short Name: {dept.ShortName}");
    System.Console.WriteLine($"Long Name: {dept.LongName}");
}

System.Console.WriteLine();
System.Console.WriteLine();
System.Console.WriteLine("__________LINQ__________");

var whereDepts = departments.Where(d => d.Id > 1);

foreach (var d in whereDepts) {
    System.Console.WriteLine("********************");
    System.Console.WriteLine($"Id: {d.Id}");
    System.Console.WriteLine($"Short Name: {d.ShortName}");
    System.Console.WriteLine($"Long Name: {d.LongName}");
}

System.Console.WriteLine();
System.Console.WriteLine();

var joinList = from emp in employees
               join d in departments
               on emp.DepartmentId equals d.Id
               select new {
                FirstName = emp.FirstName,
                LastName = emp.LastName,
                AnnualSalary = emp.AnnualSalary,
                Manager = emp.IsManager,
                Department = d.LongName
               };

System.Console.WriteLine("______Inner join______");
foreach (var rec in joinList) {
    System.Console.WriteLine($"First Name: {rec.FirstName}");
    System.Console.WriteLine($"Last Name: {rec.LastName}");
    System.Console.WriteLine($"Annual Salary: {rec.AnnualSalary}");
    System.Console.WriteLine($"Manager: {rec.Manager}");
    System.Console.WriteLine($"Department: {rec.Department}");
    System.Console.WriteLine();
}

System.Console.WriteLine("______Aggregations______");
var avgSalary = joinList.Average(a => a.AnnualSalary);
var maxSalary = joinList.Max(a => a.AnnualSalary);
var minSalary = joinList.Min(a => a.AnnualSalary);
System.Console.WriteLine($"Average Salary: {avgSalary}");
System.Console.WriteLine($"Max Salary: {maxSalary}");
System.Console.WriteLine($"Min Salary: {minSalary}");


System.Console.WriteLine();
System.Console.WriteLine();
System.Console.WriteLine("**************** Part 2 - LINQ Queries ****************");
System.Console.WriteLine("______Method syntax______");
var methodFullName = employees.Select(e => new {
    FullName = e.FirstName + " " + e.LastName,
    AnnualSalary = e.AnnualSalary
}).Where(s => s.AnnualSalary > 50_000);

foreach (var e in methodFullName) {
    System.Console.WriteLine($"{e.FullName, -20} {e.AnnualSalary, 10}");
}

System.Console.WriteLine();
System.Console.WriteLine("______Query syntax______");
var queryFullName = (from emp in employees
                    where emp.AnnualSalary > 50_000
                    select new {
                        FullName = emp.FirstName + " " + emp.LastName,
                        AnnualSalary = emp.AnnualSalary
                    }).ToList();
employees.Add(new Employee{
    Id = 5,
    FirstName = "Sam",
    LastName = "Davis",
    AnnualSalary = 100_000.20m,
    IsManager = true,
    DepartmentId = 2
});

// Sam Jones is printed below because the LINQ query is executed during the loop
// When LINQ query is converted to a list, then it's executed immediately and Sam doesn't appear in the results
foreach (var qfn in queryFullName) {
    System.Console.WriteLine($"{qfn.FullName, -20} {qfn.AnnualSalary, 10}");
}

System.Console.WriteLine();
System.Console.WriteLine("______Join Example______");
var joinFulName = from e in employees
                  join d in departments
                  on e.DepartmentId equals d.Id
                  select new {
                    FullName = e.FirstName + " " + e.LastName,
                    AnnualSalary = e.AnnualSalary,
                    DepartmentName = d.LongName
                  };

foreach (var row in joinFulName) {
    System.Console.WriteLine($"{row.FullName, -20} {row.AnnualSalary, 10}\t{row.DepartmentName}");
}

// Example from a different video
// Inner join on miltiple columns
// Column names and data types must be the same --> rename ShipCountry to Country & cast ShipCountry as string
//from c in Customers
//join o in Orders
//on new {c.CustomerId, c.Country} equals new {o.CustomerId, Country = (string)o.ShipCountry}
//join s in Shippers
//on o.ShipVia equals s.ShipperId
//select new {
//  c.CustomerId,
//  c.CompanyName,
//  o.OrderId,
//  o.ShipAddress,
//  ShipperName = s.CompanyName
//}

System.Console.WriteLine();
System.Console.WriteLine("______ Group Join Example - list of employees for each department ______");
var groupJoinFullName = from d in departments
                       join e in employees
                       on d.Id equals e.DepartmentId
                       into employeeGroup
                       select new {
                        Employees = employeeGroup,
                        DepartmentName = d.LongName
                       };
foreach (var row in groupJoinFullName) {
    System.Console.WriteLine($"Department Name: {row.DepartmentName}");
    foreach (var e in row.Employees) {
        System.Console.WriteLine($"\t{e.FirstName} {e.LastName}");
    }
}

// ______ Left join example ______
//from c in Customers
//join o in Orders on c.CustomerId equals o.CustomerId
//into orderGroup
//from og in orderGroup.DefaultIfEmpty()
//select new {
//  c.CustomerId,
//  c.CompanyName,
//  g.OrderId,
//  g.ShipCountry
//}

// ______ Cartesian Product Example
//from c in Customers
//join o in Orders
//select new {
//  c.CustomerId,
//  o.OrderId
// }

System.Console.WriteLine();
System.Console.WriteLine("______OrderBy Example______");
var orderbyDeptIdSalary = from e in employees
                          join d in departments
                          on e.DepartmentId equals d.Id
                          orderby e.DepartmentId, e.AnnualSalary descending
                          select new {
                            Id = e.Id,
                            FirstName = e.FirstName,
                            LastName = e.LastName,
                            AnnualSalary = e.AnnualSalary,
                            DepartmentId = e.DepartmentId,
                            DepartmentName = d.LongName
                          };
foreach (var e in orderbyDeptIdSalary) {
    System.Console.WriteLine($"{e.Id, -5} {e.FirstName, -15} {e.LastName, -15} {e.AnnualSalary, -10} {e.DepartmentId}\t{e.DepartmentName}");
}

System.Console.WriteLine();
System.Console.WriteLine("______GroupBy Example______");
// .ToLookup produces same results as group by operator except that .ToLookup is executed immediately
// whereas group by is deferred.
var groupbyEmpDeptId = from e in employees
                       orderby e.DepartmentId
                       group e by e.DepartmentId;
foreach (var group in groupbyEmpDeptId) {
    System.Console.WriteLine($"Department Id: {group.Key}");
    foreach (Employee e in group) {
        System.Console.WriteLine($"\tFull Name: {e.FirstName} {e.LastName}");
    }
}

System.Console.WriteLine();
System.Console.WriteLine("______Quantifier Operations ______");
System.Console.WriteLine("______ All, Any, Contains ______");
var annualSalaryCompare = 40000;
bool isTrueAll = employees.All(e => e.AnnualSalary > annualSalaryCompare);
if (isTrueAll) {
    System.Console.WriteLine($"All salaries are above {annualSalaryCompare}");
}
else {
    System.Console.WriteLine($"Not all salaries are above {annualSalaryCompare}");
}

bool isTrueAny = employees.Any(e => e.AnnualSalary > annualSalaryCompare);
if (isTrueAny) {
    System.Console.WriteLine($"At least one salary is greater than {annualSalaryCompare}");
}
else {
    System.Console.WriteLine($"No salary is greater than {annualSalaryCompare}");
}
var searchEmployee = new Employee {
    Id = 3,
    FirstName = "Douglas",
    LastName = "Roberts",
    AnnualSalary = 40000.2m,
    IsManager = false,
    DepartmentId = 1
};
bool containsEmployee = employees.Contains(searchEmployee, new EmployeeComparer());
if (containsEmployee) {
    System.Console.WriteLine($"An employee record was found for {searchEmployee.FirstName} {searchEmployee.LastName}");
}
else {
    System.Console.WriteLine($"An employee record was NOT found for {searchEmployee.FirstName} {searchEmployee.LastName}");
}

System.Console.WriteLine("______ OfType ______");
// OfType is used when an ArrayList has multiple data types. OfType can filter for a specific data type
var departmentResults = from e in employees.OfType<Department>()
                        select e;
foreach (var d in departmentResults) {
    System.Console.WriteLine($"{d.Id}\t{d.LongName}");
}


System.Console.WriteLine();
System.Console.WriteLine("______ Element Operators ______");
// ElementAt() = return object in collection at specified index
var person = employees.ElementAt(2);
System.Console.WriteLine($"{person.Id, -5} {person.FirstName, -10} {person.LastName, -10}");

System.Console.WriteLine();
System.Console.WriteLine("______ ElementAtOrDefault ______");
// ElementAtOrDefault doesn't throw an exception. Rather, it returns a default value
var employeeDefault = employees.ElementAtOrDefault(8);
var employeeReturned = employees.ElementAtOrDefault(1);
System.Console.WriteLine($"employeeDefault: {employeeDefault?.Id}\t{employeeDefault?.FirstName}");
System.Console.WriteLine($"employeeReturned: {employeeReturned?.Id}\t{employeeReturned?.FirstName}");

System.Console.WriteLine();
System.Console.WriteLine("______ FirstOrDefault ______");
List<int> intList = new List<int> {3,13,23,17,27,89};
//default value for integer is 0
int resultFirst = intList.FirstOrDefault(i => i % 2 == 0);
if (resultFirst == 0) {
    System.Console.WriteLine("There are no even numbers in the list");
}
else {
    System.Console.WriteLine($"First even number = {resultFirst}");
}
int resultLast = intList.LastOrDefault(i => i % 2 > 0);
if (resultLast == 0) {
    System.Console.WriteLine("There are no odd numbers in the list");
}
else {
    System.Console.WriteLine($"Last odd number = {resultLast}");
}

System.Console.WriteLine();
System.Console.WriteLine("______ SingleOrDefault ______");
// Because there is no condition specified, this will throw an exception if the list 
// has more than 1 element
// var resultSingle = employees.Single();
// This line throws an exception because there is more than 1 element that satisfies the criteria
// An exception is not thrown when there are 1 or 0 elements that satisfy the condition
// var resultSingle = employees.SingleOrDefault(e => e.Id > 1);

System.Console.WriteLine();
System.Console.WriteLine("______ Linq Operators ______");
System.Console.WriteLine("______ SquenceEqual ______");
var intList1 = new List<int> {1,2,3,4,5,6};
var intList2 = new List<int> {1,2,3,4,5,6};
var intList3 = new List<int> {1,2,3,4,6,5};

var boolSeqEqual = intList1.SequenceEqual(intList2);
System.Console.WriteLine($"boolSeqEqual\t=\t{boolSeqEqual}");

var boolSeqEqual1 = intList1.SequenceEqual(intList3);
System.Console.WriteLine($"boolSeqEqual1\t=\t{boolSeqEqual1}");

var employeesBase = Data.GetEmployees();
var employeesCompare = Data.GetEmployees();
// False because nothing defines how to compare the 2 lists
var boolSE = employeesBase.SequenceEqual(employeesCompare);
System.Console.WriteLine($"boolSE\t=\t{boolSE}");
var boolSE1 = employeesBase.SequenceEqual(employeesCompare, new EmployeeComparer());
System.Console.WriteLine($"boolSE1\t=\t{boolSE1}");

System.Console.WriteLine("______ Concat ______");
var intList4 = new List<int> {1,2,3,4,5};
var intList5 = new List<int> {6,7,8,9};
IEnumerable<int> intListConcat = intList4.Concat(intList5);
foreach (var i in intListConcat) {
    System.Console.WriteLine(i);
}
System.Console.WriteLine();

List<Employee> employeesSuperHero = new List<Employee> {
    new Employee{Id = 10, FirstName = "Tony", LastName = "Stark"},
    new Employee{Id = 11, FirstName = "Bruce", LastName = "Wayne"}
};
var employeesConcat = employeesBase.Concat(employeesSuperHero);
foreach (var e in employeesConcat) {
    System.Console.WriteLine($"{e.Id,-5} {e.FirstName,-10} {e.LastName,-10}");
}
System.Console.WriteLine();

System.Console.WriteLine("______ Aggregates ______");
// sum of total salaries - managers get 4% bonus, non-managers get 2% bonus
decimal totalAnnualSalary = employees.Aggregate<Employee, decimal> (0, (totalAnnualSalary, e) => {
    var bonus = (e.IsManager) ? 0.04m : 0.02m;
    totalAnnualSalary += e.AnnualSalary * (1 + bonus);
    return totalAnnualSalary;
});
foreach (var e in employees) {
    System.Console.WriteLine($"{e.Id}\t{e.FirstName}\t{e.LastName}\t{e.IsManager}\t{e.AnnualSalary}");
}
System.Console.WriteLine($"Total annual salary of all employees: {totalAnnualSalary}");

System.Console.WriteLine();
string data = employees.Aggregate<Employee, string> ("Employee Annual Salaries (including bonus)\n", 
    (s, e) => {
        var bonus = e.IsManager ? 0.04m : 0.02m;
        s += $"\t{e.FirstName} {e.LastName} - {e.AnnualSalary * (1+bonus)}\n";
        return s;
    });
System.Console.WriteLine(data);

int sumList4 = intList4.Aggregate<int, int>(0, (i, n) => i + n);
string msg = "";
foreach (int i in intList4) {
    if (msg.Equals("")) {
        msg = i.ToString();
    }
    else {
        msg += ", " + i.ToString();
    }
}
System.Console.WriteLine("intList4: " + msg);
System.Console.WriteLine($"sumIntList4 = {sumList4}");

System.Console.WriteLine();

System.Console.WriteLine("______ Average ______");
// avg salary of employees in tech department
decimal avgSalaryTech = employees.Where(e => e.DepartmentId == 3).Average(e => e.AnnualSalary);
System.Console.WriteLine($"Average tech salary: {avgSalaryTech}");

var techEmployees = from e in employees
                    where e.DepartmentId == 3
                    select e;
var avgSalaryTechLinq = techEmployees.Average(e => e.AnnualSalary);
System.Console.WriteLine($"Average tech salary (LINQ): {avgSalaryTechLinq}");

System.Console.WriteLine();
System.Console.WriteLine("______ Count ______");
var employeeTechCountLinq = techEmployees.Count(e => e.DepartmentId == 3);
var employeTechCount = employees.Count(e => e.DepartmentId == 3);
System.Console.WriteLine($"employeeTechCountLinq: {employeeTechCountLinq}");
System.Console.WriteLine($"employeTechCount: {employeTechCount}");

System.Console.WriteLine();
System.Console.WriteLine("______ Sum ______");
var totalSalary = employees.Sum(e => e.AnnualSalary);
System.Console.WriteLine($"Total Salary: {totalSalary}");

System.Console.WriteLine();
System.Console.WriteLine("______ Max ______");;
var maxSalary2 = employees.Max(e => e.AnnualSalary);
var maxEmployees = employees.Where(e => e.AnnualSalary == maxSalary2);
System.Console.WriteLine($"Max Salary: {maxSalary2}");
foreach (var e in maxEmployees) {
    System.Console.WriteLine($"{e.FirstName}\t{e.LastName}\t{e.AnnualSalary}");
}

System.Console.WriteLine();
System.Console.WriteLine("______ Generation Operators ______");
System.Console.WriteLine("______ DefaultIfEmpty ______");
List<Employee> emptyEmployees = new List<Employee>();
// var emptyResult = emptyEmployees.ElementAt(0);  --> Exception: Index out of range
// System.Console.WriteLine(emptyResult.ToString());
var isEmptyEmployees = emptyEmployees.DefaultIfEmpty(new Employee {Id = 0, FirstName = "Daffy", LastName = "Duck"}).ElementAt(0);
if (isEmptyEmployees.Id == 0 && isEmptyEmployees.FirstName == "Daffy" && isEmptyEmployees.LastName == "Duck") {
    System.Console.WriteLine("The list is empty.");
}

emptyEmployees.Add(new Employee {Id = 42, FirstName = "Sylvester", LastName = "Cat"});
isEmptyEmployees = emptyEmployees.DefaultIfEmpty(new Employee {Id = 0, FirstName = "Daffy", LastName = "Duck"}).ElementAt(0);
if (isEmptyEmployees.Id == 0 && isEmptyEmployees.FirstName == "Daffy" && isEmptyEmployees.LastName == "Duck") {
    System.Console.WriteLine("The list is empty");
}
else {
    System.Console.WriteLine("The list is not empty");
}

System.Console.WriteLine();
System.Console.WriteLine("______ Empty ______");
// Creates an empty collection
List<Employee> emptyEmployeeList = Enumerable.Empty<Employee>().ToList();
emptyEmployeeList.Add(new Employee {Id = 0, FirstName = "Tweety", LastName = "Bird"});
foreach (var e in emptyEmployeeList) {
    System.Console.WriteLine($"{e.FirstName} {e.LastName}");
}

System.Console.WriteLine();
System.Console.WriteLine("______ Range ______");
var intCollection = Enumerable.Range(25, 6);
foreach (var i in intCollection) {
    System.Console.WriteLine(i);
}

System.Console.WriteLine();
System.Console.WriteLine("______ Repeat ______");
var strCollection = Enumerable.Repeat<string>("All work and no play makes Jeff a dull boy", 5);
foreach (var m in strCollection)
    System.Console.WriteLine(m);

System.Console.WriteLine();
System.Console.WriteLine("______ Set operators ______");
System.Console.WriteLine("______ Distinct ______");
List<int> repeatList = new List<int> {1,2,1,4,6,2,6,7,8,7,7};
System.Console.WriteLine("Original List");
msg = "";
foreach (int i in repeatList) {
    if (msg.Equals(""))
        msg = i.ToString();
    else
        msg += $", {i.ToString()}";
}
System.Console.WriteLine(msg);

var distinctList = repeatList.Distinct();
System.Console.WriteLine("Distinct List");
msg = "";
foreach (int i in distinctList) {
    if (msg.Equals(""))
        msg = i.ToString();
    else
        msg += $", {i.ToString()}";
}
System.Console.WriteLine(msg);


System.Console.WriteLine();
// which elements are only in collection1
System.Console.WriteLine("______ Except - unique elements only in collection 1 ______");
IEnumerable<int> collection1 = new List<int> {1,2,3,4};
IEnumerable<int> collection2 = new List<int> {3,4,5,6};
msg = "collection1: ";
foreach (int i in collection1) {
    if (msg.Equals("collection1: "))
        msg += i.ToString();
    else
        msg += $", {i.ToString()}";
}
System.Console.WriteLine(msg);
msg = "collection2: ";
foreach (int i in collection2) {
    if (msg.Equals("collection2: "))
        msg += i.ToString();
    else
        msg += $", {i.ToString()}";
}
System.Console.WriteLine(msg);

var collection1exceptions = collection1.Except(collection2);
msg = "collection1exceptions: ";
foreach (int i in collection1exceptions) {
    if (msg.Equals("collection1exceptions: "))
        msg += i.ToString();
    else
        msg += $", {i.ToString()}";
}
System.Console.WriteLine(msg);

Employee emp1 = new Employee {
    Id = 1,
    FirstName = "Jerry",
    LastName = "Seinfeld",
    AnnualSalary = 20000.0m,
    IsManager = true,
    DepartmentId = 1
};
Employee emp2 = new Employee {
    Id = 2,
    FirstName = "Kosmo",
    LastName = "Kramer",
    AnnualSalary = 10000.0m,
    IsManager = false,
    DepartmentId = 2
};
Employee emp3 = new Employee {
    Id = 3,
    FirstName = "Elaine",
    LastName = "Benes",
    AnnualSalary = 40000.0m,
    IsManager = true,
    DepartmentId = 1
};
Employee emp4 = new Employee {
    Id = 4,
    FirstName = "George",
    LastName = "Costanza",
    AnnualSalary = 15000.0m,
    IsManager = false,
    DepartmentId = 2
};
Employee emp5 = new Employee {
    Id = 5,
    FirstName = "J",
    LastName = "Peterman",
    AnnualSalary = 60505.0m,
    IsManager = true,
    DepartmentId = 3
};

List<Employee> emp1collection = new List<Employee>
{
    emp1,
    emp2,
    emp3,
    emp4,
    emp1,
    emp2
};

List<Employee> emp2collection = new List<Employee>
{
    emp4,
    emp5
};

System.Console.WriteLine("emp1collection:");
foreach (var e in emp1collection)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}\t{e.AnnualSalary}\t{e.IsManager}\t{e.DepartmentId}");

System.Console.WriteLine("emp2collection:");
foreach (var e in emp2collection)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}\t{e.AnnualSalary}\t{e.IsManager}\t{e.DepartmentId}");

var emp1exceptions = emp1collection.Except(emp2collection, new EmployeeComparer());
System.Console.WriteLine("emp1exceptions:");
foreach (var e in emp1exceptions)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}");


System.Console.WriteLine();
// which elements are in both collections
System.Console.WriteLine("______ Intersect - elements in both collections ______");
var empIntersection = emp1collection.Intersect(emp2collection, new EmployeeComparer());
foreach (var e in empIntersection)
    System.Console.WriteLine($"{e.Id}\t{e.FirstName}\t{e.LastName}");

System.Console.WriteLine();
// distinct elements from both collections
System.Console.WriteLine("______ Union - unique elements after merging 2 collections ______");
var empUnion = emp1collection.Union(emp2collection, new EmployeeComparer());
foreach(var e in empUnion)
    System.Console.WriteLine($"{e.Id}\t{e.FirstName}\t{e.LastName}");


System.Console.WriteLine();
System.Console.WriteLine("______ Partitioning Operators ______");
System.Console.WriteLine("______ Skip - similar to a right() method ______");
var emp1Skip2 = emp1collection.Skip(2);
foreach (var e in emp1Skip2)
    System.Console.WriteLine($"{e.Id}\t{e.FirstName}\t{e.LastName}");

System.Console.WriteLine("______ SkipWhile - skips all elements that match criteria until the first false ______");
var emp1SkipWhileIsManager = emp1collection.SkipWhile(e => e.IsManager);
var emp1SkipWhileSalary30k = emp1collection.SkipWhile(e => e.AnnualSalary < 30000);
System.Console.WriteLine("SkipWhileIsManager");
foreach (var e in emp1SkipWhileIsManager)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}");

System.Console.WriteLine("SkipWhileSalaryLT30k");
foreach (var e in emp1SkipWhileSalary30k)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}");


System.Console.WriteLine();
System.Console.WriteLine("______ Take - similar to left() and mid() methods ______");
var empTake = emp1collection.Take(2);
System.Console.WriteLine("empTake(2)");
foreach (var e in empTake)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}");

Range range = 2..4;
// ^1 --> the last element
// ..4 --> 0,1,2,3
// 4.. --> 4,5 --> emp1collection has indices 0-5
var empTakeMid = emp1collection.Take(range);
System.Console.WriteLine("empTake(2..4)");
foreach (var e in empTakeMid)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}");

System.Console.WriteLine();
System.Console.WriteLine("______ TakeWhile - returns all elements that match criteria and skips everything else ______");
var empTakeWhileLT30k = emp1collection.TakeWhile(e => e.AnnualSalary < 30000);
System.Console.WriteLine("empTakeWhileLT30k");
foreach (var e in empTakeWhileLT30k)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}");

System.Console.WriteLine();
System.Console.WriteLine("______ ToList() ______");
var empWhereSalary40k = (from e in emp1collection
                        where e.AnnualSalary == 40000
                        select e).ToList();
foreach (var e in empWhereSalary40k) 
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}");

System.Console.WriteLine();
System.Console.WriteLine("______ ToDictionary() ______");
Dictionary<int, Employee> emp2Dict = (from e in emp2collection
                                      where e.IsManager == true
                                      select e).ToDictionary<Employee, int> (e => e.Id);
System.Console.WriteLine("emp2Dict");
foreach (var k in emp2Dict.Keys)
    System.Console.WriteLine($"\tKey: {k}\tValue: {emp2Dict[k].FirstName}\t{emp2Dict[k].LastName}");

System.Console.WriteLine();
System.Console.WriteLine("______ let - create new columns that can be used in WHERE clause ______");
var emp1Let = from e in emp1collection
              let Initials = e.FirstName[..1].ToUpper() + e.LastName[..1].ToUpper()
              let SalaryPlusBonus = e.IsManager ? e.AnnualSalary * (1 + 0.04m) : e.AnnualSalary * (1 + 0.02m)
              where Initials == "GC" || e.AnnualSalary < 20000
              select new {
                Initials = Initials,
                FullName = e.FirstName + " " + e.LastName,
                IsManager = e.IsManager,
                AnnualSalary = e.AnnualSalary,
                SalaryPlusBonus = SalaryPlusBonus
              };
System.Console.WriteLine("emp1Let");
foreach (var e in emp1Let)
    System.Console.WriteLine($"\t{e.Initials}\t{e.FullName}\t{e.IsManager}\t{e.AnnualSalary}\t{e.SalaryPlusBonus}");

System.Console.WriteLine();
System.Console.WriteLine("______ Into - used to group things and then able to use it in where clause ______");
var emp1Into = from e in emp1collection
               where e.AnnualSalary > 30_000
               select e
               into HighEarners
               where HighEarners.IsManager == true
               select HighEarners;
System.Console.WriteLine("emp1Into");
foreach (var e in emp1Into)
    System.Console.WriteLine($"\t{e.Id}\t{e.FirstName}\t{e.LastName}\t{e.IsManager}\t{e.AnnualSalary}");

System.Console.WriteLine();
System.Console.WriteLine("______ Projection Operators ______");
System.Console.WriteLine("______ Select ______");
var emp1Select = departments.Select(d => d.Employees); // list of lists
foreach (var eList in emp1Select) {
    if (eList != null)
        foreach (var e in eList)
            System.Console.WriteLine($"{e.Id, -3}{e.FirstName, -10}{e.LastName, -15}{e.IsManager, -7}{e.AnnualSalary, -15:c}{e.DepartmentId}");
    System.Console.WriteLine("-------------------------------------------------------");
}


System.Console.WriteLine();
System.Console.WriteLine("______ SelectMany - flattens results - returns list instead of list of lists ______");
var empSelectMany = departments.SelectMany(d => d.Employees ?? Enumerable.Empty<Employee>());
foreach (var e in empSelectMany)
    System.Console.WriteLine($"{e.Id, -3}{e.FirstName, -10}{e.LastName, -15}{e.IsManager, -7}{e.AnnualSalary, -15:C}{e.DepartmentId}");

// Example from linqpad.net's home page
// from p in Products
// let totalStock = p.productInventories.Sum(pi => pi.quantity)
// where totalStock < p.reorderPoint
// select new {
//  p.Name,
//  totalStock,
//  p.reorderPoint,
//  VendorDetails = from pv in p.ProductVendors
//                  select new {
//                      pv.Vendor.Name,
//                      pv.AverageLeadTime,
//                      pv.MinOrderQty
//                  }
// }


System.Console.WriteLine();
// ---------------------------------------------------------------
// ---------------------------------------------------------------

public class EmployeeComparer : IEqualityComparer<Employee>
{
    public bool Equals(Employee? x, Employee? y) {
        if (x == null || y == null) {
            return false;
        }

        return x.Id == y.Id && 
            x.FirstName.ToLower() == y.FirstName.ToLower() && 
            x.LastName.ToLower() == y.LastName.ToLower();
    }

    public int GetHashCode([DisallowNull] Employee obj) {
        return obj.Id.GetHashCode();
    }
}

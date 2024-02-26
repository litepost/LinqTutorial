namespace TPCData;
public class Department
{
    public int Id { get; set; }
    public required string ShortName { get; set; }
    public required string LongName { get; set; }
    public IEnumerable<Employee>? Employees { get; set; }

}

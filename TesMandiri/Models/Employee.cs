namespace TesMandiri.Models;

public class EmployeeBase
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
}
public class Employee : EmployeeBase
{
    public EmployeeIdCard EmployeeCardId { get; set; } = new();
    public Division DivisionId { get; set; } = new();
    public List<Task> Tasks { get; set; } = new();
}

public class EmployeeIdCard
{
    public int CardId { get; set; }
    public int CardNumber { get; set; }
    public EmployeeBase Employee { get; set; } = new();
}
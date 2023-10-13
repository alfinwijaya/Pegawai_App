namespace TesMandiri.Models;

public class Task
{
    public int TaskId { get; set; }
    public string TaskName { get; set; } = string.Empty;
    public List<EmployeeBase> Employee { get; set; } = new();
}

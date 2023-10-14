using System.Text.Json.Serialization;

namespace TesMandiri.Models;

public class TaskBase
{
    public string TaskCode { get; set; } = string.Empty;
    public string? TaskName { get; set; }
}

public class Task : TaskBase
{
    public List<EmployeeBase> Employee { get; set; } = new();
}
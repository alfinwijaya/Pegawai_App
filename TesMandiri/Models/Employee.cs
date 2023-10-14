using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;
using TesMandiri.Models;

namespace TesMandiri.Models;

public class EmployeeBase
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
}

public class EmployeeCard : EmployeeBase
{
    public IdCardBase Card { get; set; } = new();
}

public class EmployeeDto
{
    public int EmployeeId { get; set; }

}

public class EmployeeCardDto : EmployeeDto
{

    public int CardNumber { get; set; }
}

public class EmployeeTask : EmployeeBase
{
    public List<TaskBase> Tasks { get; set; } = new();
}

public class EmployeeTaskDto : EmployeeDto
{
    public List<TaskDto> Tasks { get; set; } = new();
}
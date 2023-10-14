using System.Text.Json.Serialization;

namespace TesMandiri.Models;

public class DivisionBase
{
    public string DivisionCode { get; set; } = string.Empty;
    public string? DivisionName { get; set; }
}

public class Division : DivisionBase
{
    public List<EmployeeBase> Employee { get; set; } = new();
}

public class DivisionMemberDto
{
    public string DivisionCode { get; set; } = string.Empty;
    public List<EmployeeDto> Employee { get; set; } = new();
}
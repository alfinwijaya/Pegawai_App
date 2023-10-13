namespace TesMandiri.Models;

public class Division
{
    public int DivisionId { get; set; }
    public string DivisionName { get; set; } = string.Empty;
    public List<EmployeeBase> Employee { get; set; } = new();
}

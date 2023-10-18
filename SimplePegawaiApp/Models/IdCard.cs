namespace TesMandiri.Models;

public class IdCardBase
{
    public int CardNumber { get; set; }
    public string? CardDescription { get; set; }
}

public class IdCard : IdCardBase
{
    public EmployeeBase Employee { get; set; } = new();

}
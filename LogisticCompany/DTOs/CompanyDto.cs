namespace LogisticCompany.DTOs;

public class CompanyDto
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public DateOnly CreationDate { get; set; }
    public List<EmployeeDto>? Employees { get; set; } 
}
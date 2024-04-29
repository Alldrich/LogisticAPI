using Carter;
using LogisticCompany.data;
using LogisticCompany.DTOs;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace LogisticCompany.controllers;

public class CompanyController : ICarterModule
{

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("company").WithOpenApi();
        group.MapGet("/", GetCompanies).WithName("GetCompanies").RequireAuthorization();
        group.MapGet("{id:int}", GetCompaniesById).WithName("GetCompaniesById");
    }

    private static async Task<Results<Ok<List<CompanyDto>>, NotFound<string>>> GetCompaniesById(int id,
        LogisticcompanyContext context
    )
    {
        var company = await context.Companies
            .Where(c => c.Id == id)
            .Include(company => company.Employees)
            .Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                CreationDate = c.CreationDate,
                Employees = c.Employees.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToList()
            }).ToListAsync();
        if (company.Count == 0) return TypedResults.NotFound($"Company with id {id} was not found");
        return TypedResults.Ok(company);
    }

    private static async Task<Results<Ok<List<CompanyDto>>, NotFound>> GetCompanies(LogisticcompanyContext context)
    {
        var company = await context.Companies
            .Include(company => company.Employees)
            .Select(c => new CompanyDto
            {
                Id = c.Id,
                Name = c.Name,
                CreationDate = c.CreationDate,
                Employees = c.Employees.Select(e => new EmployeeDto
                {
                    Id = e.Id,
                    Name = e.Name
                }).ToList()
            }).ToListAsync();
        if (company.Count == 0) return TypedResults.NotFound();
        return TypedResults.Ok(company);
    }
}
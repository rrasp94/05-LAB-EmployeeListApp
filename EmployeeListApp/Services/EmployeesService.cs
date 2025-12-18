using EmployeeListApp.Data;
using Microsoft.EntityFrameworkCore;

namespace EmployeeListApp.Services;

public class EmployeesService
{
    private readonly AppDbContext _db;

    public EmployeesService(AppDbContext db)
    {
        _db = db;
    }

    public async Task<List<Employee>> GetAllAsync()
    {
        return await _db.Employees
                        .AsNoTracking()
                        .OrderBy(e => e.FullName)
                        .ToListAsync();
    }

    public async Task<Employee?> GetByIdAsync(Guid id)
    {
        return await _db.Employees
                        .AsNoTracking()
                        .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task AddAsync(Employee employee)
    {
        _db.Employees.Add(employee);
        await _db.SaveChangesAsync();
    }

    public async Task UpdateAsync(Employee employee)
    {
        var existing = await _db.Employees.FindAsync(employee.Id);
        if (existing == null)
            throw new Exception("Employee not found.");

        existing.FullName = employee.FullName;
        existing.Department = employee.Department;
        existing.Salary = employee.Salary;

        await _db.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var employee = await _db.Employees.FindAsync(id);
        if (employee == null)
            throw new Exception("Employee not found.");

        _db.Employees.Remove(employee);
        await _db.SaveChangesAsync();
    }
}

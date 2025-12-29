using EmployeePortal.Data;
using EmployeePortal.Models;
using EmployeePortal.Models.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography.Xml;

namespace EmployeePortal.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly ApplicationDbContext _dbContext;

        
        public EmployeesController(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }


        [HttpGet]
        public IActionResult GetAllEmployee()
        {
            return Ok(_dbContext.Employees.ToList());
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<Employee>> GetEmployeeById(Guid id)
        {
            var employee = await _dbContext.Employees
                                           .AsNoTracking()
                                           .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound($"Employee with id {id} not found");
            }

            return Ok(employee);
        }

        [HttpPost]
        public IActionResult AddEmployee(AddEmployeeDto addEmployeeDto) {

            var employeeEntity = new Employee() {
                Name = addEmployeeDto.Name,
                Email = addEmployeeDto.Email,
                Phone = addEmployeeDto.Phone,
                Salary = addEmployeeDto.Salary
            };

            _dbContext.Employees.Add(employeeEntity);
            _dbContext.SaveChanges();

            return Ok(employeeEntity);
        
        }


        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Employee>> UpdateEmployee(Guid id,UpdateEmployeeDto updateEmployeeDto)
        {
            var employee = await _dbContext.Employees
                                           .FirstOrDefaultAsync(e => e.Id == id);

            if (employee == null)
            {
                return NotFound($"Employee with id {id} not found");
            }

            employee.Name = updateEmployeeDto.Name;
            employee.Email = updateEmployeeDto.Email;
            employee.Phone = updateEmployeeDto.Phone;
            employee.Salary = updateEmployeeDto.Salary;

            await _dbContext.SaveChangesAsync();

            return Ok(employee);
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> DeleteEmployee(Guid id)
        {
            var employee = await _dbContext.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound($"Employee with id {id} not found");
            }

            _dbContext.Employees.Remove(employee);
            await _dbContext.SaveChangesAsync();

            return NoContent(); // 204
        }
    }
}

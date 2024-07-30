using Castor_employee.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Castor_employee.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : ControllerBase
    {
        private readonly CastorEmployeeDbContext _context;

        public EmployeesController(CastorEmployeeDbContext context)
        {
            _context = context;
        }

        // Implementar métodos CRUD
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            return await _context.Employees.Include(e => e.IdCargoNavigation).ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Employee>> GetEmployee(int id)
        {
            var employee = await _context.Employees.Include(e => e.IdCargoNavigation).FirstOrDefaultAsync(e => e.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        [HttpPost]
        public async Task<IActionResult> PostEmployee([FromForm] Employee employee, IFormFile file)
        {
            if (file != null && file.Length > 0)
            {
                //var filePath = Path.Combine("wwwroot/images", file.FileName);
                var filePath = Path.Combine("ClientApp/src/assets/images/", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                employee.FotoPath = $"/images/{file.FileName}";
            }

            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmployee), new { id = employee.Id }, employee);
        }


        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, [FromForm] Employee employee, IFormFile file)
        {
            if (id != employee.Id)
            {
                return BadRequest();
            }

            if (file != null && file.Length > 0)
            {
                var filePath = Path.Combine("ClientApp/src/assets/images/", file.FileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(stream);
                }

                employee.FotoPath = $"/images/{file.FileName}";
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
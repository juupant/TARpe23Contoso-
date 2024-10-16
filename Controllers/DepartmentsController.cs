using Contoso_University.Data;
using Contoso_University.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Contoso_University.Controllers
{
    public class DepartmentsController : Controller
    {
        private readonly SchoolContext _context;
        public DepartmentsController(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var schoolContext = _context.Departments.Include(department => department.Administrator);
            return View(await schoolContext.ToListAsync());
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string query = "SELECT * FROM Departments WHERE DepartmentID = {0}";
            var department = await _context.Departments.FromSqlRaw(query, id).Include(d => d.Administrator).AsNoTracking().FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }


        [HttpGet]
        public IActionResult Create()
        {
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Budget,StartDate,RowVersion,InstructorID,SuperImportantString")] Department department)
        {
            if (ModelState.IsValid)
            {
                _context.Add(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName", department.InstructorID);
            return View(department);
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string query = "SELECT * FROM Departments WHERE DepartmentID = {0}";
            var department = await _context.Departments.FromSqlRaw(query, id).Include(d => d.Administrator).AsNoTracking().FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View(department);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit([Bind("DepartmentID,InstructorID,Name,Budget,StartDate,SuperImportantString")] Department department)
        {
            if (ModelState.IsValid)
            {
                var existingDepartment = _context.Departments.AsNoTracking().FirstOrDefault(m => m.DepartmentID == department.DepartmentID);

                if (existingDepartment == null)
                {
                    return NotFound();
                }

                _context.Departments.Update(department);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            ViewData["InstructorID"] = new SelectList(_context.Instructors, "ID", "FullName");
            return View(department);
        }

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            string query = "SELECT * FROM Departments WHERE DepartmentID = {0}";
            var department = await _context.Departments.FromSqlRaw(query, id).Include(d => d.Administrator).AsNoTracking().FirstOrDefaultAsync();
            if (department == null)
            {
                return NotFound();
            }
            return View(department);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var department = await _context.Departments.FindAsync(ID);
            _context.Departments.Remove(department);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }


    }
}

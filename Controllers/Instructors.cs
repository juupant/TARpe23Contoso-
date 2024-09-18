using Contoso_University.Data;
using Contoso_University.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Contoso_University.Controllers
{
    public class InstructorsController : Controller
    {
        private readonly SchoolContext _context;
        public InstructorsController(SchoolContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int? id, int? courseId)
        {
            var vm = new InstructorIndexData();
            vm.Instructors = await _context.Instructors
                .Include(i => i.OfficeAssignment)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .ThenInclude(i => i.Enrollments)
                .ThenInclude(i => i.Student)
                .Include(i => i.CourseAssignments)
                .ThenInclude(i => i.Course)
                .AsNoTracking()
                .OrderBy(i => i.LastName)
                .ToListAsync();
            if (id != null)
            {
                ViewData["InstructorID"] = id.Value;
                Instructor instructor = vm.Instructors
                    .Where(i => i.ID == id.Value).Single();
                vm.Courses = instructor.CourseAssignments
                    .Select(i => i.Course);

            }
            if (courseId != null)
            {
                ViewData["CourseID"] = courseId.Value;
                vm.Enrollments = vm.Courses
                    .Where(x => x.CourseId == courseId.Value)
                    .Single().Enrollments;

            }
            return View(vm);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var instructor = new Instructor();
            instructor.CourseAssignments = new List<CourseAssignment>();
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Instructor instructor)
        {
            /*
            if (selectedCourses == null)
            {
                instructor.CourseAssignments = new List<CourseAssignment>();
                foreach (var course in selectedCourses)
                {
                    var courseToAdd = new CourseAssignment { InstructorID = instructor.ID, CourseID = course };
                    instructor.CourseAssignments.Add(courseToAdd);
                }

            }
            */
            //ModelState.Remove(selectedCourses);
            //ModelState.Remove();
            if (ModelState.IsValid)
            {
                _context.Add(instructor);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //PopulateAssignedCourseData(instructor); // uuendab instructori juures olevaid kursuseid
            return View(instructor);
        }

        private void PopulateAssignedCourseData(Instructor instructor)
        {
            var allCourses = _context.Courses;
            var instructorCourses = new HashSet<int>(instructor.CourseAssignments.Select(c => c.CourseID));
            var vm = new List<AssignedCourseData>();
            foreach (var course in allCourses)
            {
                vm.Add(new AssignedCourseData { CourseID = course.CourseId, Title = course.Title, Assigned = instructorCourses.Contains(course.CourseId) });
            }
            ViewData["Courses"] = vm;

        }
        public async Task<IActionResult> Delete(int? ID)
        {
            if (ID == null)
            {
                return NotFound();
            }
            var instructor = await _context.Instructors.FirstOrDefaultAsync(m => m.ID == ID);
            if (instructor == null)
            {
                return NotFound();
            }

            return View(instructor);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int ID)
        {
            var instructor = await _context.Instructors.FindAsync(ID);
            _context.Instructors.Remove(instructor);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) //checkib et null ei ole
            {
                return NotFound();
            }

            var instructor = await _context.Instructors.FindAsync(id);
            if (instructor == null) //checkib et null ei ole
            {
                return NotFound();
            }
            return View(instructor);
        }
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,LastName,FirstMidName,EnrollmentDate")] Instructor instructor) //edit 
        {
            if (id != instructor.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(instructor);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!InstructorExists(instructor.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(instructor);
        }
        private bool InstructorExists(int id)
        {
            return _context.Instructors.Any(e => e.ID == id);
        }
        
    }
}

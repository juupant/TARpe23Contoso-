﻿using Contoso_University.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Contoso_University.Models;

namespace Contoso_University.Controllers
{
    public class CoursesController : Controller
    {
        private readonly SchoolContext _context;
        public CoursesController(SchoolContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            return View(await _context.Courses.ToListAsync());
        }
        [HttpGet, ActionName("Details")]
        public async Task<IActionResult> Details(int? id, string name)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }

            if (name != "Details" && name != "Delete")
            {
                return NotFound();
            }
            ViewBag.Title = name;
            return View(course);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteCourse(int? courseId)
        {
            if (courseId == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(courseId);
            if (course == null)
            {
                return NotFound();
            }
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        public async Task<IActionResult> Clone(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            var biggestCourseId = _context.Courses.OrderByDescending(m => m.CourseId).First();
            var clonedCourse = new Course
            {
                CourseId = biggestCourseId.CourseId + 1,
                Title = course.Title,
                Credits = course.Credits,
            };
            _context.Add(clonedCourse);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
        [HttpGet]
        public async Task<IActionResult> CreateEdit(int? id)
        {
            if (id == null)
            {
                ViewBag.Title = "Create";
                ViewBag.Description = "Create a new course";
                return View();
            }
            var course = await _context.Courses.FirstOrDefaultAsync(m => m.CourseId == id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Title = "Edit";
            ViewBag.Description = "Edit a course";
            return View(course);
        }
        [HttpPost]
        public async Task<IActionResult> CreateEdit(Course course)
        {
            if (ModelState.IsValid)
            {
                if (course.CourseId == 0)
                {
                    var biggestCourseId = _context.Courses.OrderByDescending(m => m.CourseId).First();
                    course.CourseId = biggestCourseId.CourseId + 1;
                    _context.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                var existingCourse = _context.Courses.AsNoTracking().FirstOrDefault(m => m.CourseId == course.CourseId);
                if (existingCourse == null)
                {
                    return NotFound();
                }
                _context.Update(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");

            }
            return View(course);
        }

    }
}
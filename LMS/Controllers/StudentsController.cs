using LMS.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LMS.Controllers
{
    [Route("students")]
    public class StudentsController : Controller
    {
        private readonly LMSDbContext _context;

        public StudentsController(LMSDbContext context)
        {
            _context = context;
        }

        // GET: /students
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .OrderByDescending(s => s.Id)
                .ToListAsync();

            return View(students);
        }

        // GET: /students/{id}
        [HttpGet("{id:int}", Name = "StudentDetails")]
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                TempData["ErrorMessage"] = $"Student with ID {id} not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        // GET: /students/create
        [HttpGet("create")]
        public IActionResult Create()
        {
            return View();
        }

        // POST: /students (Create)
        [HttpPost]
        [Route("create")] // ← Bu qo'shildi
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                // Email uniqueness check
                if (!string.IsNullOrEmpty(student.Email))
                {
                    var existingStudent = await _context.Students
                        .FirstOrDefaultAsync(s => s.Email == student.Email);

                    if (existingStudent != null)
                    {
                        ModelState.AddModelError("Email", "Bu email allaqachon ro'yxatdan o'tgan.");
                        return View(student);
                    }
                }

                student.CreatedAt = DateTime.UtcNow;
                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                TempData["SuccessMessage"] = $"{student.FullName} muvaffaqiyatli qo'shildi!";
                return RedirectToAction(nameof(Index));
            }

            // Validation xatolarini ko'rsatish uchun
            return View(student);
        }

        // GET: /students/{id}/edit
        [HttpGet("{id:int}/edit")]
        public async Task<IActionResult> Edit(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                TempData["ErrorMessage"] = $"Student with ID {id} not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }
        // POST: /students/{id}/edit
        [HttpPost("{id:int}/edit")] // ← Bu qo'shildi
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                TempData["ErrorMessage"] = "Invalid request.";
                return RedirectToAction(nameof(Index));
            }

            if (ModelState.IsValid)
            {
                try
                {
                    student.UpdatedAt = DateTime.UtcNow;
                    _context.Update(student);
                    await _context.SaveChangesAsync();

                    TempData["SuccessMessage"] = $"{student.FullName} ma'lumotlari yangilandi!";
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(id))
                    {
                        TempData["ErrorMessage"] = "Student topilmadi.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        // GET: /students/{id}/delete
        [HttpGet("{id:int}/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var student = await _context.Students.FindAsync(id);

            if (student == null)
            {
                TempData["ErrorMessage"] = $"Student with ID {id} not found.";
                return RedirectToAction(nameof(Index));
            }

            return View(student);
        }

        // POST: /students/{id}/delete
        [HttpPost("{id:int}/delete")] // ← To'g'rilandi
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletee(int id) // ← To'g'rilandi
        {
            var student = await _context.Students.FindAsync(id);

            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{student.FullName} o'chirildi.";
            }

            return RedirectToAction(nameof(Index));
        }

        // Helper method
        private bool StudentExists(int id)
        {
            return _context.Students.Any(e => e.Id == id);
        }
    }
}
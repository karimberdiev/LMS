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
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .OrderByDescending(s => s.Id)
                .ToListAsync();
            return View(students);
        }
        [HttpGet("{id:int}",Name = "StudentDetails")]
        public async Task<IActionResult> Details(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if(student == null)
            {
                TempData["ErrorMessage"] = $"Student with id {id} not found";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        [HttpGet("create")]
        public IActionResult Create() => View();
        [HttpPost]
        [Route("")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Student student)
        {
            if (ModelState.IsValid)
            {
                if (!string.IsNullOrEmpty(student.Email))
                {
                    var existingStudent = await _context.Students.FirstOrDefaultAsync(s => s.Email == student.Email);
                    if (existingStudent != null)
                    {
                        ModelState.AddModelError("Email", "Bu Email allaqachon ro'yxatdan o'tgan");
                        return View(student);
                    }
                }
                student.CreatedAt = DateTime.Now;
                _context.Students.Add(student);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = $"{student.FullName} muvaffaqiyatli ro'yxatdan o'tdi!";
                return RedirectToAction(nameof(Index));
            }
            var errors = ModelState.Values.SelectMany(v => v.Errors);
            return View(student);
        }
        [HttpGet("{id:int}/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.Students.FindAsync(id);
            if (student == null) 
            {
                TempData["ErrorMessage"] = $"Talaba topilmadi Id = {id}";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        [HttpPost("{id:int}/edit")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Student student)
        {
            if (id != student.Id)
            {
                TempData["ErrorMessage"] = "Id xato";
                return RedirectToAction(nameof(Index));
            }
            if (ModelState.IsValid)
            {
                try
                {
                    student.UpdatedAt = DateTime.Now;
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                    TempData["SuccessMessage"] = $"{student.FullName} muvaffaqiyatli o'zgartirildi!";

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!_context.Students.Any(s => s.Id == id))
                    {
                        TempData["ErrorMessage"] = "Talaba topilmadi";
                        return RedirectToAction(nameof(Index));
                    }
                    else throw;
                }
                    return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        [HttpGet("{id:int}/delete")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                TempData["ErrorMessage"] = "Talaba topilmadi";
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }
        [HttpPost("{id:int}/delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Deletee(int id)
        {
            var student = await _context.Students.FindAsync(id);
            if (student != null)
            {
                _context.Students.Remove(student);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}

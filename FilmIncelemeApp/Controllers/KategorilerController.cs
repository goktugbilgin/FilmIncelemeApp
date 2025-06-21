using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FilmIncelemeApp.Models;

namespace FilmIncelemeApp.Controllers
{
    public class KategorilerController : Controller
    {
        private readonly FilmIncelemeContext _context;

        public KategorilerController(FilmIncelemeContext context)
        {
            _context = context;
        }

        // GET: Kategoriler
        public async Task<IActionResult> Index()
        {
            var kategoriler = await _context.Kategorilers
                .Include(k => k.Filmlers)
                .ToListAsync();
            return View(kategoriler);
        }

        // GET: Kategoriler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoriler = await _context.Kategorilers
                .Include(k => k.Filmlers)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategoriler == null)
            {
                return NotFound();
            }

            return View(kategoriler);
        }

        // GET: Kategoriler/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Kategoriler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Ad")] Kategoriler kategoriler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(kategoriler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(kategoriler);
        }

        // GET: Kategoriler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoriler = await _context.Kategorilers.FindAsync(id);
            if (kategoriler == null)
            {
                return NotFound();
            }
            return View(kategoriler);
        }

        // POST: Kategoriler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Ad")] Kategoriler kategoriler)
        {
            if (id != kategoriler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(kategoriler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KategorilerExists(kategoriler.Id))
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
            return View(kategoriler);
        }

        // GET: Kategoriler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var kategoriler = await _context.Kategorilers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (kategoriler == null)
            {
                return NotFound();
            }

            return View(kategoriler);
        }

        // POST: Kategoriler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var kategoriler = await _context.Kategorilers.FindAsync(id);
            if (kategoriler != null)
            {
                _context.Kategorilers.Remove(kategoriler);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KategorilerExists(int id)
        {
            return _context.Kategorilers.Any(e => e.Id == id);
        }
    }
} 
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmIncelemeApp.Models;
using System.Linq;

namespace FilmIncelemeApp.Controllers
{
    public class FilmlerController : Controller
    {
        private readonly FilmIncelemeContext _context;

        public FilmlerController(FilmIncelemeContext context)
        {
            _context = context;
        }

        // GET: Filmler
        public async Task<IActionResult> Index()
        {
            var filmler = await _context.Filmlers
                .Include(f => f.Kategori)
                .Include(f => f.Yorumlars)
                .ToListAsync();

            // Ortalama puan hesaplama
            var filmlerOrtalamaPuan = filmler.Select(f => new
            {
                Film = f,
                OrtalamaPuan = f.Yorumlars.Any() ? f.Yorumlars.Average(y => y.Puan) : 0
            }).ToList();

            return View(filmlerOrtalamaPuan);
        }

        // GET: Filmler/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmler = await _context.Filmlers
                .Include(f => f.Kategori)
                .Include(f => f.Yorumlars)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmler == null)
            {
                return NotFound();
            }

            return View(filmler);
        }

        // GET: Filmler/Create
        public IActionResult Create()
        {
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "Id", "Ad");
            return View();
        }

        // POST: Filmler/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Baslik,Aciklama,Yayinyili,KategoriId")] Filmler filmler)
        {
            if (ModelState.IsValid)
            {
                _context.Add(filmler);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "Id", "Ad", filmler.KategoriId);
            return View(filmler);
        }

        // GET: Filmler/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmler = await _context.Filmlers.FindAsync(id);
            if (filmler == null)
            {
                return NotFound();
            }
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "Id", "Ad", filmler.KategoriId);
            return View(filmler);
        }

        // POST: Filmler/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Baslik,Aciklama,Yayinyili,KategoriId")] Filmler filmler)
        {
            if (id != filmler.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(filmler);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FilmlerExists(filmler.Id))
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
            ViewData["KategoriId"] = new SelectList(_context.Kategorilers, "Id", "Ad", filmler.KategoriId);
            return View(filmler);
        }

        // GET: Filmler/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var filmler = await _context.Filmlers
                .Include(f => f.Kategori)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (filmler == null)
            {
                return NotFound();
            }

            return View(filmler);
        }

        // POST: Filmler/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var filmler = await _context.Filmlers.FindAsync(id);
            if (filmler != null)
            {
                _context.Filmlers.Remove(filmler);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FilmlerExists(int id)
        {
            return _context.Filmlers.Any(e => e.Id == id);
        }
    }
} 
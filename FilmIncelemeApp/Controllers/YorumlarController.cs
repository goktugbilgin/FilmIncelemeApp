using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FilmIncelemeApp.Models;

namespace FilmIncelemeApp.Controllers
{
    public class YorumlarController : Controller
    {
        private readonly FilmIncelemeContext _context;

        public YorumlarController(FilmIncelemeContext context)
        {
            _context = context;
        }

        // GET: Yorumlar
        public async Task<IActionResult> Index()
        {
            // JOIN işlemi: Yorumlarda film adı ve kategori adı görünsün
            var yorumlar = await _context.Yorumlars
                .Include(y => y.Film)
                .ThenInclude(f => f.Kategori)
                .ToListAsync();

            return View(yorumlar);
        }

        // GET: Yorumlar/YuksekPuanliYorumlar
        public async Task<IActionResult> YuksekPuanliYorumlar()
        {
            // WHERE işlemi: Sadece puanı 4 ve üstü olan yorumlar gösterilsin
            var yuksekPuanliYorumlar = await _context.Yorumlars
                .Include(y => y.Film)
                .ThenInclude(f => f.Kategori)
                .Where(y => y.Puan >= 4)
                .ToListAsync();

            return View(yuksekPuanliYorumlar);
        }

        // GET: Yorumlar/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yorumlar = await _context.Yorumlars
                .Include(y => y.Film)
                .ThenInclude(f => f.Kategori)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yorumlar == null)
            {
                return NotFound();
            }

            return View(yorumlar);
        }

        // GET: Yorumlar/Create
        public IActionResult Create()
        {
            ViewData["FilmId"] = new SelectList(_context.Filmlers, "Id", "Baslik");
            return View();
        }

        // POST: Yorumlar/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FilmId,Puan,YorumMetni,YorumYapan")] Yorumlar yorumlar)
        {
            if (ModelState.IsValid)
            {
                yorumlar.Tarih = DateTime.Now;
                _context.Add(yorumlar);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["FilmId"] = new SelectList(_context.Filmlers, "Id", "Baslik", yorumlar.FilmId);
            return View(yorumlar);
        }

        // GET: Yorumlar/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yorumlar = await _context.Yorumlars.FindAsync(id);
            if (yorumlar == null)
            {
                return NotFound();
            }
            ViewData["FilmId"] = new SelectList(_context.Filmlers, "Id", "Baslik", yorumlar.FilmId);
            return View(yorumlar);
        }

        // POST: Yorumlar/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FilmId,Puan,YorumMetni,Tarih,YorumYapan")] Yorumlar yorumlar)
        {
            if (id != yorumlar.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(yorumlar);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!YorumlarExists(yorumlar.Id))
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
            ViewData["FilmId"] = new SelectList(_context.Filmlers, "Id", "Baslik", yorumlar.FilmId);
            return View(yorumlar);
        }

        // GET: Yorumlar/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var yorumlar = await _context.Yorumlars
                .Include(y => y.Film)
                .ThenInclude(f => f.Kategori)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (yorumlar == null)
            {
                return NotFound();
            }

            return View(yorumlar);
        }

        // POST: Yorumlar/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var yorumlar = await _context.Yorumlars.FindAsync(id);
            if (yorumlar != null)
            {
                _context.Yorumlars.Remove(yorumlar);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool YorumlarExists(int id)
        {
            return _context.Yorumlars.Any(e => e.Id == id);
        }
    }
} 
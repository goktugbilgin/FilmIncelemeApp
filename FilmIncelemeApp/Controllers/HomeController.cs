using FilmIncelemeApp.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace FilmIncelemeApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly FilmIncelemeContext _context;

        public HomeController(ILogger<HomeController> logger, FilmIncelemeContext context)
        {
            _logger = logger;
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            // İstatistikler
            var filmSayisi = await _context.Filmlers.CountAsync();
            var kategoriSayisi = await _context.Kategorilers.CountAsync();
            var yorumSayisi = await _context.Yorumlars.CountAsync();
            
            // Ortalama puan hesaplama
            double ortalamaPuan = 0;
            if (await _context.Yorumlars.AnyAsync(y => y.Puan.HasValue))
            {
                ortalamaPuan = await _context.Yorumlars
                    .Where(y => y.Puan.HasValue)
                    .AverageAsync(y => y.Puan.Value);
            }

            // En yüksek puanlı filmler
            var enYuksekPuanliFilmler = await _context.Filmlers
                .Include(f => f.Kategori)
                .Include(f => f.Yorumlars)
                .Select(f => new
                {
                    Film = f,
                    OrtalamaPuan = f.Yorumlars.Any() ? f.Yorumlars.Average(y => y.Puan) : 0
                })
                .Where(f => f.OrtalamaPuan > 0)
                .OrderByDescending(f => f.OrtalamaPuan)
                .Take(5)
                .ToListAsync();

            ViewBag.FilmSayisi = filmSayisi;
            ViewBag.KategoriSayisi = kategoriSayisi;
            ViewBag.YorumSayisi = yorumSayisi;
            ViewBag.OrtalamaPuan = ortalamaPuan;
            ViewBag.EnYuksekPuanliFilmler = enYuksekPuanliFilmler;

            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

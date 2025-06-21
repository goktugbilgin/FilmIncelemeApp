using System;
using System.Collections.Generic;

namespace FilmIncelemeApp.Models;

public partial class Filmler
{
    public int Id { get; set; }

    public string Baslik { get; set; } = null!;

    public string? Aciklama { get; set; }

    public int? Yayinyili { get; set; }

    public int? KategoriId { get; set; }

    public virtual Kategoriler? Kategori { get; set; }

    public virtual ICollection<Yorumlar> Yorumlars { get; set; } = new List<Yorumlar>();
}

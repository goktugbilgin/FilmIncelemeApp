using System;
using System.Collections.Generic;

namespace FilmIncelemeApp.Models;

public partial class Kategoriler
{
    public int Id { get; set; }

    public string Ad { get; set; } = null!;

    public virtual ICollection<Filmler> Filmlers { get; set; } = new List<Filmler>();
}

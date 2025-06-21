using System;
using System.Collections.Generic;

namespace FilmIncelemeApp.Models;

public partial class Yorumlar
{
    public int Id { get; set; }

    public int? FilmId { get; set; }

    public int? Puan { get; set; }

    public string? YorumMetni { get; set; }

    public DateTime? Tarih { get; set; }

    public string? YorumYapan { get; set; }

    public virtual Filmler? Film { get; set; }
}

﻿using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using WPR_project.Models;

public class Abonnement
{
    [Key]
    public Guid AbonnementId { get; set; }

    [Required]
    public string Naam { get; set; }

    [Column("begindatum")]
    public DateTime beginDatum { get; set; }

    [Column("vervaldatum")]
    public DateTime vervaldatum { get; set; }

    [Precision(18, 2)]
    public decimal Kosten { get; set; }

    public Guid zakelijkeId { get; set; }

    [ForeignKey(nameof(zakelijkeId))]
    public ZakelijkHuurder ZakelijkHuurder { get; set; }

    public AbonnementType AbonnementType { get; set; }

    public AbonnementTermijnen AbonnementTermijnen { get; set; }

    public bool? directZichtbaar { get; set; }
    public decimal? korting { get; set; }
    public string? details { get; set; }

    public int? AantalDagen { get; set; }

    public DateTime? updateDatum { get; set; }
    public bool IsActive { get; set; } = true;

    public bool status { get; set; }

    public string? notitie { get; set; }
}
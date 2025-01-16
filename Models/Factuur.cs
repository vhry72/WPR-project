using System;
using System.ComponentModel.DataAnnotations;

public class Factuur
{
    [Key]
    public Guid FactuurId { get; set; }
    public Guid BeheerderId { get; set; }
    public Guid AbonnementId { get; set; }
    public byte[] FactuurPDF { get; set; }
    public DateTime FactuurDatum { get; set; }
}

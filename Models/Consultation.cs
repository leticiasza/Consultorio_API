using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultorio.Models;
public class Consultation
{
    public int Id { get; set; }

    [Required]
    [Column(TypeName = "DATE")]
    public DateOnly Date { get; set; }

    [Required]
    [MaxLength(50)]
    public string Specialty { get; set; } = string.Empty;

    [Required]
    public decimal Price { get; set; }

    // FK
    [Required]
    public int PatientId { get; set; }
    public Patient Patient { get; set; } = null!;
}
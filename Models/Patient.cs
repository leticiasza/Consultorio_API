using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Consultorio.Models;

public class Patient
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(120)]
    public string Name { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(14)]
    public string CPF { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required]
    [Column(TypeName = "DATE")]
    public DateOnly BirthDate { get; set; }
}
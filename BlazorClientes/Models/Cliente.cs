using System.ComponentModel.DataAnnotations;

namespace BlazorClientes.Models;

public class Cliente
{
    public int id_cliente { get; set; }

    [Required(ErrorMessage = "El CUI es obligatorio.")]
    [RegularExpression(@"^\d{13}$", ErrorMessage = "El CUI debe contener 13 dígitos.")]
    public string? cui { get; set; }

    [Required(ErrorMessage = "El NIT es obligatorio.")]
    [StringLength(20, MinimumLength = 2, ErrorMessage = "Ingrese un NIT válido.")]
    public string? nit { get; set; }

    [Required(ErrorMessage = "Los nombres son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Ingrese nombres válidos.")]
    public string? nombres { get; set; }

    [Required(ErrorMessage = "Los apellidos son obligatorios.")]
    [StringLength(100, MinimumLength = 2, ErrorMessage = "Ingrese apellidos válidos.")]
    public string? apellidos { get; set; }

    [Required(ErrorMessage = "La dirección es obligatoria.")]
    [StringLength(200, ErrorMessage = "La dirección es demasiado larga.")]
    public string? direccion { get; set; }

    [Required(ErrorMessage = "El teléfono es obligatorio.")]
    [RegularExpression(@"^\d{8}$", ErrorMessage = "El teléfono debe contener 8 números.")]
    public string? telefono { get; set; }

    [DataType(DataType.Date)]
    public DateTime fecha_nacimiento { get; set; } = DateTime.Today;
}
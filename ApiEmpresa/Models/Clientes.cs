using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiEmpresa.Models;

[Table("clientes", Schema = "dbo")]
public class Clientes
{
    [Key]
    [Column("id_cliente")]
    public int id_cliente { get; set; }

    [Column("cui")]
    public string? cui { get; set; }

    [Column("nit")]
    public string? nit { get; set; }

    [Column("nombres")]
    public string? nombres { get; set; }

    [Column("apellidos")]
    public string? apellidos { get; set; }

    [Column("direccion")]
    public string? direccion { get; set; }

    [Column("telefono")]
    public string? telefono { get; set; }

    [Column("fecha_nacimiento", TypeName = "date")]
    public DateTime fecha_nacimiento { get; set; }
}
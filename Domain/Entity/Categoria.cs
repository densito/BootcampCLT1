namespace BootcampCLT.Domain.Entity
{
  public class Categoria
  {
    public int Id { get; set; }
    public string Nombre { get; set; } = default!;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaActualizacion { get; set; }

    public ICollection<Producto> Productos { get; set; } = new List<Producto>();
  }
}
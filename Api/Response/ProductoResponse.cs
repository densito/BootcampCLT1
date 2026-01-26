using Microsoft.AspNetCore.SignalR;

namespace BootcampCLT.Api.Response;

public record ProductoResponse
{
    public int Id { get; set; }
    public string Codigo { get; set; } = default!;
    public string Nombre { get; set; } = default!;
    public string? Descripcion { get; set; }
    public double Precio { get; set; }
    public bool Activo { get; set; }
    public int CategoriaId { get; set; }
};

using BootcampCLT.Api.Response;
using BootcampCLT.Application.Commands;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Handlers
{
  public class PatchProductoHandler : IRequestHandler<PatchProductoCommand, ProductoResponse?>
  {
    private readonly PostegresDbContext _postgresDbContext;

    public PatchProductoHandler(PostegresDbContext postgresDbContext)
    {
      _postgresDbContext = postgresDbContext;
    }

    public async Task<ProductoResponse?> Handle(PatchProductoCommand command, CancellationToken cancellationToken)
    {
      var producto = await _postgresDbContext.Productos
          .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

      if (producto == null)
        return null;

      if (command.Request.CategoriaId.HasValue)
      {
        var categoriaExiste = await _postgresDbContext.Categorias
            .AnyAsync(c => c.Id == command.Request.CategoriaId.Value, cancellationToken);

        if (!categoriaExiste)
        {
          throw new InvalidOperationException($"La categoría con ID {command.Request.CategoriaId.Value} no existe.");
        }
      }

      if (command.Request.Codigo != null)
        producto.Codigo = command.Request.Codigo;

      if (command.Request.Nombre != null)
        producto.Nombre = command.Request.Nombre;

      if (command.Request.Descripcion != null)
        producto.Descripcion = command.Request.Descripcion;

      if (command.Request.Precio.HasValue)
        producto.Precio = command.Request.Precio.Value;

      if (command.Request.Activo.HasValue)
        producto.Activo = command.Request.Activo.Value;

      if (command.Request.CategoriaId.HasValue)
        producto.CategoriaId = command.Request.CategoriaId.Value;

      producto.FechaActualizacion = DateTime.UtcNow;

      await _postgresDbContext.SaveChangesAsync(cancellationToken);

      return new ProductoResponse
      {
        Id = producto.Id,
        Codigo = producto.Codigo,
        Nombre = producto.Nombre,
        Descripcion = producto.Descripcion ?? string.Empty,
        Precio = (double)producto.Precio,
        Activo = producto.Activo,
        CategoriaId = producto.CategoriaId,
      };
    }
  }
}
using BootcampCLT.Api.Response;
using BootcampCLT.Application.Commands;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Handlers
{
    public class UpdateProductoHandler : IRequestHandler<UpdateProductoCommand, ProductoResponse?>
    {
        private readonly PostegresDbContext _postgresDbContext;

        public UpdateProductoHandler(PostegresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<ProductoResponse?> Handle(UpdateProductoCommand command, CancellationToken cancellationToken)
        {
            var producto = await _postgresDbContext.Productos
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (producto == null)
                return null;

            producto.Codigo = command.Request.Codigo;
            producto.Nombre = command.Request.Nombre;
            producto.Descripcion = command.Request.Descripcion;
            producto.Precio = command.Request.Precio;
            producto.Activo = command.Request.Activo;
            producto.CategoriaId = command.Request.CategoriaId;
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
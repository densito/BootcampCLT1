using BootcampCLT.Api.Response;
using BootcampCLT.Application.Commands;
using BootcampCLT.Domain.Entity;
using BootcampCLT.Infraestructure.Context;
using MediatR;

namespace BootcampCLT.Application.Handlers
{
    public class CreateProductoCommandHandler : IRequestHandler<CreateProductoCommand, ProductoResponse>
    {
        private readonly PostegresDbContext _postgresDbContext;

        public CreateProductoCommandHandler(PostegresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<ProductoResponse> Handle(CreateProductoCommand request, CancellationToken cancellationToken)
        {
            var productoEntity = new Producto
            {
                Codigo = request.Request.Codigo,
                Nombre = request.Request.Nombre,
                Descripcion = request.Request.Descripcion,
                Precio = request.Request.Precio,
                Activo = request.Request.Activo,
                CategoriaId = request.Request.CategoriaId,
                FechaCreacion = DateTime.UtcNow
            };

            _postgresDbContext.Productos.Add(productoEntity);
            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            return new ProductoResponse
            {
                Id = productoEntity.Id,
                Codigo = productoEntity.Codigo,
                Nombre = productoEntity.Nombre,
                Descripcion = productoEntity.Descripcion ?? string.Empty,
                Precio = (double)productoEntity.Precio,
                Activo = productoEntity.Activo,
                CategoriaId = productoEntity.CategoriaId
            };
        }
    }
}
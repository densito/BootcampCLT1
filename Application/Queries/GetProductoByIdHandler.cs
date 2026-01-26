using BootcampCLT.Api.Response;
using BootcampCLT.Application.Queries;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Queries
{
    public class GetProductoByIdHandler : IRequestHandler<GetProductoByIdQuery, ProductoResponse?>
    {
        private readonly PostegresDbContext _postgresDbContext;

        public GetProductoByIdHandler(PostegresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<ProductoResponse?> Handle(GetProductoByIdQuery request, CancellationToken cancellationToken)
        {
            var entity = await _postgresDbContext.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == request.id, cancellationToken);

            if (entity == null)
                return null;

            return new ProductoResponse
            {
                Id = entity.Id,
                Codigo = entity.Codigo,
                Nombre = entity.Nombre,
                Descripcion = entity.Descripcion ?? string.Empty,
                Precio = (double)entity.Precio,
                Activo = entity.Activo,
                CategoriaId = entity.CategoriaId,
            };
        }
    }
}

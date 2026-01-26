using BootcampCLT.Api.Response;
using BootcampCLT.Application.Queries;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Handlers
{
    public class GetAllProductosHandler : IRequestHandler<GetAllProductosQuery, List<ProductoResponse>>
    {
        private readonly PostegresDbContext _postgresDbContext;

        public GetAllProductosHandler(PostegresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<List<ProductoResponse>> Handle(GetAllProductosQuery request, CancellationToken cancellationToken)
        {
            var productos = await _postgresDbContext.Productos
                .AsNoTracking()
                .Select(p => new ProductoResponse
                {
                    Id = p.Id,
                    Codigo = p.Codigo,
                    Nombre = p.Nombre,
                    Descripcion = p.Descripcion ?? string.Empty,
                    Precio = (double)p.Precio,
                    Activo = p.Activo,
                    CategoriaId = p.CategoriaId,
                })
                .ToListAsync(cancellationToken);

            return productos;
        }
    }
}
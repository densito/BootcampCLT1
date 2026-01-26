using BootcampCLT.Application.Commands;
using BootcampCLT.Infraestructure.Context;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BootcampCLT.Application.Handlers
{
    public class DeleteProductoHandler : IRequestHandler<DeleteProductoCommand, bool>
    {
        private readonly PostegresDbContext _postgresDbContext;

        public DeleteProductoHandler(PostegresDbContext postgresDbContext)
        {
            _postgresDbContext = postgresDbContext;
        }

        public async Task<bool> Handle(DeleteProductoCommand command, CancellationToken cancellationToken)
        {
            var producto = await _postgresDbContext.Productos
                .FirstOrDefaultAsync(p => p.Id == command.Id, cancellationToken);

            if (producto == null)
                return false;

            _postgresDbContext.Productos.Remove(producto);

            await _postgresDbContext.SaveChangesAsync(cancellationToken);

            return true;
        }
    }
}
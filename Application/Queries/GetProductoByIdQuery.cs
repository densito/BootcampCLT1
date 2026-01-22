using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Queries
{
    public record GetProductoByIdQuery(int id) : IRequest<ProductoResponse?>;
}

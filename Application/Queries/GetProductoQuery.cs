using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Queries
{
    public class GetAllProductosQuery : IRequest<List<ProductoResponse>>
    {
    }
}
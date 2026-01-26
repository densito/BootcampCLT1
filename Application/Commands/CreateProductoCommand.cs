using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Commands
{
    public class CreateProductoCommand : IRequest<ProductoResponse>
    {
        public CreateProductoRequest Request { get; }

        public CreateProductoCommand(CreateProductoRequest request)
        {
            Request = request;
        }
    }
}
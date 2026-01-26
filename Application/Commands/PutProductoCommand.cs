using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Commands
{
    public class UpdateProductoCommand : IRequest<ProductoResponse?>
    {
        public int Id { get; set; }
        public UpdateProductoRequest Request { get; set; }

        public UpdateProductoCommand(int id, UpdateProductoRequest request)
        {
            Id = id;
            Request = request;
        }
    }
}
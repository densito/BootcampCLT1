using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using MediatR;

namespace BootcampCLT.Application.Commands
{
  public class PatchProductoCommand : IRequest<ProductoResponse?>
  {
    public int Id { get; set; }
    public PatchProductoRequest Request { get; set; }

    public PatchProductoCommand(int id, PatchProductoRequest request)
    {
      Id = id;
      Request = request;
    }
  }
}
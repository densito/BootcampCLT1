using MediatR;

namespace BootcampCLT.Application.Commands
{
    public class DeleteProductoCommand : IRequest<bool>
    {
        public int Id { get; set; }

        public DeleteProductoCommand(int id)
        {
            Id = id;
        }
    }
}
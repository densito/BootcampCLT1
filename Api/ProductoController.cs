using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using System.Threading.Tasks;
using BootcampCLT.Application.Queries;

[ApiController]
public class ProductoController : Controller
{
    private readonly IMediator _mediator;
    private readonly ILogger<ProductoController> _logger;

    public ProductoController(IMediator mediator, ILogger<ProductoController> logger)
    {
        _mediator = mediator;
        _logger = logger;
    }
    /// <summary>
    /// Obtiene el detalle de un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>Producto encontrado.</returns>
    [HttpGet("v1/api/productos")]
    [ProducesResponseType(typeof(IEnumerable<ProductoResponse>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<IEnumerable<ProductoResponse>> GetProductoById()
    {
        return Ok();
    }

    /// <summary>
    /// Obtiene el detalle de un producto por su identificador.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <returns>Producto encontrado.</returns>
    [HttpGet("v1/api/productos/{id:int}")]
    [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<ProductoResponse>> GetProductoById([FromRoute] int id)
    {
        try
        {
            _logger.LogInformation("Inicia la petición con ProductoId={ProductoId}", id);
            var result = await _mediator.Send(new GetProductoByIdQuery(id));

            if (result is null)
            {
                _logger.LogWarning("No se pudo encontrar el ProductoId={ProductoId}", id);
                return NotFound();
            }

            _logger.LogInformation("Finaliza la consulta del Prod {aa}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Ocurrio un error con el ProductoId={ProductoId}", id);
            return Ok();
        }
        
    }

    /// <summary>
    /// Crea un nuevo producto.
    /// </summary>
    /// <param name="request">Datos del producto a crear.</param>
    /// <returns>Producto creado.</returns>
    [HttpPost("v1/api/productos")]
    [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ProductoResponse> CreateProducto([FromBody] CreateProductoRequest request)
    {
        return Created(string.Empty, null);
    }

    /// <summary>
    /// Actualiza completamente un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="request">Datos completos a actualizar.</param>
    [HttpPut("v1/api/productos/{id:int}")]
    [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ProductoResponse> UpdateProducto(
        [FromRoute] int id,
        [FromBody] UpdateProductoRequest request)
    {
        return Ok();
    }

    /// <summary>
    /// Actualiza parcialmente un producto existente.
    /// Solo se modificarán los campos enviados.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    /// <param name="request">Campos a actualizar.</param>
    [HttpPatch("v1/api/productos/{id:int}")]
    [ProducesResponseType(typeof(ProductoResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public ActionResult<ProductoResponse> PatchProducto(
        [FromRoute] int id,
        [FromBody] PatchProductoRequest request)
    {
        return Ok();
    }

    /// <summary>
    /// Elimina un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    [HttpDelete("v1/api/productos/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public IActionResult DeleteProducto([FromRoute] int id)
    {
        return NoContent();
    }
}

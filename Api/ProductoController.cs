using BootcampCLT.Api.Request;
using BootcampCLT.Api.Response;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using BootcampCLT.Application.Queries;
using BootcampCLT.Infrastructure.Services;
using BootcampCLT.Application.Commands;

[ApiController]
public class ProductoController : Controller
{
    private readonly IMediator _mediator;
    private readonly LoggingService<ProductoController> _loggingService;

    public ProductoController(IMediator mediator, LoggingService<ProductoController> loggingService)
    {
        _mediator = mediator;
        _loggingService = loggingService;
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
    public async Task<ActionResult<IEnumerable<ProductoResponse>>> GetProductoById()
    {
        try
        {
            _loggingService.LogInformation("Inicia la consulta de todos los productos.");
            var result = await _mediator.Send(new GetAllProductosQuery());

            if (result == null || !result.Any())
            {
                _loggingService.LogInformation("No se encontraron productos. EstadoHTTP={EstadoHTTP}", StatusCodes.Status204NoContent);
                return NoContent();
            }

            _loggingService.LogInformation("Productos obtenidos exitosamente. Cantidad={Cantidad}, EstadoHTTP={EstadoHTTP}",
                result.Count, StatusCodes.Status200OK);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error al obtener todos los productos. Detalle={Detalle}, EstadoHTTP={EstadoHTTP}",
            ex.Message, StatusCodes.Status500InternalServerError);
            return StatusCode(500, new { error = "Error interno del servidor." });
        }
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
            _loggingService.LogInformation("Inicia la petición con ProductoId={ProductoId}", id);
            var result = await _mediator.Send(new GetProductoByIdQuery(id));

            if (result is null)
            {
                _loggingService.LogWarning("No se pudo encontrar el ProductoId={ProductoId}", id);
                return NotFound();
            }

            _loggingService.LogInformation("Finaliza la consulta del Producto {aa}", id);
            return Ok(result);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Ocurrio un error con el ProductoId={ProductoId}", id);
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
    public async Task<ActionResult<ProductoResponse>> CreateProducto([FromBody] CreateProductoRequest request)
    {
        try
        {
            _loggingService.LogInformation("Inicia la creación de un nuevo producto. Código={Codigo}, Nombre={Nombre}", request.Codigo, request.Nombre);
            var result = await _mediator.Send(new CreateProductoCommand(request));

            _loggingService.LogInformation("Producto creado exitosamente. ID={ProductoId}, Código={Codigo}, Nombre={Nombre}, EstadoHTTP={EstadoHTTP}",
           result.Id, result.Codigo, result.Nombre, StatusCodes.Status201Created);
            return CreatedAtAction(nameof(GetProductoById), new { id = result.Id }, result);
        }
        catch (Exception ex)
        {
            _loggingService.LogWarning("Error de validación al crear producto. Código={Codigo}, Nombre={Nombre}, Detalle={Detalle}, EstadoHTTP={EstadoHTTP}",
                       request.Codigo, request.Nombre, ex.Message, StatusCodes.Status400BadRequest);
            return BadRequest(new { error = ex.Message });
        }
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
    public async Task<ActionResult<ProductoResponse>> UpdateProducto(
    [FromRoute] int id,
    [FromBody] UpdateProductoRequest request)
    {
        try
        {
            _loggingService.LogInformation("Inicia la actualización de producto. ProductoId={ProductoId}, Código={Codigo}, Nombre={Nombre}",
                id, request.Codigo, request.Nombre);

            var result = await _mediator.Send(new UpdateProductoCommand(id, request));

            if (result == null)
            {
                _loggingService.LogWarning("Producto no encontrado para actualizar. ProductoId={ProductoId}, EstadoHTTP={EstadoHTTP}",
                    id, StatusCodes.Status404NotFound);
                return NotFound(new { error = $"Producto con ID {id} no encontrado." });
            }

            _loggingService.LogInformation("Producto actualizado exitosamente. ProductoId={ProductoId}, Código={Codigo}, Nombre={Nombre}, EstadoHTTP={EstadoHTTP}",
                result.Id, result.Codigo, result.Nombre, StatusCodes.Status200OK);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error al actualizar producto. ProductoId={ProductoId}, Código={Codigo}, Detalle={Detalle}, EstadoHTTP={EstadoHTTP}",
                id, request.Codigo, ex.Message, StatusCodes.Status500InternalServerError);
            return StatusCode(500, new { error = "Error interno del servidor." });
        }
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
    public async Task<ActionResult<ProductoResponse>> PatchProducto(
    [FromRoute] int id,
    [FromBody] PatchProductoRequest request)
    {
        try
        {
            _loggingService.LogInformation("Inicia la actualización parcial de producto. ProductoId={ProductoId}", id);

            var result = await _mediator.Send(new PatchProductoCommand(id, request));

            if (result == null)
            {
                _loggingService.LogWarning("Producto no encontrado para parchear. ProductoId={ProductoId}, EstadoHTTP={EstadoHTTP}",
                    id, StatusCodes.Status404NotFound);
                return NotFound(new { error = $"Producto con ID {id} no encontrado." });
            }

            _loggingService.LogInformation("Producto Parcheado exitosamente. ProductoId={ProductoId}, Código={Codigo}, Nombre={Nombre}, EstadoHTTP={EstadoHTTP}",
                result.Id, result.Codigo, result.Nombre, StatusCodes.Status200OK);

            return Ok(result);
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error al actualizar parcialmente producto. ProductoId={ProductoId}, Detalle={Detalle}, EstadoHTTP={EstadoHTTP}",
                id, ex.Message, StatusCodes.Status500InternalServerError);
            return StatusCode(500, new { error = "Error interno del servidor." });
        }
    }

    /// <summary>
    /// Elimina un producto existente.
    /// </summary>
    /// <param name="id">Identificador del producto.</param>
    [HttpDelete("v1/api/productos/{id:int}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult> DeleteProducto([FromRoute] int id)
    {
        try
        {
            _loggingService.LogInformation("Inicia la eliminación de producto. ProductoId={ProductoId}", id);

            var result = await _mediator.Send(new DeleteProductoCommand(id));

            if (!result)
            {
                _loggingService.LogWarning("Producto no encontrado para eliminar. ProductoId={ProductoId}, EstadoHTTP={EstadoHTTP}",
                    id, StatusCodes.Status404NotFound);
                return NotFound(new { error = $"Producto con ID {id} no encontrado." });
            }

            _loggingService.LogInformation("Producto eliminado exitosamente. ProductoId={ProductoId}, EstadoHTTP={EstadoHTTP}",
                id, StatusCodes.Status204NoContent);

            return NoContent();
        }
        catch (Exception ex)
        {
            _loggingService.LogError(ex, "Error al eliminar producto. ProductoId={ProductoId}, Detalle={Detalle}, EstadoHTTP={EstadoHTTP}",
                id, ex.Message, StatusCodes.Status500InternalServerError);
            return StatusCode(500, new { error = "Error interno del servidor." });
        }
    }
}

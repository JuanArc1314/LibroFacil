using LibroFacil.Api.DTOs;
using LibroFacil.Application.Services;
using LibroFacil.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace LibroFacil.Api.Controllers;

[ApiController]
[Route("api/libros")]
public class LibrosController : ControllerBase
{
    private readonly LibroService _service;

    public LibrosController(LibroService service)
    {
        _service = service;
    }

    // GET /api/libros
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var libros = await _service.ListarAsync();
        var result = libros.Select(MapToDto);
        return Ok(result);
    }

    // GET /api/libros/{id}
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var libro = await _service.ObtenerPorIdAsync(id);
        if (libro is null) return NotFound(new { mensaje = $"Libro con Id {id} no encontrado." });
        return Ok(MapToDto(libro));
    }

    // POST /api/libros
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] LibroRequestDto dto)
    {
        try
        {
            var libro = await _service.RegistrarAsync(dto.ISBN, dto.Titulo, dto.Autor, dto.AnioPublicacion, dto.Stock);
            return CreatedAtAction(nameof(GetById), new { id = libro.Id }, MapToDto(libro));
        }
        catch (ArgumentException ex) { return BadRequest(new { mensaje = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    // PUT /api/libros/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] LibroRequestDto dto)
    {
        try
        {
            await _service.ActualizarAsync(id, dto.ISBN, dto.Titulo, dto.Autor, dto.AnioPublicacion, dto.Stock);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensaje = ex.Message }); }
        catch (ArgumentException ex) { return BadRequest(new { mensaje = ex.Message }); }
        catch (InvalidOperationException ex) { return Conflict(new { mensaje = ex.Message }); }
    }

    // DELETE /api/libros/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.EliminarAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex) { return NotFound(new { mensaje = ex.Message }); }
    }

    private static LibroDto MapToDto(Libro l) => new()
    {
        Id = l.Id,
        ISBN = l.ISBN,
        Titulo = l.Titulo,
        Autor = l.Autor,
        AnioPublicacion = l.AnioPublicacion,
        Stock = l.Stock
    };
}
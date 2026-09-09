using LibroFacil.Application.Interfaces;
using LibroFacil.Domain.Entities;

namespace LibroFacil.Application.Services;

public class LibroService
{
    private readonly ILibroRepository _repo;

    public LibroService(ILibroRepository repo)
    {
        _repo = repo;
    }

    public async Task<IEnumerable<Libro>> ListarAsync()
        => await _repo.ObtenerTodosAsync();

    public async Task<Libro?> ObtenerPorIdAsync(int id)
        => await _repo.ObtenerPorIdAsync(id);

    public async Task<Libro> RegistrarAsync(string isbn, string titulo, string autor, int anioPublicacion, int stock)
    {
        if (await _repo.ExisteIsbnAsync(isbn))
            throw new InvalidOperationException("Ya existe un libro con ese ISBN.");

        var libro = new Libro(isbn, titulo, autor, anioPublicacion, stock);
        await _repo.AgregarAsync(libro);
        return libro;
    }

    public async Task ActualizarAsync(int id, string isbn, string titulo, string autor, int anioPublicacion, int stock)
    {
        var libro = await _repo.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró el libro con Id {id}.");

        if (await _repo.ExisteIsbnAsync(isbn, id))
            throw new InvalidOperationException("Ya existe otro libro con ese ISBN.");

        libro.Actualizar(isbn, titulo, autor, anioPublicacion, stock);
        await _repo.ActualizarAsync(libro);
    }

    public async Task EliminarAsync(int id)
    {
        var libro = await _repo.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"No se encontró el libro con Id {id}.");

        await _repo.EliminarAsync(libro);
    }
}
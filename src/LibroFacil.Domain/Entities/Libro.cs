namespace LibroFacil.Domain.Entities;
public class Libro
{
    public int Id { get; private set; }
    public string ISBN { get; private set; } = string.Empty;
    public string Titulo { get; private set; } = string.Empty;
    public string Autor { get; private set; } = string.Empty;
    public int AnioPublicacion { get; private set; }
    public int Stock { get; private set; }

    private Libro() { }

    public Libro(string isbn, string titulo, string autor, int anioPublicacion, int stock)
    {
        Validar(isbn, titulo, autor, anioPublicacion, stock);
        ISBN = isbn;
        Titulo = titulo;
        Autor = autor;
        AnioPublicacion = anioPublicacion;
        Stock = stock;
    }

    public void Actualizar(string isbn, string titulo, string autor, int anioPublicacion, int stock)
    {
        Validar(isbn, titulo, autor, anioPublicacion, stock);
        ISBN = isbn;
        Titulo = titulo;
        Autor = autor;
        AnioPublicacion = anioPublicacion;
        Stock = stock;
    }

    private static void Validar(string isbn, string titulo, string autor, int anioPublicacion, int stock)
    {
        if (string.IsNullOrWhiteSpace(isbn))
            throw new ArgumentException("El ISBN es obligatorio.");

        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El título es obligatorio.");

        if (string.IsNullOrWhiteSpace(autor))
            throw new ArgumentException("El autor es obligatorio.");

        if (anioPublicacion <= 0 || anioPublicacion > DateTime.Now.Year)
            throw new ArgumentException($"El año de publicación debe ser mayor a 0 y no mayor a {DateTime.Now.Year}.");

        if (stock < 0)
            throw new ArgumentException("El stock no puede ser negativo.");
    }
}
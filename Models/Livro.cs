namespace Biblioteca.Models
{
    public class Livro
    {
        public int Id { get; set; }
        public string? Titulo { get; set; }
        public string? Editora { get; set; }
        public int AnoPublicacao { get; set; }
        public string? Autor { get; set; }
        public int QtdEstoque { get; set; }
    }
}
using Biblioteca.Data;
using Biblioteca.Models;
using Microsoft.AspNetCore.Mvc;

namespace Biblioteca.Controllers
{
    [ApiController]
    public class HomeController : ControllerBase
    {
        // RETORNAR TODOS OS LIVROS
        [HttpGet("/livros")]
        public IActionResult GetAllBooks([FromServices] AppDbContext context)
        {
            var todosLivros = context.Livros.ToList();
            return Ok(todosLivros);
        }

        // RETORNAR UM LIVRO PELO ID
        [HttpGet("/livro/{id:int}")]
        public IActionResult GetBookById
        (
            [FromServices] AppDbContext context,
            [FromRoute] int id
        )
        {
            var buscarLivro = context.Livros.Find(id);
            if (buscarLivro is null) return NotFound();

            return Ok(buscarLivro);
        }

        // RETORNAR LIVROS COM BASE NA EDITORA
        [HttpGet("/livros/editora/{nomeEditora}")]
        public IActionResult GetBookByEditor
        (
            [FromServices] AppDbContext context,
            [FromRoute] string nomeEditora
        )
        {
            bool editoraExists = context.Livros.Any(l => l.Editora == nomeEditora);

            if (!editoraExists) return NotFound("Editora não encontrada.");

            List<Livro> livrosPorEditora = context.Livros
                .Where(x => x.Editora == nomeEditora)
                .ToList();

            return Ok(livrosPorEditora);
        }

        // CRIAR UM NOVO LIVRO
        [HttpPost("/livro")]
        public IActionResult Post
        (
            [FromServices] AppDbContext context,
            [FromBody] Livro novoLivro
        )
        {
            context.Livros.Add(novoLivro);
            context.SaveChanges();
            return Created($"/livro/{novoLivro.Id}", novoLivro);
        }

        // ATUALIZAR UM LIVRO PELO ID
        [HttpPut("/livro/{id:int}")]
        public IActionResult UpdateBookById
        (
            [FromServices] AppDbContext context,
            [FromRoute] int id,
            [FromBody] Livro novoLivro
        )
        {
            var livroAtual = context.Livros.Find(id);
            if (livroAtual is null) return NotFound();

            livroAtual.Titulo = novoLivro.Titulo;
            livroAtual.Editora = novoLivro.Editora;
            livroAtual.AnoPublicacao = novoLivro.AnoPublicacao;
            livroAtual.Autor = novoLivro.Autor;
            livroAtual.QtdEstoque = novoLivro.QtdEstoque;

            context.SaveChanges();
            return Ok($"O livro {livroAtual.Titulo} foi atualizado!");
        }

        // ACRESCENTAR ESTOQUE
        [HttpPut("/livro/{id:int}/adicionar-estoque/{qtd:int}")]
        public IActionResult IncreaseStockBookById
        (
            [FromServices] AppDbContext context,
            [FromRoute] int id,
            [FromRoute] int qtd
        )
        {
            var livroAtual = context.Livros.Find(id);
            if (livroAtual is null) return NotFound("Este livro não foi encontrado");
            livroAtual.QtdEstoque += qtd;

            context.SaveChanges();
            return Ok($"O estoque do livro {livroAtual.Titulo} aumentou para {livroAtual.QtdEstoque}!");
        }

        // REMOVER ESTOQUE
        [HttpPut("/livro/{id:int}/remover-estoque/{qtd:int}")]
        public IActionResult DecreaseStockBookById
        (
            [FromServices] AppDbContext context,
            [FromRoute] int id,
            [FromRoute] int qtd
        )
        {
            var livroAtual = context.Livros.Find(id);
            if (livroAtual is null) return NotFound("Este livro não foi encontrado");
            if (livroAtual.QtdEstoque < qtd) return BadRequest($"A quantidade informada não pode ser maior que a quantidade em estoque. O estoque atual é {livroAtual.QtdEstoque}.");

            livroAtual.QtdEstoque -= qtd;
            context.SaveChanges();
            return Ok($"O estoque do livro {livroAtual.Titulo} diminuiu para {livroAtual.QtdEstoque}!");
        }


        // DELETAR UM LIVRO PELO ID
        [HttpDelete("/livro/{id:int}")]
        public IActionResult DeleteBookById
        (
            [FromServices] AppDbContext context,
            [FromRoute] int id
        )
        {
            var excluirLivro = context.Livros.Find(id);
            if (excluirLivro is null) return NotFound();

            context.Livros.Remove(excluirLivro);
            context.SaveChanges();
            return Ok($"O livro {excluirLivro.Titulo} foi excluído!");
        }
    }
}
using Biblioteca.Models;
using Microsoft.EntityFrameworkCore;


namespace Biblioteca.Data
{
    public class AppDbContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder options) => options.UseSqlite("DataSource=app.db;Cache=Shared");
        public DbSet<Livro> Livros { get; set; }
    }
}
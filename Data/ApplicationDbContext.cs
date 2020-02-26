using APIRest.Models;
using Microsoft.EntityFrameworkCore;

namespace APIRest.Data
{
    public class ApplicationDbContext : DbContext
    {

        public DbSet<Produto> Produtos {get; set;}
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext>options) : base (options){}


    }
}
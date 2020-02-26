using System;
using System.Linq;
using APIRest.Data;
using APIRest.Models;
using Microsoft.AspNetCore.Mvc;

namespace APIRest.Controllers
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class ProdutosController : ControllerBase
    {
        private readonly ApplicationDbContext database;
        public ProdutosController(ApplicationDbContext database)
        {

            this.database = database;
        }

        [HttpGet]
        public IActionResult Get(){
            var produtos = database.Produtos.ToList();
            return Ok(produtos);
        }
        [HttpGet("{id}")]
        public IActionResult Get(int id){
            try{
               var produtos = database.Produtos.First(p => p.Id == id);
                return Ok(produtos); 
            }catch(Exception e){
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
            
        }
        [HttpPost]
        public IActionResult Post([FromBody] ProdutoTemp aux){
            if(aux.Preco <=0){
                Response.StatusCode = 400;
                return new ObjectResult(new { msg = "O preço do produto não pode ser menor ou igual a 0." });
            }
            if(aux.Nome.Length <= 1){
                Response.StatusCode = 400;
                return new ObjectResult(new { msg = "O Nome deve ter mais de  1 caracter" });
            }
            Produto p = new Produto();

            p.Nome = aux.Nome;
            p.Preco = aux.Preco;

            database.Produtos.Add(p);
            database.SaveChanges();
            Response.StatusCode = 201;
            return new ObjectResult("");
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id){
            try{
               var produtos = database.Produtos.First(p => p.Id == id);
                database.Produtos.Remove(produtos);
                database.SaveChanges();
                return Ok(); 
            }catch(Exception e){
                Response.StatusCode = 404;
                return new ObjectResult("");
            }
        }
        [HttpPatch]
        public IActionResult Patch([FromBody] Produto produto){
            if(produto.Id > 0){
                try{
                    var p = database.Produtos.First(p => p.Id == produto.Id);
                    if(p != null){
                        p.Nome = produto.Nome != null ? produto.Nome : p.Nome;
                        p.Preco = produto.Preco != 0 ? produto.Preco : p.Preco;

                        database.SaveChanges();
                        return Ok();
                    }else{
                        Response.StatusCode = 400;
                        return new ObjectResult(new { msg = "Produto não encontrado!" });
                    }
                }catch{
                    Response.StatusCode = 400;
                    return new ObjectResult(new { msg = "Produto não encontrado!" });
                }
                
            }else{
                Response.StatusCode = 400;
                return new ObjectResult(new { msg = "Id do produto inválido!" });
            }

        }
        
        
    }
}
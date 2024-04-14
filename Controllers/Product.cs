using Microsoft.AspNetCore.Mvc;
using apiC.Models;
using Microsoft.EntityFrameworkCore;
using apiC.DataBase;

namespace apiC.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ProductsController(EcomContext db) : ControllerBase
  {
    private readonly EcomContext _db = db;
    [NonAction]
    public async Task<IActionResult> ExistProduct([FromBody] Product _product)
    {
      try
      {
        if (_product == null)
        {
          return BadRequest("Produto nulo");
        }
        if (string.IsNullOrEmpty(_product.Name))
        {
          return BadRequest("Nome nulo ou vazio");
        }
        var ExistingProduct = await _db.Products.FirstOrDefaultAsync(a => a.Name == _product.Name);
        return ExistingProduct != null ? Ok(true) : Ok(false);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
      }
    }

    [HttpGet("/products/")]
    public async Task<IActionResult> GetAllProducts()
    {
      try
      {
        var products = await _db.Products.ToListAsync();
        return products != null ? Ok(products) : NotFound();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("/Products/create")]
    public async Task<IActionResult> Create([FromBody] Product _Product)
    {
      try
      {
        var _existProduct = await ExistProduct(_Product);
        if (string.IsNullOrEmpty(_Product.Name))
        {
          return Conflict("Produto já existe");
        }
        else
        {
          if (_Product.Price <= 0 || _Product.Stock < 0)
          {
            return BadRequest("Preço e Estoque devem ser valores positivos");
          }
          var newProduct = new Product { Name = _Product.Name, Price = _Product.Price, Stock = _Product.Stock };
          _db.Products.Add(newProduct);
          await _db.SaveChangesAsync();
          return Created("/products/create", newProduct);
        }
      }
      catch (Exception ex)
      {
        return BadRequest($"Erro ao criar produto: {ex.Message}");
      }
    }
    [HttpPut("/products/")]
    public async Task<IActionResult> PutProduct([FromBody] Product _Product)
    {
      try
      {
        var ProductUpdate = await _db.Products.FirstOrDefaultAsync(a => a.Name == _Product.Name);
        if (string.IsNullOrEmpty(_Product.Name))
        {
          return Conflict("Produto não existe");
        }
        else
        {
          var _ProductUpdate = new Product { Name = _Product.Name, Price = _Product.Price, Stock = _Product.Stock };
          _db.Products.Update(_ProductUpdate);
          await _db.SaveChangesAsync();
          return Created("/products/", _ProductUpdate);
        }
      }
      catch (Exception ex)
      {
        return BadRequest($"error no put admin : {ex.Message}");
      }
    }

    [HttpDelete("/product/delete/")]
    public async Task<IActionResult> ProductDelete([FromQuery] int productId) // Assumindo ID para exclusão
    {
      try
      {

        var _ProductDelete = await _db.Admins.FindAsync(productId);
        if (_ProductDelete == null)
        {
          return Conflict("Produto não encontrado");  // Mensagem de administrador não encontrado
        }

        _db.Admins.Remove(_ProductDelete);
        await _db.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest($"Erro ao excluir produto: {ex.Message}");
      }
    }
  }
}

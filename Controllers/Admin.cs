using Microsoft.AspNetCore.Mvc;
using apiC.Models;
using Microsoft.EntityFrameworkCore;
using apiC.DataBase;
using System.Linq.Expressions;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Runtime.CompilerServices;
using System.Reflection.Metadata.Ecma335;



// TODO simples CRUD operations
namespace apiC.Controllers
{
  [Route("api/[controllerName]")]
  [ApiController]
  public class AdminController(EcomContext db) : ControllerBase
  {
    private readonly EcomContext _db = db;

    // TODO nao estou vendo sentido neste exist admin voce pode pega o admin pelo BD ATRAVES DO FIND ASYNC


    [NonAction]
    public async Task<IActionResult> ExistAdmin([FromBody] Admin _admin)
    {
      try
      {
        if (_admin == null)
        {
          return BadRequest("admin is null");
        }
        if (string.IsNullOrEmpty(_admin.Name))
        {
          return BadRequest("Nome nulo ou vazio");
        }
        var ExistingAdmin = await _db.Admins.FirstOrDefaultAsync(a => a.Name == _admin.Name);
        return ExistingAdmin != null ? Ok(true) : Ok(false);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
      }
    }

    [HttpGet("/admin/")]
    public async Task<IActionResult> GetAllAdmins()
    {
      try
      {
        var Admins = await _db.Admins.ToListAsync();
        return Admins != null ? Ok(Admins) : NotFound();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("/Admin/create")]
    public async Task<IActionResult> Create([FromBody] Admin _admin)
    {
      try
      {
        var _ExistAdmin = await ExistAdmin(_admin);
        if (string.IsNullOrEmpty(_admin.Name))
        {
          return Conflict("admin não  existe");
        }
        else
        {
          var newAdmin = new Admin { Name = _admin.Name, Email = _admin.Email, Password = _admin.Password };
          _db.Admins.Add(newAdmin);
          await _db.SaveChangesAsync();
          return Created("/Admins/create", newAdmin);
        }
      }
      catch (Exception ex)
      {
        return BadRequest($"Erro ao criar produto: {ex.Message}");
      }
    }
    [HttpPut("/Admin/")]
    public async Task<IActionResult> PutAdmin([FromBody] Admin _admin)

    {
      try
      {
        var adminUpdate = await _db.Admins.FirstOrDefaultAsync(a => a.Name == _admin.Name);
        if (string.IsNullOrEmpty(_admin.Name))
        {
          return Conflict("admin não existe");
        }
        else
        {
          var _AdminUpdate = new Admin { Name = _admin.Name, Email = _admin.Email, Password = _admin.Password };
          _db.Admins.Update(_AdminUpdate);
          await _db.SaveChangesAsync();
          return Created("/Admins/", _AdminUpdate);
        }
      }
      catch (Exception ex)
      {
        return BadRequest($"error no put admin : {ex.Message}");
      }
    }
    [HttpDelete("/Admin/delete/")]
    public async Task<IActionResult> DeleteAdmin([FromQuery] int adminId) // Assumindo ID para exclusão
    {
      try
      {

        var adminToDelete = await _db.Admins.FindAsync(adminId);
        if (adminToDelete == null)
        {
          return Conflict("Administrador não encontrado");  // Mensagem de administrador não encontrado
        }

        _db.Admins.Remove(adminToDelete);
        await _db.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest($"Erro ao excluir administrador: {ex.Message}");
      }
    }
  };
};

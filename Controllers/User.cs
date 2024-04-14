using Microsoft.AspNetCore.Mvc;
using apiC.Models;
using Microsoft.EntityFrameworkCore;
using apiC.DataBase;



// TODO simples CRUD operations
// criar user  com os parametros senha email idade documentos e localizaçao
namespace apiC.Controllers
{
  [Route("api/[controllerName]")]
  [ApiController]
  public class UserController(EcomContext db) : ControllerBase
  {
    private readonly EcomContext _db = db;
    [NonAction]
    public async Task<IActionResult> ExistUser([FromBody] User _user)
    {
      try
      {
        if (_user == null)
        {
          return BadRequest("User is null");
        }
        if (string.IsNullOrEmpty(_user.Name))
        {
          return BadRequest("Nome nulo ou vazio");
        }
        var ExistingUser = await _db.Users.FirstOrDefaultAsync(a => a.Name == _user.Name);
        return ExistingUser != null ? Ok(true) : Ok(false);
      }
      catch (Exception ex)
      {
        return StatusCode(500, $"Erro interno do servidor: {ex.Message}");
      }
    }

    [HttpGet("/User/")]
    public async Task<IActionResult> GetAllUsers()
    {
      try
      {
        var Users = await _db.Users.ToListAsync();
        return Users != null ? Ok(Users) : NotFound();
      }
      catch (Exception ex)
      {
        return BadRequest(ex.Message);
      }
    }

    [HttpPost("/User/create")]
    public async Task<IActionResult> Create([FromBody] User _user)
    {
      try
      {
        var _ExistUser = await ExistUser(_user);
        if (string.IsNullOrEmpty(_user.Name))
        {
          return Conflict("User já existe");
        }
        else
        {
          var newUser = new User { Name = _user.Name, Email = _user.Email, Password = _user.Password };
          _db.Users.Add(newUser);
          await _db.SaveChangesAsync();
          return Created("/Users/create", newUser);
        }
      }
      catch (Exception ex)
      {
        return BadRequest($"Erro ao criar produto: {ex.Message}");
      }
    }
    [HttpDelete("/user/delete/")]
    public async Task<IActionResult> UserDelete([FromQuery] int userId) // Assumindo ID para exclusão
    {
      try
      {

        var _userDelete = await _db.Users.FindAsync(userId);
        if (_userDelete == null)
        {
          return Conflict("user não encontrado");  // Mensagem de administrador não encontrado
        }

        _db.Users.Remove(_userDelete);
        await _db.SaveChangesAsync();
        return Ok();
      }
      catch (Exception ex)
      {
        return BadRequest($"Erro ao excluir produto: {ex.Message}");
      }
    }
    [HttpPut("/user/")]
    public async Task<IActionResult> PutUser([FromBody] User _User)
    {
      try
      {
        var UserUpdate = await _db.Users.FirstOrDefaultAsync(a => a.Name == _User.Name);
        if (string.IsNullOrEmpty(_User.Name))
        {
          return Conflict("user não existe");
        }
        else
        {
          var _UserUpdate = new User { Name = _User.Name, Localization = _User.Localization, Email = _User.Email, Id = _User.Id, Password = _User.Password };
          _db.Users.Update(_UserUpdate);
          await _db.SaveChangesAsync();
          return Created("/user/", _UserUpdate);
        }
      }
      catch (Exception ex)
      {
        return BadRequest($"error no update user: {ex.Message}");
      }
    }
  }
}

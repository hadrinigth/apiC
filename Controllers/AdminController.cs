using Microsoft.AspNetCore.Mvc;
using apiC.Models;
using Microsoft.EntityFrameworkCore;
using apiC.DataBase;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Data.Entity;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace apiC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly EcomContext _dbContext;
        private readonly IConfiguration _config;

        public AdminController(EcomContext dbContext, IConfiguration config) //instancias 
        {

            _config = config;
           _dbContext = dbContext;
        }
        [HttpPost("[action]")]//metodos htttp
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> Register([FromBody] Admin admin)//metodos e paramentros 
        {
            var usuarioExiste = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Email == admin.Email);//verific inicial  

            if (usuarioExiste is not null) //handle
            {
                return BadRequest("Já existe usuário com este email"); 
            }
            //salvamento de dados 
            _dbContext.Add(admin);
            await _dbContext.SaveChangesAsync();
            return StatusCode(StatusCodes.Status201Created);
        }
        [HttpPost( "[action]")]
        public async Task<IActionResult> Login([FromBody] Admin admin)
        {
            var adminAtual = await _dbContext.Admins.FirstOrDefaultAsync(a => a.Email == admin.Email && a.Password == admin.Password);
            if (adminAtual is not null)
            {
                return BadRequest("admminstrador nao encontrado");

            }
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Key"]));//pegando a key nas variaveis locais 
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);//define o algoritmo de assinatura 

            //claims basicamente as config do token dado para o user 
            var claims = new[]//declara o claims para depois preechelos
        {
            new Claim(ClaimTypes.Email , admin.Email)
        };

            var token = new JwtSecurityToken(
                issuer: _config["JWT:Issuer"],
                audience: _config["JWT:Audience"],
                claims: claims,
                expires: DateTime.Now.AddDays(30),
                signingCredentials: credentials);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);
            //o retuno do objeto jwt
            return new ObjectResult(new
            {
                access_token = jwt,
                token_type = "bearer",
                admin_id = adminAtual.Id,
                admin_name = adminAtual.Name
            });
        }
    } 
}

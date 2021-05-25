using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoApi.Models;

namespace TodoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly TodoContext _context;

        public UsuariosController(TodoContext context)
        {
            _context = context;
        }

        // GET: api/Usuarios
        [HttpGet]
        public
            async  
                 Task<ActionResult<IEnumerable<UsuarioDTO>>> // tipo de saída ou retorno
                     GetUsuarios()
        {
            return await _context.Usuarios
                .Select(cadaItem => ItemToDTO(cadaItem))
                .ToListAsync();
        }

        [HttpGet("{id}")] 
        public // tipo de acesso
            async 
                Task<ActionResult<UsuarioDTO>> // tipo de saída ou retorno
                     GetUsuario // nome do método
                            (long id, long a, int b, Usuario u) // long = tipo de entrada do paramêtro a
        {
            var Usuario = await _context.Usuarios.FindAsync(id);

            if (Usuario == null)
            {
                return NotFound();
            }

            return ItemToDTO(Usuario);
        }

        [HttpPut("{id}")]
        public 
            async 
            Task<IActionResult> 
            UpdateUsuario(long id, UsuarioDTO UsuarioDTO)
        {
            if (id != UsuarioDTO.Id)
            {
                return BadRequest();
            }

            var Usuario = await _context.Usuarios.FindAsync(id);
            if (Usuario == null)
            {
                return NotFound();
            }

            Usuario.Nome = UsuarioDTO.Nome;
            Usuario.Email = UsuarioDTO.Email;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!UsuarioExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        [HttpPost]
        public // tanto faz
            async // tanto faz (mas se usar, precisa de um await)
                Task<ActionResult<UsuarioDTO>> // tipo de retorno do método, ou seja, oq será convertido pra JSON quando eu CONSULTAR
                    CreateUsuario // nome do método
                        (Usuario usuario) //  tipo de entrada, ou seja, eu preciso mandar um objeto JSON que reflita a classe "Usuario"
        {
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();

            return CreatedAtAction(
                nameof(GetUsuario),
                new { id = usuario.Id },
                ItemToDTO(usuario));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(long id)
        {
            var Usuario = await _context.Usuarios.FindAsync(id);

            if (Usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(Usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // => expressão lambda
        private bool UsuarioExists(long id) =>
             _context.Usuarios.Any(e => e.Id == id);        

        private static UsuarioDTO ItemToDTO(Usuario Usuario) =>
            new UsuarioDTO
            {
                Id = Usuario.Id,
                Nome = Usuario.Nome,
                Email = Usuario.Email,
                // Senha = Usuario.Senha
            };


        public static UsuarioDTO CriarUsuario(Usuario usuario)
        {

        }


    }
}
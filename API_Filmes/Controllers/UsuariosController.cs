using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Filmes.Data;
using API_Filmes.Models;
using Microsoft.AspNetCore.Authorization;
using API_Filmes.Services;
using System.Security.Claims;

namespace API_Filmes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly BancoContext _context;

        public UsuariosController(BancoContext context)
        {
            _context = context;
        }

        // LOGIN 

        [Route("Login")]
        [HttpPost]
        public async Task<ActionResult<dynamic>> Login(Usuario usuario)
        {

            var usuarioBanco = await _context.Usuarios.Where(s => s.Login == usuario.Login
                       && s.Senha == usuario.Senha).FirstOrDefaultAsync();

            if(usuarioBanco == null)
            {
                return NotFound(new { message = "Deu erro aí fera. Usuário ou senha inválidos." });
            }

            var token = ServiceToken.GerarToken(usuarioBanco);
            usuarioBanco.Senha = "";
            return new
            {
                Usuario = usuarioBanco,
                token = token
            };

        }


        // USUÁRIO - EDIÇÃO 

        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(long id, Usuario usuario)
        {
            var usuarioId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            // Usuário só deve poder editar ele mesmo

            if (usuarioId == id)
            {
                usuario.IsAdministrador = false;
                usuario.IsAtivo = true;

                if (id != usuario.UsuarioId)
                {
                    return BadRequest();
                }

                _context.Entry(usuario).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }

            return NoContent();

        }

        // USUÁRIO - CADASTRO 

        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.IsAdministrador = false;
            usuario.IsAtivo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(usuario.Nome, new { id = usuario.UsuarioId }, usuario);
        }

        // USUÁRIO - EXCLUSÃO LÓGICA 

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(long id)
        {
            var usuarioId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);


            // Usuário só deve poder excluir ele mesmo
            if (usuarioId == id)
            {
                var usuario = await _context.Usuarios.FindAsync(id);
                if (usuario == null)
                {
                    return NotFound();
                }

                // Não exclui, apenas desativa.

                usuario.IsAtivo = false;
                _context.Entry(usuario).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsuarioExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
                return NoContent();

        }

        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}

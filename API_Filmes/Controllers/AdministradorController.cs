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
using API_Filmes.Wrappers;

namespace API_Filmes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AdministradorController : ControllerBase
    {
        private readonly BancoContext _context;
        private readonly IUriService _uriService;

        public AdministradorController(BancoContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;

        }
        // ADMINISTRADOR - LISTAGEM DE USUÁRIOS NÃO ADMINISTRADORES ATIVOS 

        [Route("Usuarios")]
        [HttpGet]
        [Authorize(Roles = "Administrador")] // Somente administradores tem acesso a essa consulta

        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios([FromQuery] PaginationFilter filter)
        {
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var route = Request.Path.Value;
            // Lista apenas usuários ativos
            var usuarios = await _context.Usuarios.Where(c => c.IsAtivo == true && c.IsAdministrador == false).OrderBy(c => c.Nome).ToListAsync();

            var totalRecords = await _context.Filmes.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Usuario>(usuarios, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);

        }

        // ADMINISTRADOR - EDIÇÃO 

        [HttpPut("{id}")]
        [Authorize(Roles = "Administrador")] // Somente administradores tem acesso a essa consulta

        public async Task<IActionResult> PutUsuario(long id, Usuario usuario)
        {
            usuario.IsAdministrador = true;
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

        // ADMINISTRADOR - CADASTRO 

        [HttpPost]
        [Authorize(Roles = "Administrador")] // Somente administradores tem acesso a essa consulta

        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            usuario.IsAdministrador = true;
            usuario.IsAtivo = true;

            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            return CreatedAtAction(usuario.Nome, new { id = usuario.UsuarioId }, usuario);
        }


        // ADMINISTRADOR - EXCLUSÃO LÓGICA 

        [HttpDelete("{id}")]
        [Authorize(Roles = "Administrador")] // Somente administradores tem acesso a essa consulta
        public async Task<IActionResult> DeleteUsuario(long id)
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

            return NoContent();
        }

        private bool UsuarioExists(long id)
        {
            return _context.Usuarios.Any(e => e.UsuarioId == id);
        }
    }
}

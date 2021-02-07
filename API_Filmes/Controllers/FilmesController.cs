using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using API_Filmes.Data;
using API_Filmes.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using API_Filmes.Wrappers;
using API_Filmes.Services;

namespace API_Filmes.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly BancoContext _context;
        private readonly IUriService _uriService;

        public FilmesController(BancoContext context, IUriService uriService)
        {
            _context = context;
            _uriService = uriService;

        }

        // FILMES - LISTAGEM     

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Filme>>> GetFilmes([FromQuery] PaginationFilterFilme filter)
        {
            
            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);

            var filmesFiltro = _context.Filmes.Include(c => c.Atores).Where(c => c.FilmeId != 0).ToList();


            if (filter.Ator != string.Empty)
            {
                filmesFiltro = _context.Atores.Include(c => c.Filmes).Where(c => c.Nome.Contains(filter.Ator)).FirstOrDefault().Filmes.ToList();
            }

            if (filter.Diretor != string.Empty)
            {
                filmesFiltro = filmesFiltro.Where(c => c.Direcao.Contains(filter.Diretor)).ToList();
            }

            if (filter.Genero != string.Empty)
            {
                filmesFiltro = filmesFiltro.Where(c => c.Genero.Contains(filter.Genero)).ToList();
            }

            if (filter.Nome != string.Empty)
            {
                filmesFiltro = filmesFiltro.Where(c => c.Nome.Contains(filter.Nome)).ToList();
            }


            var a = filmesFiltro.ToList();

            var filmes = a.Skip((validFilter.PageNumber - 1) * validFilter.PageSize)
                .Take(validFilter.PageSize).ToList();

            foreach(var filme in filmes)
            {
                filme.Nota = NotaFilme(filme);
            }

            filmes = filmes.OrderByDescending(c => c.Nota).ThenBy(c => c.Nome).ToList();

            var route = Request.Path.Value;

            var totalRecords = await _context.Filmes.CountAsync();
            var pagedReponse = PaginationHelper.CreatePagedReponse<Filme>(filmes, validFilter, totalRecords, _uriService, route);
            return Ok(pagedReponse);
        }

        // FILMES - DETALHES     

        [HttpGet("{id}")]
        public async Task<ActionResult<Filme>> GetFilme(long id)
        {
            var filme = await _context.Filmes.FindAsync(id);

            if (filme == null)
            {
                return NotFound();
            }

            return filme;
        }

        // FILMES - CADASTRO     

        [HttpPost]
        [Authorize(Roles = "Administrador")] // Somente administradores tem acesso a esse cadastro
        public async Task<ActionResult<Filme>> PostFilme(Filme filme)
        {
            _context.Filmes.Add(filme);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFilme", new { id = filme.FilmeId }, filme);
        }

        // FILMES - VOTAÇÃO     

        [Route("Voto")]
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Votacao(Voto voto)
        {
            var usuarioId = Convert.ToInt64(User.FindFirst(ClaimTypes.NameIdentifier)?.Value);
            voto.UsuarioId = usuarioId;
            var jaVotou = _context.Votos.Where(c => c.FilmeId == voto.FilmeId && c.UsuarioId == voto.UsuarioId).FirstOrDefault();

            if(jaVotou == null) // Só é possível votar uma vez para cada filme
            {
                _context.Votos.Add(voto);
                await _context.SaveChangesAsync();

                return NoContent();
            }
            else
            {
                return BadRequest("Você já votou nesse filme!");
            }


        }

        private decimal NotaFilme(Filme filme)
        {
            var votos = _context.Votos.Where(c => c.FilmeId == filme.FilmeId).ToList();
            if(votos.Count > 0)
            {
                var quantidade = votos.Count;
                var somaVotos = votos.Sum(c => c.Nota);

                var nota = somaVotos / quantidade;

                return Math.Round(nota, 2);
            }
            else
            {
                return 0;
            }

        }
    }
}

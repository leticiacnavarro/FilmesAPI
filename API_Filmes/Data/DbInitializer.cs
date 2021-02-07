using API_Filmes.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Filmes.Data
{
    public class DbInitializer
    {
        public static void Initialize(BancoContext context)
        {
            context.Database.EnsureCreated();

            if (context.Usuarios.Any())
            {
                return;
            }

            var usuarios = new Usuario[]
            {
                new Usuario{ Nome = "Sherlock", Login = "sherlock", Senha = "sherlocked", IsAdministrador = true, IsAtivo = true},
                new Usuario{ Nome = "John Watson", Login = "johnwat", Senha = "123", IsAdministrador = false, IsAtivo = true},
                new Usuario{ Nome = "Mycroft", Login = "mycroft", Senha = "123", IsAdministrador = false, IsAtivo = true},
                new Usuario{ Nome = "Moriarty", Login = "moriarty", Senha = "didyoumissme", IsAdministrador = true, IsAtivo = true},

            };

            foreach (Usuario u in usuarios)
            {
                context.Usuarios.Add(u);
            }

            var atores = new Ator[]
            {
                new Ator{ Nome = "Keanu Reeves"},
                new Ator{ Nome = "Leonardo DiCaprio"},
                new Ator{ Nome = "Gal Gadot"},
                new Ator{ Nome = "Ryan Gosling"},
                new Ator{ Nome = "Emma Stone"},
                new Ator{ Nome = "Matthew McConaughey"},
            };

            foreach (Ator a in atores)
            {
                context.Atores.Add(a);
            }


            var filmes = new Filme[]
            {
                new Filme{ Nome = "Matrix", Genero = "Ação", Direcao = "Lana Wachowski e Lilly Wachowski"},
                new Filme{ Nome = "Interestelar", Genero = "Ficção Científica", Direcao = "Christopher Nolan"},
                new Filme{ Nome = "A Origem", Genero = "Ficção Científica", Direcao = "Christopher Nolan"},
                new Filme{ Nome = "Mulher Maravilha", Genero = "Ação, Aventura", Direcao = "Patty Jenkins"},
                new Filme{ Nome = "La La Land", Genero = "Musical", Direcao = "Damien Chazelle"},
            };

            foreach (Filme f in filmes)
            {
                context.Filmes.Add(f);
            }

            context.SaveChanges();

            var atoresfilmes = new AtorFilme[]
            {
                new AtorFilme{ AtorId = 1, FilmeId = 1},
                new AtorFilme{ AtorId = 2, FilmeId = 3},
                new AtorFilme{ AtorId = 3, FilmeId = 4},
                new AtorFilme{ AtorId = 4, FilmeId = 5},
                new AtorFilme{ AtorId = 5, FilmeId = 5},

            };

            foreach (AtorFilme af in atoresfilmes)
            {
                context.AtoresFilmes.Add(af);
            }

            var votos = new Voto[]
            {
                new Voto{ FilmeId = 1, UsuarioId = 1, Nota = 3},
                new Voto{ FilmeId = 1, UsuarioId = 2, Nota = 1},
                new Voto{ FilmeId = 1, UsuarioId = 3, Nota = 4},
                new Voto{ FilmeId = 2, UsuarioId = 1, Nota = 4},
                new Voto{ FilmeId = 2, UsuarioId = 2, Nota = 2},
                new Voto{ FilmeId = 3, UsuarioId = 1, Nota = 4},
                new Voto{ FilmeId = 3, UsuarioId = 2, Nota = 3},
                new Voto{ FilmeId = 4, UsuarioId = 1, Nota = 1},
                new Voto{ FilmeId = 4, UsuarioId = 2, Nota = 2},
                new Voto{ FilmeId = 4, UsuarioId = 3, Nota = 3},
                new Voto{ FilmeId = 5, UsuarioId = 1, Nota = 4},
                new Voto{ FilmeId = 5, UsuarioId = 2, Nota = 4},
                new Voto{ FilmeId = 5, UsuarioId = 3, Nota = 4},
                new Voto{ FilmeId = 5, UsuarioId = 4, Nota = 3},


            };

            foreach (Voto voto in votos)
            {
                context.Votos.Add(voto);
            }

            context.SaveChanges();
        }
    }
}

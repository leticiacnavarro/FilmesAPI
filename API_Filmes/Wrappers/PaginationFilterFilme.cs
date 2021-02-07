using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Filmes.Wrappers
{
    public class PaginationFilterFilme : PaginationFilter
    {
        public string Diretor { get; set; }
        public string Ator { get; set; }
        public string Nome { get; set; }
        public string Genero { get; set; }


        public PaginationFilterFilme()
        {
            this.PageNumber = 1;
            this.PageSize = 10;
            this.Diretor = string.Empty;
            this.Ator = string.Empty;
            this.Nome = string.Empty;
            this.Genero = string.Empty;
        }

    }
}

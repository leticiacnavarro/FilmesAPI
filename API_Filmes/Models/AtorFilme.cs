using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace API_Filmes.Models
{
    public class AtorFilme
    {
        public long AtorId { get; set; }
        public long FilmeId { get; set; }
        public Ator Ator { get; set; }
        public Filme Filme { get; set; }
    }
}

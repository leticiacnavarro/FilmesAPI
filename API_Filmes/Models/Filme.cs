using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace API_Filmes.Models
{
    public class Filme
    {
        public long FilmeId { get; set; }
        public string Nome { get; set; }
        public string Direcao { get; set; }
        public string Genero { get; set; }
        [NotMapped]
        public decimal Nota { get; set; }
        public virtual ICollection<Ator> Atores { get; set; }
        [JsonIgnore]
        public virtual List<AtorFilme> AtoresFilmes { get; set; }


    }
}

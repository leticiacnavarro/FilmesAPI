using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace API_Filmes.Models
{
    public class Ator
    {
        public long AtorId { get; set; }
        public string Nome { get; set; }
        [JsonIgnore]
        public virtual ICollection<Filme> Filmes { get; set; }
        [JsonIgnore]
        public virtual List<AtorFilme> AtoresFilmes { get; set; }

    }
}

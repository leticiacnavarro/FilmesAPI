using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Filmes.Models
{
    public class Voto
    {
        public long VotoId { get; set; }
        public long FilmeId { get; set; }
        public long UsuarioId { get; set; }

        [Range(0, 4, ErrorMessage = "Nota deve ser de 0 a 4")]
        public decimal Nota { get; set; }


    }
}

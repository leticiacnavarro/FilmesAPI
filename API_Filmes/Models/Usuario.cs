using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Filmes.Models
{
    public class Usuario
    {
        public long UsuarioId { get; set; }
        public string Nome { get; set; }
        public string Login { get; set; }
        public string Senha { get; set; }
        public bool IsAdministrador { get; set; }
        public bool IsAtivo { get; set; }

    }
}

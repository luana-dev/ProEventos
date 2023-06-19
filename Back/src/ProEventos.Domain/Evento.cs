using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Domain
{
    public class Evento
    {
        public int Id {get; set;}
        public String Local {get; set;}
        public DateTime? DataEvento {get; set;}
        public String Tema {get; set;}
        public int QtdPessoas {get; set;}
        public String ImagemURL {get; set;}
        public String Telefone {get; set;}
        public String Email {get; set;}
        public IEnumerable<Lote> Lote {get; set;}
        public IEnumerable<RedeSocial> RedesSociais {get; set;}
        public IEnumerable<PalestranteEvento> PalestrantesEventos {get; set;}
    }
}
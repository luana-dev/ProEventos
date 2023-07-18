using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ProEventos.Application.Dtos
{
    public class EventoDto
    {
        public int Id {get; set;}
        public String Local {get; set;}
        public String DataEvento {get; set;}

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
        // MinLength(3, ErrorMessage = "O campo {0} deve ter no mínimo 3 caracteres."),
        // MaxLength(50, ErrorMessage = "O campo {0} deve ter no máximo 50 caracteres."),
         StringLength(50, MinimumLength = 3,
                      ErrorMessage = "Intervalo permitido de 3 a 50 caracteres.")]
        public String Tema {get; set;}

        [Display(Name = "Qtd Pessoas"),
         Range(1,120000, ErrorMessage = "O campo {0} não pode ser menor que 1 e maior que 120.000")]
        public int QtdPessoas {get; set;}

        [RegularExpression(@".*\.(gif|jpe?g|bmp|png)$", 
                           ErrorMessage = "Não é uma imagem válida.")]
        public String ImagemURL {get; set;}

        [Required(ErrorMessage = "O campo {0} é obrigatório."),
         Phone(ErrorMessage = "O campo {0} está com número inválido.")]
        public String Telefone {get; set;}

        [Display(Name = "E-mail"),
         EmailAddress(ErrorMessage = "O campo {0} deve ser válido."),
         Required(ErrorMessage = "O campo {0} é obrigatório.")]
        public String Email {get; set;}

        public IEnumerable<LoteDto> Lote {get; set;}
        public IEnumerable<RedeSocialDto> RedesSociais {get; set;}
        public IEnumerable<PalestranteDto> PalestrantesEventos {get; set;}
        
    }
}
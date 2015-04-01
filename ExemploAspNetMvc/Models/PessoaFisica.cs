using ExemploAspNetMvc.Models.CustomValidations;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace ExemploAspNetMvc.Models
{
    [Table("Cad_Pessoas")]
    public class PessoaFisica
    {
        [Display(Name = "ID")]
        [Key]
        public int ID { get; set; }

        [Display(Name = "Cpf"),
        StringLength(11),
        CPFCustomValidation]
        public string CPF { get; set; }

        [Display(Name = "Nome"),
        StringLength(50),
        Required]
        public string Nome { get; set; }

    }
}
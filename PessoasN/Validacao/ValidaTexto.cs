using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PessoasN.Validacao
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidaTexto : ValidaBase
    {

        public bool Obrigatorio { get; set; }

        public Int32 ComprimentoMaximo { get; set; }

        public ValidaTexto(string NomeUI, bool Obrigatorio, Int32 ComprimentoMaximo)
        {
            this.NomeUI = NomeUI;
            this.Obrigatorio = Obrigatorio;
            this.ComprimentoMaximo = ComprimentoMaximo;

        }

        public ValidaTexto(string NomeUI, bool Obrigatorio) : this(NomeUI, Obrigatorio, 0)
        {
            

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PessoasN.Validacao
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class ValidaBase : System.Attribute
    {

        public string NomeUI { get; set; }
        
    }
}

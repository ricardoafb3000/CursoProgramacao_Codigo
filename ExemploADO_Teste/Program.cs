using ExemploADO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExemploADO_Teste
{
    class Program
    {
        static void Main(string[] args)
        {
            Setor novoSetor = new Setor();

            novoSetor.ID = 1;
            novoSetor.Nome = "teste asdasdas";
            novoSetor.Descricao = "teste descrição";

            if (!novoSetor.Salvar())
            {
                Console.WriteLine(novoSetor.Erros.Values);

            }



        }
    }
}

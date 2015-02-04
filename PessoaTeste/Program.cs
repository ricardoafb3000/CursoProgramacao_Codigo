using PessoasN;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PessoaTeste
{
    class Program
    {
        static void Main(string[] args)
        {
            //sobe banco de dados
            BancoDados.CarregaBancoDados();

            //adiciona tp de logradouro
            TpLogradouro tpLogradouro = new TpLogradouro()
            {
                Nome = "Beco"

            };

            if (!tpLogradouro.Salvar())
            {
                Console.WriteLine(tpLogradouro.Erros.Values);
                Console.Read();

            }

        }
    }
}

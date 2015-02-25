using ExemploEntity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace ExemploEntity_Exemplo
{
    class Program
    {
        static void Main(string[] args)
        {

            Profissao novo = new Profissao();

            novo.ID = 1;
            novo.Nome = "nova Profissão de novo";
            novo.Descricao = "nova Profissão";

            novo.RemoveSetorAtuacao(1);

            if (!novo.Excluir())
            {
                Console.WriteLine(novo.Erros.Values);
                Console.Read();

            }


        }

    }
}

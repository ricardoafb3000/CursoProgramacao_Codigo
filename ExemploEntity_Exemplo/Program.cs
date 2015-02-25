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

            Setor novo = new Setor();

            novo.Nome = "novo Setor";

            if (!novo.Salvar())
            {
                Console.WriteLine(novo.Erros.Values);

            }


        }
    }
}

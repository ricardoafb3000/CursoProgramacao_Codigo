namespace ExemploAspNetMvc.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Collections.Generic;

    public partial class CadastroDbInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<CadastroDbContext>
    {
        protected override void Seed(CadastroDbContext context)
        {
            var pessoas = new List<PessoaFisica>
                {
                    new PessoaFisica() { Nome = "Ricardo", CPF = "94029407072"},
                    new PessoaFisica() { Nome = "Michel", CPF = ""}
                
                };

            pessoas.ForEach(item => context.PessoasFisicas.Add(item));
            context.SaveChanges();

        }
    }
}

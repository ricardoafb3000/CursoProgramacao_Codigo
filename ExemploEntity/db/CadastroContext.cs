using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;

namespace ExemploEntity.db
{
    public class CadastroContext : DbContext
    {
        public DbSet<Cad_Setores> Cad_Setores { get; set; }
        public DbSet<Cad_Profissoes> Profissoes { get; set; } 
    }
}

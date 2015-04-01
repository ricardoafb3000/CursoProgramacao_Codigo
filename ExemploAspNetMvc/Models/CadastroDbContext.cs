namespace ExemploAspNetMvc.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class CadastroDbContext : DbContext
    {
        public CadastroDbContext()
            : base("CadastroContext")
        {
        }

        public virtual DbSet<Cad_Profissoes> Cad_Profissoes { get; set; }
        public virtual DbSet<Cad_Setores> Cad_Setores { get; set; }
        public virtual DbSet<PessoaFisica> PessoasFisicas { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Cad_Profissoes>()
                .Property(e => e.Pro_Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Cad_Profissoes>()
                .Property(e => e.Pro_Descricao)
                .IsUnicode(false);

            modelBuilder.Entity<Cad_Profissoes>()
                .HasMany(e => e.Cad_Setores)
                .WithMany(e => e.Cad_Profissoes)
                .Map(m => m.ToTable("Cad_ProfSetAtuacao").MapLeftKey("SetA_ProID").MapRightKey("SetA_SetID"));

            modelBuilder.Entity<Cad_Setores>()
                .Property(e => e.Set_Nome)
                .IsUnicode(false);

            modelBuilder.Entity<Cad_Setores>()
                .Property(e => e.Set_Descricao)
                .IsUnicode(false);
        }
    }
}

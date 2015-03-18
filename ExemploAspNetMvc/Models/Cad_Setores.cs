namespace ExemploAspNetMvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cad_Setores
    {
        public Cad_Setores()
        {
            Cad_Profissoes = new HashSet<Cad_Profissoes>();
        }

        [Display(Name = "ID")]
        [Key]
        public int Set_ID { get; set; }

        [Display(Name = "Nome")]
        [StringLength(50)]
        public string Set_Nome { get; set; }

        [Display(Name = "Descri��o")]
        [StringLength(300)]
        public string Set_Descricao { get; set; }

        [Display(Name = "Dt Inclus�o")]
        [DataType(DataType.Date),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Set_DtInc { get; set; }

        [Display(Name = "Dt Altera��o")]
        [DataType(DataType.Date),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Set_DtAlt { get; set; }

        [Display(Name = "Dt Exclus�o")]
        [DataType(DataType.Date),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Set_DtExc { get; set; }

        public virtual ICollection<Cad_Profissoes> Cad_Profissoes { get; set; }
    }
}

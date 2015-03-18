namespace ExemploAspNetMvc.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Cad_Profissoes
    {
        public Cad_Profissoes()
        {
            Cad_Setores = new HashSet<Cad_Setores>();
        }

        [Display(Name="ID")]
        [Key]
        public int Pro_ID { get; set; }

        [Display(Name = "Nome")]
        [Required, StringLength(50)]
        public string Pro_Nome { get; set; }

        [Display(Name = "Descri��o")]
        [StringLength(300)]
        public string Pro_Descricao { get; set; }

        [Display(Name = "Dt Inclus�o")]
        [DataType(DataType.Date),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Pro_DtInc { get; set; }

        [Display(Name = "Dt Altera��o")]
        [DataType(DataType.Date),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Pro_DtAlt { get; set; }

        [Display(Name = "Dt Exclus�o")]
        [DataType(DataType.Date),
        DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime? Pro_DtExc { get; set; }

        public virtual ICollection<Cad_Setores> Cad_Setores { get; set; }
    }
}

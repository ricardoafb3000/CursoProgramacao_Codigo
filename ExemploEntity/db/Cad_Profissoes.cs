//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace ExemploEntity.db
{
    using System;
    using System.Collections.Generic;
    
    public partial class Cad_Profissoes
    {
        public Cad_Profissoes()
        {
            this.Cad_Setores = new HashSet<Cad_Setores>();
        }
    
        public int Pro_ID { get; set; }
        public string Pro_Nome { get; set; }
        public string Pro_Descricao { get; set; }
        public Nullable<System.DateTime> Pro_DtInc { get; set; }
        public Nullable<System.DateTime> Pro_DtAlt { get; set; }
        public Nullable<System.DateTime> Pro_DtExc { get; set; }
    
        public virtual ICollection<Cad_Setores> Cad_Setores { get; set; }
    }
}

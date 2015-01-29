using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PessoasN
{
    public abstract class Comun
    {
        private int _ID = 0;
        public int ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        
        public DateTime DtInclusao { get; set; }
        public DateTime DtAlteracao { get; set; }
        public DateTime? DtExclusao { get; set; }

        public abstract bool Salvar(out string Erro);
        public abstract bool Excluir(out string Erro);

        public Dictionary<string, string> Erros = new Dictionary<string, string>();

        protected virtual bool Validar()
        {
            this.Erros.Clear();

            foreach(PropertyInfo propI in this.GetType().GetProperties())
            {
                foreach (Validacao.ValidaBase attr in propI.GetCustomAttributes(typeof(Validacao.ValidaTexto), true))
                {
                    if (attr is Validacao.ValidaTexto)
                    {
                        if (((Validacao.ValidaTexto) attr).Obrigatorio)
                        {
                            string valor = (string)propI.GetValue(this, null);

                            if (string.IsNullOrEmpty(valor))
                            {
                                this.Erros.Add(propI.Name, string.Concat(attr.NomeUI, " é Requerido"));
                                
                            }
                            else
                                if (valor.Length > ((Validacao.ValidaTexto)attr).ComprimentoMaximo)
                                {
                                    this.Erros.Add(propI.Name, string.Concat(attr.NomeUI, " deve ter no máximo ", ((Validacao.ValidaTexto)attr).ComprimentoMaximo, " caracteres."));
                                }
                        }
                    }
                }
            }

            return this.Erros.Count() == 0;

        }

        protected virtual bool OnSalvar(out string Erro)
        {
            Erro = "";

            if (this.ID == 0)
                //inclusão
                this.DtInclusao = DateTime.Now;

            else
                //Alteração
                this.SetaCamposAlteracao();

            return true;

        }
        protected virtual bool OnExcluir(out string Erro)
        {
            Erro = "";

            this.DtExclusao = DateTime.Now;

            return true;

        }

        protected void SetaCamposAlteracao()
        {
            //Alteração
            this.DtAlteracao = DateTime.Now;

        }

    }
}

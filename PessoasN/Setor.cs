using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PessoasN
{
    public class Setor : Comun
    {

        public string Nome { get; set; }
        public string Descricao { get; set; }

        private IEnumerable<Setor> ConsegueItemsAtivos()
        {
            return BancoDados.Corrente.Setores.Where(item => item.DtExclusao == null);

        }

        public override bool Salvar(out string Erro)
        {
            base.OnSalvar(out Erro);

            if (string.IsNullOrEmpty(this.Nome))
                Erro = "Nome deve ser informado";

            if (this.Nome.Length > 50)
                Erro = "Nome não pode ter mais de 50 caracteres";

            if (this.Descricao.Length > 50)
                Erro = "Descrição não pode ter mais de 300 caracteres";

            if (string.IsNullOrEmpty(Erro))
            {
                if (this.ID == 0)
                {
                    //inclusão
                    if (BancoDados.Corrente.Setores
                        .Where(item => item.Nome.Trim().ToLower() == this.Nome.Trim().ToLower())
                        .Count() > 0)
                        Erro = string.Concat("O Nome ", this.Nome, " já está sendo usado");

                    //seta novo id
                    this.ID = BancoDados.Corrente.Setores.Max(item => item.ID) + 1;

                    //"salva" para o Banco
                    BancoDados.Corrente.Setores.Add(this);

                }
                else
                {
                    //alteração
                    Setor paraAlterar = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

                    if (paraAlterar == null)
                        Erro = "O Item não foi encontrado para ser alterado";

                    if (BancoDados.Corrente.Setores
                        .Where(
                            item => item.Nome.Trim().ToLower() == this.Nome.Trim().ToLower()
                            && item.ID != this.ID)
                        .Count() > 0)
                        Erro = string.Concat("O Nome ", this.Nome, " já está sendo usado");

                    else
                    { 
                        //"altera" no banco
                        paraAlterar.SetaCamposAlteracao();
                        paraAlterar.Nome = this.Nome;
                        paraAlterar.Descricao = this.Descricao;

                    }
                }
                
                BancoDados.Corrente.SalvaMudancas();

            }

            return string.IsNullOrEmpty(Erro);

        }

        public override bool Excluir(out string Erro)
        {
            base.OnExcluir(out Erro);

            Setor paraExcluir = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

            if (paraExcluir == null)
                Erro = "O Item não foi encontrado para ser excluído";

            if (string.IsNullOrEmpty(Erro) && SendoUsado())
                Erro = "O Item está sendo usado em outro cadastro";

            if (string.IsNullOrEmpty(Erro))
            {
                //"Exclui"
                // dt de exclusão já setada
                BancoDados.Corrente.SalvaMudancas();

            }

            return string.IsNullOrEmpty(Erro);

        }

        private bool SendoUsado()
        {
            return false;

        }

    }
}

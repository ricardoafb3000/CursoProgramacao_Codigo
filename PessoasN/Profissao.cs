using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PessoasN.Validacao;

namespace PessoasN
{
    public class Profissao : Comun
    {

        
        public string Nome { get; set; }
        public string Descricao { get; set; }

        private List<Setor> _SetoresAtuacao = new List<Setor>();
        public List<Setor> SetoresAtuacao
        {
            get { return _SetoresAtuacao; }
            set { _SetoresAtuacao = value; }
        }


        private IEnumerable<Profissao> ConsegueItemsAtivos()
        {
            return BancoDados.Corrente.Profissoes.Where(item => item.DtExclusao == null);

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
                    if (BancoDados.Corrente.Profissoes
                        .Where(item => item.Nome.Trim().ToLower() == this.Nome.Trim().ToLower())
                        .Count() > 0)
                        Erro = string.Concat("O Nome ", this.Nome, " já está sendo usado");

                    //seta novo id
                    this.ID = BancoDados.Corrente.Profissoes.Max(item => item.ID) + 1;

                    //"salva" para o Banco
                    BancoDados.Corrente.Profissoes.Add(this);

                }
                else
                {
                    //alteração
                    Profissao paraAlterar = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

                    if (paraAlterar == null)
                        Erro = "O Item não foi encontrado para ser alterado";

                    if (BancoDados.Corrente.Profissoes
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

            Profissao paraExcluir = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

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

        public bool AdicionaSetorAtuacao(int SetorID, out string Erro)
        {
            Erro = "";

            Setor setorParaAdicionar = BancoDados.Corrente.Setores.Where(item => item.ID == SetorID).FirstOrDefault();

            if (setorParaAdicionar == null)
                Erro = "ID de Setor inválido";

            if (string.IsNullOrEmpty(Erro) && this.SetoresAtuacao.Where(item => item.ID == SetorID).FirstOrDefault() != null)
                Erro = "Setor de Atuação já adicionado";

            if (string.IsNullOrEmpty(Erro))
                //adiciona o setor de atuação
                this.SetoresAtuacao.Add(setorParaAdicionar);

            return string.IsNullOrEmpty(Erro);

        }

        public bool RemoveSetorAtuacao(int SetorID, out string Erro)
        {
            Erro = "";

            Setor setorParaRemover = this.SetoresAtuacao.Where(item => item.ID == SetorID).FirstOrDefault();

            if (setorParaRemover == null)
                Erro = "Setor de Atuação inexistente para Excluir";

            if (string.IsNullOrEmpty(Erro))
                //remove o setor de atuação
                this.SetoresAtuacao.Remove(setorParaRemover);

            return string.IsNullOrEmpty(Erro);

        }

    }
}

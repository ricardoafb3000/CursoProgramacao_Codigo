using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PessoasN.Validacao;

namespace PessoasN
{
    public class Profissao : Comun
    {

        [ValidaTexto("Nome", true, 50)]        
        public string Nome { get; set; }
        [ValidaTexto("Descrição", true, 300)]
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

        public override bool Salvar()
        {
            
            if (base.Validar())
            {
                base.OnSalvar();

                if (this.ID == 0)
                {
                    //inclusão
                    if (BancoDados.Corrente.Profissoes
                        .Where(item => item.Nome.Trim().ToLower() == this.Nome.Trim().ToLower())
                        .Count() > 0)
                        Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));

                    else
                    { 
                        //seta novo id
                        this.ID = BancoDados.Corrente.Profissoes.Max(item => item.ID) + 1;

                        //"salva" para o Banco
                        BancoDados.Corrente.Profissoes.Add(this);

                    }
                }
                else
                {
                    //alteração
                    Profissao paraAlterar = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

                    if (paraAlterar == null)
                        throw new Exception("O Item não foi encontrado para ser alterado");

                    if (BancoDados.Corrente.Profissoes
                        .Where(
                            item => item.Nome.Trim().ToLower() == this.Nome.Trim().ToLower()
                            && item.ID != this.ID)
                        .Count() > 0)
                        Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));

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

            return Erros.Count == 0;

        }

        public override bool Excluir()
        {
            base.OnExcluir();

            Profissao paraExcluir = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

            if (paraExcluir == null)
                throw new Exception("O Item não foi encontrado para ser excluído");

            if (SendoUsado())
                Erros.Add("Nome", "O Item está sendo usado em outro cadastro");

            if (Erros.Count == 0)
            {
                //"Exclui"
                // dt de exclusão já setada
                BancoDados.Corrente.SalvaMudancas();

            }

            return Erros.Count == 0;

        }

        private bool SendoUsado()
        {
            return false;

        }

        public bool AdicionaSetorAtuacao(int SetorID)
        {
            Erros.Clear();

            Setor setorParaAdicionar = BancoDados.Corrente.Setores.Where(item => item.ID == SetorID).FirstOrDefault();

            if (setorParaAdicionar == null)
                throw new  Exception("ID de Setor inválido");

            if (this.SetoresAtuacao.Where(item => item.ID == SetorID).FirstOrDefault() != null)
                Erros.Add("", "Setor de Atuação já adicionado");

            if (Erros.Count == 0)
                //adiciona o setor de atuação
                this.SetoresAtuacao.Add(setorParaAdicionar);

            return Erros.Count == 0;

        }

        public bool RemoveSetorAtuacao(int SetorID)
        {
            Erros.Clear();

            Setor setorParaRemover = this.SetoresAtuacao.Where(item => item.ID == SetorID).FirstOrDefault();

            if (setorParaRemover == null)
                throw new Exception("Setor de Atuação inexistente para Excluir");

            //remove o setor de atuação
            this.SetoresAtuacao.Remove(setorParaRemover);

            return Erros.Count == 0;

        }

    }
}

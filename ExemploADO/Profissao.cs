using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExemploADO
{
    public class Profissao : PessoasN.Profissao
    {

        public override bool Salvar()
        {
            if (base.Validar())
            {
                base.OnSalvar();

                using (ExemploADO.dsCadastroTableAdapters.Cad_ProfissoesTableAdapter adapter = new dsCadastroTableAdapters.Cad_ProfissoesTableAdapter())
                {
                    dsCadastro.Cad_ProfissoesDataTable dt = new dsCadastro.Cad_ProfissoesDataTable();

                    if (ID == 0)
                    {
                        //Inclusão
                        adapter.FillByNome(dt, this.Nome);

                        if (dt.Rows.Count > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;

                        }

                        dsCadastro.Cad_ProfissoesRow newRow = dt.NewCad_ProfissoesRow();

                        newRow.Pro_Nome = this.Nome;
                        newRow.Pro_Descricao = this.Descricao;
                        newRow.Pro_DtInc = this.DtInclusao;

                        dt.AddCad_ProfissoesRow(newRow);

                        adapter.Update(dt);

                        //seta o novo ID
                        this.ID = newRow.Pro_ID;

                    }
                    else
                    {
                        //Alteração
                        adapter.FillByNomeNaoEsteID(dt, this.Nome, this.ID);

                        if (dt.Rows.Count > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;
                        }

                        //obtém o registro para ser alterado
                        adapter.FillByID(dt, this.ID);

                        if (dt.Rows.Count == 0)
                            throw new Exception("O Item não foi encontrado para ser alterado");

                        //define o registro a ser alterado
                        dsCadastro.Cad_ProfissoesRow rowSendoAlterado = (dsCadastro.Cad_ProfissoesRow)dt.Rows[0];

                        //Altera o registro
                        rowSendoAlterado.Pro_Nome = this.Nome;
                        rowSendoAlterado.Pro_Descricao = this.Descricao;
                        rowSendoAlterado.Pro_DtAlt = this.DtAlteracao;

                        adapter.Update(dt);

                    }
                }
            }

            return Erros.Count == 0;

        }

        public override bool Excluir()
        {
            using (ExemploADO.dsCadastroTableAdapters.Cad_ProfissoesTableAdapter adapter = new dsCadastroTableAdapters.Cad_ProfissoesTableAdapter())
            {
                dsCadastro.Cad_ProfissoesDataTable dt = new dsCadastro.Cad_ProfissoesDataTable();

                //obtém o registro para ser alterado
                adapter.FillByID(dt, this.ID);

                if (dt.Rows.Count == 0)
                    throw new Exception("O Item não foi encontrado para ser excluído");

                adapter.Deletar(this.ID);

            }

            return true;
        }

        public override bool AdicionaSetorAtuacao(int SetorID)
        {
            Erros.Clear();

            Setor setorParaAdicionar = Setor.PesquisarPorID(SetorID);

            if (setorParaAdicionar == null)
                throw new Exception("ID de Setor inválido");

            if (this.ID == 0)
            {
                //Profissão não incluída no Banco de Dados
                if (this.SetoresAtuacao.Where(item => item.ID == SetorID).FirstOrDefault() != null)
                    Erros.Add("", "Setor de Atuação já adicionado");

                else
                    SetoresAtuacao.Add(setorParaAdicionar);

            }
            else
            {
                //Profissão no Banco de dados
                using(ExemploADO.dsCadastroTableAdapters.Cad_ProfSetAtuacaoTableAdapter adapter = new dsCadastroTableAdapters.Cad_ProfSetAtuacaoTableAdapter())
                {
                    dsCadastro.Cad_ProfSetAtuacaoDataTable dt = new dsCadastro.Cad_ProfSetAtuacaoDataTable();

                    adapter.FillByIDs(dt, this.ID, SetorID);

                    if (dt.Rows.Count > 0)
                        Erros.Add("", "Setor de Atuação já adicionado");

                    else
                    {
                        dsCadastro.Cad_ProfSetAtuacaoRow novo = dt.NewCad_ProfSetAtuacaoRow();

                        novo.SetA_ProID = this.ID;
                        novo.SetA_SetID = SetorID;

                        dt.AddCad_ProfSetAtuacaoRow(novo);

                        adapter.Update(dt);


                    }
                }
            }
            
            return Erros.Count == 0;

        }

        public override bool RemoveSetorAtuacao(int SetorID)
        {
            Erros.Clear();

            PessoasN.Setor setorParaRemover = this.SetoresAtuacao.Where(item => item.ID == SetorID).FirstOrDefault();

            if (setorParaRemover == null)
                throw new Exception("Setor de Atuação inexistente para Excluir");

            if (this.ID == 0)
            {
                //Profissão não incluída no Banco de Dados
                //  Remove da coleção interna
                SetoresAtuacao.Add(setorParaRemover);

            }
            else
            {
                //Profissão no Banco de dados
                using (ExemploADO.dsCadastroTableAdapters.Cad_ProfSetAtuacaoTableAdapter adapter = new dsCadastroTableAdapters.Cad_ProfSetAtuacaoTableAdapter())
                {
                    adapter.Deletar(this.ID, SetorID);
                }
            }

            return Erros.Count == 0;

        }


    }
}

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExemploLinqToSql
{
    public class Profissao : PessoasN.Profissao
    {

        private new List<Setor> _SetoresAtuacao = null;
        public new List<Setor> SetoresAtuacao
        {
            get 
            { 
                if (_SetoresAtuacao == null)
                {
                    this._SetoresAtuacao = new List<Setor>();

                    if (this.ID != 0)
                    {
                        //Está no banco de dados
                        //  procura pelos setores de atuação
                        using (dsCadastroDataContext db = new dsCadastroDataContext())
                        {
                            var query = from setAtuacao in db.Cad_ProfSetAtuacaos
                                        join setores in db.Cad_Setores
                                            on setAtuacao.SetA_SetID equals setores.Set_ID
                                        where
                                            setAtuacao.SetA_ProID == this.ID
                                        select setores;

                            foreach (Cad_Setores setorDoBanco in query)
                            {
                                this._SetoresAtuacao.Add(Setor.ObtemDoDb(setorDoBanco));

                            }
                        }
                    }
                }
                
                return _SetoresAtuacao; 

            }
        }

        public Cad_Profissoes ObtemDb()
        {
            return new Cad_Profissoes()
            {
                Pro_ID = this.ID,
                Pro_Nome = this.Nome,
                Pro_Descricao = this.Descricao,
                Pro_DtInc = this.DtInclusao,
                Pro_DtAlt = this.DtAlteracao,
                Pro_DtExc = this.DtExclusao

            };
        }

        public static Profissao ObtemDoDb(Cad_Profissoes dbItem)
        {
            Profissao resposta = new Profissao();

            resposta.CarregaDoDb(dbItem);

            return resposta;

        }

        public void CarregaDoDb(Cad_Profissoes dbItem)
        {
            this.ID = dbItem.Pro_ID;
            this.Nome = dbItem.Pro_Nome;
            this.Descricao = dbItem.Pro_Descricao;
            this.DtInclusao = dbItem.Pro_DtInc.Value;
            this.DtAlteracao = dbItem.Pro_DtAlt.Value;
            this.DtExclusao = dbItem.Pro_DtExc;

        }

        public void CarregaProDb(Cad_Profissoes dbItem)
        {
            dbItem.Pro_ID = this.ID;
            dbItem.Pro_Nome = this.Nome;
            dbItem.Pro_Descricao = this.Descricao;
            dbItem.Pro_DtInc = this.DtInclusao;
            dbItem.Pro_DtAlt = this.DtAlteracao;
            dbItem.Pro_DtExc = this.DtExclusao;

        }

        public static Profissao PesquisarPorID(int ID)
        {

            using (dsCadastroDataContext db = new dsCadastroDataContext())
            {
                Cad_Profissoes resposta = db.Cad_Profissoes.Where(item => item.Pro_ID == ID).FirstOrDefault();

                if (resposta != null)
                    return ObtemDoDb(resposta);

            }

            return null;

        }

        public override bool Salvar()
        {
            if (base.Validar())
            {
                base.OnSalvar();

                using (dsCadastroDataContext db = new dsCadastroDataContext())
                {
                    if (ID == 0)
                    {
                        //Inclusão
                        if (db.Cad_Profissoes.Where(item => item.Pro_Nome.Trim().ToLower() == this.Nome.Trim().ToLower()).Count() > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;

                        }

                        Cad_Profissoes novo = this.ObtemDb();

                        db.Cad_Profissoes.InsertOnSubmit(novo);
                        db.SubmitChanges();
                        
                        //seta o novo ID
                        this.ID = novo.Pro_ID;

                    }
                    else
                    {
                        //Alteração
                        if (db.Cad_Profissoes.Where(item => 
                            item.Pro_Nome.Trim().ToLower() == this.Nome.Trim().ToLower()
                            && item.Pro_ID != this.ID).Count() > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;
                        }

                        //obtém o registro para ser alterado
                        Cad_Profissoes itemSendoAlterado = db.Cad_Profissoes.Where(item => item.Pro_ID == this.ID).FirstOrDefault();

                        if (itemSendoAlterado == null)
                            throw new Exception("O Item não foi encontrado para ser alterado");

                        //Altera o registro
                        this.CarregaProDb(itemSendoAlterado);

                        db.SubmitChanges();

                    }
                }
            }

            return Erros.Count == 0;

        }

        public override bool Excluir()
        {
            using (dsCadastroDataContext db = new dsCadastroDataContext())
            {
                //obtém o registro para ser alterado
                Cad_Profissoes itemSendoExcluido = db.Cad_Profissoes.Where(item => item.Pro_ID == this.ID).FirstOrDefault();

                if (itemSendoExcluido == null)
                    throw new Exception("O Item não foi encontrado para ser excluído");

                //Exclui primeiro os setores de atuação
                db.Cad_ProfSetAtuacaos.DeleteAllOnSubmit<Cad_ProfSetAtuacao>(itemSendoExcluido.Cad_ProfSetAtuacaos);
                //Exclue Profissão
                db.Cad_Profissoes.DeleteOnSubmit(itemSendoExcluido);
                
                db.SubmitChanges();

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
                using (dsCadastroDataContext db = new dsCadastroDataContext())
                {
                    if (db.Cad_ProfSetAtuacaos
                        .Where(item =>
                            item.SetA_ProID == this.ID && item.SetA_SetID == SetorID).Count() > 0)
                        Erros.Add("", "Setor de Atuação já adicionado");

                    else
                    {
                        Cad_ProfSetAtuacao novo = new Cad_ProfSetAtuacao();

                        novo.SetA_ProID = this.ID;
                        novo.SetA_SetID = SetorID;

                        db.Cad_ProfSetAtuacaos.InsertOnSubmit(novo);
                        
                        db.SubmitChanges();

                    }
                }
            }

            return Erros.Count == 0;

        }

        public override bool RemoveSetorAtuacao(int SetorID)
        {
            Erros.Clear();

            Setor setorParaRemover = this.SetoresAtuacao.Where(item => item.ID == SetorID).FirstOrDefault();

            if (setorParaRemover == null)
                throw new Exception("Setor de Atuação inexistente para Excluir");

            if (this.ID == 0)
            {
                //Profissão não incluída no Banco de Dados
                //  Remove da coleção interna
                SetoresAtuacao.Remove(setorParaRemover);

            }
            else
            {
                //Profissão no Banco de dados
                using (dsCadastroDataContext db = new dsCadastroDataContext())
                {
                    Cad_ProfSetAtuacao itemSendoExcluido = db.Cad_ProfSetAtuacaos
                        .Where(item => 
                            item.SetA_ProID == this.ID
                            && item.SetA_SetID == SetorID).FirstOrDefault();

                    if (itemSendoExcluido == null)
                        throw new Exception("O Item não foi encontrado para ser excluído");

                    db.Cad_ProfSetAtuacaos.DeleteOnSubmit(itemSendoExcluido);
                    db.SubmitChanges();

                }
            }

            return Erros.Count == 0;

        }

    }
}

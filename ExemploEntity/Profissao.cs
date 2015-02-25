using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExemploEntity
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
                        using (db.CadastroEntities db = new db.CadastroEntities())
                        {
                            db.Cad_Profissoes profissao = db.Cad_Profissoes.Find(this.ID);


                            foreach (db.Cad_Setores setorDoBanco in profissao.Cad_Setores)
                            {
                                this._SetoresAtuacao.Add(Setor.ObtemDoDb(setorDoBanco));

                            }
                        }
                    }
                }
                
                return _SetoresAtuacao; 

            }
        }

        public db.Cad_Profissoes ObtemDb()
        {
            return new db.Cad_Profissoes()
            {
                Pro_ID = this.ID,
                Pro_Nome = this.Nome,
                Pro_Descricao = this.Descricao,
                Pro_DtInc = this.DtInclusao,
                Pro_DtAlt = this.DtAlteracao,
                Pro_DtExc = this.DtExclusao

            };
        }

        public static Profissao ObtemDoDb(db.Cad_Profissoes dbItem)
        {
            Profissao resposta = new Profissao();

            resposta.CarregaDoDb(dbItem);

            return resposta;

        }

        public void CarregaDoDb(db.Cad_Profissoes dbItem)
        {
            this.ID = dbItem.Pro_ID;
            this.Nome = dbItem.Pro_Nome;
            this.Descricao = dbItem.Pro_Descricao;
            this.DtInclusao = dbItem.Pro_DtInc.Value;
            this.DtAlteracao = dbItem.Pro_DtAlt.Value;
            this.DtExclusao = dbItem.Pro_DtExc;

        }

        public void CarregaProDb(db.Cad_Profissoes dbItem)
        {
            dbItem.Pro_ID = this.ID;
            dbItem.Pro_Nome = this.Nome;
            dbItem.Pro_Descricao = this.Descricao;
            if (this.ID == 0)
                dbItem.Pro_DtInc = this.DtInclusao;
            dbItem.Pro_DtAlt = this.DtAlteracao;
            dbItem.Pro_DtExc = this.DtExclusao;

        }

        public static Profissao PesquisarPorID(int ID)
        {

            using (db.CadastroEntities db = new db.CadastroEntities())
            {
                db.Cad_Profissoes resposta = db.Cad_Profissoes.Where(item => item.Pro_ID == ID).FirstOrDefault();

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

                using (db.CadastroEntities db = new db.CadastroEntities())
                {
                    if (ID == 0)
                    {
                        //Inclusão
                        if (db.Cad_Profissoes.Where(item => item.Pro_Nome.Trim().ToLower() == this.Nome.Trim().ToLower()).Count() > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;

                        }

                        db.Cad_Profissoes novo = this.ObtemDb();

                        db.Cad_Profissoes.Add(novo);
                        db.SaveChanges();
                        
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
                        db.Cad_Profissoes itemSendoAlterado = db.Cad_Profissoes.Find(this.ID);

                        if (itemSendoAlterado == null)
                            throw new Exception("O Item não foi encontrado para ser alterado");

                        //Altera o registro
                        this.CarregaProDb(itemSendoAlterado);

                        db.SaveChanges();

                    }
                }
            }

            return Erros.Count == 0;

        }

        public override bool Excluir()
        {
            using (db.CadastroEntities db = new db.CadastroEntities())
            {
                //obtém o registro para ser alterado
                db.Cad_Profissoes itemSendoExcluido = db.Cad_Profissoes.Find(this.ID);

                if (itemSendoExcluido == null)
                    throw new Exception("O Item não foi encontrado para ser excluído");

                //Exclue Profissão
                db.Cad_Profissoes.Remove(itemSendoExcluido);
                
                db.SaveChanges();

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
                using (db.CadastroEntities db = new db.CadastroEntities())
                {
                    db.Cad_Profissoes profissao = db.Cad_Profissoes.Find(this.ID);

                    if (profissao.Cad_Setores
                        .Where(item =>
                            item.Set_ID == SetorID).Count() > 0)
                        Erros.Add("", "Setor de Atuação já adicionado");

                    else
                    {
                        db.Cad_Setores novo = db.Cad_Setores.Find(SetorID);

                        profissao.Cad_Setores.Add(novo);
                       
                        db.SaveChanges();

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
                using (db.CadastroEntities db = new db.CadastroEntities())
                {
                    db.Cad_Profissoes profissao = db.Cad_Profissoes.Find(this.ID);

                    db.Cad_Setores itemSendoExcluido = profissao.Cad_Setores
                        .Where(item => 
                            item.Set_ID == SetorID).FirstOrDefault();

                    if (itemSendoExcluido == null)
                        throw new Exception("O Item não foi encontrado para ser excluído");

                    profissao.Cad_Setores.Remove(itemSendoExcluido);
                    db.SaveChanges();

                }
            }

            return Erros.Count == 0;

        }

    }
}

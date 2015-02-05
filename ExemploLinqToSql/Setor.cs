using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExemploLinqToSql
{
    public class Setor : PessoasN.Setor
    {

        public Cad_Setores ObtemDb()
        {
            return new Cad_Setores()
            {
                Set_ID = this.ID,
                Set_Nome = this.Nome,
                Set_Descricao = this.Descricao,
                Set_DtInc = this.DtInclusao,
                Set_DtAlt = this.DtAlteracao,
                Set_DtExc = this.DtExclusao

            };
        }

        public static Setor ObtemDoDb(Cad_Setores dbItem)
        {
            Setor resposta = new Setor();

            resposta.CarregaDoDb(dbItem);

            return resposta;

        }

        public void CarregaDoDb(Cad_Setores dbItem)
        {
            this.ID = dbItem.Set_ID;
            this.Nome = dbItem.Set_Nome;
            this.Descricao = dbItem.Set_Descricao;
            this.DtInclusao = dbItem.Set_DtInc.Value;
            this.DtAlteracao = dbItem.Set_DtAlt.Value;
            this.DtExclusao = dbItem.Set_DtExc;

        }

        public void CarregaProDb(Cad_Setores dbItem)
        {
            dbItem.Set_ID = this.ID;
            dbItem.Set_Nome = this.Nome;
            dbItem.Set_Descricao = this.Descricao;
            dbItem.Set_DtInc = this.DtInclusao;
            dbItem.Set_DtAlt = this.DtAlteracao;
            dbItem.Set_DtExc = this.DtExclusao;

        }

        public static Setor PesquisarPorID(int ID)
        {

            using (dsCadastroDataContext db = new dsCadastroDataContext())
            {
                Cad_Setores resposta = db.Cad_Setores.Where(item => item.Set_ID == ID).FirstOrDefault();

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
                        if (db.Cad_Setores.Where(item => item.Set_Nome.Trim().ToLower() == this.Nome.Trim().ToLower()).Count() > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;

                        }

                        Cad_Setores novo = this.ObtemDb();

                        db.Cad_Setores.InsertOnSubmit(novo);
                        db.SubmitChanges();
                        
                        //seta o novo ID
                        this.ID = novo.Set_ID;

                    }
                    else
                    {
                        //Alteração
                        if (db.Cad_Setores.Where(item => 
                            item.Set_Nome.Trim().ToLower() == this.Nome.Trim().ToLower()
                            && item.Set_ID != this.ID).Count() > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;
                        }

                        //obtém o registro para ser alterado
                        Cad_Setores itemSendoAlterado = db.Cad_Setores.Where(item => item.Set_ID == this.ID).FirstOrDefault();

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
                Cad_Setores itemSendoExcluido = db.Cad_Setores.Where(item => item.Set_ID == this.ID).FirstOrDefault();

                if (itemSendoExcluido == null)
                    throw new Exception("O Item não foi encontrado para ser excluído");

                db.Cad_Setores.DeleteOnSubmit(itemSendoExcluido);
                db.SubmitChanges();

            }

            return true;
        }

    }
}

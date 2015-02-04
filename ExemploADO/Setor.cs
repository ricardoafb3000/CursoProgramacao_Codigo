using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace ExemploADO
{
    public class Setor : PessoasN.Setor
    {

        public override bool Salvar()
        {
            if (base.Validar())
            {
                base.OnSalvar();

                using (ExemploADO.dsCadastroTableAdapters.Cad_SetoresTableAdapter adapter = new dsCadastroTableAdapters.Cad_SetoresTableAdapter())
                {
                    dsCadastro.Cad_SetoresDataTable dt = new dsCadastro.Cad_SetoresDataTable();

                    if (ID == 0)
                    {
                        //Inclusão
                        adapter.FillByNome(dt, this.Nome);

                        if (dt.Rows.Count > 0)
                        {
                            Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));
                            return false;

                        }

                        dsCadastro.Cad_SetoresRow newRow = dt.NewCad_SetoresRow();

                        newRow.Set_Nome = this.Nome;
                        newRow.Set_Descricao = this.Descricao;
                        newRow.Set_DtInc = this.DtInclusao;

                        dt.AddCad_SetoresRow(newRow);
                        //seta o novo ID
                        this.ID = newRow.Set_ID;

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
                        dsCadastro.Cad_SetoresRow rowSendoAlterado = (dsCadastro.Cad_SetoresRow)dt.Rows[0];

                        //Altera o registro
                        rowSendoAlterado.Set_Nome = this.Nome;
                        rowSendoAlterado.Set_Descricao = this.Descricao;
                        rowSendoAlterado.Set_DtAlt = this.DtAlteracao;

                    }

                    adapter.Adapter.Update(dt);

                }
            }

            return Erros.Count == 0;

        }

        public override bool Excluir()
        {
            using (ExemploADO.dsCadastroTableAdapters.Cad_SetoresTableAdapter adapter = new dsCadastroTableAdapters.Cad_SetoresTableAdapter())
            {
                dsCadastro.Cad_SetoresDataTable dt = new dsCadastro.Cad_SetoresDataTable();

                //obtém o registro para ser alterado
                adapter.FillByID(dt, this.ID);

                if (dt.Rows.Count == 0)
                    throw new Exception("O Item não foi encontrado para ser excluído");

                adapter.Deletar(this.ID);

            }

            return true;
        }

    }
}

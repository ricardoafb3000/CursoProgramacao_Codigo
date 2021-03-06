﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PessoasN.Validacao;

namespace PessoasN
{
    public class TpLogradouro : Comun
    {
        [ValidaTexto("Nome", true, 50)]
        public string Nome { get; set; }

        private IEnumerable<TpLogradouro> ConsegueItemsAtivos()
        {
            return BancoDados.Corrente.TpLogradouros.Where(item => item.DtExclusao == null);

        }

        public override bool Salvar()
        {
            
            if (base.Validar())
            {
                base.OnSalvar();

                if (this.ID == 0)
                {
                    //inclusão
                    if (BancoDados.Corrente.TpLogradouros
                        .Where(item => item.Nome.Trim().ToLower() == this.Nome.Trim().ToLower())
                        .Count() > 0)
                        Erros.Add("Nome", string.Concat("O Nome ", this.Nome, " já está sendo usado"));

                    else
                    { 
                        //seta novo id
                        this.ID = BancoDados.Corrente.TpLogradouros.Max(item => item.ID) + 1;

                        //"salva" para o Banco
                        BancoDados.Corrente.TpLogradouros.Add(this);

                    }
                }
                else
                {
                    //alteração
                    TpLogradouro paraAlterar = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

                    if (paraAlterar == null)
                        throw new Exception("O Item não foi encontrado para ser alterado");

                    if (BancoDados.Corrente.TpLogradouros
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

                    }
                }
                
                BancoDados.Corrente.SalvaMudancas();

            }

            return Erros.Count == 0;

        }

        public override bool Excluir()
        {
            base.OnExcluir();

            TpLogradouro paraExcluir = ConsegueItemsAtivos().Where(item => item.ID == this.ID).FirstOrDefault();

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

    }
}

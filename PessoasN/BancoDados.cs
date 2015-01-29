using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace PessoasN
{
    public class BancoDados
    {

        public static BancoDados Corrente = null;

        #region |   Tabelas   |

        public List<TpLogradouro> TpLogradouros = new List<TpLogradouro>();
        public List<Setor> Setores = new List<Setor>();
        public List<Profissao> Profissoes = new List<Profissao>();

        #endregion

        private static string CaminhoDbArquivo
        {
            get
            {
                return Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location),
                    "data.xml");
            }
        }


        public static BancoDados CarregaBancoDados()
        {
            if (File.Exists(CaminhoDbArquivo))
            {
                //carrega o Banco Corrente
                string xml = File.ReadAllText(CaminhoDbArquivo, Encoding.Default);

                BancoDados.Corrente = xml.GetObjectXml<BancoDados>();

            }
            else
            {
                //carrega dados de teste
                BancoDados.Corrente = new BancoDados();
                BancoDados.Corrente.CarregaDadosTeste();

            }

            return BancoDados.Corrente;

        }

        private void CarregaDadosTeste()
        {
            //Fazer:
            this.TpLogradouros = new List<TpLogradouro>(
                new TpLogradouro[] { 
                    new TpLogradouro() {ID = 1, Nome = "Rua", DtInclusao = DateTime.Now, DtAlteracao = DateTime.Now},
                    new TpLogradouro() {ID = 2, Nome = "Avenida", DtInclusao = DateTime.Now, DtAlteracao = DateTime.Now}

                    }
                );

        }

        public void SalvaMudancas()
        {
            string xml = this.GetXml();

            //Salva o xml para o arquivo de banco de dados
            using (StreamWriter outfile = new StreamWriter(CaminhoDbArquivo, false, Encoding.Default))
            {
                outfile.Write(xml);
            }

        }

    }
}

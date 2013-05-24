using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

using upSight.Consulta.Base;
using System.Diagnostics;

namespace upSight.CartaoCorp
{
    public class crtACSOIDTSCCabecalho
    {
        public crtACSOIDTSCCabecalho(){ }


        /// <summary>
        /// Parseia os dados da linha referente ao cabeçalho do arquivo e insere na base de dados
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        public void MapeiaLinhaCabArquivoEInsereBD(string linha, int idArquivo)
        {
            try
            {
                string tpRegistro = linha.Substring(0, 1);
                string nomeLayout = linha.Substring(1, 20);
                string versao = linha.Substring(21, 8);
                DateTime dtGeracao = Data.ParseEstendido(linha.Substring(29, 14), Data.FormatoData.AAAAMMDDHHMMSS);
                int seqArquivo = Convert.ToInt32(linha.Substring(43, 2));
                string nomeArquivo = linha.Substring(45, 50);
                string codConvenio = linha.Substring(95, 10);
                string codEmpresa = linha.Substring(105, 14);
                int numLinha = Convert.ToInt32(linha.Substring(linha.Length - 6, 6));

                this.InsereCabecalho(idArquivo, tpRegistro, nomeLayout, versao, dtGeracao, seqArquivo, nomeArquivo, codConvenio, codEmpresa, numLinha);
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Cab", e });
                throw;
            }
        }

        /// <summary>
        /// Insere os dados na crtACSOIDTSCCabecalho
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="tpRegistro"></param>
        /// <param name="nomeLayout"></param>
        /// <param name="versao"></param>
        /// <param name="dtGeracao"></param>
        /// <param name="seqArquivo"></param>
        /// <param name="nomeArquivo"></param>
        /// <param name="codConvenio"></param>
        /// <param name="codEmpresa"></param>
        /// <param name="numLinha"></param>
        public void InsereCabecalho(int idArquivo, string tpRegistro, string nomeLayout, string versao, DateTime dtGeracao, int seqArquivo, string nomeArquivo,
                                     string codConvenio, string codEmpresa, int numLinha)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT [crtACSOIDTSCCabecalho] " +
                               " (IdArquivo, TpRegistro, NomeLayout, Versao, DataGeracaoArquivo, SeqArquivo, NomeArquivo, " +
                               " CodConvenio, CodEmpresa, NumLinha) " +
                               " SELECT @IdArquivo, @TpRegistro, @NomeLayout, @Versao, @DataGeracaoArquivo, @SeqArquivo, @NomeArquivo, " +
                               " @CodConvenio, @CodEmpresa, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = tpRegistro;
                        cmd.Parameters.Add("NomeLayout", SqlDbType.VarChar, 20).Value = nomeLayout.TrimEnd(null);
                        cmd.Parameters.Add("Versao", SqlDbType.VarChar, 8).Value = versao.TrimEnd(null);
                        cmd.Parameters.Add("DataGeracaoArquivo", SqlDbType.DateTime).Value = dtGeracao;
                        cmd.Parameters.Add("SeqArquivo", SqlDbType.Int).Value = seqArquivo;
                        cmd.Parameters.Add("NomeArquivo", SqlDbType.VarChar, 50).Value = nomeArquivo.TrimEnd(null);
                        cmd.Parameters.Add("CodConvenio", SqlDbType.VarChar, 10).Value = codConvenio.TrimEnd(null);
                        cmd.Parameters.Add("CodEmpresa", SqlDbType.VarChar, 14).Value = codEmpresa.TrimEnd(null);
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = numLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Cab", sqlExc });
                    throw;
                }
            }
        }
    }
}

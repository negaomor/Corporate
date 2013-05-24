using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public static class crtACSOIDTSCCabecalhoBD
    {
        /// <summary>
        /// Insere os dados na crtACSOIDTSCCabecalho
        /// </summary>
        /// <param name="acsIdstCab"></param>
        public static void Insere(this crtACSOIDTSCCabecalhoEN acsIdstCab)
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

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsIdstCab.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = acsIdstCab.TpRegistro;
                        cmd.Parameters.Add("NomeLayout", SqlDbType.VarChar, 20).Value = acsIdstCab.NomeLayout;
                        cmd.Parameters.Add("Versao", SqlDbType.VarChar, 8).Value = acsIdstCab.Versao;
                        cmd.Parameters.Add("DataGeracaoArquivo", SqlDbType.DateTime).Value = acsIdstCab.DtGeracao;
                        cmd.Parameters.Add("SeqArquivo", SqlDbType.Int).Value = acsIdstCab.SeqArquivo;
                        cmd.Parameters.Add("NomeArquivo", SqlDbType.VarChar, 50).Value = acsIdstCab.NomeArquivo;
                        cmd.Parameters.Add("CodConvenio", SqlDbType.VarChar, 10).Value = acsIdstCab.CodConvenio;
                        cmd.Parameters.Add("CodEmpresa", SqlDbType.VarChar, 14).Value = acsIdstCab.CodEmpresa;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsIdstCab.NumLinha;

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

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BDGeral = upSight.Consulta.Base.BD.Geral;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTSC_R
{
    public static class ACSOIDTSC_RCabecalhoBD
    {

        private static void Mapeia(SqlDataReader dr, ACSOIDTSC_RCabecalhoEN acsIdtCabEN)
        {
            if (dr.HasRows)
            {
                acsIdtCabEN.IdArquivo = (int)dr["IdArquivo"];
                acsIdtCabEN.DataGeracao = Convert.ToDateTime(dr["DataGeracao"].ToString());
                acsIdtCabEN.SeqArquivo = (byte)dr["SeqArquivo"];
                acsIdtCabEN.NomeArquivo = dr["NomeArquivo"].ToString();
                acsIdtCabEN.CodConvenio = dr["CodConvenio"].ToString();
                acsIdtCabEN.CodEmpresa = dr["CodEmpresa"].ToString();
                acsIdtCabEN.NumLinha = (int)dr["NumLinha"];
            }
        }

        /// <summary>
        /// Dado um idArquivo consulto os dados para geração do arquivo
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static void Insere(this ACSOIDTSC_RCabecalhoEN acsIdtretCab)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpACSOIDTSC_RCabecalho " +
                                   "        (idArquivo, TpRegistro, NomeLayout, Versao, DataGeracao, SeqArquivo, NomeArquivo, CodConvenio, CodEmpresa, NumLinha) " +
                                   " SELECT @idArquivo, @TpRegistro, @NomeLayout, @Versao, @DataGeracao, @SeqArquivo, @NomeArquivo, @CodConvenio, @CodEmpresa, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("idArquivo", SqlDbType.Int).Value = acsIdtretCab.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOIDTSC_RCabecalhoEN.TpRegistro;
                        cmd.Parameters.Add("NomeLayout", SqlDbType.VarChar, 20).Value = ACSOIDTSC_RCabecalhoEN.NomeLayout;
                        cmd.Parameters.Add("Versao", SqlDbType.VarChar, 8).Value = ACSOIDTSC_RCabecalhoEN.Versao;
                        cmd.Parameters.Add("DataGeracao", SqlDbType.DateTime2).Value = acsIdtretCab.DataGeracao;
                        cmd.Parameters.Add("SeqArquivo", SqlDbType.TinyInt).Value = acsIdtretCab.SeqArquivo;
                        cmd.Parameters.Add("NomeArquivo", SqlDbType.VarChar, 50).Value = acsIdtretCab.NomeArquivo;
                        cmd.Parameters.Add("CodConvenio", SqlDbType.VarChar, 10).Value = acsIdtretCab.CodConvenio;
                        cmd.Parameters.Add("CodEmpresa", SqlDbType.VarChar, 14).Value = acsIdtretCab.CodEmpresa;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsIdtretCab.NumLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTSC_R.CabBD", sqlExc });
                    throw;
                }
                catch (Exception ex)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTSC_R.Cab", ex });
                    throw;
                }
            }
        }

        /// <summary>
        /// Dado um idArquivo consulto os dados para geração do arquivo
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static ACSOIDTSC_RCabecalhoEN ConsultaPorIdArquivo(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                ACSOIDTSC_RCabecalhoEN acsIdtCabEN = null;
                try
                {
                    string query = " SELECT IdArquivo, DataGeracao, SeqArquivo, NomeArquivo, CodConvenio, CodEmpresa, NumLinha " +
                                   " FROM crpACSOIDTSC_RCabecalho " +
                                   " WHERE IdArquivo = @IdArquivo ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;

                        cnx.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            while (dr.Read())
                            {
                                acsIdtCabEN = new ACSOIDTSC_RCabecalhoEN();
                                Mapeia(dr, acsIdtCabEN);
                            }
                        }
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.Integ.Servicos.CrtCorp, IdtnfRBD", sqlExc });
                    throw;
                }
                catch (Exception ex)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.Integ.Servicos.CrtCorp, IdtnfR", ex });
                    throw;
                }
                finally
                {
                    if (cnx.State == ConnectionState.Open)
                        cnx.Close();
                }
                return acsIdtCabEN;
            }
        }
    }
}

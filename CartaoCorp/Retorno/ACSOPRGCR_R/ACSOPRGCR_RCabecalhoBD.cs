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


namespace upSight.CartaoCorp.Carga.ACSOPRGCR_R
{
    public static class ACSOPRGCR_RCabecalhoBD
    {
        private static void Mapeia(SqlDataReader dr, ACSOPRGCR_RCabecalhoEN acsPrgCrgCabEN)
        {
            if (dr.HasRows)
            {
                acsPrgCrgCabEN.IdArquivo = (int)dr["IdArquivo"];
                acsPrgCrgCabEN.DataGeracao = Convert.ToDateTime(dr["DataGeracao"].ToString());
                acsPrgCrgCabEN.SeqArquivo = (byte)dr["SeqArquivo"];
                acsPrgCrgCabEN.NomeArquivo = dr["NomeArquivo"].ToString();
                acsPrgCrgCabEN.CodConvenio = dr["CodConvenio"].ToString();
                acsPrgCrgCabEN.CodEmpresa = dr["CodEmpresa"].ToString();
                acsPrgCrgCabEN.NumLinha = (int)dr["NumLinha"];
            }
        }

        public static void Insere(this ACSOPRGCR_RCabecalhoEN acsCrgRetCab)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpACSOPRGCR_RCabecalho " +
                                   "        (idArquivo, TpRegistro, NomeLayout, Versao, DataGeracao, SeqArquivo, NomeArquivo, CodConvenio, CodEmpresa, NumLinha) " +
                                   " SELECT @idArquivo, @TpRegistro, @NomeLayout, @Versao, @DataGeracao, @SeqArquivo, @NomeArquivo, @CodConvenio, @CodEmpresa, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("idArquivo", SqlDbType.Int).Value = acsCrgRetCab.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCR_RCabecalhoEN.TpRegistro;
                        cmd.Parameters.Add("NomeLayout", SqlDbType.VarChar, 20).Value = ACSOPRGCR_RCabecalhoEN.NomeLayout;
                        cmd.Parameters.Add("Versao", SqlDbType.VarChar, 8).Value = ACSOPRGCR_RCabecalhoEN.Versao;
                        cmd.Parameters.Add("DataGeracao", SqlDbType.DateTime2).Value = acsCrgRetCab.DataGeracao;
                        cmd.Parameters.Add("SeqArquivo", SqlDbType.TinyInt).Value = acsCrgRetCab.SeqArquivo;
                        cmd.Parameters.Add("NomeArquivo", SqlDbType.VarChar, 50).Value = acsCrgRetCab.NomeArquivo;
                        cmd.Parameters.Add("CodConvenio", SqlDbType.VarChar, 10).Value = acsCrgRetCab.CodConvenio;
                        cmd.Parameters.Add("CodEmpresa", SqlDbType.VarChar, 14).Value = acsCrgRetCab.CodEmpresa;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsCrgRetCab.NumLinha;

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
        public static ACSOPRGCR_RCabecalhoEN ConsultaPorIdArquivo(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                ACSOPRGCR_RCabecalhoEN acsPrgCrgCabEN = null;
                try
                {
                    string query = " SELECT IdArquivo, DataGeracao, SeqArquivo, NomeArquivo, CodConvenio, CodEmpresa, NumLinha " +
                                   " FROM crpACSOPRGCR_RCabecalho " +
                                   " WHERE IdArquivo = @idArquivo ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;

                        cnx.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            while(dr.Read())
                            {
                                acsPrgCrgCabEN = new ACSOPRGCR_RCabecalhoEN();
                                Mapeia(dr, acsPrgCrgCabEN);
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
                return acsPrgCrgCabEN;
            }
        }
    }
}

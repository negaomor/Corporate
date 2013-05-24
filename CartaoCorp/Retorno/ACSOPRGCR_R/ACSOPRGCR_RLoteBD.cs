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
    public static class ACSOPRGCR_RLoteBD
    {
        private static void Mapeia(SqlDataReader dr, ACSOPRGCR_RLoteEN acsPrgCrgLoteEN)
        {
            if (dr.HasRows)
            {
                acsPrgCrgLoteEN.IdArquivo = (int)dr["IdArquivo"];
                acsPrgCrgLoteEN.CodPrgCrg = dr["CodPrgCrg"].ToString();
                acsPrgCrgLoteEN.StatusProc = (EnumRetornoBase.StatusProcessamento)dr["StatusProc"];
                acsPrgCrgLoteEN.NumCart = (int)dr["NumCart"];
                acsPrgCrgLoteEN.NumRejeit = (int)dr["NumRejeit"];
                acsPrgCrgLoteEN.NumLinha = (int)dr["NumLinha"];
            }
        }

        public static void Insere(this ACSOPRGCR_RLoteEN acsCrgRetLote)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpACSOPRGCR_RLote " +
                                   "         (IdArquivo, TpRegistro, CodPrgCrg, StatusProc, NumCart, NumRejeit, NumLinha) " +
                                   " SELECT  @IdArquivo, @TpRegistro, @CodPrgCrg, @StatusProc, @NumCart, @NumRejeit, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsCrgRetLote.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCR_RLoteEN.TpRegistro;
                        cmd.Parameters.Add("CodPrgCrg", SqlDbType.VarChar, 10).Value = acsCrgRetLote.CodPrgCrg;
                        cmd.Parameters.Add("StatusProc", SqlDbType.Int).Value = (int)acsCrgRetLote.StatusProc;
                        cmd.Parameters.Add("NumCart", SqlDbType.Int).Value = acsCrgRetLote.NumCart;
                        cmd.Parameters.Add("NumRejeit", SqlDbType.Int).Value = acsCrgRetLote.NumRejeit;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsCrgRetLote.NumLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTSC_R.LoteBD", sqlExc });
                    throw;
                }
                catch (Exception ex)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTSC_R.Det", ex });
                    throw;
                }
            }
        }


        /// <summary>
        /// Dado um idArquivo consulto os dados para geração do arquivo
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static ACSOPRGCR_RLoteEN ConsultaPorIdArquivo(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                ACSOPRGCR_RLoteEN acsPrgCrgLoteEN = null;
                try
                {
                    string query = " SELECT IdArquivo, CodPrgCrg, StatusProc , NumCart, NumRejeit, NumLinha " +
                                   " FROM crpACSOPRGCR_RLote " +
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
                                acsPrgCrgLoteEN = new ACSOPRGCR_RLoteEN();
                                Mapeia(dr, acsPrgCrgLoteEN);
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
                return acsPrgCrgLoteEN;
            }
        }
    }
}

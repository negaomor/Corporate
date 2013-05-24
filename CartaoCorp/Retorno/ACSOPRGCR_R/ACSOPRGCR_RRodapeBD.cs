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
    public static class ACSOPRGCR_RRodapeBD
    {

        private static void Mapeia(SqlDataReader dr, ACSOPRGCR_RRodapeEN acsPrgCrgRdpEN)
        {
            if(dr.HasRows)
            {
                acsPrgCrgRdpEN.IdArquivo = (int)dr["IdArquivo"];
                acsPrgCrgRdpEN.NumCrg = (int)dr["NumCrg"];
                acsPrgCrgRdpEN.NumCart = (int)dr["NumCart"];
                acsPrgCrgRdpEN.ValorCrg = (decimal)dr["ValorCrg"];
                acsPrgCrgRdpEN.NumCrgRej = (int)dr["NumCrgRej"];
                acsPrgCrgRdpEN.ValCgrRej = (decimal)dr["ValCgrRej"];
                acsPrgCrgRdpEN.NumLinha = (int)dr["NumLinha"];
            }
        }

        public static void Insere(this ACSOPRGCR_RRodapeEN acsPrgCrgRdpEN)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpACSOPRGCR_RRodape " +
	                               "        (IdArquivo, TpRegistro, NumCrg, NumCart, ValorCrg, NumCrgRej, ValCgrRej, NumLinha) " +
                                   " SELECT @IdArquivo, @TpRegistro, @NumCrg, @NumCart, @ValorCrg, @NumCrgRej, @ValCgrRej, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsPrgCrgRdpEN.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCR_RRodapeEN.TpRegistro;
                        cmd.Parameters.Add("NumCrg", SqlDbType.Int).Value = acsPrgCrgRdpEN.NumCrg;
                        cmd.Parameters.Add("NumCart", SqlDbType.Int).Value = acsPrgCrgRdpEN.NumCart;
                        cmd.Parameters.Add("ValorCrg", SqlDbType.Money).Value = acsPrgCrgRdpEN.ValorCrg;
                        cmd.Parameters.Add("NumCrgRej", SqlDbType.Int).Value = acsPrgCrgRdpEN.NumCrgRej;
                        cmd.Parameters.Add("ValCgrRej", SqlDbType.Money).Value = acsPrgCrgRdpEN.ValCgrRej;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsPrgCrgRdpEN.NumLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTSC_R.RdpBD", sqlExc });
                    throw;
                }
                catch (Exception ex)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTSC_R.Rdp", ex });
                    throw;
                }
            }
        }

        /// <summary>
        /// Dado um idArquivo consulto os dados para geração do arquivo
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static ACSOPRGCR_RRodapeEN ConsultaPorIdArquivo(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                ACSOPRGCR_RRodapeEN acsPrgCrgRdpEN = null;
                try
                {
                    string query = " SELECT IdArquivo, NumCrg, NumCart, ValorCrg, NumCrgRej, ValCgrRej, NumLinha " +
                                   " FROM crpACSOPRGCR_RRodape " +
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
                                acsPrgCrgRdpEN = new ACSOPRGCR_RRodapeEN();
                                Mapeia(dr, acsPrgCrgRdpEN);
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
                return acsPrgCrgRdpEN;
            }
        }
    }
}

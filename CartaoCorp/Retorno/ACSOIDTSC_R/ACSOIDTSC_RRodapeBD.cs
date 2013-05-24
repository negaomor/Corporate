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
    public static class ACSOIDTSC_RRodapeBD
    {
        private static void Mapeia(SqlDataReader dr, ACSOIDTSC_RRodapeEN acsIdtRdp)
         {
                acsIdtRdp.IdArquivo = (int)dr["IdArquivo"];
                acsIdtRdp.NumIdent = (int)dr["NumIdent"];
                acsIdtRdp.NumLinha = (int)dr["NumLinha"];
        }


        public static void Insere(this ACSOIDTSC_RRodapeEN acsIdtRetDet)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpACSOIDTSC_RRodape (IdArquivo, TpRegistro, NumIdent, NumLinha) " +
                                   " SELECT @IdArquivo, @TpRegistro, @NumIdent, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsIdtRetDet.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOIDTSC_RRodapeEN.TpRegistro;
                        cmd.Parameters.Add("NumIdent", SqlDbType.Int).Value = acsIdtRetDet.NumIdent;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsIdtRetDet.NumLinha;

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
        public static ACSOIDTSC_RRodapeEN ConsultaPorIdArquivo(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                ACSOIDTSC_RRodapeEN acsIdtRdp = null;
                try
                {
                    string query = " SELECT IdArquivo, NumIdent, NumLinha " +
                                   " FROM crpACSOIDTSC_RRodape " +
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
                                acsIdtRdp = new ACSOIDTSC_RRodapeEN();
                                Mapeia(dr, acsIdtRdp);
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
                return acsIdtRdp;
            }
        }
    }
}

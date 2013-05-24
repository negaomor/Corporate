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

namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public static class ACSOPRGCRDetalheDB
    {
        /// <summary>
        /// Mapeia dados do limite de cartao
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="prmCrg"></param>
        private static void Mapeia(SqlDataReader dr, ACSOPRGCRDetalheEN prmcrgLimite)
        {
            if (dr.HasRows)
            {
                prmcrgLimite.ValMinCredito = (decimal)dr["ValMinCredito"];
                prmcrgLimite.ValMaxCredito = (decimal)dr["ValMaxCredito"];
                prmcrgLimite.ValLimiteCreditoMes = (decimal)dr["ValLimiteCreditoMes"];
            }
        }

        public static void Insere(this ACSOPRGCRDetalheEN acsCrgDet)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = "[crpInsereCargaDetalhe]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsCrgDet.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCRDetalheEN.TpRegistro;
                        cmd.Parameters.Add("CodPrgCrg", SqlDbType.VarChar, 10).Value = acsCrgDet.CodPrgCrg;
                        cmd.Parameters.Add("TpPanProxy", SqlDbType.TinyInt).Value = (byte)acsCrgDet.TpPanProxy;
                        cmd.Parameters.Add("PanProxy", SqlDbType.VarChar, 32).Value = acsCrgDet.PanProxy;
                        cmd.Parameters.Add("Valor", SqlDbType.Money).Value = acsCrgDet.Valor;
                        cmd.Parameters.Add("IdRegistro", SqlDbType.VarChar, 10).Value = BDGeral.BDObtemValor(acsCrgDet.IdRegistro);
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsCrgDet.NumLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.DetBD", sqlExc });
                    throw;
                }
                catch (Exception exp)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.Det", exp });
                    throw;
                }
            }
        }


        public static ACSOPRGCRDetalheEN ConsultaLimiteCliente(string idEntidade)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                ACSOPRGCRDetalheEN prmCrgLimite = null;
                try
                {
                    string query = "[crpConsultaLimiteCliente]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("idEntidade", SqlDbType.Int).Value = idEntidade;

                        cnx.Open();

                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow);

                        prmCrgLimite = new ACSOPRGCRDetalheEN();
                        while (dr.Read())
                            Mapeia(dr, prmCrgLimite);
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.DetBD", sqlExc });
                    throw;
                }
                catch (Exception exp)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.Det", exp });
                    throw;
                }
                return prmCrgLimite;
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        public static bool ConsultaCartoes(ACSOPRGCRDetalheEN prgCrDet)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                bool bRetorno = false;
                try
                {
                    string query = "[crpConsultaCartaoCarga]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Identificacao", SqlDbType.VarChar, 32).Value = prgCrDet.Identificacao.TrimEnd(null);
                        cmd.Parameters.Add("@idEntidade", SqlDbType.VarChar, 10).Value = prgCrDet.IdEntidade;
                        cmd.Parameters.Add("@codConvenio", SqlDbType.VarChar, 32).Value = prgCrDet.CodConvenio;

                        cnx.Open();

                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);

                        while (dr.Read())
                        {
                            bRetorno = true;
                            break;
                        }
                    }
                }
                catch (SqlException sql)
                {
                    throw sql;
                }
                catch (Exception e)
                {

                    throw e;
                }

                return bRetorno;
            }
        }
    }
}

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
    public static class ACSOPRGCRRodapeBD
    {
        public static void Insere(this ACSOPRGCRRodapeEN acsCrgRdp)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = "[crpInsereCargaDetalhe]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsCrgRdp.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCRRodapeEN.TpRegistro;
                        cmd.Parameters.Add("NumCrg", SqlDbType.Int).Value = acsCrgRdp.NumCrg;
                        cmd.Parameters.Add("NumCart", SqlDbType.Int).Value = acsCrgRdp.NumCart;
                        cmd.Parameters.Add("ValorCrg", SqlDbType.Money).Value = acsCrgRdp.ValorCrg;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsCrgRdp.NumLinha;
                        cmd.Parameters.Add("Linha", SqlDbType.Int).Value = acsCrgRdp.Linha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.RdpBD", sqlExc });
                    throw;
                }
                catch (Exception exp)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.Rdp", exp });
                    throw;
                }
            }
        }
    }
}

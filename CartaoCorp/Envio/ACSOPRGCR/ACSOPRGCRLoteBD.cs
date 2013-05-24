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
    public static class ACSOPRGCRLoteBD
    {
        public static void Insere(this ACSOPRGCRLoteEN acsCrgLot)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = "[crpInsereCargaDetalhe]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsCrgLot.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCRLoteEN.TpRegistro;
                        cmd.Parameters.Add("CodPrgCrg", SqlDbType.VarChar, 10).Value = acsCrgLot.CodPrgCrg;
                        cmd.Parameters.Add("NomePrg", SqlDbType.VarChar, 20).Value = BDGeral.BDObtemValor(acsCrgLot.NomePrg);
                        cmd.Parameters.Add("StatCart", SqlDbType.TinyInt).Value = (byte)acsCrgLot.StatCart;
                        cmd.Parameters.Add("DataAgend", SqlDbType.Date).Value = BDGeral.BDObtemValor<DateTime>(acsCrgLot.DataAgend);
                        cmd.Parameters.Add("CodConvenio", SqlDbType.VarChar, 10).Value = acsCrgLot.CodConvenio;
                        cmd.Parameters.Add("NumCart", SqlDbType.VarChar, 6).Value = acsCrgLot.NumCart;
                        cmd.Parameters.Add("ValorCrg", SqlDbType.Money).Value = acsCrgLot.ValorCrg;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsCrgLot.NumLinha;
                        cmd.Parameters.Add("Linha", SqlDbType.Int).Value = acsCrgLot.Linha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.LotBD", sqlExc });
                    throw;
                }
                catch (Exception exp)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.Lot", exp });
                    throw;
                }
            }
        }
    }
}

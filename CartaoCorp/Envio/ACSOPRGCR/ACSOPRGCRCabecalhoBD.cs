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
    public static class ACSOPRGCRCabecalhoBD
    {
        public static void Insere(this ACSOPRGCRCabecalhoEN acsCrgCab)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = "[crpInsereCargaDetalhe]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsCrgCab.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCRCabecalhoEN.TpRegistro;
                        cmd.Parameters.Add("NomeLayout", SqlDbType.VarChar, 20).Value = acsCrgCab.NomeLayout;
                        cmd.Parameters.Add("Versao", SqlDbType.VarChar, 8).Value = acsCrgCab.Versao;
                        cmd.Parameters.Add("DataGeracao", SqlDbType.DateTime).Value = acsCrgCab.DataGeracao;
                        cmd.Parameters.Add("SeqArquivo", SqlDbType.TinyInt).Value = acsCrgCab.SeqArquivo;
                        cmd.Parameters.Add("NomeArquivo", SqlDbType.VarChar, 50).Value = acsCrgCab.NomeArquivo;
                        cmd.Parameters.Add("CodConvenio", SqlDbType.VarChar, 10).Value = acsCrgCab.CodConvenio;
                        cmd.Parameters.Add("CodEmpresa", SqlDbType.VarChar, 14).Value = acsCrgCab.CodEmpresa;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsCrgCab.NumLinha;
                        cmd.Parameters.Add("Linha", SqlDbType.Int).Value = acsCrgCab.Linha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                 catch (SqlException sqlExc)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.CabBD", sqlExc });
                    throw;
                }
                catch (Exception exp)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.CabBD", exp });
                    throw;
                }
            }
        }
    }
}

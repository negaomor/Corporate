using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDGeral = upSight.Consulta.Base.BD.Geral;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public static class ACSPRGCRBD
    {
        /// <summary>
        /// Consulta a tabela temporaria gerada pela importação da planilha em Excel
        /// Não esquecer de se for outro nome da sheet, trocar o nome da tabela temporária
        /// </summary>
        /// <returns></returns>
        public static void InsereNovoServico(int idProcesso)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query = "[crtProgramacaoCargaFinaliza]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("idProcesso", SqlDbType.Int).Value = idProcesso;
                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sql)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.BD", sql });

                    throw sql;
                }
                catch (Exception e)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.Ex", e });

                    throw e;
                }

                return;
            }
        }
    }
}

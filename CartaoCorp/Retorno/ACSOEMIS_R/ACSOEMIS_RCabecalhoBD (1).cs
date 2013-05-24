using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Emissao.ACSOEMIS_R
{
    public static class ACSOEMIS_RCabecalhoBD
    {
        public static IEnumerable<ACSOEMIS_RCabecalhoEN> ConsultaCabecalho(this ACSOEMIS_RCabecalhoEN acsemisRCab)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query =
                                    "crtCabecalhoCartoesEmitidos";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idProcesso", SqlDbType.Int).Value = acsemisRCab.IdArquivo;

                        cnx.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while (dr.Read())
                            {
                                acsemisRCab.CodConvenio = dr["codConvenio"].ToString();
                                yield return acsemisRCab;
                            }
                        }
                    }
                }
                finally
                {
                    if (cnx.State == ConnectionState.Open)
                        cnx.Close();
                }
            }
        }
    }
}

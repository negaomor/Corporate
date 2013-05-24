using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Emissao.ACSOEMIS_R
{
    public static class ACSOEMIS_RDetalheBD
    {
        /// <summary>
        /// dado um idCabLote, é consultado os detalhes associados a ele
        /// </summary>
        /// <param name="idCabLote"></param>
        /// <returns></returns>
        public static IEnumerable<ACSOEMIS_RDetalheEN> ConsultaRetornoDetalhe(this ACSOEMIS_RDetalheEN acsemisRDet)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query =
                                    "[impIntegracaoDetalheRetornoCartoesEmitidos]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idProcesso", SqlDbType.Int).Value = acsemisRDet.IdArquivo;

                        cnx.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while (dr.Read())
                            {
                                acsemisRDet.NumCart = (int)dr["NumCartao"];
                                acsemisRDet.NumGerados = (int)dr["NumCartao"];
                                acsemisRDet.CodConvenio = dr["codConvenio"].ToString();
                                acsemisRDet.CodEmissao = dr["codEmissao"].ToString();                                
                                yield return acsemisRDet;
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

        public static IEnumerable<ACSOEMIS_RDetalheEN> ConsultaDetalhe(this ACSOEMIS_RDetalheEN acsemisRDet)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query =
                                    "[impIntegracaoDetalheCartoesEmitidos]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@idProcesso", SqlDbType.Int).Value = acsemisRDet.IdArquivo;

                        cnx.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while (dr.Read())
                            {
                                acsemisRDet.NumCartao = dr["NumCartao"].ToString();
                                acsemisRDet.Proxy = dr["Proxy"].ToString();
                                acsemisRDet.CodConvenio = dr["codConvenio"].ToString();
                                acsemisRDet.CodEmissao = dr["codEmissao"].ToString();
                                yield return acsemisRDet;
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

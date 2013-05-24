using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upSight.Consulta.Base;
using BDGeral = upSight.Consulta.Base.BD.Geral;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTSC_R
{
    public static class ACSOIDTSC_RDetalheBD
    {
        private static ACSOIDTSC_RDetalheEN Mapeia(SqlDataReader dr)
        {
            var acsIdtDetEN = new ACSOIDTSC_RDetalheEN()
            {
                IdArquivo = (int)dr["IdArquivo"],
                TpIdentificacao = (EnumRetornoBase.TipoIdentificacao)dr["TpPanProxy"],
                Identificacao = dr["PanProxy"].ToString(),
                Cpf = dr["CPF"].ToString(),
                DataProc = Convert.ToDateTime(dr["DataProc"].ToString()),
                StatusProc = (EnumRetornoBase.StatusProcessamento)dr["StatusProc"],
                StatusCart = (EnumRetornoBase.StatusCartao)dr["StatusCart"],
                Retorno = dr["Descricao"].ToString(),
                IdRegistro = dr["IdRegistro"].ToString(),
                NumLinha = (int)dr["NumLinha"]
            };

            return acsIdtDetEN;
        }

        /// <summary>
        /// Insere os detalhes da identificação
        /// </summary>
        /// <param name="acsIdtretDet"></param>
        public static void Insere(this ACSOIDTSC_RDetalheEN acsIdtretDet)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpACSOIDTSC_RDetalhe " +
                                   "        (idArquivo, TpRegistro, TpPanProxy, PanProxy, Cpf, DataProc, StatusProc, StatusCart, Descricao, IdRegistro, NumLinha) " +
                                   " SELECT @idArquivo, @TpRegistro, @TpPanProxy, @PanProxy, @Cpf, @DataProc, @StatusProc, @StatusCart, @Descricao, @IdRegistro, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsIdtretDet.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOIDTSC_RDetalheEN.TpRegistro;
                        cmd.Parameters.Add("TpPanProxy", SqlDbType.TinyInt).Value = Convert.ToByte(acsIdtretDet.TpIdentificacao);
                        cmd.Parameters.Add("PanProxy", SqlDbType.VarChar, 32).Value = acsIdtretDet.Identificacao;
                        cmd.Parameters.Add("Cpf", SqlDbType.VarChar, 11).Value = acsIdtretDet.Cpf;
                        cmd.Parameters.Add("DataProc", SqlDbType.DateTime2).Value = acsIdtretDet.DataProc;
                        cmd.Parameters.Add("StatusProc", SqlDbType.Int).Value = Convert.ToInt32(acsIdtretDet.StatusProc);
                        cmd.Parameters.Add("StatusCart", SqlDbType.TinyInt).Value = BDGeral.BDObtemValor<short>(Convert.ToInt16(acsIdtretDet.StatusCart));
                        cmd.Parameters.Add("Descricao", SqlDbType.VarChar, 50).Value = BDGeral.BDObtemValor(acsIdtretDet.Retorno);
                        cmd.Parameters.Add("IdRegistro", SqlDbType.VarChar, 10).Value = BDGeral.BDObtemValor(acsIdtretDet.IdRegistro);
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsIdtretDet.NumLinha;
                     
                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTSC_R.DetBD", sqlExc });
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
        public static IEnumerable<ACSOIDTSC_RDetalheEN> ConsultaPorIdArquivo(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query = " SELECT * FROM crpACSOIDTSC_RDetalhe " +
                                   " WHERE IdArquivo = @IdArquivo ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;

                        cnx.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while (dr.Read())
                                yield return Mapeia(dr);
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

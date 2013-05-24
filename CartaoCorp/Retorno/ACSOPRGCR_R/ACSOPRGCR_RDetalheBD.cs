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
    public static class ACSOPRGCR_RDetalheBD
    {

        private static ACSOPRGCR_RDetalheEN Mapeia(SqlDataReader dr)
        {
            var acsPrgCrgDetEN = new ACSOPRGCR_RDetalheEN()
            {
                IdArquivo = (int)dr["IdArquivo"],
                TpIdentificacao = (EnumRetornoBase.TipoIdentificacao)dr["TpPanProxy"],
                Identificacao = dr["PanProxy"].ToString(),
                StatusProc = (EnumRetornoBase.StatusProcessamento)dr["StatusProc"],
                StatusCart = (EnumRetornoBase.StatusCartao)dr["StatusCart"],
                Retorno = dr["Descricao"].ToString(),
                IdRegistro = dr["IdRegistro"].ToString(),
                NumLinha = (int)dr["NumLinha"]
            };

            return acsPrgCrgDetEN;
        }

        public static void Insere(this ACSOPRGCR_RDetalheEN acsCrgRetDet)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpACSOPRGCR_RDetalhe " +
                                   "         (IdArquivo, TpRegistro, TpPanProxy, PanProxy, StatusProc, StatusCart, Descricao, IdRegistro, NumLinha) " +
                                   " SELECT  @IdArquivo, @TpRegistro, @TpPanProxy, @PanProxy, @StatusProc, @StatusCart, @Descricao, @IdRegistro, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsCrgRetDet.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ACSOPRGCR_RDetalheEN.TpRegistro;
                        cmd.Parameters.Add("TpPanProxy", SqlDbType.TinyInt).Value = (byte)acsCrgRetDet.TpIdentificacao;
                        cmd.Parameters.Add("PanProxy", SqlDbType.VarChar, 32).Value = acsCrgRetDet.Identificacao;
                        cmd.Parameters.Add("StatusProc", SqlDbType.Int).Value = (int)acsCrgRetDet.StatusProc;
                        cmd.Parameters.Add("StatusCart", SqlDbType.TinyInt).Value = BDGeral.BDObtemValor<byte>((byte)acsCrgRetDet.StatusCart);
                        cmd.Parameters.Add("Descricao", SqlDbType.VarChar, 50).Value = BDGeral.BDObtemValor(acsCrgRetDet.Retorno);
                        cmd.Parameters.Add("IdRegistro", SqlDbType.VarChar, 10).Value = BDGeral.BDObtemValor(acsCrgRetDet.IdRegistro);
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsCrgRetDet.NumLinha;

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
        public static IEnumerable<ACSOPRGCR_RDetalheEN> ConsultaPorIdArquivo(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query = " SELECT IdArquivo, TpPanProxy, PanProxy, StatusProc, StatusCart, Descricao, IdRegistro, NumLinha " +
                                   " FROM crpACSOPRGCR_RDetalhe " +
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

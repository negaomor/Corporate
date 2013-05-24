using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BDGeral = upSight.Consulta.Base.BD.Geral;

namespace upSight.CartaoCorp
{
    public static class DetalheRetornoBaseBD
    {

        #region Consultas Comuns

        /// <summary>
        /// Dado um id de arquivo gerado pelo novo serviço, consulto o nome do arquivo e o idArquivo correspondete ao arquivo CRI de identificação ou carga
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static int ConsultaPorNomeArquivo(string nomeArquivo, short tpProcesso)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                int idArquivo = 0;

                try
                {
                    string query = " SELECT idProcesso FROM impIntegracaoProcesso " +
                                   " WHERE destino LIKE '%' + @nomeArquivo " +
                                   " AND tpProcesso = @tpProcesso ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("nomeArquivo", SqlDbType.VarChar, 100).Value = nomeArquivo;
                        cmd.Parameters.Add("tpProcesso", SqlDbType.SmallInt).Value = tpProcesso;

                        cnx.Open();

                        if (cmd.ExecuteScalar() != null)
                            idArquivo = (int)cmd.ExecuteScalar();
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
                return idArquivo;
            }
        }

        #endregion

        #region Atualiza Detalhe Retorno Identificaçãon ou Carga

        /// <summary>
        /// Atualiza o registro através do arquivo de retorno
        /// </summary>
        /// <param name="idfSimpRet"></param>
        public static void AtualizaDetalheIdentificacao(this Identificacao.ACSOIDTSC_R.IdentificacaoSimplifRetornoEN idftDetRet)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query = " UPDATE A " +
                                    " SET " +
                                    " A.Retorno = @retorno, " +
                                    " A.DtRetorno = SYSDATETIME(), " +
                                    " A.DtAlteracao = SYSDATETIME() " +
                                    " FROM crpCRIIdentificacaoDetalhe AS A ";

                    string condicao = String.Empty;

                    if (!String.IsNullOrEmpty(idftDetRet.Chave))
                        condicao = " WHERE A.Chave = @chave AND A.IdArquivo = @idArquivo ";
                    else
                        condicao = " WHERE A.IdArquivo = @idArquivo AND A.Retorno IS NULL ";


                    using (SqlCommand cmd = new SqlCommand(String.Concat(query, condicao), cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("idArquivo", SqlDbType.Int).Value = idftDetRet.IdArquivo;
                        cmd.Parameters.Add("retorno", SqlDbType.VarChar, 50).Value = BDGeral.BDObtemValor(idftDetRet.Retorno);
                        cmd.Parameters.Add("chave", SqlDbType.VarChar, 15).Value = BDGeral.BDObtemValor(idftDetRet.Chave);

                        cnx.Open();

                        cmd.ExecuteNonQuery();
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
                return;
            }
        }

        /// <summary>
        /// Atualiza o registro através do arquivo de retorno
        /// </summary>
        /// <param name="idfSimpRet"></param>
        public static void AtualizaDetalheCarga(this upSight.CartaoCorp.Carga.ACSOPRGCR_R.CargaRetornoDetalheEN crgRetDetEN)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query = " UPDATE A " +
                                    " SET " +
                                    " A.Retorno = @retorno, " +
                                    " A.DtRetorno = SYSDATETIME(), " +
                                    " A.DtAlteracao = SYSDATETIME() " +
                                    " FROM crpCRICargaDetalhe AS A ";

                    string condicao = String.Empty;

                    if (!String.IsNullOrEmpty(crgRetDetEN.Chave))
                        condicao = " WHERE A.Chave = @chave AND A.IdArquivo = @idArquivo ";
                    else
                        condicao = " WHERE A.IdArquivo = @idArquivo AND A.Retorno IS NULL ";


                    using (SqlCommand cmd = new SqlCommand(String.Concat(query, condicao), cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("idArquivo", SqlDbType.Int).Value = crgRetDetEN.IdArquivo;
                        cmd.Parameters.Add("retorno", SqlDbType.VarChar, 50).Value = BDGeral.BDObtemValor(crgRetDetEN.Retorno);
                        cmd.Parameters.Add("chave", SqlDbType.VarChar, 15).Value = BDGeral.BDObtemValor(crgRetDetEN.Chave);

                        cnx.Open();

                        cmd.ExecuteNonQuery();
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
                return;
            }
        }

        #endregion

        #region Consultas Identificação ou Carga


        /// <summary>
        /// Dado um idArquivo, verifico se o arquivo de retorno CRI de carga já foi processado tendo como base dtRetorno
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static bool VerificaArquivoRetornoCRICargaJaProcessado(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                bool fProcessado = false;
                try
                {
                    string query = " SELECT * FROM crpCRICargaDetalhe " +
                                   " WHERE IdArquivo = @IdArquivo AND DtRetorno IS NOT NULL ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;

                        cnx.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            dr.Read();
                            if (dr.HasRows)
                                fProcessado = true;
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
                return fProcessado;
            }
        }

        /// <summary>
        /// Dado um idArquivo, verifico se o arquivo de retorno CRI de identificação já foi processado tendo como base dtRetorno
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static bool VerificaArquivoRetornoCRIIdentificacaoJaProcessado(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                bool fProcessado = false;
                try
                {
                    string query = " SELECT * FROM crpCRIIdentificacaoDetalhe " +
                                   " WHERE IdArquivo = @IdArquivo AND DtRetorno IS NOT NULL ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;

                        cnx.Open();

                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            dr.Read();
                            if (dr.HasRows)
                                fProcessado = true;
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
                return fProcessado;
            }
        }


        /// <summary>
        /// Dado um id, consulto a tabela de detalhe de retorno. Se houver retorno processo o arquivo de retorno de identificação
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static IEnumerable<Identificacao.ACSOIDTSC_R.IdentificacaoSimplifRetornoEN> ConsultaDetalheIdentificacao(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query = " SELECT IdCRIIdentDet, IdArquivo, TpIdentificacao, Identificacao, Cpf, StatusCart, Chave, Retorno, DtRetorno " +
                                   " FROM crpCRIIdentificacaoDetalhe " +
                                   " WHERE IdArquivo = @idArquivo AND DtRetorno IS NOT NULL ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cnx.Open();

                        using(SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while(dr.Read())
                                yield return MapeiaDetalheIdentificacao(dr);
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

        /// <summary>
        /// Dado um id, consulto a tabela de detalhe de retorno. Se houver retorno processo o arquivo de retorno de identificação
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public static List<Carga.ACSOPRGCR_R.CargaRetornoDetalheEN> ConsultaDetalheCarga(int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                List<Carga.ACSOPRGCR_R.CargaRetornoDetalheEN> lstCrgRetDetEN = null;
                try
                {
                    string query = " SELECT IdCRICrgtDet, IdArquivo, TpIdentificacao, Identificacao, StatusCart, Valor, Chave, Retorno, DtRetorno " +
                                   " FROM crpCRICargaDetalhe " +
                                   " WHERE IdArquivo = @idArquivo AND DtRetorno IS NOT NULL ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cnx.Open();

                        lstCrgRetDetEN = new List<Carga.ACSOPRGCR_R.CargaRetornoDetalheEN>();
                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {
                            while (dr.Read())
                                lstCrgRetDetEN.Add(MapeiaDetalheCarga(dr));
                        }
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.Integ.Servicos.CrtCorp, CrgRBD", sqlExc });
                    throw;
                }
                catch (Exception ex)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.Integ.Servicos.CrtCorp, CrgR", ex });
                    throw;
                }
                finally
                {
                    if (cnx.State == ConnectionState.Open)
                        cnx.Close();
                }
                return lstCrgRetDetEN;
            }
        }

        #endregion

        #region Mapeamento dos dados

        /// <summary>
        /// Mapeio os dados do detalhe do arquivo cri de identificação
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static Identificacao.ACSOIDTSC_R.IdentificacaoSimplifRetornoEN MapeiaDetalheIdentificacao(SqlDataReader dr)
        {
            var idfSimpRetEN = new Identificacao.ACSOIDTSC_R.IdentificacaoSimplifRetornoEN()
            {
                IdArquivo = (int)dr["IdArquivo"],
                TpIdentificacao = (EnumRetornoBase.TipoIdentificacao)((byte)(dr["TpIdentificacao"])),
                Identificacao = dr["Identificacao"].ToString(),
                Cpf = dr["Cpf"].ToString(),
                StatusCart = (upSight.CartaoCorp.EnumRetornoBase.StatusCartao)((byte)(dr["StatusCart"])),
                Chave = dr["Chave"].ToString(),
                Retorno = BDGeral.ObtemValorBD(dr["Retorno"]),
                DtRetorno = Convert.ToDateTime(dr["DtRetorno"])
            };

            return idfSimpRetEN;
        }

        /// <summary>
        /// Mapeio os dados do detalhe do arquivo cri de carga
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static Carga.ACSOPRGCR_R.CargaRetornoDetalheEN MapeiaDetalheCarga(SqlDataReader dr)
        {
            var crgRetDetEN = new Carga.ACSOPRGCR_R.CargaRetornoDetalheEN()
            {
                IdArquivo = (int)dr["IdArquivo"],
                TpIdentificacao = (EnumRetornoBase.TipoIdentificacao)((byte)(dr["TpIdentificacao"])),
                Identificacao = dr["Identificacao"].ToString(),
                StatusCart = (EnumRetornoBase.StatusCartao)dr["StatusCart"],

                Valor = (decimal)dr["Valor"],
                Chave = dr["Chave"].ToString(),
                Retorno = BDGeral.ObtemValorBD(dr["Retorno"]),
                DtRetorno = Convert.ToDateTime(dr["DtRetorno"])
            };

            return crgRetDetEN;
        }

        #endregion

    }
}

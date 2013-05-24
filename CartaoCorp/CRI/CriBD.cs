using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upSight.BD;
using BDGeral = upSight.Consulta.Base.BD.Geral;


namespace upSight.CartaoCorp.CRI
{
    public static class CriBD
    {
        /// <summary>
        /// Mapeia os dados da consulta(idProduto e quantidade)
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static void Mapeia(SqlDataReader dr, ParamCRICarga prmCrg)
        {
            if (dr.HasRows)
            {
                prmCrg.IdProduto = (int)dr["idProduto"];
                prmCrg.Quantitade = (int)dr["quantidade"];
            }
        }

        private static CriCartao MapeiaCard(SqlDataReader dr)
        {
            CriCartao prmCard =  new CriCartao();
            if (dr.HasRows)
            {
                prmCard.IdProduto = (int)dr["idProduto"];
                prmCard.PanProxy =  dr["PanProxy"].ToString();
                prmCard.Valor = (decimal)dr["Valor"];
            }
            return prmCard;
        }

        /// <summary>
        /// Dados um idReferência(idEmissão), a quantidade de cartõers a serem emitidos será retornada
        /// </summary>
        /// <param name="acsIdstCab"></param>
        public static ParamCRICarga ObtemQuantidadeCartoesEmissao(int idReferencia)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                ParamCRICarga prmCrg = null;
                try
                {
                    string query = "[crtObtemQuantidadeCartoesEmissao]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("IdProcesso", SqlDbType.Int).Value = idReferencia;
                        cnx.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            prmCrg = new ParamCRICarga();
                            while (dr.Read())
                                Mapeia(dr, prmCrg);
                        }
                    }
                }
                catch (Exception sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CRP.CRI.CRIBD", sqlExc });
                    throw;
                }
                finally
                {
                    if (cnx.State == ConnectionState.Open)
                        cnx.Close();
                }
                return prmCrg;
            }
        }


        /// <summary>
        /// Dados um idReferência(idEmissão), a quantidade de cartõers a serem emitidos será retornada
        /// </summary>
        /// <param name="acsIdstCab"></param>
        public static List<CriCartao> ObtemCartoesCarga(int idProcesso)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {      
                List<CriCartao> lstCard = null;
                try
                {
                    string query = "[crtObtemQuantidadeCartoesCarga]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        lstCard = new List<CriCartao>();
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.Add("@IdProcesso", SqlDbType.Int).Value = idProcesso;
                        cnx.Open();
                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult))
                        {                            
                            while (dr.Read())
                            {                                
                                lstCard.Add(MapeiaCard(dr));
                            }
                            
                        }
                    }
                }
                catch (Exception sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CRP.CRI.CRIBD", sqlExc });
                    throw;
                }
                finally
                {
                    if (cnx.State == ConnectionState.Open)
                        cnx.Close();
                }
                return lstCard;
            }
        }
        

        /// <summary>
        /// Obtem parâmetros para compor o CRI(CarProfile, CustomerProfile, BranchCode, ProgramId e StatCode)
        /// </summary>
        /// <param name="card"></param>
        public static CARD ObtemParametroCRI(TpStatCode tpParametro, int idProduto, int idArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                CARD criEn = null;
                try
                {
                    string query = "[crtObtemParametroCri]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var cmdParameter = cmd.Parameters;
                        cmdParameter.Add("idProduto", SqlDbType.Int).Value = idProduto;
                        cmdParameter.Add("idParametro", SqlDbType.Int).Value = Convert.ToInt32(tpParametro);
                        cmdParameter.Add("idProcesso", SqlDbType.Int).Value = idArquivo;
                        cnx.Open();

                        criEn = new CARD();
                        using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleRow))
                        {
                            while (dr.Read())
                                MapeiaDetalheCard(dr, criEn, tpParametro);
                        }
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (Base.TS.TraceError)
                        Trace.TraceWarning("{0}: {1}", new object[] { "CRTPG.T.TSQL", sqlExc });
                }
                catch (Exception e)
                {
                    if (Base.TS.TraceError)
                        Trace.TraceWarning("{0}: {1}", new object[] { "CRTPG.T.T", e });
                }
                finally
                {
                    if (cnx.State == ConnectionState.Open)
                        cnx.Close();
                }
                return criEn;
            }
        }

        /// <summary>
        /// Atualiza o registro gerado com o nome e path do arquivo origem
        /// </summary>
        /// <param name="card"></param>
        public static void CriFinaliza(int idArquivo, string origem, string nomeArquivo)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                try
                {
                    string query = "[criFinaliza]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        var cmdParameter = cmd.Parameters;
                        cmdParameter.Add("idArquivo", SqlDbType.Int).Value = idArquivo;
                        cmdParameter.Add("origem", SqlDbType.VarChar, 200).Value = origem;
                        cmdParameter.Add("nomeArquivo", SqlDbType.VarChar, 100).Value = nomeArquivo;
                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (Base.TS.TraceError)
                        Trace.TraceWarning("{0}: {1}", new object[] { "CRP.CRI.CRISQL", sqlExc });
                }
                catch (Exception e)
                {
                    if (Base.TS.TraceError)
                        Trace.TraceWarning("{0}: {1}", new object[] { "CRP.CRI.CRI", e });
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
        /// Mapeio os dados do detalhe do arquivo cri de carga
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        private static void MapeiaDetalheCard(SqlDataReader dr, CARD criEn, TpStatCode tpStatusCode)
        {

            criEn.CUSTPROFILE = dr["CUSTPROFILE"].ToString();
            criEn.PROGRAMID = dr["PROGRAMID"].ToString();
            criEn.BRNCODE = dr["BRNCODE"].ToString();
            criEn.STATCODE = dr["StatCode"].ToString();            
            criEn.CRDPROFILE = dr["CRDPROFILE"].ToString();            

            switch (criEn.STATCODE)
            {
                case "00":
                    criEn.PAN = Base.ObtemValorBD(dr["pan"]);
                    break;
                case "09":
                    criEn.DESIGNREF = Base.ObtemValorBD(dr["DesignRef"]);
                    criEn.CRDPRODUCT = dr["CRDPRODUCT"].ToString();
                    break;
                default:
                    break;
            }

            switch (tpStatusCode)
            {
                case TpStatCode.Carga:
                    criEn.CRDPRODUCT = dr["CRDPRODUCT"].ToString();
                    break;
            }
            
        }


        

        /// <summary>
        /// Registro todos os cpfs e chaves(RECID) do arquivo de identificacao
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="cpf"></param>
        /// <param name="chave"></param>
        public static void InsereCRIIdentificacaoDet(int idArquivo, string cpf, string chave, CriBaseCN.TipoIdentificacao tpIdentificacao, string identificacao, CriBaseCN.StatusCartao stausCartao)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpCRIIdentificacaoDetalhe(IdArquivo, TpIdentificacao, Identificacao, Cpf, StatusCart, Chave, Retorno, DtRetorno) " +
                                   " SELECT @IdArquivo, @TpIdentificacao, @Identificacao, @Cpf, @StatusCart, @Chave, NULL, NULL ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cmd.Parameters.Add("TpIdentificacao", SqlDbType.TinyInt).Value = Convert.ToByte(tpIdentificacao);
                        cmd.Parameters.Add("Identificacao", SqlDbType.VarChar, 32).Value = identificacao;
                        cmd.Parameters.Add("StatusCart", SqlDbType.TinyInt).Value = Convert.ToByte(stausCartao);
                        cmd.Parameters.Add("Cpf", SqlDbType.VarChar, 11).Value = cpf;
                        cmd.Parameters.Add("Chave", SqlDbType.VarChar, 15).Value = chave;
                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    throw;
                }
                catch (Exception exp)
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Registro todos os cpfs e chaves(RECID) do arquivo de identificacao
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="cpf"></param>
        /// <param name="chave"></param>
        public static void InsereCRICargaDet(int idArquivo, string chave, upSight.CartaoCorp.EnumRetornoBase.TipoIdentificacao tpIdentificacao, string identificacao, EnumRetornoBase.StatusCartao statusCartao, decimal valor)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crpCRICargaDetalhe(IdArquivo, TpIdentificacao, Identificacao, StatusCart, Valor, Chave, Retorno, DtRetorno) " +
                                   " SELECT @IdArquivo, @TpIdentificacao, @Identificacao, @StatusCart, @Valor, @Chave, NULL, NULL ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cmd.Parameters.Add("TpIdentificacao", SqlDbType.TinyInt).Value = Convert.ToByte(tpIdentificacao);
                        cmd.Parameters.Add("Identificacao", SqlDbType.VarChar, 32).Value = identificacao;
                        cmd.Parameters.Add("StatusCart", SqlDbType.TinyInt).Value = Convert.ToByte(statusCartao);
                        cmd.Parameters.Add("Valor", SqlDbType.Money).Value = valor;
                        cmd.Parameters.Add("Chave", SqlDbType.VarChar, 15).Value = chave;
                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (SqlException sqlExc)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.G.S.CRP.CrgCrtRetSql", sqlExc });
                    throw;
                }
                catch (Exception e)
                {
                    if (BDGeral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.G.S.CRP.CrgCrtRetEx", e });
                    throw;
                }
            }
        }
    }
}

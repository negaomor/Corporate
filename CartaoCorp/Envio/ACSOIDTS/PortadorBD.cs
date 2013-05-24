using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;

using upSight.Consulta.Base;
using BDGeral = upSight.Consulta.Base.BD.Geral;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public static class PortadorBD
    {
        public static void Insere(this Portador ptr, int idArquivo, int numLinha)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = "[crtCargaDetalheIdentificacao]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = ptr.TpRegistro;
                        cmd.Parameters.Add("TpPanProxy", SqlDbType.Char, 1).Value = ptr.TpIdentif;
                        cmd.Parameters.Add("PanProxy", SqlDbType.VarChar, 32).Value = ptr.Identificacao.TrimEnd(null);
                        cmd.Parameters.Add("CPF", SqlDbType.VarChar, 11).Value = ptr.CPF.TrimEnd(null);
                        cmd.Parameters.Add("Nome", SqlDbType.VarChar, 50).Value = ptr.Nome.TrimEnd(null);
                        cmd.Parameters.Add("NomeFacial", SqlDbType.VarChar, 25).Value = BDGeral.BDObtemValor(ptr.NomeFacial.TrimEnd(null));
                        cmd.Parameters.Add("DtNascimento", SqlDbType.Date).Value = BDGeral.BDObtemValor<DateTime>(ptr.DtNascimento);
                        cmd.Parameters.Add("Sexo", SqlDbType.Char, 1).Value = BDGeral.BDObtemValor(ptr.Sexo);
                        cmd.Parameters.Add("CnpjFilial", SqlDbType.VarChar, 14).Value = BDGeral.BDObtemValor(ptr.CnpjFilial.TrimEnd(null));
                        cmd.Parameters.Add("Grupo", SqlDbType.VarChar, 20).Value = BDGeral.BDObtemValor(ptr.Grupo.TrimEnd(null));
                        cmd.Parameters.Add("Email", SqlDbType.VarChar, 30).Value = BDGeral.BDObtemValor(ptr.Email.TrimEnd(null));
                        cmd.Parameters.Add("DDDCel", SqlDbType.VarChar, 2).Value = BDGeral.BDObtemValor(ptr.DDDCel.TrimEnd(null));
                        cmd.Parameters.Add("Celular", SqlDbType.VarChar, 9).Value = BDGeral.BDObtemValor(ptr.Celular.TrimEnd(null));
                        cmd.Parameters.Add("NomeMae", SqlDbType.VarChar, 50).Value = BDGeral.BDObtemValor(ptr.NomeMae.TrimEnd(null));
                        cmd.Parameters.Add("IdRegistro", SqlDbType.VarChar, 10).Value = BDGeral.BDObtemValor(ptr.IdRegistro);
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = numLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrBD", sqlExc });
                    throw;
                }
            }
        }


        public static bool ConsultaCartoes(Portador ptr)
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                bool bRetorno = false;
                try
                {
                    string query = "[crpConsultaCartaoCarga]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        cmd.Parameters.Add("@Identificacao", SqlDbType.VarChar, 32).Value = ptr.Identificacao.TrimEnd(null);
                        cmd.Parameters.Add("@idEntidade", SqlDbType.VarChar, 32).Value = ptr.IdEntidade;
                        cmd.Parameters.Add("@codConvenio", SqlDbType.VarChar, 32).Value = ptr.CodConvenio;

                        cnx.Open();

                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);
                     
                        while (dr.Read())
                        {
                            bRetorno = true;
                            break;
                        }
                    }
                }
                catch (SqlException sql)
                {
                    throw sql;
                }
                catch (Exception e)
                {

                    throw e;
                }

                return bRetorno;
            }
        }
        /// <summary>
        /// Consulta a tabela temporaria gerada pela importação da planilha em Excel
        /// Não esquecer de se for outro nome da sheet, trocar o nome da tabela temporária
        /// </summary>
        /// <returns></returns>
        public static List<string> ConsultaDadosDetalhe()
        {
            using (SqlConnection cnx = new SqlConnection(upSight.Consulta.Base.BD.Conexao.StringConexaoBDGlobal))
            {
                List<string> lst = null;
                try
                {
                    string query = "SELECT * FROM [crtACSImpSimp$]";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;
                        cnx.Open();

                        SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.SingleResult);

                        lst = new List<string>();
                        while (dr.Read())
                        {
                            lst.Add(IdentificacaoProcessamento.CompoeLinhaDetalhe(dr));
                        }
                    }
                }
                catch (SqlException sql)
                {
                    throw sql;
                }
                catch (Exception e)
                {

                    throw e;
                }

                return lst;
            }
        }

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
                    string query = "[crtIdentificacaoSimplificadaFinaliza]";

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
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.ISC.ISC", sql });

                    throw sql;
                }
                catch (Exception e)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.ISC.ISC", e });

                    throw e;
                }

                return;
            }
        }
    }
}

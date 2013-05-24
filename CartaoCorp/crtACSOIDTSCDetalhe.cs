using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;

using upSight.Consulta.Base;
using BDGeral = upSight.Consulta.Base.BD.Geral;
using System.Diagnostics;


namespace upSight.CartaoCorp
{
    public class crtACSOIDTSCDetalhe
    {

        /// <summary>
        /// Construtor utilizado para receber os dados originados de um arquivo
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        public crtACSOIDTSCDetalhe() { }

        /// <summary>
        /// Parseia os dados de uma planilha de excel referente ao detalhe do arquivo e insere na base de dados
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
       public void MapeiaXLSDetArquivoEInsereBD(DataRow dr, int idArquivo, int numLinha)
        {
            try
            {
                string tpRegistro = "1";
                string tpPanProxy = dr["TpPanProxy"].ToString();
                string panProxy = dr["PanProxy"].ToString();
                string cpf = dr["CPF"].ToString();
                string nome = dr["Nome"].ToString();
                string nomeFacial = dr["NomeFacial"].ToString();

                string dtNasc = dr["DtNascimento"].ToString();
                DateTime? dtNascimento = String.IsNullOrEmpty(dtNasc) ? (DateTime?)null : Data.ParseEstendido(dtNasc, Data.FormatoData.AAAAMMDD);
                string sexo = dr["Sexo"].ToString();
                string cnpjFilial = dr["CnpjFilial"].ToString();
                string grupo = dr["Grupo"].ToString();
                string email = dr["Email"].ToString();
                string dddCel = dr["DDDCel"].ToString();
                string celular = dr["Celular"].ToString();
                string nomeMae = dr["NomeMae"].ToString();
                string idRegistro = dr["IdRegistro"].ToString();

                this.InsereDetalhe(idArquivo, tpRegistro, tpPanProxy, panProxy, cpf, nome, nomeFacial, dtNascimento, sexo, cnpjFilial, grupo, email, dddCel, celular,
                                   nomeMae, idRegistro, numLinha);
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Det", e });
                throw;
            }
        }

        /// <summary>
        /// Parseia os dados da linha referente ao detalhe do arquivo e insere na base de dados
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        public void MapeiaLinhaDetArquivoEInsereBD(string linha, int idArquivo)
        {
            try
            {
                string tpRegistro = linha.Substring(0, 1);
                string tpPanProxy = linha.Substring(1, 1);
                string panProxy = linha.Substring(2, 32);
                string cpf = linha.Substring(34, 11);
                string nome = linha.Substring(45, 50);
                string nomeFacial = linha.Substring(95, 25);
                DateTime? dtNascimento = String.IsNullOrEmpty(linha.Substring(120, 8).Trim()) ? (DateTime?)null : Data.ParseEstendido(linha.Substring(120, 8), Data.FormatoData.AAAAMMDD);
                string sexo = linha.Substring(128, 1);
                string cnpjFilial = linha.Substring(129, 14);
                string grupo = linha.Substring(143, 20);
                string email = linha.Substring(163, 30);
                string dddCel = linha.Substring(193, 2);
                string celular = linha.Substring(195, 9);
                string nomeMae = linha.Substring(204, 50);
                string idRegistro = linha.Substring(284, 10);
                int numLinha = Convert.ToInt32(linha.Substring(294, 6));

                this.InsereDetalhe(idArquivo, tpRegistro, tpPanProxy, panProxy, cpf, nome, nomeFacial, dtNascimento, sexo, cnpjFilial, grupo, email, dddCel, celular,
                                   nomeMae, idRegistro, numLinha);
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Det", e });
                throw;
            }
        }

        /// <summary>
        /// Insere os dados em crtACSOIDTSCDetalhe
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="tpRegistro"></param>
        /// <param name="tpPanProxy"></param>
        /// <param name="panProxy"></param>
        /// <param name="cpf"></param>
        /// <param name="nome"></param>
        /// <param name="nomeFacial"></param>
        /// <param name="dtNascimento"></param>
        /// <param name="sexo"></param>
        /// <param name="cnpjFilial"></param>
        /// <param name="grupo"></param>
        /// <param name="email"></param>
        /// <param name="dddCel"></param>
        /// <param name="celular"></param>
        /// <param name="nomeMae"></param>
        /// <param name="idRegistro"></param>
        /// <param name="numLinha"></param>
        private void InsereDetalhe(int idArquivo, string tpRegistro, string tpPanProxy, string panProxy, string cpf, string nome, string nomeFacial, DateTime? dtNascimento,
                                   string sexo, string cnpjFilial, string grupo, string email, string dddCel, string celular, string nomeMae, string idRegistro,
                                   int numLinha)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT [crtACSOIDTSCDetalhe] " +
                               " (IdArquivo, TpRegistro, TpPanProxy, PanProxy, CPF, Nome, NomeFacial, DtNascimento, Sexo, CnpjFilial, " +
                               " Grupo, Email, DDDCel, Celular, NomeMae, IdRegistro, NumLinha) " +
                               " SELECT @IdArquivo, @TpRegistro, @TpPanProxy, @PanProxy, @CPF, @Nome, @NomeFacial, @DtNascimento, @Sexo, @CnpjFilial, " +
                               " @Grupo, @Email, @DDDCel, @Celular, @NomeMae, @IdRegistro, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = tpRegistro;
                        cmd.Parameters.Add("TpPanProxy", SqlDbType.Char, 1).Value = tpPanProxy;
                        cmd.Parameters.Add("PanProxy", SqlDbType.VarChar, 32).Value = panProxy;
                        cmd.Parameters.Add("CPF", SqlDbType.VarChar, 11).Value = cpf.TrimEnd(null);
                        cmd.Parameters.Add("Nome", SqlDbType.VarChar, 50).Value = nome.TrimEnd(null);
                        cmd.Parameters.Add("NomeFacial", SqlDbType.VarChar, 25).Value = BDGeral.BDObtemValor(nomeFacial.TrimEnd(null));
                        cmd.Parameters.Add("DtNascimento", SqlDbType.Date).Value = BDGeral.BDObtemValor<DateTime>(dtNascimento);
                        cmd.Parameters.Add("Sexo", SqlDbType.Char, 1).Value = BDGeral.BDObtemValor(sexo);
                        cmd.Parameters.Add("CnpjFilial", SqlDbType.VarChar, 14).Value = BDGeral.BDObtemValor(cnpjFilial.TrimEnd(null));
                        cmd.Parameters.Add("Grupo", SqlDbType.VarChar, 20).Value = BDGeral.BDObtemValor(grupo.TrimEnd(null));
                        cmd.Parameters.Add("Email", SqlDbType.VarChar, 30).Value = BDGeral.BDObtemValor(email.TrimEnd(null));
                        cmd.Parameters.Add("DDDCel", SqlDbType.VarChar, 2).Value = BDGeral.BDObtemValor(dddCel.TrimEnd(null));
                        cmd.Parameters.Add("Celular", SqlDbType.VarChar, 9).Value = BDGeral.BDObtemValor(celular.TrimEnd(null));
                        cmd.Parameters.Add("NomeMae", SqlDbType.VarChar, 50).Value = BDGeral.BDObtemValor(nomeMae.TrimEnd(null));
                        cmd.Parameters.Add("IdRegistro", SqlDbType.VarChar, 10).Value = BDGeral.BDObtemValor(idRegistro);
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = numLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Det", sqlExc });
                    throw;
                }
            }
        }
    }
}

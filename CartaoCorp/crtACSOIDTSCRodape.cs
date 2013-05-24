using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace upSight.CartaoCorp
{
    public class crtACSOIDTSCRodape
    {
        public crtACSOIDTSCRodape() { }

        /// <summary>
        /// Parseia os dados da linha referente ao rodapé do arquivo e insere na base de dados
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        public void MapeiaLinhaRdpArquivoEInsereBD(string linha, int idArquivo)
        {
            try
            {
                string tpRegistro = linha.Substring(0, 1);
                int numLotes = Convert.ToInt32(linha.Substring(1, 6));
                int numLinha = Convert.ToInt32(linha.Substring(294, 6));

                this.InsereRodape(idArquivo, tpRegistro, numLotes, numLinha);
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Rdp", e });
                throw;
            }
        }

        /// <summary>
        /// Insere os dados em crtACSOIDTSCRodape
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="tpRegistro"></param>
        /// <param name="numLotes"></param>
        /// <param name="numLinha"></param>
        private void InsereRodape(int idArquivo, string tpRegistro, int numLotes, int numLinha)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT crtACSOIDTSCRodape " +
                               " (IdArquivo, TpRegistro, NumLotes, NumLinha) " +
                               " SELECT @IdArquivo, @TpRegistro, @NumLotes, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = idArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = tpRegistro;
                        cmd.Parameters.Add("NumLotes", SqlDbType.Int).Value = numLotes;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = numLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Rdp", sqlExc });
                    throw;
                }
            }
        }
    }
}

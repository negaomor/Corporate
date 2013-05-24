using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public static class crtACSOIDTSCRodapeBD
    {
        /// <summary>
        /// Insere os dados em crtACSOIDTSCRodape
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="tpRegistro"></param>
        /// <param name="numLotes"></param>
        /// <param name="numLinha"></param>
        public static void Insere(this crtACSOIDTSCRodapeEN acsItdsRdp)
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

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsItdsRdp.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = acsItdsRdp.TpRegistro;
                        cmd.Parameters.Add("NumLotes", SqlDbType.Int).Value = acsItdsRdp.NumLotes;
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsItdsRdp.NumLinha;

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

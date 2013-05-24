using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Configuration;
using System.Data;
using System.Diagnostics;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public class crtACSOIDTSCRodapeEN
    {
        #region Propriedades

        public int IdArquivo { get; set; }
        public string TpRegistro { get; set; }
        public int NumLotes { get; set; }
        public int NumLinha { get; set; }

        #endregion

        public crtACSOIDTSCRodapeEN() { }

        /// <summary>
        /// Parseia os dados da linha referente ao rodapé do arquivo e insere na base de dados
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        public static crtACSOIDTSCRodapeEN Mapeia(string linha, int idArquivo)
        {
            try
            {
                var acsItdsRdp = new crtACSOIDTSCRodapeEN()
                {
                    IdArquivo = idArquivo,
                    TpRegistro = linha.Substring(0, 1),
                    NumLotes = Convert.ToInt32(linha.Substring(1, 6).TrimEnd(null)),
                    NumLinha = Convert.ToInt32(linha.Substring(294, 6).TrimEnd(null))
                };
                return acsItdsRdp;
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Rdp", e });
                throw;
            }
        }
    }
}

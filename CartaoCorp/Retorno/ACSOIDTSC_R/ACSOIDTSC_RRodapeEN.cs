using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTSC_R
{
    public class ACSOIDTSC_RRodapeEN
    {

        [Column(Name = "TpRegistro", DbType = "CHAR(1) NOT NULL")]
        public const string TpRegistro = "9";

        #region Propriedades

        [Column(Name = "IdRetIdentRdp", DbType = "INT NOT NULL")]
        public int IdRetIdentRdp { get; set; }

        [Column(Name = "IdArquivo", DbType = "INT NOT NULL")]
        public int IdArquivo { get; set; }

        [Column(Name = "NumIdent", DbType = "INT NOT NULL")]
        public int NumIdent { get; set; }

        [Column(Name = "NumLinha", DbType = "INT NOT NULL")]
        public int NumLinha { get; set; }

        #endregion

        #region Cosntrutores

        public ACSOIDTSC_RRodapeEN() { }

        #endregion

        #region Métodos

        /// <summary>
        /// ToString do rodapé
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat( ACSOIDTSC_RRodapeEN.TpRegistro,
                                  this.NumIdent.ToString("000000"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 137),
                                  this.NumLinha.ToString("000000")
                                );
        }


        /// <summary>
        /// Monta o rodapé
        /// </summary>
        /// <param name="numIdent"></param>
        /// <param name="numLinha"></param>
        /// <returns></returns>
        public ACSOIDTSC_RRodapeEN MontaRodape(int numIdent, int numLinha)
        {
            var rdp = new ACSOIDTSC_RRodapeEN()
            {
                NumIdent = numIdent,
                NumLinha = numLinha
            };

            return rdp;
        }

        #endregion
    }
}

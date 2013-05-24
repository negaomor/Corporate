using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Emissao.ACSOEMIS_R
{
    public class ACSOEMIS_RRodapeEN
    {
        public const string TpRegistro = "9";
        public int NumEmis { get; set; }
        public int NumCart { get; set; }
        public int NumGerados { get; set; }
        public int NumLinha { get; set; }

        public override string ToString()
        {
            return String.Concat(
                    TpRegistro,
                    this.NumEmis.ToString("000000"),
                    this.NumCart.ToString("000000"),
                    this.NumGerados.ToString("000000"),
                    upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 105),
                    this.NumLinha.ToString("000000"));
        }

        /// <summary>
        /// Compõe o rodapé do arquivo
        /// </summary>
        /// <param name="numEmis"></param>
        /// <param name="numCart"></param>
        /// <param name="numGerados"></param>
        /// <param name="numLinha"></param>
        /// <returns></returns>
        public static ACSOEMIS_RRodapeEN CompoeACSOEMIS_RRodapeEN(int numEmis, int numLinha)
        {
            var acsemisRRdp = new ACSOEMIS_RRodapeEN()
            {
                NumEmis = numEmis,
                NumCart = numEmis,
                NumGerados = numEmis,
                NumLinha = numLinha
            };

            return acsemisRRdp;
        }
    }
}

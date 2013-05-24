using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using BaseSistema = upSight.Consulta.Base.Sistema;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public class ACSOPRGCRRodapeEN
    {
        public const string TpRegistro = "9";

        #region

        public int IdRodape { get; set; }
        public int IdArquivo { get; set; }
        public int NumCrg { get; set; }
        public int NumCart { get; set; }
        public decimal ValorCrg { get; set; }
        public int NumLinha { get; set; }
        public int Linha { get; set; }

        #endregion


        #region Construtores

        public ACSOPRGCRRodapeEN() { }

        public ACSOPRGCRRodapeEN(int idArquivo, string linha)
        {
            this.IdArquivo = idArquivo;
            this.Mapeia(linha);
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Mapeia a linha do arquivo
        /// </summary>
        /// <param name="linha"></param>
        private void Mapeia(string linha)
        {
            this.NumCrg = Convert.ToInt32(linha.Substring(1, 6).TrimEnd());
            this.NumCart = Convert.ToInt32(linha.Substring(7, 6).TrimEnd());
            this.ValorCrg = Convert.ToDecimal(linha.Substring(13, 12).TrimEnd()) / 100;
            this.NumLinha = Convert.ToInt32(linha.Substring(124, 6).TrimEnd());
            this.Linha = Convert.ToInt32(linha.Substring(0, 1));
        }

        /// <summary>
        /// Gera linha Rodapé
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat(
                                TpRegistro,
                                BaseSistema.CompletaComZerosEsquerda(this.NumCrg, 6),
                                BaseSistema.CompletaComZerosEsquerda(this.NumCart, 6),
                                BaseSistema.CompletaComZerosEsquerda((Decimal.Truncate(this.ValorCrg * 100)), 12),
                                BaseSistema.CompletaEspacoDireita(String.Empty, 99),
                                BaseSistema.CompletaComZerosEsquerda(this.NumLinha, 6)
                                );
        }


        /// <summary>
        /// Gera linha Rodapé
        /// </summary>
        /// <param name="numCrg"></param>
        /// <param name="numCrt"></param>
        /// <param name="vlrCrg"></param>
        /// <param name="numLinha"></param>
        public ACSOPRGCRRodapeEN MontaACSOPRGCRRodapeEN(int numCrg, int numCrt, decimal vlrCrg, int numLinha)
        {
            var rdp = new ACSOPRGCRRodapeEN() 
                                            { 
                                                NumCrg = numCrg,
                                                NumCart = numCrt,
                                                ValorCrg = vlrCrg,
                                                NumLinha = numLinha,
                                            };

            return rdp;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upSight.Consulta.Base;
using BaseSistema = upSight.Consulta.Base.Sistema;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public class ACSOPRGCRLoteEN
    {
        #region Propriedades

        public const string TpRegistro  = "1";

        public int IdLote { get; set; }
        public int IdArquivo { get; set; }
        public string CodPrgCrg { get; set; }
        public string NomePrg { get; set; }
        public StatCart StatCart { get; set; }
        public DateTime? DataAgend { get; set; }
        public string CodConvenio { get; set; }
        public int NumCart { get; set; }
        public decimal ValorCrg { get; set; }
        public int NumLinha { get; set; }
        public int Linha { get; set; }

        #endregion

        #region Construtores

        public ACSOPRGCRLoteEN() { }

        public ACSOPRGCRLoteEN(int idArquivo, string linha)
        {            
            this.Mapeia(linha);
            this.IdArquivo = idArquivo;
        }

        #endregion

        #region Métodos

        /// <summary>
        /// Mapeia linha do arquivo
        /// </summary>
        /// <param name="linha"></param>
        private void Mapeia(string linha)
        {
            this.CodPrgCrg = linha.Substring(1, 10).TrimEnd();
            
            string nomePrg = linha.Substring(11, 20).TrimEnd();
            this.NomePrg = (!String.IsNullOrEmpty(nomePrg)) ? nomePrg.TrimEnd() : null;
            this.StatCart = (StatCart)Convert.ToByte(linha.Substring(31, 1).TrimEnd());

            string dtAgend = linha.Substring(32, 8).TrimEnd();
            this.DataAgend = (!String.IsNullOrEmpty(dtAgend)) ? Data.ParseEstendido(dtAgend.TrimEnd(), Data.FormatoData.AAAAMMDD) : (DateTime?)null;
            this.CodConvenio = linha.Substring(40, 10).TrimEnd();
            this.NumCart = Convert.ToInt32(linha.Substring(50, 6).TrimEnd());
            this.ValorCrg = Convert.ToDecimal(linha.Substring(56, 12).TrimEnd()) / 100;
            this.NumLinha = Convert.ToInt32(linha.Substring(124, 6).TrimEnd());
            this.Linha = Convert.ToInt16(linha.Substring(0, 1));
        }

        /// <summary>
        /// Gera linha do Lote
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat(
                                 TpRegistro,
                                 BaseSistema.CompletaEspacoDireita(this.CodPrgCrg, 10),
                                 BaseSistema.CompletaEspacoDireita(this.NomePrg, 20),
                                 Convert.ToByte(this.StatCart).ToString(),
                                 BaseSistema.CompletaEspacoDireita((this.DataAgend.HasValue)? this.DataAgend.Value.ToString("yyyyMMdd") : String.Empty, 8),
                                 BaseSistema.CompletaEspacoDireita(this.CodConvenio, 10),
                                 BaseSistema.CompletaComZerosEsquerda(this.NumCart, 6),
                                 BaseSistema.CompletaComZerosEsquerda(Decimal.Truncate(this.ValorCrg * 100M), 12),
                                 BaseSistema.CompletaEspacoDireita(String.Empty, 56),
                                 BaseSistema.CompletaComZerosEsquerda(this.NumLinha, 6)
                                 );

        }

        /// <summary>
        /// Gera linha lote
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="codConvenio"></param>
        /// <param name="numLinha"></param>
        public ACSOPRGCRLoteEN MontaACSOPRGCRLoteEN(int idArquivo, string codConvenio, int numLinha)
        {
            var lote = new ACSOPRGCRLoteEN() 
                                            { 
                                                CodPrgCrg = "125AA4",
                                                NomePrg = "PrePago",
                                                StatCart = ACSOPRGCR.StatCart.Imediata,
                                                DataAgend = null,
                                                CodConvenio = codConvenio,
                                                NumCart = 5,
                                                ValorCrg = 207.07M,
                                                NumLinha = numLinha
                                            };
            return lote;
        }
        
        #endregion
    }


    public enum StatCart : byte
    {
        Imediata = 1,
        Agendada = 2
    }
}

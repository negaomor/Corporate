using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upSight.Consulta.Base;
using BaseSistema = upSight.Consulta.Base.Sistema;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public class ACSOPRGCRCabecalhoEN
    {
       public const string TpRegistro = "0";

        #region Propriedades

        public int IdCabecalho { get; set; }
        public int IdArquivo { get; set; }
        public string NomeLayout { get; set; }
        public string Versao { get; set; }
        public DateTime DataGeracao { get; set; }
        public byte SeqArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string CodConvenio { get; set; }
        public string CodEmpresa { get; set; }
        public int NumLinha { get; set; }
        public int Linha { get; set; }

        #endregion

        #region Construtores

        public ACSOPRGCRCabecalhoEN() { }

        public ACSOPRGCRCabecalhoEN(int idArquivo, string linha) 
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
            this.NomeLayout= linha.Substring(1, 20).TrimEnd();
            this.Versao = linha.Substring(21, 8).TrimEnd();
            this.DataGeracao = Data.ParseEstendido(linha.Substring(29, 14).TrimEnd(), Data.FormatoData.AAAAMMDDHHMMSS);
            this.SeqArquivo = Convert.ToByte(linha.Substring(43, 2).TrimEnd());
            this.NomeArquivo = linha.Substring(45, 50).TrimEnd();
            this.CodConvenio = linha.Substring(95, 10).TrimEnd();
            this.CodEmpresa = linha.Substring(105, 14).TrimEnd();
            this.NumLinha = Convert.ToInt32(linha.Substring(124, 6).TrimEnd());
            this.Linha = Convert.ToInt16(linha.Substring(0, 1));
        }

        /// <summary>
        /// Gera linha do cabeçalho
        /// </summary>
        /// <param name="linha"></param>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat(
                                TpRegistro,
                                BaseSistema.CompletaEspacoDireita(this.NomeLayout, 20),
                                BaseSistema.CompletaEspacoDireita(this.Versao, 8),
                                BaseSistema.CompletaEspacoDireita(this.DataGeracao.ToString("yyyyMMdd"), 8),
                                BaseSistema.CompletaEspacoDireita(this.DataGeracao.ToString("HHmmss"), 6),
                                BaseSistema.CompletaComZerosEsquerda(this.SeqArquivo, 2),
                                BaseSistema.CompletaEspacoDireita(this.NomeArquivo, 50),
                                BaseSistema.CompletaEspacoDireita(this.CodConvenio, 10),
                                BaseSistema.CompletaEspacoDireita(this.CodEmpresa, 14),
                                BaseSistema.CompletaEspacoDireita(String.Empty, 5),
                                BaseSistema.CompletaComZerosEsquerda(this.NumLinha, 6)
                                );
        }


        /// <summary>
        /// Gera linha cabeçalho
        /// </summary>
        /// <param name="nomeArquivo"></param>
        /// <param name="numLinha"></param>
        public ACSOPRGCRCabecalhoEN MontaACSOPRGCRCabecalhoEN(int idArquivo, string nomeArquivo, int numLinha, string codConvenio)
        {
            var cab = new ACSOPRGCRCabecalhoEN() 
                                                { 
                                                    IdArquivo = idArquivo,
                                                    NomeLayout = "ACSOPRGCR",
                                                    Versao = "1.1",
                                                    CodEmpresa = "ACS",
                                                    CodConvenio = codConvenio,
                                                    DataGeracao = DateTime.Now,
                                                    SeqArquivo = 12,
                                                    NomeArquivo = nomeArquivo,
                                                    NumLinha = numLinha
                                                };
            return cab;
        }

        #endregion
    }
}

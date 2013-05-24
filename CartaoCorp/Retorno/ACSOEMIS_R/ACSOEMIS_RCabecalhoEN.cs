using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Emissao.ACSOEMIS_R
{
    public class ACSOEMIS_RCabecalhoEN
    {
        public const string TpRegistro = "0";
        public const string NomeLayout = "ACSOEMIS_R";

        public int IdCabecalho { get; set; }
        public int IdArquivo { get; set; }
        public string Versao { get; set; }
        public DateTime DataGeracao { get; set; }
        public int SeqArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string CodConvenio { get; set; }
        public string CodEmpresa { get; set; }
        public int NumLinha { get; set; }

        /// <summary>
        /// Retorna uma string composta pela concatenação do objeto
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat(
                TpRegistro,
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(NomeLayout, 20),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Versao, 8),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.DataGeracao.ToString("yyyyMMdd"), 8),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.DataGeracao.ToString("HHmmss"), 6),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.SeqArquivo, 2),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.NomeArquivo, 50),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodConvenio, 10),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodEmpresa, 14),
                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 5),
                this.NumLinha.ToString("000000"));
        }

        /// <summary>
        /// Compõe o objeto para testes
        /// </summary>
        /// <param name="acsEmisRCab"></param>
        public void CompoeACSOEMIS_RCabecalho(ACSOEMIS_RCabecalhoEN acsEmisRCab)
        {
           
            var cab = acsEmisRCab.ConsultaCabecalho();

            foreach (var detalhe in cab)
            {
                acsEmisRCab.Versao = "1.1";
                acsEmisRCab.DataGeracao = DateTime.Now;
                acsEmisRCab.SeqArquivo = 3;
                acsEmisRCab.NomeArquivo = "ic13021504.xml";
                acsEmisRCab.CodEmpresa = "08903632000177";
            }


            
        }
    }
}

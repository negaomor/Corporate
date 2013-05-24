using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Emissao.ACSOEMIS_R
{
    public class ACSOEMIS_RDetalheEN
    {
        public TpRetornoDetalhe TpRegistro;

        //Propriedades tipo do retorno = 1
        public int IdCabecalho { get; set; }
        public int IdArquivo { get; set; }
        public string CodEmissao { get; set; }
        public TpStatusProc StatusProc { get; set; }
        public int NumCart { get; set; }
        public int NumGerados { get; set; }
        public DateTime DtProc { get; set; }
        public string CodConvenio { get; set; }
        public string Descricao { get; set; }

        //Complementação dos detalhes para tipo de retorno = 2
        public string NumCartao { get; set; }
        public string Proxy { get; set; }
        public string numSeq { get; set; }
        public string IdRegistro { get; set; }

        public const TpStatusCart StatusCart = TpStatusCart.Dormente;

        public int NumLinha { get; set; }

        /// <summary>
        /// Retorna uma string composta pela concatenação do objeto
        /// </summary>
        /// <returns></returns>
        public string ToString(TpRetornoDetalhe tpRetorno)
        {
            string linhaRetorno = String.Empty;

            if (tpRetorno == TpRetornoDetalhe.DetalheDosCartões)
            {
                linhaRetorno = String.Concat(
                                Convert.ToInt32(TpRetornoDetalhe.DetalheDosCartões).ToString("0"),
                                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodEmissao, 10),
                                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.NumCartao, 16),
                                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Proxy, 32),
                                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(Convert.ToInt32(StatusCart).ToString("00"), 2),
                                this.numSeq,
                                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 47),
                                upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.IdRegistro, 10),
                                this.NumLinha.ToString("000000"));
            }
            else
            {
                linhaRetorno = String.Concat(
                                  Convert.ToInt32(TpRetornoDetalhe.RetornoDeEmissão).ToString("0"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodEmissao, 10),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(Convert.ToInt32(this.StatusProc).ToString("000"), 3),
                                  this.NumCart.ToString("000000"),
                                  this.NumGerados.ToString("000000"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.DtProc.ToString("yyyyMMdd"), 8),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodConvenio, 10),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Descricao, 30),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 50),
                                  this.NumLinha.ToString("000000"));
            }

            return linhaRetorno;
        }

        public void CompoeACSOEMIS_RDetalheENEmissRet(ACSOEMIS_RDetalheEN acsEmisRDet)
        {

            var det = acsEmisRDet.ConsultaRetornoDetalhe();

            foreach (var detalhe in det)
            {
                acsEmisRDet.StatusProc = TpStatusProc.Sucesso;
                //acsEmisRDet.NumGerados = acsEmisRDet.NumGerados;
                acsEmisRDet.DtProc = DateTime.Today;
                acsEmisRDet.Descricao = String.Empty;
            }
        }
    }

    /// <summary>
    /// Tipo de retorno do detalhe do arquivo
    /// </summary>
    public enum TpRetornoDetalhe : int
    {
        RetornoDeEmissão = 1,
        DetalheDosCartões = 2
    }

    /// <summary>
    /// Status de Processamento
    /// </summary>
    public enum TpStatusProc : int
    {
        Sucesso = 0,
        EmissãoNãoRealizada = 400,
        ConvênioNãoDisnível = 401,
        ErroGenérico = 909
    }

    /// <summary>
    /// Status do cartão
    /// </summary>
    public enum TpStatusCart : int
    {
        Ativo = 0,
        Inativo = 2,
        Dormente = 9,
        NãoInformado_NãoConsultado = 99
    }
}

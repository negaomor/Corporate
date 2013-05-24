using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

using BaseSistema = upSight.Consulta.Base.Sistema;
using System.Data;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public class ACSOPRGCRDetalheEN
    {
        public const string TpRegistro = "2";

        #region Propriedades

        public int IdDetalhe { get; set; }
        public int IdArquivo { get; set; }
        public string Identificacao { get; set; }
        public string CodPrgCrg { get; set; }
        public TipoPanProxy TpPanProxy { get; set; }
        public string PanProxy { get; set; }
        public decimal Valor { get; set; }
        public string IdRegistro { get; set; }
        public int NumLinha { get; set; }
        public string IdEntidade { get; set; }
        public string CodConvenio { get; set; }
        public decimal ValMinCredito { get; set; }
        public decimal ValMaxCredito { get; set; }
        public decimal ValLimiteCreditoMes { get; set; }

        #endregion

        #region Construtores

        public ACSOPRGCRDetalheEN() { }

        public ACSOPRGCRDetalheEN(int idArquivo, string linha) 
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
            this.CodPrgCrg = linha.Substring(1, 10).TrimEnd();
            this.TpPanProxy = (TipoPanProxy)Convert.ToByte(linha.Substring(11, 1).TrimEnd());
            this.PanProxy = linha.Substring(12, 32).TrimEnd();
            this.Valor = Convert.ToDecimal(linha.Substring(44, 12).TrimEnd()) / 100;

            string idRegistro = linha.Substring(114, 10).TrimEnd();
            this.IdRegistro = (!String.IsNullOrEmpty(idRegistro)) ? idRegistro.TrimEnd() : null;
            this.NumLinha = Convert.ToInt32(linha.Substring(124, 6).TrimEnd());
        }

        /// <summary>
        ///  Mapeia a partir da leitura Excel
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        /// <param name="codConvenio"></param>
        /// <param name="idEntidade"></param>
        /// <param name="dt"></param>
        /// <returns></returns>
        public static DataRow MapeiaTXT(string linha, int idArquivo, DataTable dt, int idEntidade)
        {
            if (!dt.Columns.Contains("TpIdentif"))
                dt.Columns.Add("TpIdentif");
            if (!dt.Columns.Contains("CodPrgCrg"))
                dt.Columns.Add("CodPrgCrg");
            if (!dt.Columns.Contains("Identificacao"))
                dt.Columns.Add("Identificacao");//PanProxy          
            if (!dt.Columns.Contains("Valor"))
                dt.Columns.Add("Valor");
            if (!dt.Columns.Contains("IdRegistro"))
                dt.Columns.Add("IdRegistro");
            if (!dt.Columns.Contains("IdEntidade"))
                dt.Columns.Add("IdEntidade");


            DataRow dr = dt.NewRow();

            dr["TpIdentif"] = linha.Substring(0, 1);
            dr["CodPrgCrg"] = linha.Substring(1, 1);
            dr["Identificacao"] = linha.Substring(2, 32).TrimEnd(null);//PanProxy
            dr["Valor"] = linha.Substring(34, 11).TrimEnd(null);
            dr["IdRegistro"] = linha.Substring(45, 50).TrimEnd(null);
            dr["IdEntidade"] = idEntidade;

            return dr;
        }

        /// <summary>
        /// Monta o objeto
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="numLinha"></param>
        /// <param name="dr"></param>
        /// <returns></returns>
        public static ACSOPRGCRDetalheEN MapeiaExcel(int idArquivo, int numLinha, DataRow dr)
        {
            var acsCrgDetEn = new ACSOPRGCRDetalheEN ()
            {
                IdArquivo = idArquivo,
                CodPrgCrg = dr["Codigo Prg Carga"].ToString(),
                TpPanProxy = (TipoPanProxy)Convert.ToByte(dr["TpPanProxy"].ToString()),
                PanProxy = dr["PanProxy"].ToString(),
                Valor = Convert.ToDecimal(dr["Valor"]),
                IdRegistro = BaseSistema.ObtemValor(dr["IdRegistro"].ToString()),
                NumLinha = numLinha
            };

            return acsCrgDetEn;
        }

        /// <summary>
        /// Gera linha do detalhe
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return String.Concat(
                                   TpRegistro,
                                   BaseSistema.CompletaEspacoDireita(this.CodPrgCrg, 10),
                                   Convert.ToByte(this.TpPanProxy).ToString(),
                                   BaseSistema.CompletaEspacoDireita(this.PanProxy, 32),
                                   BaseSistema.CompletaComZerosEsquerda((Decimal.Truncate(this.Valor * 100)), 12),
                                   BaseSistema.CompletaEspacoDireita(String.Empty, 58),
                                   BaseSistema.CompletaEspacoDireita(this.IdRegistro, 10),
                                   BaseSistema.CompletaComZerosEsquerda(this.NumLinha, 6)
                                );
        }

        /// <summary>
        /// Gera linhas detalhe
        /// </summary>
        /// <returns></returns>
        public static ACSOPRGCRDetalheEN[] MontaACSOPRGCRDetalheEN()
        {
            ACSOPRGCRDetalheEN[] detalhes = new ACSOPRGCRDetalheEN[] 
                                            { new ACSOPRGCRDetalheEN(){
                                                                        CodPrgCrg = "125AA4",
                                                                        TpPanProxy = TipoPanProxy.PAN,
                                                                        PanProxy = "5292050025256369",
                                                                        Valor = 25.3M,
                                                                        IdRegistro = "12541"},
                                              new ACSOPRGCRDetalheEN(){
                                                                        CodPrgCrg = "125AA4",
                                                                        TpPanProxy = TipoPanProxy.PAN,
                                                                        PanProxy = "5292050052847596",
                                                                        Valor = 65.0M,
                                                                        IdRegistro = "12541"},
                                              new ACSOPRGCRDetalheEN(){
                                                                        CodPrgCrg = "125AA4",
                                                                        TpPanProxy = TipoPanProxy.PAN,
                                                                        PanProxy = "5292050014586532",
                                                                        Valor = 62.25M,
                                                                        IdRegistro = "12541"},
                                              new ACSOPRGCRDetalheEN(){
                                                                        CodPrgCrg = "125AA4",
                                                                        TpPanProxy = TipoPanProxy.PAN,
                                                                        PanProxy = "5292050054752135",
                                                                        Valor = 33.0M,
                                                                        IdRegistro = "12541"},
                                              new ACSOPRGCRDetalheEN(){
                                                                        CodPrgCrg = "125AA4",
                                                                        TpPanProxy = TipoPanProxy.PAN,
                                                                        PanProxy = "5292050054455547",
                                                                        Valor = 21.52M,
                                                                        IdRegistro = "12541"}};

            return detalhes;
        }

        #endregion

    }

    public class MapaColunaCrgDetalhe
    {
        public int CodPrgCrg { get; set; }
        public int TpIdentif { get; set; }
        public int Identificacao { get; set; }
        public int Valor { get; set; }
        public int IdRegistro { get; set; }
    }

    public enum TipoPanProxy : byte
    {
        PAN = 1,
        Proxy = 2
    }
}

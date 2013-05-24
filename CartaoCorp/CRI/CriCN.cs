using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upSight.BD;
using ENLog = upSight.Global.Log.EN;
using CNLog = upSight.Global.Log.CN;
using WebService.WebService;


namespace upSight.CartaoCorp.CRI
{
    public class CriCN : ENLog.ProcessamentoArquivo
    {               
        /// <summary>
        /// Compoe os dados para geraçõa de CRI de solicitação
        /// </summary>
        /// <param name="idArquivo"></param>
        private List<CARD> GeraListaDeCartoesParaSolicitacao(int idArquivo)
        {
            List<CARD> lstCriEn = null;
            try
            {
                var prmCrg = CriBD.ObtemQuantidadeCartoesEmissao(idArquivo);
                var criEn = CriBD.ObtemParametroCRI(TpStatCode.Solicitação, prmCrg.IdProduto, idArquivo);
                lstCriEn = new List<CARD>();
                for (int i = 0; i < prmCrg.Quantitade; i++)
                {
                    this.CompoeCriEn(criEn);
                    lstCriEn.Add(criEn);
                }
            }
            catch (Exception e)
            {
                if (Base.TS.TraceError)
                    Trace.TraceWarning("{0}: {1}", new object[] { "CRTCorp.CRI.CRICN", e });
            }

            return lstCriEn;
        }

        /// <summary>
        /// Compoe os dados para geraçõa de CRI de carga
        /// </summary>
        /// <param name="idArquivo"></param>
        private List<CARD> GeraListaDeCartoesParaCarga(int idArquivo)
        {
            List<CARD> lstCriEn = null;
            try
            {
                var prmCard = CriBD.ObtemCartoesCarga(idArquivo);                                                
                
                    lstCriEn = new List<CARD>();
                    foreach (CriCartao crt in prmCard)
                    {
                        var cargaPan = CriBD.ObtemParametroCRI(TpStatCode.Carga, crt.IdProduto, idArquivo);
                        
                        this.CompoeCarga(cargaPan, crt.Valor);

                        CriBD.InsereCRICargaDet(idArquivo, cargaPan.RECID, upSight.CartaoCorp.EnumRetornoBase.TipoIdentificacao.PAN, crt.PanProxy, (upSight.CartaoCorp.EnumRetornoBase.StatusCartao)(Convert.ToByte(cargaPan.STATCODE)), crt.Valor);
                        
                        lstCriEn.Add(cargaPan);
                    }
                
            }
            catch (Exception e)
            {
                if (Base.TS.TraceError)
                    Trace.TraceWarning("{0}: {1}", new object[] { "CRTCorp.CRI.CRICN", e });
            }

            return lstCriEn;
        }        


        /// <summary>
        /// Gera o arquivio CRI de solicitação de cartões.
        /// </summary>
        /// <param name="idArquivo"></param>
        public void GeraArquivoCRIDeSolicitacao(int idArquivo)
        {
            try
            {
                var lstCriEn = this.GeraListaDeCartoesParaSolicitacao(idArquivo);
                var sw = CriBaseCN.SerializaDados(lstCriEn);
                string pathOrigem = String.Empty;
                CriBaseCN.GeraArquivoFisicoCRI(sw.ToString(), out pathOrigem);                                

                CriBD.CriFinaliza(idArquivo, pathOrigem, Path.GetFileName(pathOrigem));
            }
            catch (Exception e)
            {
                if (Base.TS.TraceError)
                    Trace.TraceWarning("{0}: {1}", new object[] { "CRTCorp.CRI.CRICN", e });
            }
        }


        /// <summary>
        /// Gera o arquivio CRI de carga de cartões.
        /// </summary>
        /// <param name="idArquivo"></param>
        public void GeraArquivoCRIDeCarga(int idArquivo)
        {
            try
            {
                    var lstCriEn = this.GeraListaDeCartoesParaCarga(idArquivo);
                    var sw = CriBaseCN.SerializaDados(lstCriEn);                

                    string nomeArquivoCompleto;
                    //Serializo os dados e gero o arquivo
                    string pathOrigem = String.Empty;
                    CriBaseCN.GeraArquivoFisicoCRI(sw.ToString(), out nomeArquivoCompleto);                

                    string nomeArquivo = Path.GetFileName(nomeArquivoCompleto);

                    var log = new CNLog.Logging();
                    log.IdEntidade = 1;
                    log.IdUsuario = 1;
                    this.Log = log;

                    var mapArq = new ENLog.MapaArquivos(nomeArquivo, ENLog.TipoArquivo.FISUkArquivoCRIGeracao, nomeArquivoCompleto);
                    log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, nomeArquivoCompleto, "Inicia processamento de arquivo");
                    //Insere serviço para processamento do arquivo de retorno                    
                    CriBD.CriFinaliza(idArquivo, pathOrigem,  nomeArquivo);
                    this.InsereLog(mapArq);
                    log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.ProcessadoOk, nomeArquivoCompleto, "Finaliza processamento de arquivo");
                    
            }
            catch (Exception e)
            {
                if (Base.TS.TraceError)
                    Trace.TraceWarning("{0}: {1}", new object[] { "CRTCorp.CRI.CRICN", e });
            }
        }

        /// <summary>
        /// Mapeio os dados para geração do CRI tanto para identificação quanto para solicitação
        /// </summary>
        /// <param name="criEn"></param>
        public void CompoeCriEn(CARD criEn)
        {
            switch (criEn.STATCODE)
            {
                case "00":
                    criEn.ACTION = "6";
                    break;
                case "09":
                    criEn.ACTION = "1";
                    break;
            }
            criEn.INSTCODE = "ACS";
            criEn.RENEW = "0";
            criEn.ADDRIND = "0";
            criEn.LOADTYPE = "1";
            criEn.CURRCODE = "BRL";
            criEn.MAILSHOTS = "1";
            criEn.LANG = "6";
            criEn.PRODUCEPIN = "1";
            criEn.DLVMETHOD = "0";
            criEn.ISOLANG = "PT";
            criEn.ACCOUNTID = "00";
            criEn.NEWACC = "1";
            criEn.RECID = CriBaseCN.GeraRecID();
        }

        /// <summary>
        /// Mapeio os dados para geração do CRI tanto para identificação quanto para solicitação
        /// </summary>
        /// <param name="criEn"></param>
        public void CompoeCarga(CARD criEn, decimal Valor)
        {
        
            criEn.ACCOUNTID = "00";
            criEn.ACTION = "2";
            criEn.ADDRIND = "0";
            criEn.COUNTRY = "076";
            criEn.COUNTY = "SP";            
            criEn.INSTCODE = "ACS";                       
            criEn.CURRCODE = "BRL";          
            criEn.INSTCODE = "ACS";
            criEn.ISOLANG = "PT";                        
            criEn.LANG = "6";
            criEn.LOADTYPE = "1";
            criEn.MAILSHOTS = "";
            criEn.RECID = CriBaseCN.GeraRecID();

            ADDDETLIST clsADDDETLIST = new ADDDETLIST();
            clsADDDETLIST.ADDDET = new List<ADDDET>();

            ADDDET clsADDDET = new ADDDET();
            clsADDDET.REFCODE = "OPERATION";
            clsADDDET.VALUE = "RECARGA";
            clsADDDETLIST.ADDDET.Add(clsADDDET);
            criEn.ADDDETLIST = clsADDDETLIST;
            criEn.AMTLOAD = Valor.ToString(".00").Replace(".", "").Replace(",", ".");

        }

        /// <summary>
        /// Gera os cartões a serem identificados
        /// </summary>
        /// <param name="cartao"></param>
        /// <returns></returns>
        public static CARD CompoeDadosIdentificacaoSimplifCrtCRI(CriCartao crt, int idArquivo, TpStatCode tpStateCode)
        {
            CARD card = new CARD();
            
            CriCN criCn = new CriCN();

            var criEn = CriBD.ObtemParametroCRI(tpStateCode, crt.IdProduto, idArquivo);
            criCn.CompoeCriEn(criEn);
                
            //Registro cada item do arquivo(CPF e RECID)
            CriBD.InsereCRIIdentificacaoDet(idArquivo, crt.Cpf, card.RECID, (crt.PanProxy.Length > 16) ? CriBaseCN.TipoIdentificacao.PROXY : CriBaseCN.TipoIdentificacao.PAN, crt.PanProxy, (CriBaseCN.StatusCartao)Convert.ToByte(card.STATCODE));


            var nomeAbreviado = WebServiceBLL.AbreviadorNome(WebCommom.Adapter.TextHelper.TrataTexto(crt.Nome)).Split(new char[] { ' ' }, 2);
            card.LASTNAME = nomeAbreviado.Last();
            card.FIRSTNAME = nomeAbreviado.First();
            card.DOB = (crt.DtNascimento.HasValue) ? crt.DtNascimento.Value.ToString("yyyy-MM-dd") : String.Empty;

            ADDDETLIST addetList = new ADDDETLIST();
            addetList.ADDDET = new List<ADDDET>();

            ADDDET addet = new ADDDET();
            addet.REFCODE = "ACS_CPF";
            addet.VALUE = crt.Cpf;
            addetList.ADDDET.Add(addet);


            List<String> NomeMae = crt.NomeMae.ToUpper().Split(' ').ToList();
            addet = new ADDDET();
            addet.REFCODE = "MOTHER_NAME";
            addet.VALUE = WebCommom.Adapter.TextHelper.TrataTexto((NomeMae.Count >= 2) ? NomeMae.First() + " " + NomeMae.Last() : NomeMae.First());
            addetList.ADDDET.Add(addet);
            card.ADDDETLIST = addetList;

            return card;
        }

        /// <summary>
        /// Insere log
        /// </summary>
        /// <param name="mapArq"></param>
        /// <param name="excecao"></param>
        private void InsereLog(ENLog.MapaArquivos mapArq, string excecao = null)
        {
            ENLog.ArquivoLog aLog = this.Log.ObtemArquivoLog(mapArq.IdArquivo, mapArq.Tipo);
            aLog.NumLinha = 0;
            aLog.Descricao = String.Format("{0} linhas processadas", 0);
            this.Log.InsereLog(aLog);
        }

        
    }


    /// <summary>
    /// Classe auxiliar para armazenar os dados retornados em consulta(idProduto e quantidade)
    /// </summary>
    public class ParamCRICarga
    {
        public int IdProduto { get; set; }
        public int Quantitade { get; set; }
    }

    public class CriCartao
    {
        #region Propriedades

        public int IdArquivo { get; set; }
        public string TpRegistro { get; set; }
        public string TpPanProxy { get; set; }
        public string PanProxy { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string NomeFacial { get; set; }
        public DateTime? DtNascimento { get; set; }
        public string Sexo { get; set; }
        public string CnpjFilial { get; set; }
        public string Grupo { get; set; }
        public string Email { get; set; }
        public string DddCel { get; set; }
        public string Celular { get; set; }
        public string NomeMae { get; set; }
        public string IdRegistro { get; set; }
        public int NumLinha { get; set; }
        public string NomeConvenio { get; set; }
        public string NomeProduto { get; set; }
        public int IdProduto { get; set; }
        public decimal Valor { get; set; }

        #endregion

    }
}

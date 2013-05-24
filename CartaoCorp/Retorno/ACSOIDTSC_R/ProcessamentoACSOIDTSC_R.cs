using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;

using ENLog = upSight.Global.Log.EN;
using CNLog = upSight.Global.Log.CN;
using System.Data.OleDb;
using System.Data;
using System.Collections;


namespace upSight.CartaoCorp.Identificacao.ACSOIDTSC_R
{
    public class ProcessamentoACSOIDTSC_R : ENLog.ProcessamentoArquivo
    {
    

        /// <summary>
        /// Processo o arquivo de retorno relacionado a identificação
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public void ProcessaDadosParaGerarArquivoRetornoIdentif(int idArquivo, string pathArquivoOrigem, string codConvenio = "ACSO")
        {
           // using (StringWriter sw = new StringWriter(new System.Globalization.CultureInfo("pt-BR")))
           // {
                DateTime dtAgora = DateTime.Now;
                string pathDestino = ConfigurationManager.AppSettings["ACSOIDTSC_R.CRI.DiretotioDestino"];
                string nomeArquivo = this.FormataNomeArquivo(codConvenio, dtAgora);
                string pathCompleto = Path.Combine(pathDestino, nomeArquivo);

                ENLog.MapaArquivos mapArq = new ENLog.MapaArquivos(nomeArquivo, ENLog.TipoArquivo.ACSOIDTSC_R, pathArquivoOrigem, 0);
                this.Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, pathArquivoOrigem, "Inicia processamento de arquivo");    
                int novoIdArquivo = mapArq.IdArquivo;
                
                int numLinha = 0;

                try
                {

                    this.Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, String.Empty, "Inicia processamento de arquivo");
                    numLinha++;

                    //Gero Cabeçalho
                    var cab = this.MontaCabecalho(novoIdArquivo, dtAgora, nomeArquivo, "ACSO", "ACESSO", numLinha);
                    cab.Insere();
                    //sw.WriteLine(cab.ToString());

                    //Gero Detalhe
                    var detalhe = new Identificacao.ACSOIDTSC_R.ACSOIDTSC_RDetalheEN();
                    detalhe.DataProc = dtAgora;
                    detalhe.IdArquivo = novoIdArquivo;
                    var detalhesRetorno = DetalheRetornoBaseBD.ConsultaDetalheIdentificacao(idArquivo);

                    int contTotalRetorno = 0;
                    int contErro = 0;
                    foreach (var detalheRet in detalhesRetorno)
                    {
                        try
                        {
                            numLinha++;
                            contTotalRetorno++;
                            this.CompoeDetalhe(detalheRet, detalhe, numLinha);
                            detalhe.Insere();
                            //sw.WriteLine(detalhe.ToString());
                        }
                        catch (Exception)
                        {
                            contErro++;
                            string descErro = String.Format("Total de linhas com erro: {0}", contErro);
                            this.InsereLog(mapArq, numLinha, ENLog.TipoLog.Informação, descErro);
                        }
                    }

                    numLinha++;
                    //Gero Rodapé
                    var rdp = this.MontaRodape(novoIdArquivo, numLinha, contTotalRetorno);
                    rdp.Insere();
                    //sw.WriteLine(rdp.ToString());
                }
                catch (Exception)
                {
                    string descErro = "Erro processamento arquivo";
                    Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.ProcessadoErro, pathCompleto, descErro);
                    this.InsereLog(mapArq, numLinha, ENLog.TipoLog.Informação, descErro);
                }
            //}
        }

        /// <summary>
        /// Gera o arquivo de retorno de identificação quando a partir de um arquivo que já foi gerado
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public StringWriter GeraArquivoRetornoIdentificacao(int idArquivo)
        {
            try
            {
                using (StringWriter sw = new StringWriter(new System.Globalization.CultureInfo("pt-BR")))
                {
                    //Gero Cabeçalho
                    sw.WriteLine(ACSOIDTSC_RCabecalhoBD.ConsultaPorIdArquivo(idArquivo).ToString());

                    //Gero detalhe
                    foreach (var det in ACSOIDTSC_RDetalheBD.ConsultaPorIdArquivo(idArquivo))
                        sw.WriteLine(det.ToString());

                    //Gero Rodapé
                    sw.WriteLine(ACSOIDTSC_RRodapeBD.ConsultaPorIdArquivo(idArquivo).ToString());

                    return sw;
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }


        /// <summary>
        /// Gera o arquivo de retorno de identificação quando a partir de um arquivo que já foi gerado
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public void GeraArquivoFisicoRetornoIdentificacao(int idArquivo)
        {
            try
            {
                var cab = ACSOIDTSC_RCabecalhoBD.ConsultaPorIdArquivo(idArquivo);
                string path = ConfigurationManager.AppSettings["ACSOIDTSC_R.CRI.DiretotioDestino"];
                string pathCompleto = Path.Combine(path, cab.NomeArquivo);

                using (StreamWriter sw = new StreamWriter(pathCompleto, false, Encoding.UTF8))
                {
                    //Gero Cabeçalho
                    sw.WriteLine(cab.ToString());

                    //Gero detalhe
                    foreach (var det in ACSOIDTSC_RDetalheBD.ConsultaPorIdArquivo(idArquivo))
                        sw.WriteLine(det.ToString());

                    //Gero Rodapé
                    sw.WriteLine(ACSOIDTSC_RRodapeBD.ConsultaPorIdArquivo(idArquivo).ToString());
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

        /// <summary>
        /// Monta o cabeçalho
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="dtAgora"></param>
        /// <param name="nomeArquivo"></param>
        /// <param name="codConvenio"></param>
        /// <param name="codEmpresa"></param>
        /// <param name="numLinha"></param>
        /// <returns></returns>
        private ACSOIDTSC_RCabecalhoEN MontaCabecalho(int idArquivo, DateTime dtAgora, string nomeArquivo, string codConvenio, string codEmpresa, int numLinha)
        {
            var cab = new ACSOIDTSC_RCabecalhoEN()
            {
                IdArquivo = idArquivo,
                DataGeracao = dtAgora,
                NomeArquivo = nomeArquivo,
                CodConvenio = codConvenio,
                CodEmpresa = codEmpresa,
                NumLinha = numLinha
            };

            return cab;
        }


        /// <summary>
        /// Componho os detalhes do processamento de detalhe do arquivo de retorno de identificação
        /// </summary>
        /// <param name="idfSimpRetEN"></param>
        /// <param name="acsIdtDetEN"></param>
        /// <param name="numLinha"></param>
        /// <param name="statusProc"></param>
        private void CompoeDetalhe(IdentificacaoSimplifRetornoEN idfSimpRetEN, ACSOIDTSC_RDetalheEN acsIdtDetEN, int numLinha)
        {
            acsIdtDetEN.Cpf = idfSimpRetEN.Cpf;
            acsIdtDetEN.TpIdentificacao = idfSimpRetEN.TpIdentificacao;
            acsIdtDetEN.Identificacao = idfSimpRetEN.Identificacao;
            acsIdtDetEN.StatusCart = idfSimpRetEN.StatusCart;
            acsIdtDetEN.NumLinha = numLinha;

            acsIdtDetEN.StatusProc = (!String.IsNullOrEmpty(idfSimpRetEN.Retorno)) ? upSight.CartaoCorp.EnumRetornoBase.StatusProcessamento.ErroGenérico 
                                                                                   : upSight.CartaoCorp.EnumRetornoBase.StatusProcessamento.Sucesso;
            acsIdtDetEN.Retorno = idfSimpRetEN.Retorno;
        }

        /// <summary>
        /// Componho os dados do rodapé do arquivo de retorno de identificação
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="numLinha"></param>
        /// <param name="numIdent"></param>
        /// <returns></returns>
        private ACSOIDTSC_RRodapeEN MontaRodape(int idArquivo, int numLinha, int numIdent)
        {
            var acsIdtRdpEN = new ACSOIDTSC_RRodapeEN()
            {
                IdArquivo = idArquivo,
                NumIdent = numIdent,
                NumLinha = numLinha
            };

            return acsIdtRdpEN;
        }

        private string FormataNomeArquivo(string codConvenio, DateTime dtAgora)
        {
            string padraoNomeArquivo = ConfigurationManager.AppSettings["FormatoArquivoACSOIDTSC_R"];
            return String.Format(padraoNomeArquivo, codConvenio, dtAgora);
        }

        /// <summary>
        /// Insere Log
        /// </summary>
        /// <param name="mapArq"></param>
        /// <param name="linhaAtual"></param>
        /// <param name="tpLog"></param>
        /// <param name="descricao"></param>
        private void InsereLog(ENLog.MapaArquivos mapArq, int linhaAtual, ENLog.TipoLog tpLog, string descricao)
        {
            var arqLog = Log.ObtemArquivoLog(mapArq.IdArquivo, mapArq.Tipo);
            arqLog.NumLinha = linhaAtual;
            arqLog.TipoLog = tpLog;
            arqLog.Descricao = descricao;
            this.Log.InsereLog(arqLog);
        }
    }
}

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

namespace upSight.CartaoCorp.Carga.ACSOPRGCR_R
{
    public class ProcessamentoACSOPRGCR_R : ENLog.ProcessamentoArquivo
    {
        /// <summary>
        /// Processa os dados retornados no arquivo de retorno de carga
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public void ProcessaDadosParaGerarArquivoRetornoCarga(int idArquivo, string pathArquivoOrigem, string codConvenio = "ACSO")
        {
            //using (StringWriter sw = new StringWriter(new System.Globalization.CultureInfo("pt-BR")))
            //{
                DateTime dtAgora = DateTime.Now;
                string pathDestino = ConfigurationManager.AppSettings["ACSOIDTSC_R.CRI.DiretotioDestino"];
                string nomeArquivo = this.FormataNomeArquivo(codConvenio, dtAgora);
                string pathCompleto = Path.Combine(pathDestino, nomeArquivo);

                ENLog.MapaArquivos mapArq = new ENLog.MapaArquivos(nomeArquivo, ENLog.TipoArquivo.ACSOIDTSC_R, pathArquivoOrigem, 0);
                Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, pathCompleto, "Inicia processamento de arquivo");
                mapArq.IdArquivo = idArquivo;
                int novoIdArquivo = mapArq.IdArquivo;
                
                int numLinha = 0;

                EnumRetornoBase.StatusProcessamento statusProc = EnumRetornoBase.StatusProcessamento.Sucesso;

                try
                {

                    //Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, pathDestino, "Inicia processamento de arquivo");
                    numLinha++;

                    //Gero Cabeçalho
                    var cab = this.MontaCabecalho(novoIdArquivo, dtAgora, nomeArquivo, "ACSO", "ACESSO", numLinha);
                    cab.Insere();
                    //sw.WriteLine(cab.ToString());

                    //Busco os detalhes
                    var detalhe = new ACSOPRGCR_RDetalheEN();
                    detalhe.IdArquivo = novoIdArquivo;
                    var detalhesRetorno = DetalheRetornoBaseBD.ConsultaDetalheCarga(idArquivo);

                    //Totalizadores
                    var cargaRejet = detalhesRetorno.Where(crgRej => !String.IsNullOrEmpty(crgRej.Retorno));

                    int totalRejeitadas = (cargaRejet != null) ? cargaRejet.Count() : 0;
                    int totalCarga = detalhesRetorno.Count() - totalRejeitadas;
                    decimal vlrTotalCrgRej = cargaRejet.Sum(vlrCrgRejet => vlrCrgRejet.Valor);
                    decimal vlrTotalCrg = detalhesRetorno.Sum(vlrCrg => vlrCrg.Valor) - vlrTotalCrgRej;

                    //Se houve rejeições mudo o status do processamento
                    if (totalRejeitadas > 0)
                        statusProc = EnumRetornoBase.StatusProcessamento.ErroGenérico;

                    //Gero Lote
                    this.MontaLote(novoIdArquivo, "01", totalRejeitadas, totalCarga, numLinha, statusProc).Insere();
                    //sw.WriteLine(this.MontaLote(novoIdArquivo, "", totalRejeitadas, totalCarga, numLinha, statusProc).ToString());
                    
                    int contErro = 0;
                    //Gero Detalhe
                    detalhesRetorno.ForEach(delegate(CargaRetornoDetalheEN crgdet)
                        {
                            try
                            {
                                numLinha++;
                                this.CompoeDetalhe(crgdet, detalhe, codConvenio, numLinha);
                                detalhe.Insere();
                                //sw.WriteLine(detalhe.ToString());
                            }
                            catch (Exception)
                            {
                                contErro++;
                                string descErro = String.Format("Total de linhas com erro: {0}", contErro);
                                //this.InsereLog(mapArq, numLinha, ENLog.TipoLog.Informação, contErro);
                                this.InsereLog(mapArq, numLinha, ENLog.TipoLog.Alerta, descErro);
                            }
                        }
                                            );

                    numLinha++;
                    //Gero Rodapé
                    var rdp = this.MontaRodape(novoIdArquivo, totalCarga, totalCarga, vlrTotalCrg, totalRejeitadas, vlrTotalCrg, numLinha);
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
        /// Gera o arquivo de retorno de carga quando a partir de um arquivo que já foi gerado
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <returns></returns>
        public StringWriter GeraArquivoRetornoCarga(int idArquivo)
        {
            try
            {
                using (StringWriter sw = new StringWriter(new System.Globalization.CultureInfo("pt-BR")))
                {
                    //Gero Cabeçalho
                    sw.WriteLine(ACSOPRGCR_RCabecalhoBD.ConsultaPorIdArquivo(idArquivo).ToString());

                    //Gero Lote
                    sw.WriteLine(ACSOPRGCR_RLoteBD.ConsultaPorIdArquivo(idArquivo).ToString());

                    //Gero detalhe
                    foreach (var det in ACSOPRGCR_RDetalheBD.ConsultaPorIdArquivo(idArquivo))
                        sw.WriteLine(det.ToString());

                    //Gero Rodapé
                    sw.WriteLine(ACSOPRGCR_RRodapeBD.ConsultaPorIdArquivo(idArquivo));

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
                var cab = ACSOPRGCR_RCabecalhoBD.ConsultaPorIdArquivo(idArquivo);
                string path = ConfigurationManager.AppSettings["ACSOIDTSC_R.CRI.DiretotioDestino"];
                string pathCompleto = Path.Combine(path, cab.NomeArquivo);

                using (StreamWriter sw = new StreamWriter(pathCompleto, false, Encoding.UTF8))
                {
                    //Gero Cabeçalho
                    sw.WriteLine(cab.ToString());

                    //Gero Lote
                    sw.WriteLine(ACSOPRGCR_RLoteBD.ConsultaPorIdArquivo(idArquivo).ToString());

                    //Gero detalhe
                    foreach (var det in ACSOPRGCR_RDetalheBD.ConsultaPorIdArquivo(idArquivo))
                        sw.WriteLine(det.ToString());

                    //Gero Rodapé
                    sw.WriteLine(ACSOPRGCR_RRodapeBD.ConsultaPorIdArquivo(idArquivo).ToString());
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
        private ACSOPRGCR_RCabecalhoEN MontaCabecalho(int idArquivo, DateTime dtAgora, string nomeArquivo, string codConvenio, string codEmpresa, int numLinha)
        {
            var cab = new ACSOPRGCR_RCabecalhoEN()
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
        /// Monta os dados que irão compor o lote
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="codPrgCarga"></param>
        /// <param name="statusProc"></param>
        /// <param name="numCartoes"></param>
        /// <param name="numReject"></param>
        /// <param name="numLinha"></param>
        /// <returns></returns>
        private ACSOPRGCR_RLoteEN MontaLote(int idArquivo, string codPrgCarga, int numRejeit, int numCart, int numLinha, EnumRetornoBase.StatusProcessamento statusProc)
        {
            var acsPrgCrgLoteEN = new ACSOPRGCR_RLoteEN()
            {
                IdArquivo = idArquivo,
                CodPrgCrg = codPrgCarga,
                StatusProc = statusProc,
                NumCart = numCart,
                NumRejeit = numRejeit,
                NumLinha = numLinha
            };

            return acsPrgCrgLoteEN;
        }

        /// <summary>
        /// Compõe os dados do detalhe
        /// </summary>
        /// <param name="crgRetDetEn"></param>
        /// <param name="acsPrgGcrEN"></param>
        /// <param name="idRegistro"></param>
        /// <param name="numLinha"></param>
        private void CompoeDetalhe(CargaRetornoDetalheEN crgRetDetEn, ACSOPRGCR_RDetalheEN acsPrgGcrEN, string idRegistro, int numLinha)
        {
            acsPrgGcrEN.TpIdentificacao = crgRetDetEn.TpIdentificacao;
            acsPrgGcrEN.Identificacao = crgRetDetEn.Identificacao;
            acsPrgGcrEN.StatusCart = crgRetDetEn.StatusCart;
            acsPrgGcrEN.StatusProc = (!String.IsNullOrEmpty(crgRetDetEn.Retorno)) ? upSight.CartaoCorp.EnumRetornoBase.StatusProcessamento.ErroGenérico
                                                                                   : upSight.CartaoCorp.EnumRetornoBase.StatusProcessamento.Sucesso;
            acsPrgGcrEN.Retorno = crgRetDetEn.Retorno;

            acsPrgGcrEN.IdRegistro = idRegistro;
            acsPrgGcrEN.NumLinha = numLinha;
        }

        /// <summary>
        /// Monta o rodapé
        /// </summary>
        /// <param name="idArquivo"></param>
        /// <param name="numCrg"></param>
        /// <param name="numCart"></param>
        /// <param name="valorCrg"></param>
        /// <param name="numCrgRej"></param>
        /// <param name="valCrgRej"></param>
        /// <param name="numLinha"></param>
        /// <returns></returns>
        private ACSOPRGCR_RRodapeEN MontaRodape(int idArquivo, int numCrg, int numCart, decimal valorCrg, int numCrgRej, decimal valCrgRej, int numLinha)
        {
            var acsPrgCrgRdpEN = new ACSOPRGCR_RRodapeEN()
            {
                IdArquivo = idArquivo,
                NumCrg = numCrg,
                NumCart = numCart,
                ValorCrg = valorCrg,
                NumCrgRej = numCrgRej,
                ValCgrRej = valCrgRej,
                NumLinha = numLinha
            };

            return acsPrgCrgRdpEN;
        }

        /// <summary>
        /// Formata o nome do Arquivo
        /// </summary>
        /// <param name="codConvenio"></param>
        /// <param name="dtAgora"></param>
        /// <returns></returns>
        private string FormataNomeArquivo(string codConvenio, DateTime dtAgora)
        {
            string padraoNomeArquivo = ConfigurationManager.AppSettings["FormatoArquivoACSOPRGCR_R"];
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
            Log.InsereLog(arqLog);
        }
    }
}

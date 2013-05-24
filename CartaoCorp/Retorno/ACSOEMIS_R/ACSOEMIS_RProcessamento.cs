using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upSight.Negocio.Calculo;
using BDGeral = upSight.Consulta.Base.BD.Geral;
using ENLog = upSight.Global.Log.EN;


namespace upSight.CartaoCorp.Emissao.ACSOEMIS_R
{
    public class ACSOEMIS_RProcessamento : ENLog.ProcessamentoArquivo
    {
        public string GeraArquivoRetorno(string path, int idArquivo)
        {
            int numLinha = 0;
            string nomeArquivoFormatado = string.Empty;
            string nomeArquivo = string.Empty;

            ENLog.MapaArquivos mapArq = new ENLog.MapaArquivos(string.Empty,ENLog.TipoArquivo.ACSOEMIS_R,string.Empty,0);

            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);                                            

                numLinha++;
                //Gera cabeçalho
                var cab = new ACSOEMIS_RCabecalhoEN();
                cab.IdArquivo = idArquivo;
                cab.CompoeACSOEMIS_RCabecalho(cab);

                cab.NumLinha = numLinha;

                nomeArquivo = FormataNomeArquivo(cab.CodConvenio);
                nomeArquivoFormatado = System.IO.Path.Combine(path, nomeArquivo);

                mapArq = new ENLog.MapaArquivos(nomeArquivoFormatado, ENLog.TipoArquivo.ACSOEMIS_R, path, 0);
                this.Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, path, "Inicia processamento de arquivo");
                int novoIdArquivo = mapArq.IdArquivo;
                

                using (var sw = new StreamWriter(nomeArquivoFormatado, false, Encoding.UTF8))
                {
                    
                    sw.WriteLine(cab.ToString());
                    
                    //Gera detalhes
                    numLinha++;
                    var det = new ACSOEMIS_RDetalheEN();
                    det.IdArquivo = idArquivo;
                    det.NumLinha = numLinha;
                    det.CompoeACSOEMIS_RDetalheENEmissRet(det);

                    sw.WriteLine(det.ToString(TpRetornoDetalhe.RetornoDeEmissão));

                    var iDetCm = det.ConsultaDetalhe();

                    int contEmissao = 0;
                    int contErro = 0;
                    foreach (var detalhe in iDetCm)
                    {
                        try
                        {
                            this.Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, String.Empty, "Inicia processamento de arquivo");

                            contEmissao++;
                            numLinha++;                            

                            detalhe.numSeq = contEmissao.ToString("000000");
                            detalhe.Proxy = upSight.Consulta.Base.Sistema.CompletaEspacoDireita(contEmissao, 32);
                            detalhe.NumLinha = numLinha;

                            //gera proxy
                            string prxParcial = String.Concat(detalhe.CodConvenio, contEmissao.ToString("00000000000"));
                            string dv = DigitoVerificador.CalculaDV(prxParcial, DigitoVerificador.TipoDigitoVerificador.Modulo10);
                            detalhe.Proxy = String.Concat(prxParcial, dv);

                            sw.WriteLine(detalhe.ToString(TpRetornoDetalhe.DetalheDosCartões));
                        }
                        catch (Exception e)
                        {
                            if (BDGeral.TS.TraceError)
                                Trace.TraceError("{0}: {1}", new object[] { "u.E.L.E.GAE", e });
                            contErro++;
                            string descErro = String.Format("Total de linhas com erro: {0}", contErro);
                            this.InsereLog(mapArq, numLinha, ENLog.TipoLog.Informação, descErro);
                        }
                    }

                    //Gera Rodapé
                    numLinha++;
                    var rdp = ACSOEMIS_RRodapeEN.CompoeACSOEMIS_RRodapeEN(contEmissao, numLinha);
                    sw.WriteLine(rdp.ToString());
                }
                
            }
            catch (Exception e)
            {
                if (BDGeral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.E.L.E.GAE", e });
                string descErro = "Erro processamento arquivo";
                Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.ProcessadoErro, path, descErro);
                this.InsereLog(mapArq, numLinha, ENLog.TipoLog.Informação, descErro);
            }
            
            return nomeArquivo;
        }


        private void BuscarConvenioEmpresa()
        {

        }
        /// <summary>
        /// Formata a nome do arquivo
        /// </summary>
        /// <returns></returns>
        private string FormataNomeArquivo(string codConvenio = "1012")
        {
            try
            {
                string nomeArquivo = ConfigurationManager.AppSettings["FormatoArquivoACSOEMIS_R"];
                return String.Format(nomeArquivo, codConvenio, DateTime.Now);
            }
            catch (Exception e)
            {
                throw;
            }
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

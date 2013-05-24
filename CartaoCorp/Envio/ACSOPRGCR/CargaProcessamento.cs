using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using ENLog = upSight.Global.Log.EN;
using CNLog = upSight.Global.Log.CN;
using System.Data;
using System.Data.OleDb;
using System.Configuration;
using System.ComponentModel.DataAnnotations;


namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public class CargaProcessamento : ENLog.ProcessamentoArquivo
    {
        /// <summary>
        /// Dado o arquivo ele é realizado o parser e seus dados são salvos nas tabelas correspondentes
        /// </summary>
        /// <param name="path"></param>
        public void ProcessaArquivoCarga(string path, int idEdntidade, int idArquivo = 0)
        {
            string nomeArquivo = Path.GetFileName(path);

            ENLog.MapaArquivos mapArq = new ENLog.MapaArquivos(nomeArquivo, ENLog.TipoArquivo.ACSOPRGCR, path, 0, idArquivo);

            Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, mapArq.Arquivo, "Inicia processamento de arquivo");
            int novoidArquivo = mapArq.IdArquivo;

            int linhaAtual = 0;
            int qtdCarga = 0;


            using (StreamReader sr = new StreamReader(path))
            {

                DataSet ds = new DataSet();
                DataTable dt = new DataTable();

                ENLog.TipoLog tpLog = ENLog.TipoLog.Informação;
                Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, mapArq.Arquivo, "Em processamento de arquivo");
                string descErro = String.Empty;
                while (!sr.EndOfStream)
                {
                    try
                    {
                        linhaAtual++;
                        string linha = sr.ReadLine();

                        switch (linha.Substring(0, 1))
                        {
                            case "0":
                                var cab = new ACSOPRGCRCabecalhoEN(novoidArquivo, linha);
                                cab.Insere();
                                break;
                            case "1":
                                var lot = new ACSOPRGCRLoteEN(novoidArquivo, linha);
                                lot.Insere();
                                break;
                            case "2":
                                var det = new ACSOPRGCRDetalheEN(novoidArquivo, linha);
                                dt.Rows.Add(ACSOPRGCRDetalheEN.MapeiaTXT(linha, novoidArquivo, dt, idEdntidade));
                                //det.Insere();
                                qtdCarga++;
                                break;
                            case "9":
                                var rdp = new ACSOPRGCRRodapeEN(novoidArquivo, linha);
                                rdp.Insere();
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        descErro = String.Format("Erro ao processar arquivo de Carga de Cartões. Linha: {0}", linhaAtual);
                        this.InsereLog(mapArq, linhaAtual, ENLog.TipoLog.Alerta, descErro);
                        throw;
                    }
                }

                ds.Tables.Add(dt);
                InsereCartoes(ds, idArquivo, tpLog, mapArq);

                //Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.ProcessadoOk, mapArq.Arquivo, "Finaliza processamento de arquivo");

                //descErro = String.Format("Total de cargas processadas: {0}", qtdCarga);
                //this.InsereLog(mapArq, linhaAtual, ENLog.TipoLog.Informação, descErro);
            }

            ACSPRGCRBD.InsereNovoServico(idArquivo);
        }
        
        /// <summary>
        /// Percorre tabela em excel referente a carga
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>
        public void LePlanilhaExcelEInsereDados(string path, string nomePlanilha = "Sheet1")
        {
            string nomeArquivo = Path.GetFileName(path);
            ENLog.MapaArquivos mapArq = new ENLog.MapaArquivos(nomeArquivo, ENLog.TipoArquivo.ACSOPRGCR, path, 0);

            Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, mapArq.Arquivo, "Inicia processamento de arquivo");
            int idArquivo = mapArq.IdArquivo;

            //using (OleDbConnection conexao = new OleDbConnection(String.Format(ConfigurationManager.AppSettings["ConexaoExcel"], path)))
            using (OleDbConnection conexao = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';", path)))
            {
                conexao.Open();
                Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, mapArq.Arquivo, "Em processamento de arquivo");

                ENLog.TipoLog tpLog = ENLog.TipoLog.Informação;
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", String.Format("{0}$", nomePlanilha)), conexao))
                {
                    DataSet ds = new DataSet();
                    int linhas = adapter.Fill(ds);

                    try
                    {
                        InsereCartoes(ds, idArquivo, tpLog, mapArq);
                    }
                    finally
                    {
                        conexao.Close();
                    }
                    ACSPRGCRBD.InsereNovoServico(idArquivo);
                }
            }
        }

        /// <summary>
        /// Insere cartões de arquivos XLS e TXT.
        /// </summary>
        /// <param name="ds"></param>
        /// <param name="idArquivo"></param>
        /// <param name="tpLog"></param>
        /// <param name="mapArq"></param>
        private void InsereCartoes(DataSet ds, int idArquivo, ENLog.TipoLog tpLog, ENLog.MapaArquivos mapArq)
        {
            int qtdCgr = 0;
            try
            {
                var acsCrgDetEn = new ACSOPRGCRDetalheEN();
                var crgCn = new CargaCN();

                List<ValidationResult> lstVr = null;

                int numLinha = 0;
                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    try
                    {
                        if (!String.IsNullOrEmpty(dr["Identificacao"].ToString()))
                        {
                            numLinha++;
                            lstVr = acsCrgDetEn.Mapeia(dr, crgCn.CriaMapaColuna(dr));

                            if (lstVr.Count == 0)
                            {
                                lstVr = crgCn.Valida(acsCrgDetEn);
                                if (lstVr.Count == 0)
                                {
                                    acsCrgDetEn.NumLinha = numLinha;
                                    acsCrgDetEn.IdArquivo = idArquivo;
                                    acsCrgDetEn.Insere();
                                    qtdCgr++;
                                }
                                else
                                {
                                    tpLog = ENLog.TipoLog.Informação;
                                    this.InsereLogErros(mapArq, tpLog, qtdCgr, numLinha, lstVr);
                                }
                            }
                            else
                            {
                                tpLog = ENLog.TipoLog.Informação;
                                this.InsereLogErros(mapArq, tpLog, qtdCgr, numLinha, lstVr);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                        tpLog = ENLog.TipoLog.Alerta;
                        string descErro = String.Format("Erro ao processar arquivo de Programação de Carga de Cartões. Linha: {0}", numLinha);
                        this.InsereLog(mapArq, qtdCgr, tpLog, descErro);
                    }
                }
            }
            catch (Exception e)
            {
                tpLog = ENLog.TipoLog.Erro;
                string descErro = "Erro ao processar arquivo de Programação de Carga de Cartões";
                this.InsereLog(mapArq, qtdCgr, tpLog, descErro);
                throw;
            }


            switch (tpLog)
            {
                case upSight.Global.Log.EN.TipoLog.Erro:
                    Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, mapArq.Arquivo, "Finaliza processamento de arquivo");
                    break;
                default:
                    Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.ProcessadoOk, mapArq.Arquivo, "Finaliza processamento de arquivo");
                    break;
            }

            string desc = String.Format("Total de cargas realizadas: {0}", qtdCgr);
            this.InsereLog(mapArq, qtdCgr, ENLog.TipoLog.Informação, desc);
        }

        /// <summary>
        /// Gera arquivo de teste de carga
        /// </summary>
        /// <param name="path"></param>
        public void GeraArquivoCarga(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string pathCompleto = Path.Combine(path, FormataNomeArquivo());
                int numLinha = 0;
                int qtdCrg = 0;
                int numCrt = 0;
                int idArquivo = 1;
                string nomeArquivo = Path.GetFileName(pathCompleto);
                string codConvenio = "ACS1";
                decimal vlrCrg = 0.0M;

                using (StreamWriter sw = new StreamWriter(pathCompleto, false, Encoding.UTF8))
                {
                    numLinha++;
                    //Gera Cabeçalho
                    sw.WriteLine(new ACSOPRGCRCabecalhoEN().MontaACSOPRGCRCabecalhoEN(idArquivo, nomeArquivo, numLinha, codConvenio).ToString());

                    numLinha++;
                    //Gera Lote
                    sw.WriteLine(new ACSOPRGCRLoteEN().MontaACSOPRGCRLoteEN(idArquivo, codConvenio, numLinha).ToString());

                    //Gera detalhes
                    foreach (var det in ACSOPRGCRDetalheEN.MontaACSOPRGCRDetalheEN())
                    {
                        numLinha++;
                        qtdCrg++;
                        numCrt++;
                        vlrCrg += det.Valor;
                        det.NumLinha = numLinha;

                        sw.WriteLine(det.ToString());
                    }

                    numLinha++;
                    //Gera Rodapé
                    sw.WriteLine(new ACSOPRGCRRodapeEN().MontaACSOPRGCRRodapeEN(qtdCrg, numCrt, vlrCrg, numLinha).ToString());
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }



        /// <summary>
        /// Loga erros retornados da validação dos campos
        /// </summary>
        /// <param name="mapArq"></param>
        /// <param name="linhaAtual"></param>
        /// <param name="qtdCartoes"></param>
        /// <param name="lstVr"></param>
        private void InsereLogErros(ENLog.MapaArquivos mapArq, ENLog.TipoLog tpLog, int linhaAtual, int qtdCartoes, List<ValidationResult> lstVr)
        {
            lstVr.ForEach(vr =>
            {
                string descErro = String.Format("Msg: {0}, Linha: {1}", vr.ErrorMessage, linhaAtual);
                this.InsereLog(mapArq, qtdCartoes, tpLog, descErro);
            });
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

        /// <summary>
        /// Formata a nome do arquivo
        /// </summary>
        /// <returns></returns>
        private string FormataNomeArquivo()
        {
            try
            {
                string codConvenio = "ACS1";
                string arquivo = ConfigurationManager.AppSettings["FormatoArquivoACSOPRGCR"];
                return String.Format(arquivo, codConvenio, DateTime.Now);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}

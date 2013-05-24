using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Configuration;

using ENLog = upSight.Global.Log.EN;
using CNLog = upSight.Global.Log.CN;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public class IdentificacaoProcessamento : ENLog.ProcessamentoArquivo
    {
        /// <summary>
        /// Dado o arquivo ele é realizado o parser e seus dados são salvos nas tabelas correspondentes
        /// </summary>
        /// <param name="path"></param>
        public void ProcessaArquivoIdentificacaoSimpCrt(string path, int idEntidade)
        {
            string nomeArquivo = Path.GetFileName(path);

            ENLog.MapaArquivos mapArq = new ENLog.MapaArquivos(nomeArquivo, ENLog.TipoArquivo.ACSOIDTSC, path, 0);

            Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, String.Empty, "Inicia processamento de arquivo");
            int idArquivo = mapArq.IdArquivo;

            int linhaAtual = 0;
            int qtdCartoes = 0;
            int qtdCartoesErro = 0;

            using (StreamReader sr = new StreamReader(path))
            {
                Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, mapArq.Arquivo, "Em processamento de arquivo");
                string descErro = String.Empty;
                string codConvenio = string.Empty;

                ENLog.TipoLog tpLog = ENLog.TipoLog.Informação;
                DataSet ds = new DataSet();
                DataTable dt = new DataTable();
                

                while (!sr.EndOfStream)
                {
                    try
                    {                        
                        linhaAtual++;
                        string linha = sr.ReadLine();                        

                        switch (linha.Substring(0, 1))
                        {
                            case "0":
                                codConvenio = linha.Substring(95, 10).TrimEnd(null);
                                crtACSOIDTSCCabecalhoEN.Mapeia(linha, idArquivo).Insere();
                                break;                                
                            case "1":
                                dt.Rows.Add(crtACSOIDTSCDetalheEN.MapeiaTXTDet(linha, idArquivo, codConvenio, idEntidade, dt));                                
                                qtdCartoes++;
                                break;
                            case "9":
                                crtACSOIDTSCRodapeEN.Mapeia(linha, idArquivo).Insere();
                                break;
                        }
                    }
                    catch (Exception e)
                    {
                        tpLog = ENLog.TipoLog.Erro;
                        descErro = "Erro ao processar arquivo Identificação Simplificada de Cartões";
                        this.InsereLog(mapArq, qtdCartoes, tpLog, descErro, string.Empty);
                        throw;
                    }
                }

                ds.Tables.Add(dt);
                InsereCartoes(ds, idArquivo, tpLog, mapArq, qtdCartoes, qtdCartoesErro);

                //switch (tpLog)
                //{
                //    case upSight.Global.Log.EN.TipoLog.Erro:
                //        Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, mapArq.Arquivo, "Finaliza processamento de arquivo");
                //        break;
                //    default:
                //        Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.ProcessadoOk, mapArq.Arquivo, "Finaliza processamento de arquivo");
                //        break;
                //}

                //if (qtdCartoesErro > 0)
                //{
                //    qtdCartoes = qtdCartoes - qtdCartoesErro;
                //    descErro = String.Format("Total de cartões com erro: {0}", qtdCartoesErro);
                //    this.InsereLog(mapArq, qtdCartoes, ENLog.TipoLog.Informação, descErro, string.Empty);
                //}

                //descErro = String.Format("Total de cartões identificados: {0}", qtdCartoes);
                //this.InsereLog(mapArq, qtdCartoes, ENLog.TipoLog.Informação, descErro, string.Empty);

                PortadorBD.InsereNovoServico(idArquivo);
            }


        }


        /// <summary>
        /// Percorre a tabela de Excel e retorna um tipo de objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="obj"></param>
        /// <returns></returns>        
        public void LePlanilhaExcelEInsereDados(string path, int idEntidade, string nomePlanilha = "Sheet1")
        {
            string nomeArquivo = Path.GetFileName(path);
            ENLog.MapaArquivos mapArq = new ENLog.MapaArquivos(nomeArquivo, ENLog.TipoArquivo.ACSOIDTSC, path, 0);

            Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.NaoProcessado, String.Empty, "Inicia processamento de arquivo");
            int idArquivo = mapArq.IdArquivo;
            
            int qtdCartoes = 0;
            int qtdCartoesErro = 0;
            
            using (OleDbConnection conexao = new OleDbConnection(String.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0 Xml;HDR=YES;IMEX=1';", path)))
            {
                Log.AtualizaArquivo<ENLog.MapaArquivos>(mapArq, ENLog.StatusProcessamentoArquivo.EmProcessamento, mapArq.Arquivo, "Em processamento de arquivo");
                string descErro = String.Empty;

                conexao.Open();

                ENLog.TipoLog tpLog = ENLog.TipoLog.Informação;
                using (OleDbDataAdapter adapter = new OleDbDataAdapter(String.Format("SELECT * FROM [{0}]", String.Format("{0}$", nomePlanilha)), conexao))
                {                   
                    try
                    {
                        DataSet ds = new DataSet();
                        int linhas = adapter.Fill(ds);

                        InsereCartoes(ds, idArquivo, tpLog,mapArq, qtdCartoes, qtdCartoesErro);

                    }
                    catch (Exception e)
                    {
                        tpLog = ENLog.TipoLog.Erro;
                        descErro = "Erro ao processar arquivo Identificação Simplificada de Cartões";
                        this.InsereLog(mapArq, qtdCartoes, tpLog, descErro,string.Empty);
                        throw;
                    }
                    finally
                    {
                        conexao.Close();
                    }

                    

                    PortadorBD.InsereNovoServico(idArquivo);
                }
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
                string descErro = String.Format("Msg: {0}", vr.ErrorMessage, linhaAtual);
                string coluna = ((string[])(vr.MemberNames))[0];
                this.InsereLog(mapArq, qtdCartoes, tpLog, descErro, coluna);
            });
        }

        /// <summary>
        /// Insere Log
        /// </summary>
        /// <param name="mapArq"></param>
        /// <param name="linhaAtual"></param>
        /// <param name="tpLog"></param>
        /// <param name="descricao"></param>
        private void InsereLog(ENLog.MapaArquivos mapArq, int linhaAtual, ENLog.TipoLog tpLog, string descricao, string coluna)
        {
            var arqLog = Log.ObtemArquivoLog(mapArq.IdArquivo, mapArq.Tipo);
            arqLog.NumLinha = linhaAtual;
            arqLog.TipoLog = tpLog;
            arqLog.Descricao = descricao;
            arqLog.Coluna = coluna;
            Log.InsereLog(arqLog);
        }

        /// <summary>
        /// Gera o arquivo de identificação simplificada de cartões
        /// </summary>
        /// <param name="path"></param>
        public void GeraArquivoIdentificacaoSimpCrt(string path)
        {
            try
            {
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);

                string pathCompleto = Path.Combine(path, FormataNomeArquivo());
                int numLinha = 0;
                int qtdIdentificados = 0;
                using (StreamWriter sw = new StreamWriter(pathCompleto, false, Encoding.UTF8))
                {
                    numLinha++;

                    //Gera Cabeçalho
                    sw.WriteLine(CompoeLinhaCabecalho(numLinha));

                    //Gera detalhes
                    foreach (var det in PortadorBD.ConsultaDadosDetalhe())
                    {
                        numLinha++;
                        qtdIdentificados++;
                        sw.WriteLine(String.Concat(det, numLinha.ToString("000000")));
                    }

                    numLinha++;
                    //Gera Rodapé
                    sw.WriteLine(CompeLinhaRodape(qtdIdentificados, numLinha));
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Compõe os dados do cabeçalho
        /// </summary>
        /// <returns></returns>
        private string CompoeLinhaCabecalho(int numLinha)
        {
            string tpRegistro = "0";
            string nomeLayout = "ACSOIDTS";
            string versao = "1";
            DateTime dtAgora = DateTime.Now;
            string dtGeracao = dtAgora.ToString("yyyyMMdd");
            string horaGeracao = dtAgora.ToString("HHmmss");
            string seqArquivo = "1";
            string nomeArquivo = "Importação Simplificada Cartão";
            string codConvenio = "25";
            string codEmpresa = "ACS";

            return String.Concat(tpRegistro,
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(nomeLayout, 20),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(versao, 8),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dtGeracao, 8),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(horaGeracao, 6),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(seqArquivo, 2),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(nomeArquivo, 50),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(codConvenio, 10),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(codEmpresa, 14),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 175),
                                 numLinha.ToString("000000"));
        }

        /// <summary>
        /// Compõe os dados do detalhe
        /// </summary>
        /// <param name="dr"></param>
        /// <param name="numLinha"></param>
        /// <returns></returns>
        public static string CompoeLinhaDetalhe(SqlDataReader dr)
        {
            return String.Concat(dr["TpRegistro"].ToString(),
                                 dr["TpPanProxy"].ToString(),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["PanProxy"], 32),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["CPF"], 11),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["Nome"], 50),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["NomeFacial"], 25),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(Convert.ToDateTime(dr["DtNascimento"]).ToString("yyyyMMdd"), 8),
                                 dr["Sexo"].ToString(),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["CnpjFilial"], 14),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["Grupo"], 20),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["Email"], 30),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["DDDCel"], 2),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["Celular"], 9),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["NomeMae"], 50),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 30),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(dr["idRegistro"], 10));
        }

        /// <summary>
        /// Compõe os dados do rodapé
        /// </summary>
        /// <param name="numLotes"></param>
        /// <param name="numLinha"></param>
        /// <returns></returns>
        private string CompeLinhaRodape(int numLotes, int numLinha)
        {
            string tpRegistro = "9";

            return String.Concat(tpRegistro, numLotes.ToString("000000"),
                                 upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 287),
                                 numLinha.ToString("000000"));
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
                string arquivo = ConfigurationManager.AppSettings["FormatoArquivoACSOIDTS"];
                return String.Format(arquivo, codConvenio, DateTime.Now);
            }
            catch (Exception)
            {
                throw;
            }
        }

        private void InsereCartoes(DataSet ds, int idArquivo, ENLog.TipoLog tpLog, ENLog.MapaArquivos mapArq, int qtdCartoes, int qtdCartoesErro)
        {
            List<ValidationResult> lstVr = null;
            var ptr = new Portador();
            var ptrCn = new PortadorCN();

            int numLinha = 0;

            string descErro = string.Empty;


            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                try
                {

                    if (!String.IsNullOrEmpty(dr["CPF"].ToString()))
                    {
                        lstVr = ptr.Mapeia(dr, ptrCn.CriaMapaColuna(dr));
                        numLinha++;

                        if (lstVr.Count == 0)
                        {
                            lstVr = ptrCn.Valida(ptr);
                            if (lstVr.Count == 0)
                            {
                                qtdCartoes++;
                                ptr.Insere(idArquivo, numLinha);
                            }
                            else
                            {
                                qtdCartoesErro++;
                                tpLog = ENLog.TipoLog.Erro;
                                this.InsereLogErros(mapArq, tpLog, qtdCartoes, numLinha, lstVr);
                            }
                        }
                        else
                        {
                            tpLog = ENLog.TipoLog.Erro;
                            this.InsereLogErros(mapArq, tpLog, qtdCartoes, numLinha, lstVr);
                        }
                    }
                }

                catch (Exception)
                {
                    tpLog = ENLog.TipoLog.Erro;
                    descErro = "Erro ao processar arquivo de Identificação Simplificada de Cartões";
                    this.InsereLog(mapArq, qtdCartoes, tpLog, descErro, string.Empty);
                }
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

            if (qtdCartoesErro > 0)
            {
                qtdCartoes = qtdCartoes - qtdCartoesErro;
                descErro = String.Format("Total de cartões com erro: {0}", qtdCartoesErro);
                this.InsereLog(mapArq, qtdCartoes, ENLog.TipoLog.Informação, descErro, string.Empty);
            }

            descErro = String.Format("Total de cartões identificados: {0}", qtdCartoes);
            this.InsereLog(mapArq, qtdCartoes, ENLog.TipoLog.Informação, descErro, string.Empty);
        }
    }
}


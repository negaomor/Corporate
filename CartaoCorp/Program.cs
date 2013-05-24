using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.IO;
using upSight.CartaoCorp.Identificacao.ACSOIDTS;
using upSight.CartaoCorp.Emissao.ACSOEMIS_R;
using upSight.CartaoCorp.Carga.ACSOPRGCR;
using upSight.CartaoCorp.Identificacao.ACSOIDTSC_R;
using upSight.CartaoCorp.Carga.ACSOPRGCR_R;

namespace upSight.CartaoCorp
{
    public class Program
    {
        static void Main(string[] args)
        {
            //string path = ConfigurationManager.AppSettings["DiretotioOrigemACSOIDTS"];

            var log = new upSight.Global.Log.CN.Logging();
            log.IdEntidade = 1;
            log.IdUsuario = 1;
            //IdentificacaoProcessamento imp = new IdentificacaoProcessamento();
            //imp.Log = log;

           
            //imp.GeraArquivoIdentificacaoSimpCrt(path);
            //ProcessamentoACSOIDTSC_R retIdentificao = new ProcessamentoACSOIDTSC_R();
            //retIdentificao.GeraArquivoFisicoRetornoIdentificacao(1399);

            //ProcessamentoACSOIDTSC_R retIdentificacao = new ProcessamentoACSOIDTSC_R();
            //retIdentificacao.GeraArquivoFisicoRetornoIdentificacao(1405);

            //ProcessamentoACSOPRGCR_R acsRPRc = new ProcessamentoACSOPRGCR_R();
            //acsRPRc.GeraArquivoFisicoRetornoIdentificacao(1406);

            //string path = "C:\\Temp\\Identificacao\\";

            //IdentificacaoProcessamento idProc = new IdentificacaoProcessamento();
            //idProc.ProcessaArquivoIdentificacaoSimpCrt(path + args[0].ToString(),1);
            //idProc.LePlanilhaExcelEInsereDados(path + args[0].ToString());

            //ACSOEMIS_RProcessamento acsoeMIS = new ACSOEMIS_RProcessamento();
            //acsoeMIS.GeraArquivoRetorno(path, 1612);

            //path = ConfigurationManager.AppSettings["DiretotioOrigemACSOPRGCR"];
            //CargaProcessamento carga = new CargaProcessamento();
            //carga.GeraArquivoCarga("c:\\temp\\carga\\");

            //path = "c:\\temp\\carga\\" + args[0];

            //ProcessamentoACSOPRGCR_R retornoCarga = new ProcessamentoACSOPRGCR_R();
            //retornoCarga.ProcessaDadosParaGerarArquivoRetornoCarga(2651, path);

            //CRI.CriCN cri = new CRI.CriCN();
            //cri.GeraArquivoCRIDeCarga(2651);            

            CargaProcessamento crgProc = new CargaProcessamento();
            //crgProc.Log = log;
            //crgProc.GeraArquivoCarga(path);                                   
            string path = @"C:\Temp\Acesso\";
            crgProc.ProcessaArquivoCarga(Path.Combine(path, "ACSOPRGCR_1012_20130326115949.txt"),449,2671);

       


            //var log = new upSight.Global.Log.CN.Logging();
            //log.IdEntidade = 1;
            //log.IdUsuario = 1;

            //IdentificacaoProcessamento imp = new IdentificacaoProcessamento();
            //imp.Log = log;
            //string arquivo = "Campanha2013RontanMaisOK.xlsx";
            //string nomeArquivoCompleto = Path.Combine(ConfigurationManager.AppSettings["DiretotioOrigemACSOIDTS"], arquivo);

            //imp.LePlanilhaExcelEInsereDados(nomeArquivoCompleto, "Plan1");
            //if (args.Count() > 0)
            //{
            //    var log = new upSight.Global.Log.CN.Logging();
            //    log.IdEntidade = 1;
            //    log.IdUsuario = 1;

            //    ImportacaoSimplificadaCrt imp = new ImportacaoSimplificadaCrt();
            //    imp.Log = log;
            //    string nomeArquivoCompleto = Path.Combine(ConfigurationManager.AppSettings["DiretotioOrigemACSOIDTS"], args[0]);

            //    if (args[0].EndsWith("xls"))
            //        imp.LePlanilhaExcelEInsereDados(nomeArquivoCompleto);
            //    else
            //        imp.ProcessaArquivoIdentificacaoSimpCrt(nomeArquivoCompleto);
            //}
        }
    }
}

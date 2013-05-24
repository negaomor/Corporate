using System;
using System.Configuration;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using upSight.CartaoCorp.Identificacao.ACSOIDTS;

namespace UnitTestCartaoCorp
{
    [TestClass]
    public class TesteIdentificacao
    {
        [TestMethod]
        public void ImportaPlanilha()
        {
            var log = new upSight.Global.Log.CN.Logging();
            log.IdEntidade = 1;
            log.IdUsuario = 1;

            IdentificacaoProcessamento imp = new IdentificacaoProcessamento();
            imp.Log = log;
            string arquivo = "TesteImpSimpf.xlsx";
            string nomeArquivoCompleto = Path.Combine(ConfigurationManager.AppSettings["DiretotioOrigemACSOIDTS"], arquivo);

            imp.LePlanilhaExcelEInsereDados(nomeArquivoCompleto);
        }
    }
}

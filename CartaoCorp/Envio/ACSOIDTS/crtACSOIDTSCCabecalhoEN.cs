using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data;

using upSight.Consulta.Base;
using System.Diagnostics;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public class crtACSOIDTSCCabecalhoEN
    {
        #region Propridades

        public int IdArquivo { get; set; }
        public string TpRegistro { get; set; }
        public string NomeLayout { get; set; }
        public string Versao { get; set; }
        public DateTime DtGeracao { get; set; }
        public int SeqArquivo { get; set; }
        public string NomeArquivo { get; set; }
        public string CodConvenio { get; set; }
        public string CodEmpresa { get; set; }
        public int NumLinha { get; set; }

        #endregion

        #region Construtores

        public crtACSOIDTSCCabecalhoEN() { }

        #endregion

        #region Métodos


        public static crtACSOIDTSCCabecalhoEN Mapeia(string linha, int idArquivo)
        {

            var acsIdstCab = new crtACSOIDTSCCabecalhoEN()
            {
                IdArquivo = idArquivo,
                TpRegistro = linha.Substring(0, 1),
                NomeLayout = linha.Substring(1, 20).TrimEnd(null),
                Versao = linha.Substring(21, 8).TrimEnd(null),
                DtGeracao = Data.ParseEstendido(linha.Substring(29, 14), Data.FormatoData.AAAAMMDDHHMMSS),
                SeqArquivo = Convert.ToInt32(linha.Substring(43, 2).TrimEnd(null)),
                NomeArquivo = linha.Substring(45, 50).TrimEnd(null),
                CodConvenio = linha.Substring(95, 10).TrimEnd(null),
                CodEmpresa = linha.Substring(105, 14).TrimEnd(null),
                NumLinha = Convert.ToInt32(linha.Substring(linha.Length - 6, 6).TrimEnd(null))
            };

            return acsIdstCab;
        }
        

        #endregion
    }
}


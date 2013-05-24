﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTSC_R
{
    public class ACSOIDTSC_RCabecalhoEN : RetornoCabecalhoBaseEN
    {
        [Column(Name = "TpRegistro", DbType = "CHAR(1) NOT NULL")]
        public const string TpRegistro = "0";

        [Column(Name = "NomeLayout", DbType = "VARCHAR(20) NOT NULL")]
        public const string NomeLayout = "ACSOIDTS_R";

        [Column(Name = "Versao", DbType = "VARCHAR(8) NOT NULL")]
        public const string Versao = "1.1";


        #region Construtores

        public ACSOIDTSC_RCabecalhoEN() { }

        #endregion

        #region Métodos

        /// <summary>
        /// ToString do cabeçalho
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            DateTime dtAgora = DateTime.Now;

            return String.Concat( ACSOIDTSC_RCabecalhoEN.TpRegistro,
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(ACSOIDTSC_RCabecalhoEN.NomeLayout, 20),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(ACSOIDTSC_RCabecalhoEN.Versao, 8),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.DataGeracao.ToString("yyyyMMdd"), 8),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.DataGeracao.ToString("HHmmss"), 6),
                                  this.SeqArquivo.ToString("00"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.NomeArquivo, 50),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodConvenio, 10),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodEmpresa, 14),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 25),
                                  this.NumLinha.ToString("000000")
                                );
        }

        #endregion
    }
}

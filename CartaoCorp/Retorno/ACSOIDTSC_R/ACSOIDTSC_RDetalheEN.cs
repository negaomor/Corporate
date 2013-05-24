using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.ComponentModel;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTSC_R
{
    public class ACSOIDTSC_RDetalheEN : DetalheRetornoBaseEN
    {

        [Column(Name = "TpRegistro", DbType = "CHAR(1) NOT NULL")]
        public const string TpRegistro = "1";

        #region Propriedades

        [Column(Name = "IdRetIdentDet", DbType = "INT NOT NULL")]
        public int IdRetIdentDet { get; set; }

        [Column(Name = "Cpf", DbType = "VARCHAR(11) NOT NULL")]
        public string Cpf { get; set; }

        [Column(Name = "DataProc", DbType = "DATETIME2 NOT NULL")]
        public DateTime DataProc { get; set; }

        [Column(Name = "IdRegistro", DbType = "VARCHAR(10) NULL")]
        public string IdRegistro { get; set; }

        [Column(Name = "NumLinha", DbType = "INT NOT NULL")]
        public int NumLinha { get; set; }

        #endregion

        #region Construtores

        public ACSOIDTSC_RDetalheEN() { }

        #endregion

        #region Métodos

        public override string ToString()
        {
            return String.Concat(ACSOIDTSC_RDetalheEN.TpRegistro,
                                  Convert.ToByte(this.TpIdentificacao).ToString("0"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Identificacao, 32),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Cpf, 11),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.DataProc.ToString("yyyyMMdd"), 8),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.DataProc.ToString("HHmmss"), 6),
                                  Convert.ToInt32(this.StatusProc).ToString("000"),
                                  Convert.ToInt16(this.StatusCart).ToString("00"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Retorno, 50),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 20),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.IdRegistro, 10),
                                  this.NumLinha.ToString("000000"));
        }

        #endregion
    }
}

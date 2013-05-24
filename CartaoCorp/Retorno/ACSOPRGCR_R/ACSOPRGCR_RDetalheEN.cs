using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR_R
{
    public class ACSOPRGCR_RDetalheEN : DetalheRetornoBaseEN
    {
        public const string TpRegistro = "2";

        #region Propriedades

        [Column(Name = "IdPrgCrgRetDetalhe", DbType = "INT NOT NULL")]
        public int IdPrgCrgRetDetalhe { get; set; }

        [Column(Name = "IdRegistro", DbType = "VARCHAR(10) NULL")]
        public string IdRegistro { get; set; }

        [Column(Name = "NumLinha", DbType = "INT NOT NULL")]
        public int NumLinha { get; set; }

        #endregion

        #region Construtores

        public ACSOPRGCR_RDetalheEN() { }

        #endregion

        #region Métodos

        public override string ToString()
        {
            return String.Concat(ACSOPRGCR_RDetalheEN.TpRegistro,
                                  ((byte)this.TpIdentificacao).ToString("0"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Identificacao, 32),
                                  ((int)this.StatusProc).ToString("000"),
                                  ((byte)this.StatusCart).ToString("00"),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.Retorno, 50),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 25),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.IdRegistro, 10),
                                  this.NumLinha.ToString("000000")
                                );
        }

        #endregion
    }
}

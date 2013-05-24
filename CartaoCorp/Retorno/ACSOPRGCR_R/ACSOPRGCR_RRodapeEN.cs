using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR_R
{
    public class ACSOPRGCR_RRodapeEN
    {
        public const string TpRegistro = "9";


        #region Propriedades

        [Column(Name = "IdPrgCrgRetRodape", DbType = "INT NOT NULL")]
        public int IdPrgCrgRetRodape { get; set; }

        [Column(Name = "IdArquivo", DbType = "INT NOT NULL")]
        public int IdArquivo { get; set; }

        [Column(Name = "NumCrg", DbType = "INT NOT NULL")]
        public int NumCrg { get; set; }

        [Column(Name = "NumCart", DbType = "INT NOT NULL")]
        public int NumCart { get; set; }

        [Column(Name = "ValorCrg", DbType = "MONEY NOT NULL")]
        public decimal ValorCrg { get; set; }

        [Column(Name = "NumCrgRej", DbType = "INT NOT NULL")]
        public int NumCrgRej { get; set; }

        [Column(Name = "ValCgrRej", DbType = "MONEY NOT NULL")]
        public decimal ValCgrRej { get; set; }

        [Column(Name = "NumLinha", DbType = "INT NOT NULL")]
        public int NumLinha { get; set; }

        #endregion

        #region Construtores

        public ACSOPRGCR_RRodapeEN() { }

        #endregion

        #region Métodos

        public override string ToString()
        {
            return String.Concat( ACSOPRGCR_RRodapeEN.TpRegistro,
                                  this.NumCrg.ToString("000000"),
                                  this.NumCart.ToString("000000"),
                                  upSight.Consulta.Base.Sistema.CompletaComZerosEsquerda((Decimal.Truncate(this.ValorCrg * 100)), 12),
                                  this.NumCrgRej.ToString("000000"),
                                  upSight.Consulta.Base.Sistema.CompletaComZerosEsquerda((Decimal.Truncate(this.ValCgrRej * 100)), 12),
                                  upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 81),
                                  this.NumLinha.ToString("000000")
                                );
        }

        #endregion
    }
}

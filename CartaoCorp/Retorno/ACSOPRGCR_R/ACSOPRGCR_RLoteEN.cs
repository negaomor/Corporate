using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Linq.Mapping;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR_R
{
    public class ACSOPRGCR_RLoteEN
    {
        public const string TpRegistro = "1";


        #region Propriedades

        [Column(Name = "IdArquivo", DbType = "INT NOT NULL")]
        public int IdArquivo { get; set; }

        [Column(Name = "IdPrgCrgRetLote", DbType = "INT NOT NULL")]
        public int IdPrgCrgRetLote { get; set; }

        [Column(Name = "CodPrgCrg", DbType = "VARCHAR(10) NOT NULL")]
        public string CodPrgCrg { get; set; }

        [Column(Name = "StatusProc", DbType = "INT NOT NULL")]
        public upSight.CartaoCorp.EnumRetornoBase.StatusProcessamento StatusProc { get; set; }

        [Column(Name = "NumCart", DbType = "INT NOT NULL")]
        public int NumCart { get; set; }

        [Column(Name = "NumRejeit", DbType = "INT NOT NULL")]
        public int NumRejeit { get; set; }

        [Column(Name = "NumLinha", DbType = "INT NOT NULL")]
        public int NumLinha { get; set; }

        #endregion

        #region Construtores

        public ACSOPRGCR_RLoteEN() { }

        #endregion

        #region Métodos

        public override string ToString()
        {
            return String.Concat( ACSOPRGCR_RLoteEN.TpRegistro,
                                    upSight.Consulta.Base.Sistema.CompletaEspacoDireita(this.CodPrgCrg, 10),
                                    Convert.ToInt32(this.StatusProc).ToString("000"),
                                    this.NumCart.ToString("000000"),
                                    this.NumRejeit.ToString("000000"),
                                    upSight.Consulta.Base.Sistema.CompletaEspacoDireita(String.Empty, 98),
                                    this.NumLinha.ToString("000000")
                                );
        }

        #endregion
    }
}

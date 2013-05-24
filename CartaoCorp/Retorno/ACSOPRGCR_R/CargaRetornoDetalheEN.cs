using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR_R
{
    public class CargaRetornoDetalheEN : DetalheRetornoBaseEN
    {
        #region Propriedades

        [Column(Name = "IdCRICrgtDet", DbType = "INT NOT NULL")]
        public int MyProperty { get; set; }

        [Column(Name = "Valor", DbType = "MONEY NOT NULL")]
        public decimal Valor { get; set; }

        #endregion

        #region Construtores

        public CargaRetornoDetalheEN() { }

        #endregion
    }
}

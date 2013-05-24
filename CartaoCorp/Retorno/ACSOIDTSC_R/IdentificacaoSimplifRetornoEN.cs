using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace upSight.CartaoCorp.Identificacao.ACSOIDTSC_R
{
    public class IdentificacaoSimplifRetornoEN : DetalheRetornoBaseEN
    {
        #region Propriedades

        [Column(Name = "IdCRIIdentDet", DbType = "INT NOT NULL")]
        public int IdCRIIdentDet { get; set; }

        [Column(Name = "Cpf", DbType = "VARCHAR(11) NOT NULL")]
        public string Cpf { get; set; }

        [Column(Name = "StatusCart", DbType = "TINYINT NOT NULL")]
        public upSight.CartaoCorp.EnumRetornoBase.StatusCartao StatusCart { get; set; }

        #endregion

        #region Construtores

        public IdentificacaoSimplifRetornoEN() { }

        #endregion

    }
}

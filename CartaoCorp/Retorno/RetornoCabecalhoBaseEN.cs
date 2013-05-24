using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;

namespace upSight.CartaoCorp
{
    public class RetornoCabecalhoBaseEN
    {
        #region Propriedades

        [Column(Name = "IdRetIdentCab", DbType = "INT NOT NULL")]
        public int IdRetIdentCab { get; set; }

        [Column(Name = "IdArquivo", DbType = "INT NOT NULL")]
        public int IdArquivo { get; set; }

        [Column(Name = "DataGeracao", DbType = "DATETIME2 NOT NULL")]
        public DateTime DataGeracao { get; set; }

        [Column(Name = "SeqArquivo", DbType = "TINYINT NOT NULL")]
        public byte SeqArquivo { get; set; }

        [Column(Name = "NomeArquivo", DbType = "VARCHAR(50) NOT NULL")]
        public string NomeArquivo { get; set; }

        [Column(Name = "CodConvenio", DbType = "VARCHAR(10) NOT NULL")]
        public string CodConvenio { get; set; }

        [Column(Name = "CodEmpresa", DbType = "VARCHAR(14) NOT NULL")]
        public string CodEmpresa { get; set; }

        [Column(Name = "NumLinha", DbType = "INT")]
        public int NumLinha { get; set; }

        #endregion
    }
}

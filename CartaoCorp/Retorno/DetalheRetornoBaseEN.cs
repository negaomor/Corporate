using System;
using System.Collections.Generic;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using upSight.CartaoCorp.Identificacao.ACSOIDTSC_R;

namespace upSight.CartaoCorp
{
    public class DetalheRetornoBaseEN
    {
        #region Propriedades

        [Column(Name = "IdArquivo", DbType = "INT NOT NULL")]
        public int IdArquivo { get; set; }

        [Column(Name = "TpIdentificacao", DbType = "TINYINT NOT NULL")]
        public EnumRetornoBase.TipoIdentificacao TpIdentificacao { get; set; }

        [Column(Name = "Identificacao", DbType = "VARCHAR(32) NOT NULL")]
        public string Identificacao { get; set; }

        [Column(Name = "StatusCart", DbType = "TINYINT NOT NULL")]
        public EnumRetornoBase.StatusCartao StatusCart { get; set; }

        [Column(Name = "StatusProc", DbType = "TINYINT NOT NULL")]
        public EnumRetornoBase.StatusProcessamento StatusProc { get; set; }

        [Column(Name = "Chave", DbType = "VARCHAR(15) NOT NULL")]
        public string Chave { get; set; }

        [Column(Name = "Retorno", DbType = "VARCHAR(50) NULL")]
        public string Retorno { get; set; }

        [Column(Name = "DtRetorno", DbType = "DATETIME2 NULL")]
        public DateTime? DtRetorno { get; set; }

        [Column(Name = "DtCriacao", DbType = "DATETIME2 NOT NULL")]
        public DateTime DtCriacao { get; set; }

        [Column(Name = "DtAlteracao", DbType = "DATETIME2 NOT NULL")]
        public DateTime DtAlteracao { get; set; }

        public string NomeArquivo { get; set; }

        #endregion


        ///testes
        ///testes
        
    }
}

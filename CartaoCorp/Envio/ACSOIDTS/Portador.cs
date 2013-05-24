using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public class Portador
    {
        public virtual string TpRegistro { get; set; }
        //= dr["TpPanProxy"].ToString();
        public virtual string TpIdentif { get; set; }
        //string panProxy = dr["PanProxy"].ToString();
        [Required]
        public virtual string Identificacao { get; set; }
        [Required]
        public virtual string CPF { get; set; }
        [Required]
        public virtual string Nome { get; set; }
        public virtual string NomeFacial { get; set; }

        public virtual DateTime? DtNascimento { get; set; }

        public virtual string Sexo { get; set; }

        public virtual string CnpjFilial { get; set; }
        public virtual string Grupo { get; set; }

        public virtual string Email { get; set; }

        public virtual string DDDCel { get; set; }
        public virtual string Celular { get; set; }
        public virtual string NomeMae { get; set; }
        public int? IdRegistro { get; set; }
        public virtual string CodConvenio { get; set; }
        public virtual int IdEntidade { get; set; }
    }


    public class MapaColunaPortador
    {
        public int TpIdentif { get; set; }
        public int Identificacao { get; set; }
        public int CPF { get; set; }
        public int Nome { get; set; }
        public int NomeFacial { get; set; }
        public int DtNascimento { get; set; }
        public int Sexo { get; set; }
        public int CnpjFilial { get; set; }
        public int Grupo { get; set; }
        public int Email { get; set; }
        public int DDDCel { get; set; }
        public int Celular { get; set; }
        public int NomeMae { get; set; }
        public int IdRegistro { get; set; }
        public int CodConvenio { get; set; }
        public int IdEntidade { get; set; }
    }
}

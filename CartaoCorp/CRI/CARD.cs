using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace upSight.CartaoCorp.CRI
{
    public class CARD
    {
        public ADDDETLIST ADDDETLIST { get; set; }
        public string ACCOUNTID { get; set; }
        public string ACTION { get; set; }
        public string ADDRIND { get; set; }
        public string ADDRL1 { get; set; }
        public string ADDRL2 { get; set; }
        public string ADDRL3 { get; set; }
        public string AMTLOAD { get; set; }
        public string BRNCODE { get; set; }
        public string CITY { get; set; }
        public string COUNTRY { get; set; }
        public string COUNTY { get; set; }
        public string CRDPRODUCT { get; set; }
        public string CURRCODE { get; set; }
        public string CUSTPROFILE { get; set; }
        public string DENYSVC { get; set; }
        public string DESIGNREF { get; set; }
        public string DLVMETHOD { get; set; }
        public string DOB { get; set; }
        public string FIRSTNAME { get; set; }
        public string INSTCODE { get; set; }
        public string ISOLANG { get; set; }
        public string LANG { get; set; }
        public string LASTNAME { get; set; }
        public string LOADTYPE { get; set; }
        public string EMAIL { get; set; }
        public string MAILSHOTS { get; set; }
        public string NEWACC { get; set; }
        public string PAN { get; set; }
        public string POSTCODE { get; set; }
        public string PRODUCEPIN { get; set; }
        public string PROGRAMID { get; set; }
        public string RENEW { get; set; }
        public string STATCODE { get; set; }
        public string TEL { get; set; }
        public string MOBTEL { get; set; }
        public string CRDPROFILE { get; set; }
        public string RECID { get; set; }
        public string FEETIER { get; set; }
        public string IDPARAMETRO { get; set; }

        //public CriBD() { }

    }
    public enum TpStatCode : byte
    {
        Solicitação = 70,
        Identificação = 71,
        Carga = 72
    }

    [Serializable]
    public class ADDDETLIST
    {
        [XmlElement]
        public List<ADDDET> ADDDET { get; set; }
    }

    [Serializable]
    public class ADDDET
    {
        public String REFCODE { get; set; }
        public String VALUE { get; set; }
    }
}

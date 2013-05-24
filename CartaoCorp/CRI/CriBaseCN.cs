using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace upSight.CartaoCorp.CRI
{
    public class CriBaseCN
    {
        /// <summary>
        /// Tipo da identificação
        /// </summary>
        public enum TipoIdentificacao : byte
        {
            PAN = 1,
            PROXY = 2
        }
        /// <summary>
        /// Serializa os dados a partir de uma lista de qualquer tipo de objeto
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="lstObjt"></param>
        /// <returns></returns>
        public static StringWriter SerializaDados<T>(List<T> lstObjt, string root = "CRDREQ")
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");
            StringWriter writer = new StringWriter();
            new XmlSerializer(typeof(List<T>), new XmlRootAttribute(root)).Serialize(writer, lstObjt, ns);

            return writer;
        }

        /// <summary>
        /// Gera o RecId
        /// </summary>
        /// <returns></returns>
        public static string GeraRecID()
        {
            byte[] buffer = Guid.NewGuid().ToByteArray();
            int intGuid = BitConverter.ToInt32(buffer, 0);
            intGuid = (intGuid < 0) ? intGuid * (-1) : intGuid;

            String strGuid = intGuid.ToString(); ;
            if (strGuid.Length > 20)
                strGuid = strGuid.Substring(0, 20);

            return strGuid;
        }

        /// <summary>
        /// Gera o arquivo CRI de identificação simplificada de cartão
        /// </summary>
        /// <param name="cartao">idntSimplifCrt => objeto serializado</param>
        public static void GeraArquivoFisicoCRI(string objSerializado, out string nomeArquivo)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(objSerializado);

            string nomeArq = GeraNomeArquivoCRI();
            XmlTextWriter writer = new XmlTextWriter(nomeArq, null);

            writer.Formatting = Formatting.Indented;
            doc.Save(writer);

            nomeArquivo = nomeArq;
        }

        private static string GeraNomeArquivoCRI()
        {
            string nomeArquivoCRI = null;
            for (int i = 1; i < 100; i++)
            {
                nomeArquivoCRI = System.IO.Path.Combine(ConfigurationManager.AppSettings["CRI_Sicilitacao_Envio_DiretorioOrigem"], String.Format("crdreq{0:yyMMdd}{1:00}.xml", DateTime.Now, i));
                if (!System.IO.File.Exists(nomeArquivoCRI)) break;
            }
            return nomeArquivoCRI;
        }

        public enum StatusCartao : byte
        {
            [Description("Ativo")]
            Ativo = 0,

            [Description("Bloqueado por tentativa de senha errada")]
            BloquieoTentavivaSenhaErrada = 01,

            [Description("Inativo")]
            Inativo = 02,

            [Description("Cartão expirado")]
            CartãoExpirado = 03,

            [Description("Perdido")]
            Perdido = 04,

            [Description("Roubado")]
            Roubado = 05,

            [Description("Cancelado a pedido do cliente")]
            CanceladoPedidoCliente = 06,

            [Description("Cancelado a pedido do emissor")]
            CanceladoPedidoEmissor = 07,

            [Description("Uso fraudulento")]
            UsoFraudulento = 08,

            [Description("Dormente")]
            Dormente = 09,

            [Description("Preventivo de fraude")]
            PreventivoFraude = 60,

            [Description("Definitivo de fraude")]
            DefinitivoFraude = 64,

            [Description("Cancelado")]
            Cancelado = 66,

            [Description("Não informado/não consultado")]
            NãoInformadoOuNãoConsultado = 99
        }
    }
}

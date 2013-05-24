using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Data;

using upSight.Consulta.Base;


namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public class crtACSOIDTSCDetalheEN
    {

        #region Propriedades

        public int IdArquivo { get; set; }
        public string TpRegistro { get; set; }
        public string TpPanProxy { get; set; }
        public string PanProxy { get; set; }
        public string Cpf { get; set; }
        public string Nome { get; set; }
        public string NomeFacial { get; set; }
        public DateTime? DtNascimento { get; set; }
        public string Sexo { get; set; }
        public string CnpjFilial { get; set; }
        public string Grupo { get; set; }
        public string Email { get; set; }
        public string DddCel { get; set; }
        public string Celular { get; set; }
        public string NomeMae { get; set; }
        public string IdRegistro { get; set; }
        public int NumLinha { get; set; }

        #endregion


        #region Construtores

        public crtACSOIDTSCDetalheEN() { }

        #endregion


        #region Métodos

        /// <summary>
        /// Parseia os dados de uma planilha de excel referente ao detalhe do arquivo e insere na base de dados
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        public crtACSOIDTSCDetalheEN MapeiaXLSDet(DataRow dr, int idArquivo, int numLinha)
        {
            try
            {
                var acsIdtsDet = new crtACSOIDTSCDetalheEN()
                {
                    TpRegistro = dr["TpRegistro"].ToString(),
                    TpPanProxy = dr["TpPanProxy"].ToString(),
                    PanProxy = dr["PanProxy"].ToString(),
                    Cpf = dr["CPF"].ToString(),
                    Nome = dr["Nome"].ToString(),
                    NomeFacial = dr["NomeFacial"].ToString(),
                    Sexo = dr["Sexo"].ToString(),
                    CnpjFilial = dr["CnpjFilial"].ToString(),
                    Grupo = dr["Grupo"].ToString(),
                    Email = dr["Email"].ToString(),
                    DddCel = dr["DDDCel"].ToString(),
                    Celular = dr["Celular"].ToString(),
                    NomeMae = dr["NomeMae"].ToString(),
                    IdRegistro = dr["IdRegistro"].ToString()
                };

                string dtNasc = dr["DtNascimento"].ToString();
                DateTime? dtNascimento = String.IsNullOrEmpty(dtNasc) ? (DateTime?)null : Data.ParseEstendido(dtNasc, Data.FormatoData.AAAAMMDD);

                return acsIdtsDet;
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Det", e });
                throw;
            }
        }

        /// <summary>
        /// Parseia os dados da linha referente ao detalhe do arquivo e insere na base de dados
        /// </summary>
        /// <param name="linha"></param>
        /// <param name="idArquivo"></param>
        public static crtACSOIDTSCDetalheEN Mapeia(string linha, int idArquivo)
        {
            try
            {
                var acsIdstDet = new crtACSOIDTSCDetalheEN()
                {
                    TpRegistro = linha.Substring(0, 1),
                    TpPanProxy = linha.Substring(1, 1),
                    PanProxy = linha.Substring(2, 32),
                    Cpf = linha.Substring(34, 11).TrimEnd(null),
                    Nome = linha.Substring(45, 50).TrimEnd(null),
                    NomeFacial = linha.Substring(95, 25).TrimEnd(null),
                    DtNascimento = String.IsNullOrEmpty(linha.Substring(120, 8).Trim()) ? (DateTime?)null : Data.ParseEstendido(linha.Substring(120, 8), Data.FormatoData.AAAAMMDD),
                    Sexo = linha.Substring(128, 1).TrimEnd(null),
                    CnpjFilial = linha.Substring(129, 14).TrimEnd(null),
                    Grupo = linha.Substring(143, 20).TrimEnd(null),
                    Email = linha.Substring(163, 30).TrimEnd(null),
                    DddCel = linha.Substring(193, 2).TrimEnd(null),
                    Celular = linha.Substring(195, 9).TrimEnd(null),
                    NomeMae = linha.Substring(204, 50).TrimEnd(null),
                    IdRegistro = linha.Substring(284, 10).TrimEnd(null),
                    NumLinha = Convert.ToInt32(linha.Substring(294, 6))
                };

                return acsIdstDet;
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Det", e });
                throw;
            }
        }

        public static DataRow MapeiaTXTDet(string linha, int idArquivo, string codConvenio, int idEntidade, DataTable dt)
        {
            if (!dt.Columns.Contains("TpIdentif"))
                dt.Columns.Add("TpIdentif");
            if (!dt.Columns.Contains("TpPanProxy"))
                dt.Columns.Add("TpPanProxy");
            if (!dt.Columns.Contains("Identificacao"))
                dt.Columns.Add("Identificacao");//PanProxy
            if (!dt.Columns.Contains("Cpf"))
                dt.Columns.Add("Cpf");
            if (!dt.Columns.Contains("Nome"))
                dt.Columns.Add("Nome");
            if (!dt.Columns.Contains("NomeFacial"))
                dt.Columns.Add("NomeFacial");
            if (!dt.Columns.Contains("DtNascimento"))
                dt.Columns.Add("DtNascimento");
            if (!dt.Columns.Contains("Sexo"))
                dt.Columns.Add("Sexo");
            if (!dt.Columns.Contains("CnpjFilial"))
                dt.Columns.Add("CnpjFilial");
            if (!dt.Columns.Contains("Grupo"))
                dt.Columns.Add("Grupo");
            if (!dt.Columns.Contains("Email"))
                dt.Columns.Add("Email");
            if (!dt.Columns.Contains("DddCel"))
                dt.Columns.Add("DddCel");
            if (!dt.Columns.Contains("Celular"))
                dt.Columns.Add("Celular");
            if (!dt.Columns.Contains("NomeMae"))
                dt.Columns.Add("NomeMae");
            if (!dt.Columns.Contains("IdRegistro"))
                dt.Columns.Add("IdRegistro");
            if (!dt.Columns.Contains("NumLinha"))
                dt.Columns.Add("NumLinha");
            if (!dt.Columns.Contains("CodConvenio"))
                dt.Columns.Add("CodConvenio");
            if (!dt.Columns.Contains("idEntidade"))
                dt.Columns.Add("idEntidade");


            DataRow dr = dt.NewRow();

            dr["TpIdentif"] = linha.Substring(0, 1);
            dr["TpPanProxy"] = linha.Substring(1, 1);
            dr["Identificacao"] = linha.Substring(2, 32).TrimEnd(null);//PanProxy
            dr["CPF"] = linha.Substring(34, 11).TrimEnd(null);
            dr["Nome"] = linha.Substring(45, 50).TrimEnd(null);
            dr["NomeFacial"] = linha.Substring(95, 25).TrimEnd(null);
            dr["DtNascimento"] = linha.Substring(120, 8);            
            dr["Sexo"] = linha.Substring(128, 1).TrimEnd(null);
            dr["CnpjFilial"] = linha.Substring(129, 14).TrimEnd(null);
            dr["Grupo"] = linha.Substring(143, 20).TrimEnd(null);
            dr["Email"] = linha.Substring(163, 30).TrimEnd(null);
            dr["DddCel"] = linha.Substring(193, 2).TrimEnd(null);
            dr["Celular"] = linha.Substring(195, 9).TrimEnd(null);
            dr["NomeMae"] = linha.Substring(204, 50).TrimEnd(null);
            dr["IdRegistro"] = linha.Substring(284, 10).TrimEnd(null);
            dr["NumLinha"] = Convert.ToInt32(linha.Substring(294, 6));
            dr["CodConvenio"] = codConvenio;
            dr["idEntidade"] = idEntidade;

            return dr;
        }


        #endregion
    }
}

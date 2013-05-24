using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Configuration;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

using upSight.Consulta.Base;
using BDGeral = upSight.Consulta.Base.BD.Geral;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public class PortadorCN
    {
        /// <summary>
        /// Procura por qualquer caractere não numérico
        /// </summary>
        private readonly string _ExpRglrNaoNumerico = @"\D";

        /// <summary>
        /// Validação de email
        /// </summary>
        private readonly string _ExpRglrEmail = @"\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*";

        /// <summary>
        ///Procura por qualquer caractere que não seja alfa-numérico 
        /// </summary>
        private readonly string _ExprRglrGrupo = @"\W";

        /*/// <summary>
        /// Procura por qualquer caractere que não esteja no alfabeto
        /// </summary>
        private readonly string _ExprRglrNomes = @"(?<nome>([^a-zA-Z]\s|\s[^a-zA-Z]\s|\s[^a-zA-Z])+)";*/

        private Regex _Rgx = null;

        public List<ValidationResult> Valida(Portador ptr)
        {
            try
            {
                List<ValidationResult> lstVr = lstVr = new List<ValidationResult>();
                //Validação PAN e Proxy
                this.ValidaPanProxy(ptr, lstVr);
                
                //Validação CPF
                var vldCpf = new upSight.Negocio.Calculo.ValidacaoCpf(ptr.CPF);
                if (!vldCpf.EhValido())
                    lstVr.Add(new ValidationResult(String.Concat("CPF inválido: ", ptr.CPF), new[] { "CPF" }));

                //Valida CNPJ
                if (!String.IsNullOrEmpty(ptr.CnpjFilial))
                {
                    var vldCnpj = new upSight.Negocio.Calculo.ValidacaoCnpj(ptr.CnpjFilial);
                    if (!vldCnpj.EhValido())
                        lstVr.Add(new ValidationResult(String.Concat("CnPj inválido: ", ptr.CnpjFilial), new[] { "CNPJ" }));
                }

                //Validação Data de Nascimento
                if (ptr.DtNascimento.HasValue)
                    if (ptr.DtNascimento.Value.Year < DateTime.Now.AddYears(-12).Year /*Validação para maiores de 12 anos*/ || ptr.DtNascimento > DateTime.Now)
                        lstVr.Add(new ValidationResult(String.Concat("Data de nascimento inválida: ", ptr.DtNascimento), new[] { "DtNascimento" }));


                //Validação email
                if (!String.IsNullOrEmpty(ptr.Email))
                {
                    this._Rgx = new Regex(this._ExpRglrEmail);
                    if (!this._Rgx.Match(ptr.Email).Success)
                        lstVr.Add(new ValidationResult(String.Concat("Email inválido: ", ptr.Email), new[] { "Email" }));
                }

                //Validação sexo
                if (!String.IsNullOrEmpty(ptr.Sexo))
                {
                    if (!ptr.Sexo.ToUpper().Substring(0, 1).Equals("M") && !ptr.Sexo.ToUpper().Substring(0, 1).Equals("F"))
                        lstVr.Add(new ValidationResult(String.Concat("Sexo inválido: ", ptr.Sexo), new[] { "Sexo" }));
                }

                //Validação grupo
                if (!String.IsNullOrEmpty(ptr.Grupo))
                {
                    this._Rgx = new Regex(this._ExprRglrGrupo);
                    if (!this._Rgx.Match(ptr.Grupo).Success)
                    {
                        if (ptr.Grupo.Length > 20)
                            lstVr.Add(new ValidationResult(String.Concat("Grupo contém quantidade de caracteres maior que 20: ", ptr.Grupo), new[] { "Grupo" }));
                    }
                    else
                        lstVr.Add(new ValidationResult(String.Concat("Grupo contém caracteres inválidos: ", ptr.Grupo), new[] { "Grupo" }));
                }

                //Valida nome e nome da mãe
                this.ValidaNomes(ptr.Nome, ptr.NomeMae, lstVr);

                //Valida telefone celular
                this.ValidaTelefone(ptr.DDDCel, ptr.Celular, lstVr);

                return lstVr;
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }
        }

        /// <summary>
        /// Valida Nome e Nome da Mãe
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="nomeMae"></param>
        /// <param name="lstVr"></param>
        private void ValidaNomes(string nome, string nomeMae, List<ValidationResult> lstVr)
        {
            try
            {
                //this._Rgx = new Regex(this._ExprRglrNomes);

                if (nome.All(c => Char.IsLetter(c) || c == ' '))
                {
                    if (nome.Length > 40)
                        lstVr.Add(new ValidationResult(String.Concat("Nome contém número de caractere maior que 40: ", nome), new[] { "Nome" }));
                }
                else
                    lstVr.Add(new ValidationResult(String.Concat("Nome contém caractere inválido: ", nome), new[] { "Nome" }));

                if (nomeMae.All(c => Char.IsLetter(c) || c == ' '))
                {
                    if (nomeMae.Length > 40)
                        lstVr.Add(new ValidationResult(String.Concat("Nome da mãe contém número de caractere maior que 40: ", nomeMae), new[] { "NomeMae" }));
                }
                else
                    lstVr.Add(new ValidationResult(String.Concat("Nome da mãe contém caractere inválido: ", nomeMae), new[] { "NomeMae" }));
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }
        }

        /// <summary>
        /// Valida PAN(1) ou Proxy(2) 
        /// </summary>
        /// <param name="tpIdentif"></param>
        /// <param name="Identificacao"></param>
        /// <returns></returns>
        private void ValidaPanProxy(Portador ptr , List<ValidationResult> lstVr)
        {
            try
            {
                this._Rgx = new Regex(this._ExpRglrNaoNumerico);

                if (!this._Rgx.Match(ptr.Identificacao).Success)
                {

                    if (this.ValidaExistenciaPanProxy(ptr))
                    {
                        switch (ptr.TpIdentif)
                        {
                            case "1":
                                if (!ptr.Identificacao.Length.Equals(16))
                                    lstVr.Add(new ValidationResult(String.Concat("Identificacao com quantidade de caracteres divergentes a 16: ", ptr.Identificacao), new[] { "Identificacao" }));
                                break;

                            case "2":
                                if (ptr.Identificacao.Length < 8 || ptr.Identificacao.Length > 32)
                                    lstVr.Add(new ValidationResult(String.Concat("Identificacao com quantidade de caracteres divergentes( menor que 8 ou maior que 32): ", ptr.Identificacao), new[] { "Identificacao" }));
                                break;
                        }
                    }
                    else
                    {
                        lstVr.Add(new ValidationResult(String.Concat("PanProx inexistente ou convenio não compatível. ", ptr.Identificacao), new[] { "Identificacao" }));                     
                    }
                }
                else
                    lstVr.Add(new ValidationResult(String.Concat("Identificacao com caracteres alfa-numéricos: ", ptr.Identificacao), new[] { "Identificacao" }));
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }
        }

        private bool ValidaExistenciaPanProxy(Portador ptr)
        {
            bool vExist = false;

            try
            {
                vExist = PortadorBD.ConsultaCartoes(ptr);
            }

            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }

            return vExist;
        }

        /// <summary>
        /// Valida o número de telefone
        /// </summary>
        /// <param name="dddCel"></param>
        /// <param name="celular"></param>
        private void ValidaTelefone(string dddCel, string celular, List<ValidationResult> lstVr)
        {
            const string dddCelSP = "11";
            try
            {
                this._Rgx = new Regex(this._ExpRglrNaoNumerico);
                if (!this._Rgx.Match(dddCel).Success && !this._Rgx.Match(celular).Success)
                {
                    if (dddCel.Equals(dddCelSP) && celular.Length == 8)
                        lstVr.Add(new ValidationResult(String.Concat("Número do telefone incorreto. Falta inclusão do sufixo 9", celular), new[] { "telefone" }));

                }
                else
                    lstVr.Add(new ValidationResult(String.Concat("DDD Celular ou Número de telefone contém caracteres alfa-numérico.", dddCel), new[] { "telefone" }));
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }
        }


        public MapaColunaPortador CriaMapaColuna(DataRow dr)
        {
            DataColumnCollection dcc = dr.Table.Columns;
            MapaColunaPortador mcp = new MapaColunaPortador()
            {
                TpIdentif = -1,
                Identificacao = -1,
                CPF = -1,
                Nome = -1,
                NomeFacial = -1,
                DtNascimento = -1,
                Sexo = -1,
                CnpjFilial = -1,
                Grupo = -1,
                Email = -1,
                DDDCel = -1,
                Celular = -1,
                NomeMae = -1,
                IdRegistro = -1
            };

            if (dcc.Contains("TpIdentif"))
                mcp.TpIdentif = dcc["TpIdentif"].Ordinal;
            if (dcc.Contains("Identificacao"))
                mcp.Identificacao = dcc["Identificacao"].Ordinal;
            if (dcc.Contains("CPF"))
                mcp.CPF = dcc["CPF"].Ordinal;
            if (dcc.Contains("Nome"))
                mcp.Nome = dcc["Nome"].Ordinal;
            if (dcc.Contains("NomeFacial"))
                mcp.NomeFacial = dcc["NomeFacial"].Ordinal;
            if (dcc.Contains("DtNascimento"))
                mcp.DtNascimento = dcc["DtNascimento"].Ordinal;
            if (dcc.Contains("Sexo"))
                mcp.Sexo = dcc["Sexo"].Ordinal;
            if (dcc.Contains("CnpjFilial"))
                mcp.CnpjFilial = dcc["CnpjFilial"].Ordinal;
            if (dcc.Contains("Grupo"))
                mcp.Grupo = dcc["Grupo"].Ordinal;
            if (dcc.Contains("Email"))
                mcp.Email = dcc["Email"].Ordinal;
            if (dcc.Contains("DDDCel"))
                mcp.DDDCel = dcc["DDDCel"].Ordinal;
            if (dcc.Contains("Celular"))
                mcp.Celular = dcc["Celular"].Ordinal;
            if (dcc.Contains("NomeMae"))
                mcp.NomeMae = dcc["NomeMae"].Ordinal;
            if (dcc.Contains("IdRegistro"))
                mcp.IdRegistro = dcc["IdRegistro"].Ordinal;
            return mcp;
        }
    }

    public static class PortadorCNExtensao
    {
        /// <summary>
        /// Mapeia a partir da leitura Excel
        /// </summary>
        /// <param name="port"></param>
        /// <param name="dr"></param>
        public static List<ValidationResult> Mapeia(this Portador port, DataRow dr, MapaColunaPortador mapa)
        {
            List<ValidationResult> resultValid = new List<ValidationResult>();
            try
            {
                const string tpRegistro = "1";

                port.TpRegistro = tpRegistro;

                //tpPanProxy
                if (mapa.TpIdentif > -1)
                    port.TpIdentif = dr[mapa.TpIdentif].ToString();
                else
                    port.TpIdentif = tpRegistro;

                //Obrigatórios
                //PanProxy
                if (mapa.Identificacao > -1)
                    port.Identificacao = dr[mapa.Identificacao].ToString();
                else
                    resultValid.Add(new ValidationResult("Identificação deve ser preenchida.", new[] { "Identificacao" }));

                if (mapa.CPF > -1)
                    port.CPF = dr[mapa.CPF].ToString().Replace(".", "").Replace("-", "").PadLeft(11, '0');
                else
                    resultValid.Add(new ValidationResult("CPF deve ser preenchido.", new[] { "CPF" }));

                if (mapa.Nome > -1)
                    port.Nome = dr[mapa.Nome].ToString().Replace(".", "");
                else
                    resultValid.Add(new ValidationResult("Nome deve ser preenchido.", new[] { "Nome" }));

                port.NomeFacial = dr["NomeFacial"].ToString();


                string dtNasc = String.IsNullOrEmpty(dr["DtNascimento"].ToString()) ? string.Empty : dr["DtNascimento"].ToString().Replace("/", "").Substring(0, 8);

                DateTime deliveredDate;
                if (Data.TentaParseEstendido(dtNasc, Data.FormatoData.AAAAMMDD, out deliveredDate))
                    port.DtNascimento = String.IsNullOrEmpty(dtNasc) ? (DateTime?)null : Data.ParseEstendido(dtNasc, Data.FormatoData.AAAAMMDD);
                else
                    resultValid.Add(new ValidationResult(String.Concat("Formato de data inválida:", dtNasc) , new[] { "DtNascimento" }));

                port.Sexo = dr["Sexo"].ToString();
                port.CnpjFilial = dr["CnpjFilial"].ToString().Replace(".", "").Replace("/", "").Replace("-", "");
                port.Grupo = dr["Grupo"].ToString();
                port.Email = dr["Email"].ToString();
                port.DDDCel = dr["DDDCel"].ToString().Replace("(", "").Replace(")", "");
                port.Celular = dr["Celular"].ToString().Replace("-", "").Replace(" ", "");
                port.NomeMae = dr["NomeMae"].ToString().Replace(".", "");

                string idRegistro = dr["IdRegistro"].ToString();

                port.IdRegistro = String.IsNullOrEmpty(idRegistro) ? (int?)null : Convert.ToInt32(idRegistro);

                port.CodConvenio = dr["codConvenio"].ToString();
                port.IdEntidade = Convert.ToInt32(dr["idEntidade"]);

            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }

            return resultValid;
        }
    }
}

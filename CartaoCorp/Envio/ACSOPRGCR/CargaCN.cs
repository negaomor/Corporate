using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using upSight.CartaoCorp.Identificacao.ACSOIDTS;

namespace upSight.CartaoCorp.Carga.ACSOPRGCR
{
    public class CargaCN
    {
        /// <summary>
        /// Procura por qualquer caractere não numérico
        /// </summary>
        private readonly string _ExpRglrNumerico = @"\D";

        /// <summary>
        ///Procura por qualquer caractere que não seja alfa-numérico 
        /// </summary>
        private readonly string _ExprRglrRegistro = @"\W";

        private readonly string _ExpRglrValor = @"\d+(\.\d\d)?";

        private Regex _Rgx = null;

        /// <summary>
        /// Valida os valores dos campos
        /// </summary>
        /// <param name="prgCrDet"></param>
        /// <returns></returns>
        public List<ValidationResult> Valida(ACSOPRGCRDetalheEN prgCrDet)
        {
            try
            {
                List<ValidationResult> lstVr = lstVr = new List<ValidationResult>();
                //Validação PAN e Proxy
                this.ValidaPanProxy(prgCrDet, lstVr);

                this.ValidaLimiteCliente(prgCrDet, lstVr);

                if (!String.IsNullOrEmpty(prgCrDet.IdRegistro))
                {
                    this._Rgx = new Regex(this._ExprRglrRegistro);

                    if (!this._Rgx.Match(prgCrDet.IdRegistro).Success)
                    {
                        if (prgCrDet.IdRegistro.Length > 10)
                            lstVr.Add(new ValidationResult(String.Concat("IdRegistro contém quantidade de caracteres maior que 10: ", prgCrDet.IdRegistro), new[] { "IdRegistro" }));
                    }
                    else
                        lstVr.Add(new ValidationResult(String.Concat("IdRegistro contém caracteres inválidos: ", prgCrDet.IdRegistro), new[] { "IdRegistro" }));
                }

                this._Rgx = new Regex(this._ExpRglrValor);
                if(!this._Rgx.Match(prgCrDet.Valor.ToString()).Success)
                    lstVr.Add(new ValidationResult(String.Concat("Valor somente caractere numérico: ", prgCrDet.IdRegistro), new[] { "Valor" }));

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
        /// Valida PAN(1) ou Proxy(2) 
        /// </summary>
        /// <param name="tpIdentif"></param>
        /// <param name="Identificacao"></param>
        /// <returns></returns>
        private void ValidaPanProxy(ACSOPRGCRDetalheEN prgCrDet, List<ValidationResult> lstVr)
        {
            try
            {
                this._Rgx = new Regex(this._ExpRglrNumerico);

                if (!this._Rgx.Match(prgCrDet.Identificacao).Success)
                {
                    if (this.ValidaExistenciaPanProxy(prgCrDet))
                    {
                        switch (prgCrDet.TpPanProxy)
                        {
                            case TipoPanProxy.PAN:
                                if (!prgCrDet.Identificacao.Length.Equals(16))
                                    lstVr.Add(new ValidationResult(String.Concat("Identificacao com quantidade de caracteres divergentes a 16: ", prgCrDet.Identificacao), new[] { "Identificacao" }));
                                break;

                            case TipoPanProxy.Proxy:
                                if (prgCrDet.Identificacao.Length < 8 || prgCrDet.Identificacao.Length > 32)
                                    lstVr.Add(new ValidationResult(String.Concat("Identificacao com quantidade de caracteres divergentes( menor que 8 ou maior que 32): ", prgCrDet.Identificacao), new[] { "Identificacao" }));
                                break;
                        }
                    }
                }
                else
                    lstVr.Add(new ValidationResult(String.Concat("Identificacao com caracteres alfa-numéricos: ", prgCrDet.Identificacao), new[] { "Identificacao" }));
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }
        }

        /// <summary>
        /// Valida se existe os cartões na base
        /// </summary>
        /// <param name="ptr"></param>
        /// <returns></returns>
        private bool ValidaExistenciaPanProxy(ACSOPRGCRDetalheEN prgCrDet)
        {
            bool vExist = false;

            try
            {
                vExist = ACSOPRGCRDetalheDB.ConsultaCartoes(prgCrDet);
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
        /// 
        /// </summary>
        /// <param name="prgCrDet"></param>
        /// <param name="lstVr"></param>
        private void ValidaLimiteCliente(ACSOPRGCRDetalheEN prgCrDet, List<ValidationResult> lstVr)
        {
            try
            {
                var limite = ACSOPRGCRDetalheDB.ConsultaLimiteCliente(prgCrDet.IdEntidade);

                if (limite.ValMaxCredito < prgCrDet.Valor || limite.ValMinCredito > prgCrDet.Valor)
                {
                    lstVr.Add(new ValidationResult(String.Concat("Valores de limites fora do range de aprovação.", prgCrDet.Valor), new[] { "Valor" }));
                }                              
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOIDTS.PtrCN", e });
                throw;
            }
        }

        /// <summary>
        /// Verifica se as colunas existem
        /// </summary>
        /// <param name="dr"></param>
        /// <returns></returns>
        public MapaColunaCrgDetalhe CriaMapaColuna(DataRow dr)
        {
            DataColumnCollection dcc = dr.Table.Columns;
            MapaColunaCrgDetalhe mcCrgDet = new MapaColunaCrgDetalhe()
            {
                CodPrgCrg = -1,
                TpIdentif = -1,
                Identificacao = -1,
                Valor = -1,
                IdRegistro = -1
            };

            if (dcc.Contains("CodPrgCrg"))
                mcCrgDet.CodPrgCrg = dcc["CodPrgCrg"].Ordinal;
            if (dcc.Contains("TpIdentif"))
                mcCrgDet.TpIdentif = dcc["TpIdentif"].Ordinal;
            if (dcc.Contains("Identificacao"))
                mcCrgDet.Identificacao = dcc["Identificacao"].Ordinal;
            if (dcc.Contains("Valor"))
                mcCrgDet.Valor = dcc["Valor"].Ordinal;
            if(dcc.Contains("IdRegistro"))
                mcCrgDet.IdRegistro = dcc["IdRegistro"].Ordinal;

            return mcCrgDet;
        }
    }

    public static class ExtensaoCrgDetalhe
    {
        
        /// <summary>
        /// Mapeia a partir da leitura Excel
        /// </summary>
        /// <param name="port"></param>
        /// <param name="dr"></param>
        public static List<ValidationResult> Mapeia(this ACSOPRGCRDetalheEN detCrgEn, DataRow dr, MapaColunaCrgDetalhe mapa)
        {
            List<ValidationResult> resultValid = new List<ValidationResult>();
            try
            {
                //Obrigatórios
                //PanProxy
                if (mapa.TpIdentif > -1)
                    detCrgEn.TpPanProxy = (TipoPanProxy)Convert.ToByte(dr[mapa.TpIdentif].ToString());
                else
                    resultValid.Add(new ValidationResult("Tipo da Identificação deve ser preenchida.", new[] { "TpIdentif" }));
                
                if (mapa.Identificacao > -1)
                    detCrgEn.Identificacao = dr[mapa.Identificacao].ToString();
                else
                    resultValid.Add(new ValidationResult("Identificação deve ser preenchida.", new[] { "Identificacao" }));

                if (mapa.CodPrgCrg > -1)
                    detCrgEn.CodPrgCrg = dr[mapa.CodPrgCrg].ToString();
                else
                    resultValid.Add(new ValidationResult("Código do programa de carga.", new[] { "CodPrgCrg" }));

                if (mapa.Identificacao > -1)
                    detCrgEn.PanProxy = dr[mapa.Identificacao].ToString();
                else
                    resultValid.Add(new ValidationResult("Pan/Proxy deve ser preenchida.", new[] { "Pan/Proxy" }));

                if (mapa.Valor > -1)
                    detCrgEn.Valor = Convert.ToDecimal(dr[mapa.Valor].ToString());
                else resultValid.Add(new ValidationResult("Valor da carga deve ser preenchido.", new[] { "Valor" }));

                string idRegistro = dr["IdRegistro"].ToString();
                detCrgEn.IdRegistro = (!String.IsNullOrEmpty(idRegistro)) ? idRegistro : null;

                detCrgEn.IdEntidade = dr["idEntidade"].ToString();
            }
            catch (Exception e)
            {
                if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                    Trace.TraceError("{0}: {1}", new object[] { "u.CrtCorp.ACSOPRGCR.PtrCN", e });
                throw;
            }

            return resultValid;
        }
    }
}

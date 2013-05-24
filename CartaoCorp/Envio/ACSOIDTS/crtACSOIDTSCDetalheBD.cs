using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BDGeral = upSight.Consulta.Base.BD.Geral;

namespace upSight.CartaoCorp.Identificacao.ACSOIDTS
{
    public static class crtACSOIDTSCDetalheBD
    {
        /// <summary>
        /// Insere os dados em crtACSOIDTSCDetalhe
        /// </summary>
        /// <param name="acsIdstDet"></param>
        public static void Insere(this crtACSOIDTSCDetalheEN acsIdstDet)
        {
            using (SqlConnection cnx = new SqlConnection(ConfigurationManager.ConnectionStrings["Global"].ConnectionString))
            {
                try
                {
                    string query = " INSERT [crtACSOIDTSCDetalhe] " +
                               " (IdArquivo, TpRegistro, TpPanProxy, PanProxy, CPF, Nome, NomeFacial, DtNascimento, Sexo, CnpjFilial, " +
                               " Grupo, Email, DDDCel, Celular, NomeMae, IdRegistro, NumLinha) " +
                               " SELECT @IdArquivo, @TpRegistro, @TpPanProxy, @PanProxy, @CPF, @Nome, @NomeFacial, @DtNascimento, @Sexo, @CnpjFilial, " +
                               " @Grupo, @Email, @DDDCel, @Celular, @NomeMae, @IdRegistro, @NumLinha ";

                    using (SqlCommand cmd = new SqlCommand(query, cnx))
                    {
                        cmd.CommandType = CommandType.Text;

                        cmd.Parameters.Add("IdArquivo", SqlDbType.Int).Value = acsIdstDet.IdArquivo;
                        cmd.Parameters.Add("TpRegistro", SqlDbType.Char, 1).Value = acsIdstDet.TpRegistro;
                        cmd.Parameters.Add("TpPanProxy", SqlDbType.Char, 1).Value = acsIdstDet.TpPanProxy;
                        cmd.Parameters.Add("PanProxy", SqlDbType.VarChar, 32).Value = acsIdstDet.PanProxy;
                        cmd.Parameters.Add("CPF", SqlDbType.VarChar, 11).Value = acsIdstDet.Cpf;
                        cmd.Parameters.Add("Nome", SqlDbType.VarChar, 50).Value = acsIdstDet.Nome;
                        cmd.Parameters.Add("NomeFacial", SqlDbType.VarChar, 25).Value = BDGeral.BDObtemValor(acsIdstDet.NomeFacial);
                        cmd.Parameters.Add("DtNascimento", SqlDbType.Date).Value = BDGeral.BDObtemValor<DateTime>(acsIdstDet.DtNascimento);
                        cmd.Parameters.Add("Sexo", SqlDbType.Char, 1).Value = BDGeral.BDObtemValor(acsIdstDet.Sexo);
                        cmd.Parameters.Add("CnpjFilial", SqlDbType.VarChar, 14).Value = BDGeral.BDObtemValor(acsIdstDet.CnpjFilial);
                        cmd.Parameters.Add("Grupo", SqlDbType.VarChar, 20).Value = BDGeral.BDObtemValor(acsIdstDet.Grupo);
                        cmd.Parameters.Add("Email", SqlDbType.VarChar, 30).Value = BDGeral.BDObtemValor(acsIdstDet.Email);
                        cmd.Parameters.Add("DDDCel", SqlDbType.VarChar, 2).Value = BDGeral.BDObtemValor(acsIdstDet.DddCel);
                        cmd.Parameters.Add("Celular", SqlDbType.VarChar, 9).Value = BDGeral.BDObtemValor(acsIdstDet.Celular);
                        cmd.Parameters.Add("NomeMae", SqlDbType.VarChar, 50).Value = BDGeral.BDObtemValor(acsIdstDet.NomeMae);
                        cmd.Parameters.Add("IdRegistro", SqlDbType.VarChar, 10).Value = BDGeral.BDObtemValor(acsIdstDet.IdRegistro);
                        cmd.Parameters.Add("NumLinha", SqlDbType.Int).Value = acsIdstDet.NumLinha;

                        cnx.Open();

                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Exception sqlExc)
                {
                    if (upSight.Consulta.Base.BD.Geral.TS.TraceError)
                        Trace.TraceError("{0}: {1}", new object[] { "u.TISC.TISC.Det", sqlExc });
                    throw;
                }
            }
        }
    }
}

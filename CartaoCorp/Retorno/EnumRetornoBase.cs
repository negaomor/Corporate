using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace upSight.CartaoCorp
{
    public class EnumRetornoBase
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
        /// Status do processamento
        /// </summary>
        public enum StatusProcessamento : int
        {
            [Description("Sucesso")]
            Sucesso = 0,

            [Description("Cartão não existente")]
            CartãoNãoExistente = 100,

            [Description("Cartão cancelado")]
            CartãoCancelado = 101,

            [Description("Cartão não permitido")]
            CartãoNãoPermitido = 102,

            [Description("Cartão em prevenção")]
            CartãoEmPrevenção = 103,

             [Description("Status não permitido")]
            StatusNãoPermitido = 104,

            [Description("Erro na identificação")]
            ErroNaIdentificação = 200,

            OperaçãoNãoPermitida = 201,

            [Description("Dado de campo inconsistente")]
            DadoDeCampoInconsistente = 210,

            [Description("Programação não realizada")]
            ProgramaçãoNãoRealizada = 300,

            [Description("Programação realizada parcialmente")]
            ProgramaçãoRealizadaParcialmente = 301,

            [Description("Data de agendamento passada")]
            DataDeAgendamentoPassada = 302,

            [Description("Erro genérico")]
            ErroGenérico = 909
        }


        /// <summary>
        /// Status do Cartao
        /// </summary>
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

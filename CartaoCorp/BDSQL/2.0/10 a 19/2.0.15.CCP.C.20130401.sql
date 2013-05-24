------------------------------------------------------------------------------------------------------
--	
--	2.0.15.CCP.C.20130401.sql
--  
--	crpCRIIdentificacaoDetalhe: criação de tabela
--  Data: 01/04/2013		Autor: rafaelrau
--	Versão:	2.0.15.CCP.C.20130401
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.15.CCP.C.20130401',
		@descricao = 'crpCRIIdentificacaoDetalhe: criação de tabela',
		@idUser = 1,
		@idMigracao = SUBSTRING(@versao, 5, 2),
		@major = LEFT(@versao, 3),
		@modulo = SUBSTRING(@versao, 8, 3)

SELECT @dtVersao = Cast(RIGHT(@versao,  8) as datetime)

INSERT	admVersao
(idMigracao, modulo, idUser, dtVersao, majorVersion, minorVersion, versaoCompleta, desAlteracao, dtAlteracao, idUserAlteracao) 
SELECT @idMigracao, @modulo, @idUser, @dtVersao, @major, @idMigracao, @versao, @descricao, GetDate(), 0

COMMIT

PRINT @descricao
GO

/****** Object:  Table [dbo].[crpCRIIdentificacaoDetalhe]    Script Date: 04/03/2013 10:51:37 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crpCRIIdentificacaoDetalhe](
	[IdCRIIdentDet] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpIdentificacao] [tinyint] NOT NULL,
	[Identificacao] [varchar](32) NOT NULL,
	[Cpf] [varchar](11) NOT NULL,
	[StatusCart] [tinyint] NOT NULL,
	[Chave] [varchar](15) NOT NULL,
	[Retorno] [varchar](50) NULL,
	[DtRetorno] [datetime2](7) NULL,
	[DtCriacao] [datetime2](7) NOT NULL,
	[DtAlteracao] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_crpCRIIdentificacaoCargaDetalhe] PRIMARY KEY CLUSTERED 
(
	[IdCRIIdentDet] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[crpCRIIdentificacaoDetalhe] ADD  CONSTRAINT [DF_crpCRIIdentificacaoDetalhe_DtCiracao]  DEFAULT (sysdatetime()) FOR [DtCriacao]
GO

ALTER TABLE [dbo].[crpCRIIdentificacaoDetalhe] ADD  CONSTRAINT [DF_crpCRIIdentificacaoDetalhe_DtAlteracao]  DEFAULT (sysdatetime()) FOR [DtAlteracao]
GO



PRINT 'Fim da alteração'
GO


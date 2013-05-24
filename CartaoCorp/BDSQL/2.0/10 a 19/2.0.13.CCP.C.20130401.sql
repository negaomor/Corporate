------------------------------------------------------------------------------------------------------
--	
--	2.0.13.CCP.C.20130401.sql
--  
--	crpACSOIDTSC_RDetalhe: criação de tabela
--  Data: 01/04/2013		Autor: rafaelrau
--	Versão:	2.0.13.CCP.C.20130401
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.13.CCP.C.20130401',
		@descricao = 'crpACSOIDTSC_RDetalhe: criação de tabela',
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

/****** Object:  Table [dbo].[crpACSOIDTSC_RDetalhe]    Script Date: 04/01/2013 17:52:21 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crpACSOIDTSC_RDetalhe](
	[IdRetIdentDet] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpRegistro] [char](1) NOT NULL,
	[TpPanProxy] [tinyint] NOT NULL,
	[PanProxy] [varchar](32) NOT NULL,
	[Cpf] [varchar](11) NOT NULL,
	[DataProc] [datetime2](7) NOT NULL,
	[StatusProc] [int] NOT NULL,
	[StatusCart] [tinyint] NULL,
	[Descricao] [varchar](50) NULL,
	[IdRegistro] [varchar](10) NULL,
	[NumLinha] [int] NOT NULL,
 CONSTRAINT [PK_crpACSOIDTSC_RDetalhe] PRIMARY KEY CLUSTERED 
(
	[IdRetIdentDet] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



PRINT 'Fim da alteração'
GO


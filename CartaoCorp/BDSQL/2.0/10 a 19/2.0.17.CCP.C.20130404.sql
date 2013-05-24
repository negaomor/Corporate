------------------------------------------------------------------------------------------------------
--	
--	2.0.17.CCP.C.20130404.sql
--  
--	crpACSOPRGCR_RCabecalho: criação de tabela
--  Data: 01/04/2013		Autor: rafaelrau
--	Versão:	2.0.17.CCP.C.20130404
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.17.CCP.C.20130404',
		@descricao = 'crpACSOPRGCR_RCabecalho: criação de tabela',
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

/****** Object:  Table [dbo].[crpACSOPRGCR_RCabecalho]    Script Date: 04/09/2013 11:40:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crpACSOPRGCR_RCabecalho](
	[IdPrgCrgRetCab] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpRegistro] [char](1) NOT NULL,
	[NomeLayout] [varchar](20) NOT NULL,
	[Versao] [varchar](8) NOT NULL,
	[DataGeracao] [datetime2](7) NOT NULL,
	[SeqArquivo] [tinyint] NOT NULL,
	[NomeArquivo] [varchar](50) NOT NULL,
	[CodConvenio] [varchar](10) NOT NULL,
	[CodEmpresa] [varchar](14) NOT NULL,
	[NumLinha] [int] NOT NULL,
 CONSTRAINT [PK_ACSOPRGCR_RCabecalho] PRIMARY KEY CLUSTERED 
(
	[IdPrgCrgRetCab] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO



PRINT 'Fim da alteração'
GO


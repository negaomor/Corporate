------------------------------------------------------------------------------------------------------
--	
--	2.0.07.CCP.C.20130307
--  
--	crtACSOPRGCRCabecalho: criação de tabela
--  Data: 07/03/2013		Autor: rafaelrau
--	Versão:	2.0.07.CCP.C.20130307
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.07.CCP.C.20130307',
		@descricao = 'crtACSOPRGCRCabecalho: criação de tabela',
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

/****** Object:  Table [dbo].[crtACSOPRGCRCabecalho]    Script Date: 03/07/2013 13:50:30 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crtACSOPRGCRCabecalho](
	[IdCabecalho] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpRegistro] [char](1) NOT NULL,
	[NomeLayout] [varchar](20) NOT NULL,
	[Versao] [varchar](8) NOT NULL,
	[DataGeracao] [datetime] NOT NULL,
	[SeqArquivo] [tinyint] NOT NULL,
	[NomeArquivo] [varchar](50) NOT NULL,
	[CodConvenio] [varchar](10) NOT NULL,
	[CodEmpresa] [varchar](14) NOT NULL,
	[NumLinha] [int] NOT NULL,
 CONSTRAINT [PK_crtACSOPRGCRCabecalho] PRIMARY KEY CLUSTERED 
(
	[IdCabecalho] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


PRINT 'Fim da alteração'
GO



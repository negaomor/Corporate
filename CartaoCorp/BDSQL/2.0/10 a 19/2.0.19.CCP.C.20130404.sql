------------------------------------------------------------------------------------------------------
--	
--	2.0.19.CCP.C.20130404.sql
--  
--	crpACSOPRGCR_RDetalhe: criação de tabela
--  Data: 01/04/2013		Autor: rafaelrau
--	Versão:	2.0.18.CCP.C.20130404
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.19.CCP.C.20130404',
		@descricao = 'crpACSOPRGCR_RDetalhe: criação de tabela',
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

/****** Object:  Table [dbo].[crpACSOPRGCR_RDetalhe]    Script Date: 04/04/2013 17:04:03 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crpACSOPRGCR_RDetalhe](
	[IdPrgCrgRetDetalhe] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpRegistro] [char](1) NOT NULL,
	[TpPanProxy] [tinyint] NOT NULL,
	[PanProxy] [varchar](32) NOT NULL,
	[StatusProc] [int] NOT NULL,
	[StatusCart] [tinyint] NULL,
	[Descricao] [varchar](50) NULL,
	[IdRegistro] [varchar](10) NULL,
	[NumLinha] [int] NOT NULL,
 CONSTRAINT [PK_crpACSOPRGCR_RDetalhe] PRIMARY KEY CLUSTERED 
(
	[IdPrgCrgRetDetalhe] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

PRINT 'Fim da alteração'
GO


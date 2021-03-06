﻿------------------------------------------------------------------------------------------------------
--	
--	1.0.06.CCP.C.20130307
--  
--	crtACSOPRGCRDetalhe: criação de tabela
--  Data: 07/03/2013		Autor: rafaelrau
--	Versão:	1.0.06.CCP.C.20130307
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '1.0.06.CCP.C.20130307',
		@descricao = 'crtACSOPRGCRDetalhe: criação de tabela',
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

/****** Object:  Table [dbo].[crtACSOPRGCRDetalhe]    Script Date: 03/07/2013 13:48:58 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crtACSOPRGCRDetalhe](
	[IdDetalhe] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpRegistro] [char](1) NOT NULL,
	[CodPrgCrg] [varchar](10) NOT NULL,
	[TpPanProxy] [tinyint] NOT NULL,
	[PanProxy] [varchar](32) NOT NULL,
	[Valor] [money] NOT NULL,
	[IdRegistro] [varchar](10) NULL,
	[NumLinha] [int] NOT NULL,
 CONSTRAINT [PK_crtACSOPRGCRDetalhe] PRIMARY KEY CLUSTERED 
(
	[IdDetalhe] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


PRINT 'Fim da alteração'
GO



------------------------------------------------------------------------------------------------------
--	
--	1.0.02.CCP.C.20130222
--  
--	crtACSOIDTSCDetalhe: criação de tabela
--  Data: 22/02/2013		Autor: rafaelrau
--	Versão:	1.0.02.CCP.C.20130222
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '1.0.02.CCP.C.20130222',
		@descricao = 'crtACSOIDTSCDetalhe: criação de tabela',
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

/****** Object:  Table [dbo].[crtACSOIDTSCDetalhe]    Script Date: 03/07/2013 13:40:46 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crtACSOIDTSCDetalhe](
	[IdDetalhe] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpRegistro] [char](1) NOT NULL,
	[TpPanProxy] [char](1) NOT NULL,
	[PanProxy] [varchar](32) NOT NULL,
	[CPF] [varchar](11) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[NomeFacial] [varchar](25) NULL,
	[DtNascimento] [date] NULL,
	[Sexo] [char](1) NULL,
	[CnpjFilial] [varchar](14) NULL,
	[Grupo] [varchar](20) NULL,
	[Email] [varchar](30) NULL,
	[DDDCel] [varchar](2) NULL,
	[Celular] [varchar](9) NULL,
	[NomeMae] [varchar](50) NULL,
	[IdRegistro] [varchar](10) NULL,
	[NumLinha] [int] NOT NULL,
 CONSTRAINT [PK_crtACSOIDTSCDetalhe] PRIMARY KEY CLUSTERED 
(
	[IdDetalhe] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO


PRINT 'Fim da alteração'
GO



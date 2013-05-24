------------------------------------------------------------------------------------------------------
--	
--	2.0.08.CCP.C.20130322.sql
--  
--	embACSOEMBCS1_0Cabecalho: criação de tabela
--  Data: 07/03/2013		Autor: rafaelrau
--	Versão:	1.0.07.CCP.C.20130307
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.08.CCP.C.20130322',
		@descricao = 'embACSOEMBCS1_0Cabecalho: criação de tabela',
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

/****** Object:  Table [dbo].[embACSOEMBCS1_0Cabecalho]    Script Date: 03/22/2013 11:00:24 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[embACSOEMBCS1_0Cabecalho](
	[idCabecalho] [int] IDENTITY(1,1) NOT NULL,
	[idArquivo] [int] NOT NULL,
	[tpRegistro] [char](1) NOT NULL,
	[nomeLayout] [varchar](20) NOT NULL,
	[versao] [varchar](8) NOT NULL,
	[dataGeracao] [datetime] NOT NULL,
	[seqArquivo] [int] NOT NULL,
	[nomeArquivo] [varchar](50) NOT NULL,
	[codEmprEmbss] [varchar](10) NOT NULL,
	[idUsuCriacao] [int] NOT NULL,
	[dtCriacao] [datetime2](7) NOT NULL,
	[dtGeracao] [datetime2](7) NOT NULL,
	[ts] [timestamp] NOT NULL,
 CONSTRAINT [PK_embACSOEMBCS1_0Cabecalho] PRIMARY KEY CLUSTERED 
(
	[idCabecalho] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[embACSOEMBCS1_0Cabecalho] ADD  CONSTRAINT [DF_embACSOEMBCS1_0Cabecalho_dtCriacao]  DEFAULT (sysdatetime()) FOR [dtCriacao]
GO

ALTER TABLE [dbo].[embACSOEMBCS1_0Cabecalho] ADD  CONSTRAINT [DF_embACSOEMBCS1_0Cabecalho_dtGeracao]  DEFAULT (sysdatetime()) FOR [dtGeracao]
GO

PRINT 'Fim da alteração'
GO


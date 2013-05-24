------------------------------------------------------------------------------------------------------
--	
--	2.0.10.CCP.C.20130322.sql
--  
--	embACSOEMBCS1_0Detalhe: criação de tabela
--  Data: 22/03/2013		Autor: rafaelrau
--	Versão:	2.0.09.CCP.C.20130322
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.10.CCP.C.20130322',
		@descricao = 'embACSOEMBCS1_0Detalhe: criação de tabela',
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

/****** Object:  Table [dbo].[embACSOEMBCS1_0Detalhe]    Script Date: 03/22/2013 11:02:32 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[embACSOEMBCS1_0Detalhe](
	[idDetalhe] [int] IDENTITY(1,1) NOT NULL,
	[idArquivo] [int] NOT NULL,
	[idCabLote] [int] NOT NULL,
	[tpRegistro] [char](1) NOT NULL,
	[pan] [varchar](19) NOT NULL,
	[effDate] [date] NOT NULL,
	[expDate] [date] NOT NULL,
	[brnCode] [varchar](8) NOT NULL,
	[pvki] [int] NOT NULL,
	[pvv] [varchar](4) NOT NULL,
	[firstName] [varchar](20) NOT NULL,
	[lastName] [varchar](20) NOT NULL,
	[addrl1] [varchar](35) NOT NULL,
	[addrl2] [varchar](35) NULL,
	[addrl3] [varchar](35) NULL,
	[city] [varchar](30) NOT NULL,
	[postcode] [varchar](10) NOT NULL,
	[county] [varchar](20) NOT NULL,
	[countryCode] [varchar](4) NOT NULL,
	[svcCode] [varchar](3) NOT NULL,
	[cvc] [int] NOT NULL,
	[emboss] [varchar](27) NOT NULL,
	[track1] [varchar](79) NOT NULL,
	[track2] [varchar](40) NOT NULL,
	[embossl1] [varchar](30) NULL,
	[embossl2] [varchar](30) NULL,
	[embossl3] [varchar](30) NULL,
	[embossl4] [varchar](30) NULL,
	[numSeqInt] [varchar](6) NULL,
	[pinblk] [varchar](16) NULL,
	[cardPakCode] [varchar](8) NOT NULL,
	[designref] [varchar](8) NOT NULL,
	[accessCode] [varchar](12) NULL,
	[reqType] [int] NULL,
	[dlvMethod] [char](1) NULL,
	[custType] [char](1) NULL,
	[pinFromPan] [varchar](19) NULL,
	[cardid] [varchar](19) NULL,
	[numUnicoCart] [varchar](32) NULL,
	[codBarras] [varchar](32) NULL,
	[idUsuCriacao] [int] NOT NULL,
	[dtCriacao] [datetime2](7) NOT NULL,
	[ts] [timestamp] NOT NULL,
 CONSTRAINT [PK_embACSOEMBCS1_0Detalhe] PRIMARY KEY CLUSTERED 
(
	[idDetalhe] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[embACSOEMBCS1_0Detalhe] ADD  CONSTRAINT [DF_embACSOEMBCS1_0Detalhe_tpRegistro]  DEFAULT ((2)) FOR [tpRegistro]
GO

ALTER TABLE [dbo].[embACSOEMBCS1_0Detalhe] ADD  CONSTRAINT [DF_embACSOEMBCS1_0Detalhe_dtCriacao]  DEFAULT (sysdatetime()) FOR [dtCriacao]
GO

PRINT 'Fim da alteração'
GO


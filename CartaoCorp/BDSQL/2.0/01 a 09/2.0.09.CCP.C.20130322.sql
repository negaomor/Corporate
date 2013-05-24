------------------------------------------------------------------------------------------------------
--	
--	2.0.09.CCP.C.20130322.sql
--  
--	embACSOEMBCS1_0CabLote: criação de tabela
--  Data: 07/03/2013		Autor: rafaelrau
--	Versão:	2.0.09.CCP.C.20130322
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.09.CCP.C.20130322',
		@descricao = 'embACSOEMBCS1_0CabLote: criação de tabela',
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

/****** Object:  Table [dbo].[embACSOEMBCS1_0CabLote]    Script Date: 03/22/2013 11:01:11 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[embACSOEMBCS1_0CabLote](
	[idCabLote] [int] IDENTITY(1,1) NOT NULL,
	[idArquivo] [int] NOT NULL,
	[tpRegistro] [char](1) NOT NULL,
	[numLote] [int] NOT NULL,
	[numCartLote] [int] NOT NULL,
	[agrupar] [char](1) NOT NULL,
	[brnCode] [varchar](8) NOT NULL,
	[cardPakCode] [varchar](8) NOT NULL,
	[emprDest] [varchar](40) NULL,
	[destinatario] [varchar](40) NULL,
	[logradouro] [varchar](40) NULL,
	[numLogr] [varchar](10) NULL,
	[complemento] [varchar](20) NULL,
	[bairro] [varchar](30) NULL,
	[cidade] [varchar](30) NULL,
	[uf] [char](2) NULL,
	[cep] [varchar](8) NULL,
	[idUsuCriacao] [int] NOT NULL,
	[dtCriacao] [datetime2](7) NOT NULL,
	[ts] [timestamp] NOT NULL,
 CONSTRAINT [PK_embACSOEMBCS1_0CabLote] PRIMARY KEY CLUSTERED 
(
	[idCabLote] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[embACSOEMBCS1_0CabLote] ADD  CONSTRAINT [DF_embACSOEMBCS1_0CabLote_dtCriacao]  DEFAULT (sysdatetime()) FOR [dtCriacao]
GO

PRINT 'Fim da alteração'
GO


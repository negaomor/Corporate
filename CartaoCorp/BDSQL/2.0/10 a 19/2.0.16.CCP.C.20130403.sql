------------------------------------------------------------------------------------------------------
--	
--	2.0.16.CCP.C.20130403.sql
--  
--	crpCRICargaDetalhe: criação de tabela
--  Data: 01/04/2013		Autor: rafaelrau
--	Versão:	2.0.16.CCP.C.20130403
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.16.CCP.C.20130403',
		@descricao = 'crpCRICargaDetalhe: criação de tabela',
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

/****** Object:  Table [dbo].[crpCRICargaDetalhe]    Script Date: 04/05/2013 10:38:18 ******/
SET ANSI_NULLS ON
GO

SET QUOTED_IDENTIFIER ON
GO

SET ANSI_PADDING ON
GO

CREATE TABLE [dbo].[crpCRICargaDetalhe](
	[IdCRICrgtDet] [int] IDENTITY(1,1) NOT NULL,
	[IdArquivo] [int] NOT NULL,
	[TpIdentificacao] [tinyint] NOT NULL,
	[Identificacao] [varchar](32) NOT NULL,
	[StatusCart] [tinyint] NOT NULL,
	[Valor] [money] NOT NULL,
	[Chave] [varchar](15) NOT NULL,
	[Retorno] [varchar](50) NULL,
	[DtRetorno] [datetime2](7) NULL,
	[DtCriacao] [datetime2](7) NOT NULL,
	[DtAlteracao] [datetime2](7) NOT NULL,
 CONSTRAINT [PK_crpCRICargaDetalhe] PRIMARY KEY CLUSTERED 
(
	[IdCRICrgtDet] ASC
)WITH (PAD_INDEX  = OFF, STATISTICS_NORECOMPUTE  = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS  = ON, ALLOW_PAGE_LOCKS  = ON) ON [PRIMARY]
) ON [PRIMARY]

GO

SET ANSI_PADDING OFF
GO

ALTER TABLE [dbo].[crpCRICargaDetalhe] ADD  CONSTRAINT [DF_crpCRICargaDetalhe_DtCriacao]  DEFAULT (sysdatetime()) FOR [DtCriacao]
GO

ALTER TABLE [dbo].[crpCRICargaDetalhe] ADD  CONSTRAINT [DF_crpCRICargaDetalhe_DtAlteracao]  DEFAULT (sysdatetime()) FOR [DtAlteracao]
GO


PRINT 'Fim da alteração'
GO


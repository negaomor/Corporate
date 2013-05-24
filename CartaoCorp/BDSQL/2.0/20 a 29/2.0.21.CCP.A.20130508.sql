------------------------------------------------------------------------------------------------------
--	
--	2.0.21.CCP.A.20130508
--  
--	crtTituloTarifa: Exclusão de campos de desTarifa e tpTarifa
--  Data: 18/03/2013		Autor: rafaelrau
--	Versão:	2.0.21.CCP.A.20130508
--
------------------------------------------------------------------------------------------------------

BEGIN TRAN

DECLARE @versao varchar(30), @descricao varchar(100), @dtVersao datetime,
		@idMigracao INT, @major VARCHAR(5), @idUser INT, @modulo VARCHAR(20)
SELECT	@versao = '2.0.21.CCP.A.20130508',
		@descricao = 'crtTituloTarifa: Exclusão de campos de desTarifa e tpTarifa',
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
/* To prevent any potential data loss issues, you should review this script in detail before running it outside the context of the database designer.*/
BEGIN TRANSACTION
SET QUOTED_IDENTIFIER ON
SET ARITHABORT ON
SET NUMERIC_ROUNDABORT OFF
SET CONCAT_NULL_YIELDS_NULL ON
SET ANSI_NULLS ON
SET ANSI_PADDING ON
SET ANSI_WARNINGS ON
COMMIT
BEGIN TRANSACTION
GO
ALTER TABLE dbo.crtTituloTarifa
	DROP COLUMN DesTarifa, TpTarifa
GO
ALTER TABLE dbo.crtTituloTarifa SET (LOCK_ESCALATION = TABLE)
GO
COMMIT
GO
PRINT 'Fim da alteração'
GO


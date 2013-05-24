----------------------------------------------------------------------------------------------------
--																					  v2.0  SQL2008
--	crtCargaDetalheIdentificacao
--
--	Data: 09/05/2013		Autor: mmenezes
--	Versão:	xxxx
----------------------------------------------------------------------------------------------------

IF NOT EXISTS(Select * from sysobjects WHERE name = 'crtCargaDetalheIdentificacao')
	EXEC sp_executesql N' CREATE PROC [dbo].[crtCargaDetalheIdentificacao] AS RETURN '
GO
PRINT 'Atualizando procedure crtCargaDetalheIdentificacao'
GO

ALTER PROCEDURE [dbo].[crtCargaDetalheIdentificacao]
	(	@IdArquivo as int, 
		@TpRegistro as char(1), 
		@TpPanProxy as char(1), 
		@PanProxy as varchar(32), 
		@CPF as varchar(11), 
		@Nome as varchar(50), 
		@NomeFacial as varchar(25), 
		@DtNascimento as datetime, 
		@Sexo as char(1), 
		@CnpjFilial as varchar(14),
		@Grupo as varchar(20), 
		@Email as varchar(30), 
		@DDDCel as varchar(2), 
		@Celular as varchar(9), 
		@NomeMae as varchar(50), 
		@IdRegistro as varchar(1), 
		@NumLinha int
	)
AS

	SET NOCOUNT ON

	BEGIN TRANSACTION CartaoEvento

	COMMIT TRANSACTION CartaoEvento
	
		INSERT [crtACSOIDTSCDetalhe]
		(IdArquivo, TpRegistro, TpPanProxy, PanProxy, CPF, Nome, NomeFacial, DtNascimento, Sexo, CnpjFilial,
		Grupo, Email, DDDCel, Celular, NomeMae, IdRegistro, NumLinha)
		SELECT @IdArquivo, @TpRegistro, @TpPanProxy, @PanProxy, @CPF, @Nome, @NomeFacial, @DtNascimento, @Sexo, @CnpjFilial,
		@Grupo, @Email, @DDDCel, @Celular, @NomeMae, @IdRegistro, @NumLinha
		IF @@error <> 0 GOTO ErroTran

	RETURN 

ErroTran:
	ROLLBACK TRAN CartaoEvento
	
	RETURN -1
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crtCargaDetalheIdentificacao] TO upBeneficio
GO
PRINT 'Fim atualização crtCargaDetalheIdentificacao'
GO
----------------------------------------------------------------------------------------------------
--																					  v2.0  SQL2008
--	crpInsereCargaDetalhe
--
--	Data: 09/05/2013		Autor: mmenezes
--	Versão:	xxxx
----------------------------------------------------------------------------------------------------

IF NOT EXISTS(Select * from sysobjects WHERE name = 'crpInsereCargaDetalhe')
	EXEC sp_executesql N' CREATE PROC [dbo].[crpInsereCargaDetalhe] AS RETURN '
GO
PRINT 'Atualizando procedure crpInsereCargaDetalhe'
GO

ALTER PROCEDURE [dbo].[crpInsereCargaDetalhe]
	(	@IdArquivo as int = null, 
		@TpPanProxy as char(1) = null,
		@TpRegistro as char(1)= null, 		
		@PanProxy as varchar(32)= null, 
		@IdRegistro as varchar(10)= null, 
		@NumLinha int= null,
		@NomeLayout varchar(20)= null,
		@Versao varchar(8)= null,
		@DataGeracao datetime= null,
		@SeqArquivo char(1)= null,
		@NomeArquivo varchar(50)= null,
		@CodConvenio varchar(10)= null,
		@CodEmpresa varchar(14) = null,
		@CodPrgCrg varchar(10)= null,
		@NomePrg varchar(20)= null,
		@StatCart char(1)= null,
		@DataAgend datetime= null,
		@NumCart int= null,
		@ValorCrg decimal= null,
		@Valor varchar= null,
		@NumCrg int= null,
		@Linha int
	)

AS

	SET NOCOUNT ON

	BEGIN TRANSACTION CargaEvento

	COMMIT TRANSACTION CargaEvento

	IF (@Linha = 0)
		BEGIN
			INSERT crtACSOPRGCRCabecalho
			(IdArquivo, TpRegistro, NomeLayout, Versao, DataGeracao, SeqArquivo, NomeArquivo, CodConvenio, CodEmpresa, NumLinha)
			SELECT @IdArquivo, @TpRegistro, @NomeLayout, @Versao, @DataGeracao, @SeqArquivo, @NomeArquivo, @CodConvenio, @CodEmpresa, @NumLinha		
		END
	ELSE IF (@Linha = 1)
		BEGIN
			INSERT crtACSOPRGCRLote
			(IdArquivo, TpRegistro, CodPrgCrg, NomePrg, StatCart, DataAgend, CodConvenio, NumCart, ValorCrg, NumLinha)
			SELECT   @IdArquivo, @TpRegistro, @CodPrgCrg, @NomePrg, @StatCart, @DataAgend, @CodConvenio, @NumCart, @ValorCrg, @NumLinha			
		END
	ELSE IF (@Linha = 2)
		BEGIN
			INSERT crtACSOPRGCRDetalhe
			(IdArquivo, TpRegistro, CodPrgCrg, TpPanProxy, PanProxy, Valor, IdRegistro, NumLinha)
			SELECT @IdArquivo, @TpRegistro, @CodPrgCrg, @TpPanProxy, @PanProxy, @Valor, @IdRegistro, @NumLinha					
		END
	ELSE IF (@Linha = 9)
		BEGIN
			INSERT crtACSOPRGCRRodape
			(IdArquivo, TpRegistro, NumCrg, NumCart, ValorCrg, NumLinha)
			SELECT @IdArquivo, @TpRegistro, @NumCrg, @NumCart, @ValorCrg, @NumLinha	
		END

		IF @@error <> 0 GOTO ErroTran

	RETURN 

ErroTran:
	ROLLBACK TRAN CargaEvento
	
	RETURN -1
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crpInsereCargaDetalhe] TO upBeneficio
GO
PRINT 'Fim atualização crpInsereCargaDetalhe'
GO
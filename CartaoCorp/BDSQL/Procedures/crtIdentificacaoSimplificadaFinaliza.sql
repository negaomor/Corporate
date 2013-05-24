----------------------------------------------------------------------------------------------------
--	
--	crtIdentificacaoSimplificadaFinaliza    
--  Caso não tenha ocorrido nenhum erro(tpLog = 3) durante o processamento do arquivo, é inserido na tabela svcPedido um novo pedido
--
--	Data: 24/01/2013		Autor: rafaelrau
--	Versão:	XXXX.sql
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'crtIdentificacaoSimplificadaFinaliza')
	EXEC sp_executesql N' CREATE PROC [dbo].[crtIdentificacaoSimplificadaFinaliza] AS RETURN '
GO
PRINT 'Atualizando procedure crtIdentificacaoSimplificadaFinaliza'
GO
ALTER PROCEDURE [dbo].[crtIdentificacaoSimplificadaFinaliza]
(	
@idProcesso INT
) WITH RECOMPILE
AS
 
 DECLARE @descricaoTpLog VARCHAR(100)
 
 IF NOT EXISTS(SELECT * FROM [impLog] WHERE idArquivo = @idProcesso AND tpLog = 3)
	BEGIN
	
		SET @descricaoTpLog = 'Arquivo processado automaticamente.'
		INSERT [impLog] 
		(idArquivo, tpLog, tpTabela, tpProcesso, linha, coluna, descricaoErro, dtLog ) 
		SELECT TOP 1 idArquivo, tpLog, tpTabela, tpProcesso, linha, coluna, @descricaoTpLog, dtLog 
		FROM	[impLog] 
		WHERE	idArquivo = @idProcesso 
		AND		tpLog = 1
	
		--Identificação_Simplificada_Cartão_CRI = 901
		INSERT [svcPedido]
		(tpPedido, idEntidade, idDocumento, tpOrigem, dtInicio, dtFim, idUsuCriacao, status)
		SELECT	901, idEntidade, idProcesso, 1, null, null, idUsuario, status 
		FROM	[impIntegracaoProcesso]
		WHERE	idProcesso = @idProcesso

	END 
	ELSE
	  BEGIN 
	  
		SET @descricaoTpLog = 'Arquivo não processado automaticamente. Requer processamento forçado.'
		INSERT [impLog] 
		(idArquivo, tpLog, tpTabela, tpProcesso, linha, coluna, descricaoErro, dtLog ) 
		SELECT TOP 1 idArquivo, tpLog, tpTabela, tpProcesso, linha, coluna, @descricaoTpLog, dtLog 
		FROM	[impLog] 
		WHERE	idArquivo = @idProcesso
		AND		tpLog = 3

	  END
RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].crtIdentificacaoSimplificadaFinaliza TO upBeneficio
GO
PRINT 'Fim atualização crtIdentificacaoSimplificadaFinaliza'
GO


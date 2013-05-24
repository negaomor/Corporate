----------------------------------------------------------------------------------------------------
--	
--	criSolicitacaoFinaliza    
--  Dado um idArquivo é atualizado seu diretório de origem e o nome do arquivo 
--  e preparo para o próxmo serviço de envio do cri
--
--	Data: 02/09/2012		Autor: rafaelrau
--	Versão:	XXXX.sql
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'criFinaliza')
	EXEC sp_executesql N' CREATE PROC [dbo].[criFinaliza] AS RETURN '
GO
PRINT 'Atualizando procedure criFinaliza'
GO
ALTER PROCEDURE [dbo].[criFinaliza]
(	
@idArquivo INT,
@origem VARCHAR (200),
@nomeArquivo VARCHAR(100)
) WITH RECOMPILE
AS
		
		 UPDATE A 
         SET A.origem = @origem, 
         A.nomeArquivo = @nomeArquivo 
         FROM impIntegracaoProcesso AS A 
         WHERE A.idProcesso = @idArquivo

		-- Preparo para o próximo serviço envio do CRI = 501,
		INSERT [svcPedido]
		(tpPedido, idEntidade, idDocumento, tpOrigem, dtInicio, dtFim, idUsuCriacao, status)
		SELECT 501, idEntidade, idProcesso, 1, NULL, NULL, idUsuario, status 
		FROM [impIntegracaoProcesso]
		WHERE idProcesso = @idArquivo

RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].criFinaliza TO upBeneficio
GO
PRINT 'Fim atualização criFinaliza'
GO




----------------------------------------------------------------------------------------------------
--	
--	crpConsultaLimiteCliente    
--
--	Data: 20/05/2013		Autor: mmenezes
--	Versão:	XXXX.sql

-- Consulta limites de crédito do cliente.
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'crpConsultaLimiteCliente')
	EXEC sp_executesql N' CREATE PROC [dbo].[crpConsultaLimiteCliente] AS RETURN '
GO
PRINT 'Atualizando procedure crpConsultaLimiteCliente'
GO
ALTER PROCEDURE [dbo].[crpConsultaLimiteCliente]
(	
@idEntidade as varchar(10)
) 
AS
 
	SELECT distinct valMinCredito, valMaxCredito, valLimiteCreditoMes 
	FROM crtConvenio C
	INNER JOIN crtCartaoProxy CP on CP.idEntDistribuicao = C.idConveniado
	WHERE idConveniado = @idEntidade 
	
RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crpConsultaLimiteCliente] TO upBeneficio
GO
PRINT 'Fim atualização crpConsultaLimiteCliente'
GO


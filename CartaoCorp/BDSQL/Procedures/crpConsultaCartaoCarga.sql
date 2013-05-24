----------------------------------------------------------------------------------------------------
--	
--	crpConsultaCartaoCarga    
--
--	Data: 20/05/2013		Autor: mmenezes
--	Versão:	XXXX.sql

-- Consulta cartoes de carga na base.
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'crpConsultaLimiteCliente')
	EXEC sp_executesql N' CREATE PROC [dbo].[crpConsultaLimiteCliente] AS RETURN '
GO
PRINT 'Atualizando procedure crpConsultaLimiteCliente'
GO
ALTER PROCEDURE [dbo].[crpConsultaLimiteCliente]
(	
@Identificacao varchar(32),
@idEntidade as int
) 
AS
 
	SELECT valMinCredito, valMaxCredito, valLimiteCreditoMes, * from crtConvenio C
	INNER JOIN crtCartaoProxy CP on CP.idEntDistribuicao = C.idConveniado
	WHERE idConveniado = @idConveniado  and C.idProduto = @idProduto and proxy = @Identificacao 
	
RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crpConsultaLimiteCliente] TO upBeneficio
GO
PRINT 'Fim atualização crpConsultaLimiteCliente'
GO


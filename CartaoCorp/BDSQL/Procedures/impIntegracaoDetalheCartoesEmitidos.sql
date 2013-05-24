----------------------------------------------------------------------------------------------------
--																					  v2.0  SQL2008
--	[impIntegracaoDetalheCartoesEmitidos]
--
--	Data: 08/05/2013		Autor: mmenezes
--	Versão:	xxxx
----------------------------------------------------------------------------------------------------

IF NOT EXISTS(Select * from sysobjects WHERE name = 'impIntegracaoDetalheCartoesEmitidos')
	EXEC sp_executesql N' CREATE PROC [dbo].[impIntegracaoDetalheCartoesEmitidos] AS RETURN '
GO
PRINT 'Atualizando procedure impIntegracaoDetalheCartoesEmitidos'
GO

ALTER PROC [dbo].[impIntegracaoDetalheCartoesEmitidos]
@idProcesso int
AS

	SELECT ip.idProcesso as codEmissao, co.codConvenio, left(dbo.DecriptaPan(cp.pan),6) + '*' + right(dbo.DecriptaPan(cp.pan),4)as NumCartao, cp.proxy, cp.status as StatusCart
	FROM impIntegracaoProcessoItem ipi
	inner join impIntegracaoProcesso ip on ip.idProcesso = ipi.idProcesso
	inner join crtCartaoProxy cp on cp.idCartao = ipi.idChave
	INNER join crtConvenio co on co.idProduto=cp.idProduto 
	where ip.idProcesso = @idProcesso 
	and not co.codConvenio is null
	


RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[impIntegracaoDetalheCartoesEmitidos] TO upBeneficio
GO
PRINT 'Fim atualização [impIntegracaoDetalheCartoesEmitidos]'
GO
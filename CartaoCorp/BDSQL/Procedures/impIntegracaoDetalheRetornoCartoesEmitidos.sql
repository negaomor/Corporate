----------------------------------------------------------------------------------------------------
--																					  v2.0  SQL2008
--	[impIntegracaoDetalheRetornoCartoesEmitidos]
--
--	Data: 08/05/2013		Autor: mmenezes
--	Versão:	xxxx
----------------------------------------------------------------------------------------------------

IF NOT EXISTS(Select * from sysobjects WHERE name = 'impIntegracaoDetalheRetornoCartoesEmitidos')
	EXEC sp_executesql N' CREATE PROC [dbo].[impIntegracaoDetalheRetornoCartoesEmitidos] AS RETURN '
GO
PRINT 'Atualizando procedure impIntegracaoDetalheRetornoCartoesEmitidos'
GO

ALTER PROC [dbo].[impIntegracaoDetalheRetornoCartoesEmitidos]
@idProcesso int
AS

	SELECT ip.idProcesso as codEmissao, co.codConvenio, count(ipi.idChave) as NumCartao
	FROM impIntegracaoProcessoItem ipi
	inner join impIntegracaoProcesso ip on ip.idProcesso = ipi.idProcesso
	inner join crtCartaoProxy cp on cp.idCartao = ipi.idChave
	INNER join crtConvenio co on co.idProduto=cp.idProduto 
	where ip.idProcesso = @idProcesso 
	and not co.codConvenio is null
	group by co.codConvenio,ip.idProcesso


RETURN

GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[impIntegracaoDetalheRetornoCartoesEmitidos] TO upBeneficio
GO
PRINT 'Fim atualização [impIntegracaoDetalheRetornoCartoesEmitidos]'
GO
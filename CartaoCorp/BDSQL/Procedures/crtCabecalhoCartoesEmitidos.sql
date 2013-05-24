----------------------------------------------------------------------------------------------------
--																					  v2.0  SQL2008
--	crtCabecalhoCartoesEmitidos
--
--	Data: 08/05/2013		Autor: mmenezes
--	Versão:	xxxx
----------------------------------------------------------------------------------------------------

IF NOT EXISTS(Select * from sysobjects WHERE name = 'crtCabecalhoCartoesEmitidos')
	EXEC sp_executesql N' CREATE PROC [dbo].[crtCabecalhoCartoesEmitidos] AS RETURN '
GO
PRINT 'Atualizando procedure crtCabecalhoCartoesEmitidoso'
GO

ALTER PROCEDURE [dbo].[crtCabecalhoCartoesEmitidos]
(
@idProcesso INT
)
AS

	SELECT co.codConvenio, count(ipi.idChave) as NumCartoes
	FROM impIntegracaoProcessoItem ipi
	inner join impIntegracaoProcesso ip on ip.idProcesso = ipi.idProcesso
	inner join crtCartaoProxy cp on cp.idCartao = ipi.idChave
	INNER join crtConvenio co on co.idProduto=cp.idProduto 
	where ip.idProcesso = @idProcesso 
	and not co.codConvenio is null
	group by co.codConvenio

RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crpTituloEmiteDoPedido] TO upBeneficio
GO
PRINT 'Fim atualização crpTituloEmiteDoPedido'
GO
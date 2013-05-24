----------------------------------------------------------------------------------------------------
--	
--	crtObtemQuantidadeCartoesEmissao    
--  Obtém a quantidade de cartões de carga e o produto relacionado
--
--	Data: 16/05/2013		Autor: mmenezes
--	Versão:	XXXX.sql
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'crtObtemQuantidadeCartoesCarga')
	EXEC sp_executesql N' CREATE PROC [dbo].[crtObtemQuantidadeCartoesCarga] AS RETURN '
GO
PRINT 'Atualizando procedure crtObtemQuantidadeCartoesCarga'
GO
ALTER PROCEDURE [dbo].[crtObtemQuantidadeCartoesCarga]	
(	
@IdProcesso INT
)
AS
 
SELECT P.idProduto, PanProxy, A.Valor
FROM [crtACSOPRGCRDetalhe] AS A
INNER JOIN [impIntegracaoProcesso] as IP ON IP.idProcesso = A.IdArquivo
INNER JOIN [crtCartaoProxy] AS P ON ((P.proxy = A.PanProxy AND A.TpPanProxy = 1))
								 --OR (dbo.DecriptaPan(pan) = A.PanProxy AND A.TpPanProxy = 2))
								 AND P.idEntDistribuicao = IP.idEntidade
INNER JOIN [crtConvenio] AS C ON C.idProduto = P.idProduto
								/* C.codigo = B.CodConvenio */
								AND C.idConveniado = IP.idEntidade
INNER JOIN [crtProduto] AS X ON X.idProduto = P.idProduto
WHERE A.IdArquivo = @IdProcesso

RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].crtObtemQuantidadeCartoesCarga TO upBeneficio
GO
PRINT 'Fim atualização crtObtemQuantidadeCartoesCarga'
GO

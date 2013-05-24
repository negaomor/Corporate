----------------------------------------------------------------------------------------------------
--	
--	crtObtemQuantidadeCartoesEmissao    
--  Obtém a quantidade de cartões a serem solicitados e o produto relacionado
--
--	Data: 13/05/2013		Autor: rafaelrau
--	Versão:	XXXX.sql
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'crtObtemQuantidadeCartoesEmissao')
	EXEC sp_executesql N' CREATE PROC [dbo].[crtObtemQuantidadeCartoesEmissao] AS RETURN '
GO
PRINT 'Atualizando procedure crtObtemQuantidadeCartoesEmissao'
GO
ALTER PROCEDURE [dbo].[crtObtemQuantidadeCartoesEmissao]
(	
@IdProcesso INT
) WITH RECOMPILE
AS
 
 DECLARE @idReferencia INT 
 SELECT  @idReferencia = idReferencia FROM impIntegracaoProcesso 
 WHERE idProcesso = @IdProcesso 
 SELECT A.idProduto, B.quantidade FROM crtEmissaoCartaoAvulso AS A 
 INNER JOIN crtEmissaoCartaoAvulsoDetalhe AS B ON B.idEmissao = A.idEmissao 
 WHERE A.idEmissao = @idReferencia 

RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].crtObtemQuantidadeCartoesEmissao TO upBeneficio
GO
PRINT 'Fim atualização crtObtemQuantidadeCartoesEmissao'
GO

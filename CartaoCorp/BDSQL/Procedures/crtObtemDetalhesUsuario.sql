----------------------------------------------------------------------------------------------------
--	
-- [crtObtemDetalhesUsuario]    
--
--	Data: 13/05/2013		Autor: mmenezes
--	Versão:	XXXX.sql
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'crtObtemDetalhesUsuario')
	EXEC sp_executesql N' CREATE PROC [dbo].[crtObtemDetalhesUsuario] AS RETURN '
GO
PRINT 'Atualizando procedure crtObtemDetalhesUsuario'
GO
ALTER PROCEDURE [dbo].[crtObtemDetalhesUsuario]
(	
@idArquivo INT
) 
AS
 
SELECT A.PanProxy, A.CPF, A.Nome, A.DtNascimento, A.NomeMae, C.nome AS NomeConvenio, D.nome AS NomeProduto,
(A.DDDCel + A.Celular) AS Celular, A.Email, D.idProduto FROM [crtACSOIDTSCDetalhe] AS A
INNER JOIN [crtACSOIDTSCCabecalho] AS B ON B.IdArquivo = A.IdArquivo
INNER JOIN [crtConvenio] AS C ON C.codigo = B.CodConvenio
INNER JOIN [crtProduto] AS D ON D.idProduto = C.idProduto
WHERE A.IdArquivo = @idArquivo

RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crtObtemDetalhesUsuario] TO upBeneficio
GO
PRINT 'Fim atualização crtObtemDetalhesUsuario'
GO


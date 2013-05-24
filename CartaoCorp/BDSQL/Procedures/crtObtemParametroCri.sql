----------------------------------------------------------------------------------------------------
--	
--	crtBuscaCriIdentificacao    
--
--	Data: 13/05/2013		Autor: mmenezes
--	Versão:	XXXX.sql

-- Ajuste para obter parâmetro da glbParametro e realizar a busca tanto por PAN ou idProduto => rrau
----------------------------------------------------------------------------------------------------
IF NOT EXISTS(Select * from sysobjects WHERE name = 'crtObtemParametroCri')
	EXEC sp_executesql N' CREATE PROC [dbo].[crtObtemParametroCri] AS RETURN '
GO
PRINT 'Atualizando procedure crtObtemParametroCri'
GO
ALTER PROCEDURE [dbo].[crtObtemParametroCri]
(	
@idProduto   INT = NULL,
@idParametro INT
) 
AS

IF(@idParametro = 71)
BEGIN
	SELECT P.programid, P.crdprofile, P.custprofile, CP.pan, BRNCode, PR.valor AS StatCode, PL.codigo AS DesignRef
	FROM crtProduto P
		INNER JOIN crtConvenio C on C.idProduto = P.idProduto
		INNER JOIN crtCartaoProxy CP on cp.idProduto = C.idProduto
		INNER JOIN crtConveniadoMapaFIS CMF on CMF.idConveniado = C.idConveniado
		INNER JOIN glbParametro AS PR ON PR.idParametro = @idParametro 
		INNER JOIN crtPlastico AS PL ON PL.idPlastico = P.idPlastico
	WHERE PR.idParametro = @idParametro AND P.idProduto = @idProduto 
END
ELSE IF(@idParametro = 70)
BEGIN
SELECT P.programid, P.crdprofile, P.custprofile, P.crdProduct, BRNCode, PR.valor AS StatCode, PL.codigo AS DesignRef
	FROM crtProduto P
		INNER JOIN crtConvenio C on C.idProduto = P.idProduto
		INNER JOIN embRegraEmbProxy CP on cp.idProduto = C.idProduto
		INNER JOIN crtConveniadoMapaFIS CMF on CMF.idConveniado = C.idConveniado
		INNER JOIN glbParametro AS PR ON PR.idParametro = @idParametro 
		INNER JOIN crtPlastico AS PL ON PL.idPlastico = P.idPlastico
	WHERE PR.idParametro = @idParametro AND P.idProduto = @idProduto 
END
ELSE IF(@idParametro = 72)
BEGIN
SELECT P.programid, P.crdprofile, P.custprofile, P.crdProduct, BRNCode, PR.valor AS StatCode, PL.codigo AS DesignRef
	FROM crtProduto P
		INNER JOIN crtConvenio C on C.idProduto = P.idProduto
		INNER JOIN embRegraEmbProxy CP on cp.idProduto = C.idProduto
		INNER JOIN crtConveniadoMapaFIS CMF on CMF.idConveniado = C.idConveniado
		INNER JOIN glbParametro AS PR ON PR.idParametro = @idParametro 
		INNER JOIN crtPlastico AS PL ON PL.idPlastico = P.idPlastico
	WHERE PR.idParametro = @idParametro AND P.idProduto = @idProduto 
END

RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crtObtemParametroCri] TO upBeneficio
GO
PRINT 'Fim atualização crtObtemParametroCri'
GO


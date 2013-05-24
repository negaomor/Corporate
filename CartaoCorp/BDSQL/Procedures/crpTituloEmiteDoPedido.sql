----------------------------------------------------------------------------------------------------
--																					  v2.0  SQL2008
--	crpTituloEmiteDoPedido
--
--	Data: 27/09/2012		Autor: vbloise
--	Versão:	xxxx
----------------------------------------------------------------------------------------------------

IF NOT EXISTS(Select * from sysobjects WHERE name = 'crpTituloEmiteDoPedido')
	EXEC sp_executesql N' CREATE PROC [dbo].[crpTituloEmiteDoPedido] AS RETURN '
GO
PRINT 'Atualizando procedure crpTituloEmiteDoPedido'
GO

ALTER PROCEDURE [dbo].[crpTituloEmiteDoPedido]
(
@idEmissao INT
)
AS

BEGIN

Declare @idConveniado int, @idConvenio int

Declare @qtd int

Select	@idConveniado = A.idConveniado, @idConvenio = C.idConvenio
from	crtEmissaoCartaoAvulso   AS A
INNER JOIN crtConvenio as C ON  C.idConveniado = A.idConveniado
						 AND C.idProduto = A.idProduto
where idEmissao = @idEmissao 

SELECT @qtd = quantidade from crtEmissaoCartaoAvulsoDetalhe WHERE idEmissao = @idEmissao 
--Obtem valor da tarifa
DECLARE @valorUnitario money
Select @valorUnitario = valor
From crtConvenioTarifaEmissao 
where idConveniado = @idConveniado  
and idConvenio = @idConvenio
and	 @qtd > ISNULL(qtdDe,0)
Select @valorUnitario, @qtd

if(@valorUnitario != 0)
BEGIN
	begin tran
	Declare @idTitulo INT
	INSERT	[crtTitulo]
	(idConveniado, idConvenio, idReferencia, numContabil, tpTitulo, meioPagamento, valBeneficio,
		valComissao, dtApropriacao, dtEmissao, dtVencimento, valTotalTitulo, status, idCargaAgendamentoExplodido)
	Select A.idConveniado, C.idConvenio, /*A.idProduto, */
		   A.idEmissao as idReferencia, '', 1, meioPagamento, 0 as valTotalCarga, @valorUnitario * @qtd as valComissao, 
			dbo.dia(sysdatetime()) as dtApropriacao,
			dbo.dia(sysdatetime()) as dtEmissao,
			dbo.dia(sysdatetime()) as dtVencimento, 0, 1, NULL as  idCargaAgendamentoExplodido /* status = 1: Em montagem */
	FROM	crtEmissaoCartaoAvulso  AS A
	INNER JOIN crtEmissaoCartaoAvulsoDetalhe AS B ON B.idEmissao = A.idEmissao
	INNER JOIN crtConvenio as C  ON C.idConveniado	= A.idConveniado
								AND C.idProduto		= A.idProduto
	WHERE	A.idEmissao = @idEmissao 
	Select @idTitulo = SCOPE_IDENTITY()

	--Atualizo crtEmissaoCartaoAvulso para amarrar ao título
	UPDATE A
	SET A.idTitulo = @idTitulo
	FROM crtEmissaoCartaoAvulso AS A
	WHERE A.idEmissao = @idEmissao 


	-- Atualiza a numeração do título e os valores
	UPDATE A
	SET		nossoNumero = Right('0000000000' + CAST(A.idTitulo as VARCHAR(10)), 10),
			idTituloRef = A.idTitulo,
			valTotalTitulo = A.valComissao,
			dtVencimento = [dbo].CalculaVencimento( 
							CASE 
							WHEN C.tpDiaPrazoVencto = 4 THEN dtVencimento /* à vista */
							WHEN C.tpDiaPrazoVencto = 10 /* Qnz DC */ THEN ISNULL(dtApropriacao, dtEmissao)
							ELSE dtEmissao END, 
							tpDiaPrazoVencto, 
							diasPrazoVencimento),
			status = 2 /*  marco o título como processado */
	FROM	[crtTitulo] as A
	INNER JOIN [crtConvenio] C ON C.idConvenio = A.idConvenio
	WHERE A.idTitulo = @idTitulo

	
	DECLARE @idEntidade INT
	SELECT @idEntidade = 1

	--Busco os dados da acesso
	DECLARE @TMPEntidadeCedente AS TABLE
	(IdEntidade INT, RazaoSocialCedente VARCHAR(100), CnpjCedente VARCHAR(14), CodBancoCedente VARCHAR(20), CodAgenciaCedente VARCHAR(10), DvAgenciaCedente  VARCHAR(3), 
	 NumContaCorrenteCedente VARCHAR(15), DvContaCorrenteCedente VARCHAR(3))
	 
	 INSERT @TMPEntidadeCedente(IdEntidade, RazaoSocialCedente, CnpjCedente, CodBancoCedente, CodAgenciaCedente, DvAgenciaCedente, NumContaCorrenteCedente, DvContaCorrenteCedente)
	 SELECT @idEntidade AS idEntidade, A.nomeFantasia, A.cnpj, B.codBanco, B.codAgencia, B.dvAgencia, B.numContaCorrente, dvContaCorrente FROM glbEntidade AS A 
	 INNER JOIN crtDadosBancarios AS B ON B.idEntidade = A.idEntidade
	 WHERE A.idEntidade = @idEntidade

	INSERT crtTituloNovo
			(idTitulo, idEntidadeCedente, idEntidadeSacado, idProduto, tpTitulo, meioPagamento, nossoNumero, dtRefInicial, dtRefFinal, dtEmissao, dtVencimento, valDocumento,
			valDesconto, valDeducoes, valMulta, valAcrescimos, valTotalCobrado, dtPagamento, valPagamento, nomSacado, nomCedente, cpfCnpjCedente, codBanco, codAgencia, dvAgencia,
			numContaCorrente, dvContaCorrente, idReferencia, status, cpfCnpjSacado) 

	Select  A.idTitulo, E.idEntidade, A.idConveniado as idEntidadeSacado, C.idProduto, A.tpTitulo,  A.meioPagamento, RIGHT('0000000000' + Cast(A.idTitulo as varchar(10)),10) as nossoNumero, 
	A.dtEmissao, A.dtEmissao, A.dtEmissao, DateAdd(day,  3, dtEmissao) as dtVencimento, @valorUnitario * @qtd as valDocumento, 0 as valDesconto, 0 as valDeducoes, 
	0 as valMulta, 0 as valAcrescimos, @valorUnitario * @qtd as valTotalCobrado, null as dtPagamento, null as valPagamento, F.razaoSocial, E.RazaoSocialCedente, E.CnpjCedente, 
	E.CodBancoCedente, E.CodAgenciaCedente, E.DvAgenciaCedente, E.NumContaCorrenteCedente, E.DvContaCorrenteCedente, D.idDadosBancarios, 0 as status, F.cnpj 
	From	crtTitulo AS A
	INNER JOIN crtConvenio as C  ON C.idConveniado	= A.idConveniado
								AND C.idConvenio	= A.idConvenio
	INNER join crtDadosBancarios AS D ON D.idEntidade = @idEntidade
	INNER JOIN @TMPEntidadeCedente AS E ON E.idEntidade = @idEntidade
	INNER JOIN crtConveniado AS F ON F.idConveniado = A.idConveniado
	WHERE	A.idTitulo = @idTitulo
	
	--Insiro no título detalhe	
	INSERT crtTituloDetalhe 
			(idTitulo,  dtReferencia, valTotalTransacao, numTransacoes, tpDiaPrazoReembolso, diasPrazoReembolso, taxaPercentual, valTaxaTransacao, valAluguelPOS, valTaxaFixa, valPorCartao, 
			 valTaxa1aVia, valTaxa2aViaCartao, valTaxa2aViaSenha, valTotalTaxaPercentual, valTotalTaxaTransacao, valTotalAluguelPOS, valTotalTaxaFixa, valTotalPorCartao, valTotal1aVia, 
			 valTotal2aViaCartao, valTotal2aViaSenha)
    SELECT   @idTitulo, GETDATE(), 0, 0, 0, 0, valPercAdm, 0, 0, valFixoAdm, valPorCartaoAdm, valTaxa1aVia, valTaxa2aViaCartao, valTaxa2aViaSenha, 0000, 0, 0,	0, 0, 0, 0, 0 
    FROM crtConvenio WHERE idConveniado = @idConveniado AND idConvenio = @idConvenio

--Insiro um novo registro referente a tarifa
	INSERT [crtTituloTarifa]
			(IdTitulo, Linha, CodTarifa, Qtd,  ValTarifa,      ValorTotal)
    SELECT   @idTitulo, 1,    A.tipo,    @qtd, @valorUnitario, (@qtd * @valorUnitario) AS ValorTotal  
	FROM crtDominioSimples AS A 
	WHERE A.idDominio = 60 AND A.tipo = 110 --Tarifa de emissão

COMMIT
END

END 
RETURN
GO

PRINT 'Dando acesso de execução ao upBeneficio'
GO
GRANT EXEC ON [dbo].[crpTituloEmiteDoPedido] TO upBeneficio
GO
PRINT 'Fim atualização crpTituloEmiteDoPedido'
GO





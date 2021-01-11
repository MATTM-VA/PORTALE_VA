USE [DatiVaAIA]
GO
/****** Object:  FullTextCatalog [FTC_Documenti]    Script Date: 16/11/2020 10:00:42 ******/
CREATE FULLTEXT CATALOG [FTC_Documenti] 
GO
/****** Object:  FullTextCatalog [FTC_Notizie]    Script Date: 16/11/2020 10:00:42 ******/
CREATE FULLTEXT CATALOG [FTC_Notizie] AS DEFAULT
GO
/****** Object:  FullTextCatalog [FTC_Oggetti]    Script Date: 16/11/2020 10:00:42 ******/
CREATE FULLTEXT CATALOG [FTC_Oggetti] 
GO
/****** Object:  FullTextCatalog [FTC_PagineStatiche]    Script Date: 16/11/2020 10:00:42 ******/
CREATE FULLTEXT CATALOG [FTC_PagineStatiche] 
GO
/****** Object:  UserDefinedFunction [dbo].[CK_IsDocumentoAiaRegionale]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CK_IsDocumentoAiaRegionale]
(
	-- Add the parameters for the function here
	 @OggettoProceduraID int
)
RETURNS bit
AS
BEGIN

	 RETURN (
	 
		SELECT TOP 1 1 
		FROM TBL_OggettiProcedure OP
		INNER JOIN TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		WHERE OP.OggettoProceduraID = @OggettoProceduraID 
		  AND T.MacroTipoOggettoID = 3
		  AND AIAID IS NULL
	 
	 )

END
GO
/****** Object:  UserDefinedFunction [dbo].[CK_IsOggettoProceduraAiaRegionale]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[CK_IsOggettoProceduraAiaRegionale]
(
	-- Add the parameters for the function here
	 @OggettoID int,
	 @AIAID varchar(15)
)
RETURNS bit
AS
BEGIN

	 RETURN (
	 
		SELECT TOP 1 1 
		FROM TBL_Oggetti O
		INNER JOIN TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		WHERE O.OggettoID = @OggettoID 
		  AND T.MacroTipoOggettoID = 3
		  AND @AIAID IS NULL
	 
	 )

END
GO
/****** Object:  UserDefinedFunction [dbo].[CK_IsProvvedimentoAiaRegionale]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[CK_IsProvvedimentoAiaRegionale]
(
	-- Add the parameters for the function here
	 @OggettoProceduraID int
)
RETURNS bit
AS
BEGIN

	 RETURN (
	 
		SELECT TOP 1 1 
		FROM TBL_OggettiProcedure OP
		INNER JOIN TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		WHERE OP.OggettoProceduraID = @OggettoProceduraID 
		  AND T.MacroTipoOggettoID = 3
		  AND AIAID IS NULL
	 
	 )

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_ApplicaCriterio]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_ApplicaCriterio] 
(
	-- Add the parameters for the function here
	@testo nvarchar(250),
	@criterio int
)
RETURNS nvarchar(250)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Risultato nvarchar(250)

	-- Add the T-SQL statements to compute the return value here
	SET @Risultato = REPLACE(@testo, '[', '[[]');
	SET @Risultato = REPLACE(@Risultato, '%', '[%]');
	SET @Risultato = REPLACE(@Risultato, '_', '[_]');

	SET @Risultato =
	CASE @criterio
		WHEN 0 THEN '%' + REPLACE(@Risultato, ' ', '%') + '%'
		WHEN 1 THEN @Risultato + '%'
		WHEN 2 THEN '%' + @Risultato
		WHEN 3 THEN @Risultato
		WHEN 4 THEN '%[-,.:!"£$%&/()=?''^*+@°#§;_<> ]' + REPLACE(@Risultato, ' ', '%') + '[-,.:!"£$%&/()=?''^*+@°#§;_<> ]%'
	END

	-- Return the result of the function
	RETURN @Risultato

END


GO
/****** Object:  UserDefinedFunction [dbo].[FN_CostruisciWhereLingua]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_CostruisciWhereLingua] 
(
	@testo nvarchar(128),
	--@predicato nvarchar(16), 
	@campo nvarchar(62),
	@lingua nvarchar(2)
)
RETURNS nvarchar(256)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @result nvarchar(256)
	SET @testo = dbo.FN_ApplicaCriterio(@testo, 0);

	-- Add the T-SQL statements to compute the return value here
	SET @result = @campo + '_' + @lingua + ' LIKE ''' + @testo + ''' '

	-- Return the result of the function
	RETURN @result

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_CreaQueryStatoProcedureAIA]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FN_CreaQueryStatoProcedureAIA]
(
	-- Add the parameters for the function here
	 @parametro int,
     @conteggio bit,
     @concluse bit
)
RETURNS nvarchar(MAX)
AS
BEGIN
	 -- Declare the return variable here
     DECLARE @finalQuery nvarchar(MAX)

     -- Add the T-SQL statements to compute the return value here
     DECLARE @nomeProcedura nvarchar(120)
     DECLARE @sqlQuery nvarchar(2000)
     DECLARE @exceptQuery nvarchar(2000)
     DECLARE @inClauseProcedura nvarchar(150)

     -- RICERCA
     DECLARE @filtro nvarchar(255);


     SET @sqlQuery = N'SELECT DISTINCT ' +
                     'O.OggettoID, ' +
                     'O.Nome_IT, O.Nome_EN, ' +
                     'OP.DataInserimento AS ValoreData, ' +
                     'OP.AIAID as ViperaAiaID, OP.OggettoProceduraID, O.TipoOggettoID ' +
                     'FROM TBL_Oggetti AS O INNER JOIN TBL_OggettiProcedure AS OP ' +
                     ' ON O.OggettoID = OP.OggettoID ' +
                     'INNER JOIN dbo.TBL_ValoriDatiAmministrativi AS VDA ' + 
                     '   ON OP.OggettoProceduraID = VDA.OggettoProceduraID ' +
                     'WHERE O.TipoOggettoID = 4 ';

     SET @exceptQuery = N'SELECT DISTINCT O.OggettoID, O.Nome_IT, O.Nome_EN, ' +
                    'OP.DataInserimento AS ValoreData, ' +
                    'OP.AIAID as ViperaAiaID, OP.OggettoProceduraID, O.TipoOggettoID ' +
                    'FROM TBL_Oggetti AS O INNER JOIN ' +
                    ' TBL_OggettiProcedure AS OP ON O.OggettoID = OP.OggettoID INNER JOIN ' +
                    ' TBL_ValoriDatiAmministrativi AS VDA ON ' +
                    ' OP.OggettoProceduraID = VDA.OggettoProceduraID ' +
                    'WHERE O.TipoOggettoID  = 4 ';

	 /* PROCEDURE:
		201		AIA per nuova installazione
		202		Prima AIA per installazione esistente
		203		Rinnovo AIA
		204		Riesame AIA
		205		AIA per modifica sostanziale
		206		Aggiornamento AIA per modifica non sostanziale
		207		Verifica adempimenti prescrizioni
	*/
	
     IF @parametro = 201  -- AIA per nuova installazione
       BEGIN
       
         SET @nomeProcedura = 'AIA per nuova installazione'
         SET @inClauseProcedura = N'201'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ') '
         SET @exceptQuery = @exceptQuery + ' AND (VDA.DatoAmministrativoID = 2017 OR VDA.DatoAmministrativoID = 2032) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           ' (OP.ProceduraID IN (' + @inClauseProcedura + ')) '
       END
     
    
     IF @parametro = 202  -- Prima AIA per installazione esistente
       BEGIN
         
         SET @nomeProcedura = 'Prima AIA per installazione esistente'
         SET @inClauseProcedura = N'202'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
		 SET @exceptQuery = @exceptQuery + ' AND (VDA.DatoAmministrativoID = 2017 OR VDA.DatoAmministrativoID = 2032) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           ' (OP.ProceduraID IN (' + @inClauseProcedura + ')) '
       END

	 IF @parametro = 203  -- Rinnovo AIA
       BEGIN
         
         SET @nomeProcedura = 'Rinnovo AIA'
         SET @inClauseProcedura = N'203'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
         SET @exceptQuery = @exceptQuery + ' AND (VDA.DatoAmministrativoID = 2017 OR VDA.DatoAmministrativoID = 2032) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           ' (OP.ProceduraID IN (' + @inClauseProcedura + ')) '
       END
      
     IF @parametro = 204  -- Riesame AIA
       BEGIN
         
         SET @nomeProcedura = 'Riesame AIA'
         SET @inClauseProcedura = N'204'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
         SET @exceptQuery = @exceptQuery + ' AND (VDA.DatoAmministrativoID = 2017 OR VDA.DatoAmministrativoID = 2032) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           ' (OP.ProceduraID IN (' + @inClauseProcedura + ')) '
       END
     
     IF @parametro = 205  -- AIA per modifica sostanziale
       BEGIN
         
         SET @nomeProcedura = 'AIA per modifica sostanziale'
         SET @inClauseProcedura = N'205'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
         SET @exceptQuery = @exceptQuery + ' AND (VDA.DatoAmministrativoID = 2017 OR VDA.DatoAmministrativoID = 2032) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           ' (OP.ProceduraID IN (' + @inClauseProcedura + ')) '
       END
     
     IF @parametro = 206  -- Aggiornamento AIA per modifica non sostanziale
       BEGIN
         
         SET @nomeProcedura = 'Aggiornamento AIA per modifica non sostanziale'
         SET @inClauseProcedura = N'206'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
         SET @exceptQuery = @exceptQuery + ' AND (VDA.DatoAmministrativoID = 2017 OR VDA.DatoAmministrativoID = 2032) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           ' (OP.ProceduraID IN (' + @inClauseProcedura + ')) '
       END
     
     IF @parametro = 207  -- Verifica adempimenti prescrizioni
       BEGIN
         
         SET @nomeProcedura = 'Verifica adempimenti prescrizioni'
         SET @inClauseProcedura = N'207'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
         SET @exceptQuery = @exceptQuery + ' AND (VDA.DatoAmministrativoID = 2017 OR VDA.DatoAmministrativoID = 2032) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           ' (OP.ProceduraID IN (' + @inClauseProcedura + ')) '
       END
         
     IF @concluse = 1    -- per le procedure concluse si prende solo la query che dava i progetti da scartare..

         SET @finalQuery = @exceptQuery
     ELSE                -- ...altrimenti si scartano
         SET @finalQuery = @sqlQuery + N' EXCEPT ' + @exceptQuery

 
 
 
     IF @conteggio = 1
       BEGIN
         -- CONTEGGIO
         SET @finalQuery = N'SELECT COUNT(*) AS Conteggio, ' + @inClauseProcedura + ' AS ProceduraID, ''' + CAST(@parametro AS nvarchar(5)) + ''' AS Parametro ' + 'FROM (' + @finalQuery +') T'
       END
     ELSE
       -- ?????????????????????????????????????????????????????????????????????????????????
       BEGIN
         -- SELEZIONE -> INNER JOIN CON DATI AMMINISTRATIVI E RICERCA

         SET @finalQuery = N'SELECT TOP (999999) T.*, SPA.StatoAiaID AS StatoProceduraID ' +
           ' FROM (' + @finalQuery + ') AS T ' +
           ' LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = T.OggettoProceduraID ' +
           ' LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID ';
       END
	
     ---- Return the result of the function
     RETURN @finalQuery
	 

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_CreaQueryStatoProcedureVAS]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_CreaQueryStatoProcedureVAS]
(
     -- Add the parameters for the function here
     @parametro int,
     @conteggio bit,
     @concluse bit
)
RETURNS nvarchar(MAX)
AS
BEGIN
     -- Declare the return variable here
     DECLARE @finalQuery nvarchar(MAX)

     -- Add the T-SQL statements to compute the return value here
     DECLARE @nomeProcedura nvarchar(120)
     DECLARE @sqlQuery nvarchar(2000)
     DECLARE @exceptQuery nvarchar(2000)
     DECLARE @inClauseProcedura nvarchar(150)

     -- RICERCA
     DECLARE @filtro nvarchar(255);


     SET @sqlQuery = N'SELECT DISTINCT ' +
                     'O.OggettoID, ' +
                     'O.Nome_IT, O.Nome_EN, ' +
                     'OP.DataInserimento AS ValoreData, ' +
                     'OP.ViperaID as ViperaAiaID, OP.OggettoProceduraID, O.TipoOggettoID ' +
                     'FROM TBL_Oggetti AS O INNER JOIN TBL_OggettiProcedure AS OP ' +
                     ' ON O.OggettoID = OP.OggettoID ' +
                     'INNER JOIN dbo.TBL_ValoriDatiAmministrativi AS VDA ' +
                     '   ON OP.OggettoProceduraID = 
VDA.OggettoProceduraID ' +
                     'WHERE O.TipoOggettoID IN (2,3) ';

     SET @exceptQuery = N'SELECT DISTINCT O.OggettoID, O.Nome_IT, O.Nome_EN, ' +
                        'OP.DataInserimento AS ValoreData, ' +
                        'OP.ViperaID as ViperaAiaID, OP.OggettoProceduraID, O.TipoOggettoID ' +
                        'FROM TBL_Oggetti AS O INNER JOIN ' +
                        ' TBL_OggettiProcedure AS OP ON O.OggettoID = OP.OggettoID INNER JOIN ' +
                        ' TBL_ValoriDatiAmministrativi AS VDA ON ' +
                        ' OP.OggettoProceduraID = VDA.OggettoProceduraID ' +
                        'WHERE O.TipoOggettoID IN (2,3) ';


     IF @parametro = 7  -- Valutazione Ambientale Strategica
       BEGIN
         --Data Provvedimento di VAS (ID_DatoAmmvo: 28)
         --Data notifica esiti procedura al proponente (ID_DatoAmmvo: 48)


         SET @nomeProcedura = 'Valutazione Ambientale Strategica'
         SET @inClauseProcedura = N'102'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
         SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 1028) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           '    (OP.ProceduraID IN (' + 
@inClauseProcedura + ')) OR ' +
                           '    (VDA.DatoAmministrativoID = 627) AND 
(VDA.ValoreData IS NOT NULL) AND ' +
                           '    (OP.ProceduraID IN (' + 
@inClauseProcedura + '))'
       END
     ELSE IF @parametro = 8  -- Verifica di Assoggettabilità alla VAS
       BEGIN
         --Simone, casomai metti le note di Monica?

         SET @nomeProcedura = 'Verifica di Assoggettabilità alla VAS'
         SET @inClauseProcedura = N'107'

         SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID  = ' + @inClauseProcedura + ')'
         SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 1039) AND (VDA.ValoreData IS NOT NULL) AND ' +
                           '    (OP.ProceduraID IN (' + 
@inClauseProcedura + ')) OR ' +
                           '    (VDA.DatoAmministrativoID = 627) AND 
(VDA.ValoreData IS NOT NULL) AND ' +
                           '    (OP.ProceduraID IN (' + 
@inClauseProcedura + '))'
       END

     IF @concluse = 1    -- per le procedure concluse si prende solo la query che dava i progetti da scartare..

         SET @finalQuery = @exceptQuery
     ELSE                -- ...altrimenti si scartano
         SET @finalQuery = @sqlQuery + N' EXCEPT ' + @exceptQuery


     IF @conteggio = 1
       BEGIN
         -- CONTEGGIO
         SET @finalQuery = N'SELECT COUNT(*) AS Conteggio, ' + @inClauseProcedura + ' AS ProceduraID, ''' + CAST(@parametro AS
nvarchar(2)) + ''' AS Parametro ' +
                         'FROM (' + @finalQuery +') T'
       END
     ELSE
       BEGIN
         -- SELEZIONE -> INNER JOIN CON DATI AMMINISTRATIVI E RICERCA

         SET @finalQuery = N'SELECT TOP (999999) T.*, SPV.ProSDeId AS StatoProceduraID ' + 
           ' FROM (' + @finalQuery + ') AS T ' +
           ' LEFT OUTER JOIN [VIPERA].[dbo].[vipProgetti] AS V ON T.ViperaAIAID = V.ProId ' +
           ' LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON V.ProSDeId = SPV.ProSDeId';
       END

     -- Return the result of the function
     RETURN @finalQuery

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_CreaQueryStatoProcedureVIA]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_CreaQueryStatoProcedureVIA]
(
	-- Add the parameters for the function here
	@parametro int,
	@conteggio bit,
	@concluse bit
)
RETURNS nvarchar(MAX)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @finalQuery nvarchar(MAX)

	-- Add the T-SQL statements to compute the return value here
	DECLARE @nomeProcedura nvarchar(120)
	DECLARE @sqlQuery nvarchar(2000)
	DECLARE @exceptQuery nvarchar(2000)
	DECLARE @inClauseProcedura nvarchar(150)

	-- RICERCA
	DECLARE @filtro nvarchar(255);

	SET @sqlQuery = N'SELECT DISTINCT ' +
					'O.OggettoID, O.Nome_IT, O.Nome_EN, ' +
					'OP.DataInserimento AS ValoreData, ' +
					'OP.ViperaID as ViperaAiaID, OP.OggettoProceduraID, O.TipoOggettoID ' +
					'FROM  dbo.TBL_Oggetti AS O ' +
					'INNER JOIN dbo.TBL_OggettiProcedure AS OP ' +
					'	ON O.OggettoID = OP.OggettoID ' +
					'INNER JOIN dbo.TBL_ValoriDatiAmministrativi AS VDA ' +
					'   ON OP.OggettoProceduraID = VDA.OggettoProceduraID ' + 
					'WHERE O.TipoOggettoID = 1 ';
					
	SET @exceptQuery = N'SELECT DISTINCT O.OggettoID, O.Nome_IT, O.Nome_EN, ' +
					   'OP.DataInserimento AS ValoreData, ' +
					   'OP.ViperaID as ViperaAiaID, OP.OggettoProceduraID, O.TipoOggettoID ' +
					   'FROM TBL_Oggetti AS O INNER JOIN ' +
					   ' TBL_OggettiProcedure AS OP ON O.OggettoID = OP.OggettoID INNER JOIN ' +
					   ' TBL_ValoriDatiAmministrativi AS VDA ON ' +
					   ' OP.OggettoProceduraID = VDA.OggettoProceduraID ' + 
					   'WHERE O.TipoOggettoID = 1 ';
					

	IF @parametro = 1  -- Procedura di verifica di esclusione/assoggettabilità alla VIA
	  BEGIN
	    --Data Determinazione direttoriale di Esclusione/Assoggettabilità alla VIA
		--Data notifica esiti procedura al proponente (nel caso di archiviazione)

		SET @nomeProcedura = 'Verifica di Esclusione/Assoggettabilità alla VIA'
		SET @inClauseProcedura = N'5'
		
		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')'
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 483) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 1065) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
	  END
	ELSE IF @parametro = 2  -- Procedura VIA
	  BEGIN
	    --Data Decreto VIA
		--Data notifica esiti procedura al proponente (nel caso di archiviazione)

		SET @nomeProcedura = 'Valutazione Impatto Ambientale'
		SET @inClauseProcedura = N'3'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')'
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 63) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 483) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' + 
						  '	(VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 1065) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
	  END
	ELSE IF @parametro = 3  -- Procedura VIA (Legge Obiettivo)
	  BEGIN
	    ------------------------------------------------------------------------------
	    --Escludere tutte le procedure che hanno:
	    -- Data trasmissione parere al MIT
	    -- Data Delibera CIPE
	    -- Data notifica esiti procedura al proponente (nel caso di esito negativo o di archiviazione)
	  
		SET @nomeProcedura = 'Valutazione Impatto Ambientale (Legge Obiettivo)'
		SET @inClauseProcedura = N'14'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')' 
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 137) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 138) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' + 
						  '	(VDA.DatoAmministrativoID = 483) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' + 
						  '	(VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 1065) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
						  
	    ------------------------------------------------------------------------------
	  END
	ELSE IF @parametro = 4  -- Procedura Verifica di Ottemperanza (Legge Obiettivo)
	  BEGIN
	    --Data notifica esiti procedura al proponente 
	  
		SET @nomeProcedura = 'Verifica di Ottemperanza (Legge Obiettivo)'
		SET @inClauseProcedura = N'18'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')'
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 483) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 1065) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
		
	  END
	ELSE IF @parametro = 7  -- Procedura Verifica di Ottemperanza
	  BEGIN
	    --Data notifica esiti procedura al proponente 
	  
		SET @nomeProcedura = 'Verifica di Ottemperanza'
		SET @inClauseProcedura = N'7'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')'
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 483) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 1065) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
		
	  END
	ELSE IF @parametro = 5  -- Procedura Verifica di Attuazione (Legge Obiettivo)
	  BEGIN
	    --Data notifica esiti procedura al proponente
		--Data approvazione Relazione finale di verifica e controllo

		SET @nomeProcedura = 'Verifica di Attuazione (Legge Obiettivo)'
		SET @inClauseProcedura = N'20'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')'
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 666) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
	  END
	ELSE IF @parametro = 6  -- Varianti (Legge Obiettivo)
	  BEGIN
	    --Data notifica esiti procedura al proponente

		SET @nomeProcedura = 'Varianti (Legge Obiettivo)'
		SET @inClauseProcedura = N'22'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')'
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 137) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 138) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 483) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 1065) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
	  END
	ELSE IF @parametro = 9  -- Procedura Scoping
	  BEGIN
	    --Data notifica esiti procedura al proponente 
	  
		SET @nomeProcedura = 'Scoping (art.21 D.Lgs.152/2006 e s.m.i.)'
		SET @inClauseProcedura = N'1'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')'
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 627) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 483) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + ')) OR ' +
						  '	(VDA.DatoAmministrativoID = 1065) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
	  END
	ELSE IF @parametro = 10  -- Procedura PUA
	  BEGIN
	    ------------------------------------------------------------------------------
	    -- Data Determinazione conclusiva Conferenza dei Servizi
	  
		SET @nomeProcedura = 'Valutazione Impatto Ambientale (Provvedimento Unico in materia Ambientale)'
		SET @inClauseProcedura = N'110'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')' 
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 688) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
						  
	    ------------------------------------------------------------------------------
	  END
	ELSE IF @parametro = 11  -- Procedura Valutazione Preliminare
	  BEGIN
	    ------------------------------------------------------------------------------
	    -- Data comunicazione esito al proponente
	  
		SET @nomeProcedura = 'Valutazione Preliminare'
		SET @inClauseProcedura = N'24'

		SET @sqlQuery = @sqlQuery + N'AND (OP.ProceduraID = ' + @inClauseProcedura + ')' 
		SET @exceptQuery = @exceptQuery + 'AND (VDA.DatoAmministrativoID = 1079) AND (VDA.ValoreData IS NOT NULL) AND ' +
						  '	(OP.ProceduraID IN (' + @inClauseProcedura + '))'
						  
	    ------------------------------------------------------------------------------
	  END

	IF @concluse = 1	-- per le procedure concluse si prende solo la query che dava i progetti da scartare..
		SET @finalQuery = @exceptQuery
	ELSE				-- ...altrimenti si scartano
		SET @finalQuery = @sqlQuery + N' EXCEPT ' + @exceptQuery

	
	IF @conteggio = 1
	  BEGIN
	    -- CONTEGGIO	    
		SET @finalQuery = N'SELECT COUNT(*) AS Conteggio, ' + @inClauseProcedura + ' AS ProceduraID, ''' + CAST(@parametro AS nvarchar(2)) + ''' AS Parametro ' +
						'FROM (' + @finalQuery +') T'
	  END
	ELSE
	  BEGIN
		-- SELEZIONE -> INNER JOIN CON VIPERA E RICERCA
		--SET @filtro = dbo.FN_ApplicaCriterio(@testo, 0);		
		
		SET @finalQuery = N'SELECT TOP (999999) T.*, SPV.ProSDeId AS StatoProceduraID ' + 
		  ' FROM (' + @finalQuery + ') AS T ' + 
		  ' LEFT OUTER JOIN [VIPERA].[dbo].[vipProgetti] AS V ON T.ViperaAiaID = V.ProId ' + 
		  ' LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON V.ProSDeId = SPV.ProSDeId';
	  END

	-- Return the result of the function
	RETURN @finalQuery

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_DataScadenzaPresentazioneOsservazioni]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_DataScadenzaPresentazioneOsservazioni]
(
	-- Add the parameters for the function here
	@OggettoID int
)
RETURNS Datetime
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Data datetime

	DECLARE @OggettoProceduraID int
	SET @OggettoProceduraID = (SELECT dbo.FN_UltimoOggettoProceduraIDPerOggetto(@OggettoID));

	DECLARE @ProceduraID int
	SET @ProceduraID = (SELECT ProceduraID FROM dbo.TBL_OggettiProcedure WHERE OggettoProceduraID = @OggettoProceduraID);
	
	-- Add the T-SQL statements to compute the return value here
	SELECT @Data = VDA.ValoreData 
	FROM dbo.TBL_ValoriDatiAmministrativi AS VDA INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = VDA.OggettoProceduraID
	WHERE OP.OggettoProceduraID = @OggettoProceduraID AND
		OP.ProceduraID = @ProceduraID AND 
		VDA.DatoAmministrativoID IN (39, 47, 672, 1016, 1022, 1074)

	-- Return the result of the function
	RETURN @data

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_DataScadenzaPresentazioneOsservazioniPerOggettoProcedura]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_DataScadenzaPresentazioneOsservazioniPerOggettoProcedura]
(
	@OggettoProceduraID int
)
RETURNS Datetime
AS
BEGIN
	DECLARE @Data datetime = NULL;

	-- TOP 1 Order by Data Desc

	SET @Data = (SELECT TOP 1 VDA.ValoreData 
	FROM dbo.TBL_ValoriDatiAmministrativi AS VDA INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = VDA.OggettoProceduraID
	WHERE OP.OggettoProceduraID = @OggettoProceduraID AND
		VDA.DatoAmministrativoID IN (39, 47, 672, 1016, 1022, 1074)
	ORDER BY VDA.ValoreData DESC);

	-- Return the result of the function
	RETURN @data

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_FigliRaggruppamento]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date, ,>
-- Description:	<Description, ,>
-- =============================================
CREATE FUNCTION [dbo].[FN_FigliRaggruppamento] 
(
	-- Add the parameters for the function here
	@RaggruppamentoID int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @Figli int

	-- Add the T-SQL statements to compute the return value here
	SELECT @Figli = (SELECT COUNT(*) FROM dbo.TBL_Raggruppamenti WHERE GenitoreID = @RaggruppamentoID)

	-- Return the result of the function
	RETURN @Figli

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_SintesiNonTecnicaID]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_SintesiNonTecnicaID]
(
	@OggettoProceduraID int, 
	@TipoOggettoID int
)
RETURNS int
AS
BEGIN
	DECLARE @documentoID int
	DECLARE @tipoFileID int
	
	SET @tipoFileID = 5 -- PDF

	IF(@TipoOggettoID = 1) -- VIA
	BEGIN
	
		SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 127
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC)
			, 0)

		IF (@documentoID = 0)
		  SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 7
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC), 0)

		IF (@documentoID = 0)
		  SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 227
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC), 0)
			
		IF (@documentoID = 0)
		  SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 469
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC), 0)
			
		IF (@documentoID = 0)
		  SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 482
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC), 0)
			
		IF (@documentoID = 0)
		  SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 382
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC), 0)		
			
		IF (@documentoID = 0)
		  SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 262
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC), 0)			
	END
	ELSE -- VAS
	BEGIN
	
		SET @documentoID = ISNULL(
			(SELECT TOP(1) D.DocumentoID
			FROM dbo.TBL_Documenti AS D INNER JOIN 
				dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID
			WHERE D.RaggruppamentoID = 1006
			 AND D.TipoFileID = @tipoFileID 
			 AND OP.OggettoProceduraID = @OggettoProceduraID
			 AND D.LivelloVisibilita = 1
			ORDER BY D.Ordinamento ASC, D.DocumentoID ASC)
			, 0)
			
	END
	
	RETURN @documentoID

END
GO
/****** Object:  UserDefinedFunction [dbo].[FN_UltimoOggettoProceduraIDPerOggetto]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_UltimoOggettoProceduraIDPerOggetto]
(
	@OggettoID int
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	DECLARE @OggettoProceduraID int

	-- Add the T-SQL statements to compute the return value here
	SET @OggettoProceduraID = (SELECT TOP(1) OggettoProceduraID FROM dbo.TBL_OggettiProcedure WHERE OggettoID = @OggettoID ORDER BY DataInserimento DESC);

	-- Return the result of the function
	RETURN @OggettoProceduraID

END
GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_ConcatenaArgomentiPerDocumento_EN]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[FT_FN_ConcatenaArgomentiPerDocumento_EN] (@DocumentoID int)  
RETURNS varchar(2000) AS 
BEGIN
-- Dichiarazioni
DECLARE @risultato varchar(2000);

-- Inizializzazioni
SET @risultato = '';

-- Esecuzione
SELECT @risultato = @risultato + ISNULL(Nome_EN, '') + ', '
FROM (
SELECT     A.Nome_EN
FROM         dbo.TBL_Argomenti AS A INNER JOIN
             dbo.STG_DocumentiArgomenti AS STG ON A.ArgomentoID = STG.ArgomentoID
 WHERE STG.DocumentoID = @DocumentoID) t;

IF (LEN(@risultato) > 0)
	SET @risultato = LEFT(@risultato, LEN(@risultato)-1)

RETURN @risultato;
END

GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_ConcatenaArgomentiPerDocumento_IT]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE FUNCTION [dbo].[FT_FN_ConcatenaArgomentiPerDocumento_IT] (@DocumentoID int)  
RETURNS varchar(2000) AS 
BEGIN
-- Dichiarazioni
DECLARE @risultato varchar(2000);

-- Inizializzazioni
SET @risultato = '';

-- Esecuzione
SELECT @risultato = @risultato + ISNULL(Nome_IT, '') + ', '
FROM (
SELECT     A.Nome_IT
FROM         dbo.TBL_Argomenti AS A INNER JOIN
             dbo.STG_DocumentiArgomenti AS STG ON A.ArgomentoID = STG.ArgomentoID
 WHERE STG.DocumentoID = @DocumentoID) t;

IF (LEN(@risultato) > 0)
	SET @risultato = LEFT(@risultato, LEN(@risultato)-1)

RETURN @risultato;
END

GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_ConcatenaAutoriPerDocumento]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[FT_FN_ConcatenaAutoriPerDocumento] (@DocumentoID int)  
RETURNS varchar(256) AS 
BEGIN
-- Dichiarazioni
DECLARE @risultato varchar(256);

-- Inizializzazioni
SET @risultato = '';

-- Esecuzione
SELECT @risultato = @risultato + ISNULL(Nome, '') + ' '
FROM (SELECT DISTINCT E.Nome FROM dbo.TBL_Entita AS E INNER JOIN 
	dbo.STG_DocumentiEntita AS SDE ON SDE.EntitaID = E.EntitaID
 WHERE SDE.DocumentoID = @DocumentoID AND SDE.RuoloEntitaID = 4) t; -- Autore

RETURN @risultato;
END



GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_ConcatenaProponentiPerOggetto]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[FT_FN_ConcatenaProponentiPerOggetto] (@OggettoID int)  
RETURNS varchar(256) AS 
BEGIN
-- Dichiarazioni
DECLARE @risultato varchar(256);

-- Inizializzazioni
SET @risultato = '';

-- Esecuzione
SELECT @risultato = @risultato + ISNULL(Nome, '') + ' '
FROM (SELECT DISTINCT E.Nome FROM dbo.TBL_Entita AS E INNER JOIN 
	dbo.STG_OggettiProcedureEntita AS SOE ON SOE.EntitaID = E.EntitaID INNER JOIN
	dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = SOE.OggettoProceduraID
 WHERE OP.OggettoID = @OggettoID AND SOE.RuoloEntitaID in (1,10)) t; -- Proponente

RETURN @risultato;
END
GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_ConcatenaTerritoriPerOggetto]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO



CREATE FUNCTION [dbo].[FT_FN_ConcatenaTerritoriPerOggetto] (@OggettoID int)  
RETURNS varchar(2048) AS 
BEGIN
-- Dichiarazioni
DECLARE @risultato varchar(2048);

-- Inizializzazioni
SET @risultato = '';

-- Esecuzione
SELECT @risultato = @risultato + ISNULL(Nome, '') + ' '
FROM (SELECT T.Nome FROM dbo.TBL_Territori AS T INNER JOIN dbo.STG_OggettiTerritori AS SOT
	ON T.TerritorioID = SOT.TerritorioID
 WHERE SOT.OggettoID = @OggettoID) t;

RETURN @risultato;
END



GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_CreaQueryDocumenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FT_FN_CreaQueryDocumenti]
(
	-- Add the parameters for the function here
	@Lingua varchar(2)
)
RETURNS nvarchar(4000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @finalQuery nvarchar(4000)

	-- Add the T-SQL statements to compute the return value here
	
	IF @Lingua = N'EN'
		BEGIN
	
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'(' +
			'SELECT [KEY], ([RANK] * 5) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Titolo), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Descrizione), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (CodiceElaborato), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 3) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (NomeOggetto_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (DescrizioneOggetto_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Argomenti_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 3) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Autore), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 3) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Proponente), @TestoRicerca, LANGUAGE N''Italian'') ' +
			') FT ' +
			'GROUP BY FT.[Key]' +
			') AS FTL ON D.DocumentoID = FTL.[KEY] ';
		END
	ELSE
		BEGIN
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'(' +
			'SELECT [KEY], ([RANK] * 5) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Titolo), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Descrizione), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (CodiceElaborato), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 3) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (NomeOggetto_IT), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (DescrizioneOggetto_IT), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Argomenti_IT), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 3) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Autore), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 3) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Documenti, (Proponente), @TestoRicerca, LANGUAGE N''Italian'') ' +
			') FT ' +
			'GROUP BY FT.[Key]' +
			') AS FTL ON D.DocumentoID = FTL.[KEY] ';
		END

	-- Return the result of the function
	RETURN @finalQuery

END




GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_CreaQueryNotizie]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '0' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '1' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '2' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '3' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '4' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '5' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '6' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '7' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '8' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '9' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'II' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'III' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'IV' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'VI' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'VII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'VIII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XI' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XIII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XIV' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XIX' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XV' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XVI' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XVII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XVIII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XX' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXI' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXIII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXIV' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXIX' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXV' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXVI' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXVII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXVIII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXX' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXI' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXIII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXIV' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXIX' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXV' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXVI' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXVII' LANGUAGE 'Italian';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXVIII' LANGUAGE 'Italian';
--GO
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '0' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '1' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '2' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '3' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '4' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '5' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '6' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '7' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '8' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP '9' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'II' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'III' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'IV' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'VI' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'VII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'VIII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XI' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XIII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XIV' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XIX' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XV' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XVI' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XVII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XVIII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XX' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXI' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXIII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXIV' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXIX' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXV' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXVI' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXVII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXVIII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXX' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXI' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXIII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXIV' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXIX' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXV' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXVI' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXVII' LANGUAGE 'English';
--ALTER FULLTEXT STOPLIST FTSL_Notizie DROP 'XXXVIII' LANGUAGE 'English';
--GO

CREATE FUNCTION [dbo].[FT_FN_CreaQueryNotizie]
(
	-- Add the parameters for the function here
	@Lingua varchar(2)
)
RETURNS nvarchar(4000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @finalQuery nvarchar(4000)

	-- Add the T-SQL statements to compute the return value here
	
	IF @Lingua = N'EN'
		BEGIN
	
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'(' +
			'SELECT [KEY], ([RANK] * 5) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Notizie, (Titolo_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Notizie, (Testo_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Notizie, (Abstract_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			') FT ' +
			'GROUP BY FT.[Key]' +
			') AS FTL ON N.NotiziaID = FTL.[KEY] ';
		END
	ELSE
		BEGIN
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'( ' +
			'SELECT [KEY], ([RANK] * 5) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Notizie, (Titolo_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Notizie, (Testo_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Notizie, (Abstract_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			') FT  ' +
			'GROUP BY FT.[Key] ' +
			') AS FTL ON N.NotiziaID = FTL.[KEY] ';
		END

	-- Return the result of the function
	RETURN @finalQuery

END

GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_CreaQueryOggetti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FT_FN_CreaQueryOggetti]
(
	-- Add the parameters for the function here
	@Lingua varchar(2)
)
RETURNS nvarchar(4000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @finalQuery nvarchar(4000)

	-- Add the T-SQL statements to compute the return value here
	
	IF @Lingua = N'EN'
		BEGIN
	
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'(' +
			'SELECT [KEY], ([RANK] * 5) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Nome_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Descrizione_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (NomeOpera_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Territori), @TestoRicerca, LANGUAGE N''Italian'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 4) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Proponente), @TestoRicerca, LANGUAGE N''Italian'') ' +
			') FT ' +
			'GROUP BY FT.[Key]' +
			') AS FTL ON O.OggettoID = FTL.[KEY] ';
		END
	ELSE
		BEGIN
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'( ' +
			'SELECT [KEY], ([RANK] * 5) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Nome_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Descrizione_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (NomeOpera_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Territori), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 4) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Proponente), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			') FT  ' +
			'GROUP BY FT.[Key] ' +
			') AS FTL ON O.OggettoID = FTL.[KEY] ';
		END

	-- Return the result of the function
	RETURN @finalQuery

END




GO
/****** Object:  UserDefinedFunction [dbo].[FT_FN_CreaQueryPagineStatiche]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FT_FN_CreaQueryPagineStatiche]
(
	-- Add the parameters for the function here
	@Lingua varchar(2)
)
RETURNS nvarchar(4000)
AS
BEGIN
	-- Declare the return variable here
	DECLARE @finalQuery nvarchar(4000)

	-- Add the T-SQL statements to compute the return value here
	
	IF @Lingua = N'EN'
		BEGIN
	
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'(' +
			'SELECT [KEY], ([RANK] * 5) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_PagineStatiche, (Nome_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_PagineStatiche, (Descrizione_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_PagineStatiche, (Testo_EN), @TestoRicerca, LANGUAGE N''English'') ' +
			') FT ' +
			'GROUP BY FT.[Key]' +
			') AS FTL ON VM.VoceMenuID = FTL.[KEY] ';
		END
	ELSE
		BEGIN
	SET @finalQuery = N' INNER JOIN (SELECT FT.[Key], SUM(FT.[RANK]) [RANK] FROM ' +
			'( ' +
			'SELECT [KEY], ([RANK] * 5) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_PagineStatiche, (Nome_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 1) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_PagineStatiche, (Descrizione_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			' UNION ALL ' +
			'SELECT [KEY], ([RANK] * 2) [RANK] ' +
			'	FROM FREETEXTTABLE(dbo.FTL_PagineStatiche, (Testo_IT), @TestoRicerca, LANGUAGE N''Italian'')  ' +
			') FT  ' +
			'GROUP BY FT.[Key] ' +
			') AS FTL ON VM.VoceMenuID = FTL.[KEY] ';
		END

	-- Return the result of the function
	RETURN @finalQuery

END





GO
/****** Object:  Table [dbo].[TBL_TipiOggetto]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_TipiOggetto](
	[TipoOggettoID] [int] NOT NULL,
	[MacroTipoOggettoID] [int] NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
	[Descrizione] [varchar](256) NULL,
 CONSTRAINT [PK_TBL_TipiOggetto] PRIMARY KEY CLUSTERED 
(
	[TipoOggettoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Oggetti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Oggetti](
	[OggettoID] [int] IDENTITY(1,1) NOT NULL,
	[TipoOggettoID] [int] NOT NULL,
	[Nome_IT] [varchar](1024) NOT NULL,
	[Nome_EN] [varchar](1024) NOT NULL,
	[Descrizione_IT] [varchar](1024) NULL,
	[Descrizione_EN] [varchar](1024) NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[LatitudineNord] [float] NULL,
	[LatitudineSud] [float] NULL,
	[LongitudineEst] [float] NULL,
	[LongitudineOvest] [float] NULL,
	[ImmagineLocalizzazione] [varchar](512) NULL,
 CONSTRAINT [PK_TBL_Oggetti] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_OggettiProcedure]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_OggettiProcedure](
	[OggettoProceduraID] [int] IDENTITY(1,1) NOT NULL,
	[OggettoID] [int] NOT NULL,
	[ProceduraID] [int] NULL,
	[FaseProgettazioneID] [int] NOT NULL,
	[ValutatoreID] [int] NOT NULL,
	[UltimaProcedura] [bit] NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[ViperaID] [int] NULL,
	[AIAID] [varchar](15) NULL,
 CONSTRAINT [PK_TBL_OggettiProcedure] PRIMARY KEY CLUSTERED 
(
	[OggettoProceduraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_TBL_ProcedureAvviate]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE FUNCTION [dbo].[FN_TBL_ProcedureAvviate]
(	
	@Anno int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT OP.OggettoProceduraID, OP.ProceduraID
		FROM dbo.TBL_OggettiProcedure AS OP
		INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
	WHERE 
		YEAR(OP.DataInserimento) = @anno
	AND 
		(T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL)
)
GO
/****** Object:  Table [dbo].[TBL_Procedure]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Procedure](
	[ProceduraID] [int] NOT NULL,
	[MacroTipoOggettoID] [int] NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
	[AmbitoProceduraID] [int] NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_TBL_Procedure] PRIMARY KEY CLUSTERED 
(
	[ProceduraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ValoriDatiAmministrativi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ValoriDatiAmministrativi](
	[OggettoProceduraID] [int] NOT NULL,
	[DatoAmministrativoID] [int] NOT NULL,
	[ValoreTesto] [varchar](2048) NULL,
	[ValoreData] [datetime] NULL,
	[ValoreNumero] [float] NULL,
	[ValoreBooleano] [bit] NULL,
 CONSTRAINT [PK_TBL_ValoriDatiAmministrativi] PRIMARY KEY CLUSTERED 
(
	[OggettoProceduraID] ASC,
	[DatoAmministrativoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  UserDefinedFunction [dbo].[FN_TBL_ProcedureConcluse]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_TBL_ProcedureConcluse]
(	
	@Anno int
)
RETURNS TABLE 
AS
RETURN 
(
	SELECT 	OP.OggettoProceduraID, TBL_Procedure.ProceduraID
	FROM	TBL_OggettiProcedure  OP 
	INNER JOIN	TBL_Procedure ON OP.ProceduraID = TBL_Procedure.ProceduraID
	INNER JOIN  TBL_Oggetti O ON O.OggettoID = OP.OggettoID
	INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID			
	WHERE 
		(OP.OggettoProceduraID IN
			(
				 SELECT OggettoProceduraID
				 FROM   TBL_ValoriDatiAmministrativi
				 WHERE  (DatoAmministrativoID IN (63, 137, 483, 627, 666, 1028, 1039, 1061, 1065, 688, 1079, 2017, 2032)) 
				 AND (ValoreData IS NOT NULL) 
				 AND (YEAR(ValoreData) = @anno)
			 )
		 AND
			 (T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL)
		)
	
	--- VERSIONE ORIGINALE
	
	--SELECT       TBL_OggettiProcedure.OggettoProceduraID, TBL_Procedure.ProceduraID
	--FROM         TBL_OggettiProcedure 
	--INNER JOIN   TBL_Procedure ON TBL_OggettiProcedure.ProceduraID = TBL_Procedure.ProceduraID
	--WHERE        (TBL_OggettiProcedure.OggettoProceduraID IN
 --                   (SELECT    OggettoProceduraID
	--					FROM   TBL_ValoriDatiAmministrativi
	--					WHERE        
	--					(DatoAmministrativoID IN (63, 137, 483, 627, 666, 1028, 1039, 1061, 1065, 688, 1079, 2017, 2032)) 
	--					AND (ValoreData IS NOT NULL) 
	--					AND (YEAR(ValoreData) = @anno)
						
	--				)
						
	--			)
						
)
GO
/****** Object:  UserDefinedFunction [dbo].[FN_TBL_ProcedureInCorso]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE FUNCTION [dbo].[FN_TBL_ProcedureInCorso]
(	
	@Anno int
)
RETURNS TABLE 
AS
RETURN 
(
	
	SELECT OP.OggettoProceduraID, OP.ProceduraID
	FROM dbo.TBL_OggettiProcedure AS OP
		INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		WHERE T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL
	EXCEPT 
	SELECT  TBL_OggettiProcedure.OggettoProceduraID, TBL_Procedure.ProceduraID
	FROM TBL_OggettiProcedure 
		INNER JOIN TBL_Procedure ON TBL_OggettiProcedure.ProceduraID = TBL_Procedure.ProceduraID
	WHERE TBL_OggettiProcedure.OggettoProceduraID IN
			(SELECT        OggettoProceduraID
				FROM            TBL_ValoriDatiAmministrativi
				WHERE        
				(DatoAmministrativoID IN (63, 137, 138, 483, 627, 666, 1028, 1039, 1061, 1065, 688, 1079, 2017, 2032)) 
				AND (ValoreData IS NOT NULL) 
			)
)
GO
/****** Object:  Table [dbo].[FTL_Documenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FTL_Documenti](
	[DocumentoID] [int] NOT NULL,
	[Titolo] [nvarchar](1024) NOT NULL,
	[Descrizione] [nvarchar](2048) NOT NULL,
	[CodiceElaborato] [varchar](128) NOT NULL,
	[NomeOggetto_IT] [varchar](1024) NOT NULL,
	[NomeOggetto_EN] [varchar](1024) NOT NULL,
	[DescrizioneOggetto_IT] [varchar](1024) NOT NULL,
	[DescrizioneOggetto_EN] [varchar](1024) NOT NULL,
	[Argomenti_IT] [varchar](512) NOT NULL,
	[Argomenti_EN] [varchar](512) NOT NULL,
	[Autore] [varchar](256) NOT NULL,
	[Proponente] [varchar](256) NOT NULL,
 CONSTRAINT [pk_FTL_Documenti_DocumentoID] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FTL_Notizie]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FTL_Notizie](
	[NotiziaID] [int] NOT NULL,
	[Titolo_IT] [nvarchar](256) NOT NULL,
	[Titolo_EN] [nvarchar](256) NOT NULL,
	[Abstract_IT] [nvarchar](512) NOT NULL,
	[Abstract_EN] [nvarchar](512) NOT NULL,
	[Testo_IT] [nvarchar](max) NOT NULL,
	[Testo_EN] [nvarchar](max) NOT NULL,
 CONSTRAINT [pk_FTL_Notizie_NotiziaID] PRIMARY KEY CLUSTERED 
(
	[NotiziaID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FTL_Oggetti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FTL_Oggetti](
	[OggettoID] [int] NOT NULL,
	[Nome_IT] [varchar](1024) NOT NULL,
	[Nome_EN] [varchar](1024) NOT NULL,
	[Descrizione_IT] [varchar](1024) NOT NULL,
	[Descrizione_EN] [varchar](1024) NOT NULL,
	[NomeOpera_IT] [varchar](256) NOT NULL,
	[NomeOpera_EN] [varchar](256) NOT NULL,
	[Territori] [varchar](2048) NOT NULL,
	[Proponente] [varchar](256) NOT NULL,
 CONSTRAINT [pk_FTL_Oggetti_OggettoID] PRIMARY KEY CLUSTERED 
(
	[OggettoID] DESC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[FTL_PagineStatiche]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[FTL_PagineStatiche](
	[VoceMenuID] [int] NOT NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NOT NULL,
	[Descrizione_IT] [varchar](256) NOT NULL,
	[Descrizione_EN] [varchar](256) NOT NULL,
	[Testo_IT] [varchar](4000) NOT NULL,
	[Testo_EN] [varchar](4000) NOT NULL,
 CONSTRAINT [pk_FTL_PagineStatiche_VoceMenuID] PRIMARY KEY CLUSTERED 
(
	[VoceMenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAstgCategoriaImpArgomento]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAstgCategoriaImpArgomento](
	[ArgomentoID] [uniqueidentifier] NOT NULL,
	[CategoriaImpiantoID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_AIAstgCategoriaImpArgomento] PRIMARY KEY CLUSTERED 
(
	[ArgomentoID] ASC,
	[CategoriaImpiantoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAstgEventiOggetti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAstgEventiOggetti](
	[EventoID] [int] NOT NULL,
	[OggettoID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_AIA_stgEventiOggetti] PRIMARY KEY CLUSTERED 
(
	[EventoID] ASC,
	[OggettoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAstgEventiOggettoProcedura]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAstgEventiOggettoProcedura](
	[EventoID] [int] NOT NULL,
	[OggettoProceduraID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_AIA_stgEventiOggettoProcedura] PRIMARY KEY CLUSTERED 
(
	[EventoID] ASC,
	[OggettoProceduraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAstgEventiUtenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAstgEventiUtenti](
	[EventoID] [int] NOT NULL,
	[UtenteID] [int] NOT NULL,
 CONSTRAINT [IX_GEMMA_AIAstgEventiUtenti_1] UNIQUE NONCLUSTERED 
(
	[EventoID] ASC,
	[UtenteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAstgGruppiUtenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAstgGruppiUtenti](
	[GruppoID] [int] NOT NULL,
	[UtenteID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_AIA_stgGruppiUtenti] PRIMARY KEY CLUSTERED 
(
	[GruppoID] ASC,
	[UtenteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAstgStatiOggettiProcedure]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAstgStatiOggettiProcedure](
	[StatoId] [int] NOT NULL,
	[OggettoProceduraID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblDocumenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblDocumenti](
	[DocumentoID] [int] IDENTITY(1,1) NOT NULL,
	[RaggruppamentoID] [int] NOT NULL,
	[Titolo] [nvarchar](1024) NOT NULL,
	[NomeFile] [varchar](256) NOT NULL,
	[PercorsoFile] [varchar](512) NOT NULL,
	[DataPubblicazione] [datetime] NOT NULL,
	[LivelloVisibilita] [int] NOT NULL,
	[UtenteID] [int] NULL,
	[EventoID] [int] NULL,
	[Dimensione] [int] NOT NULL,
	[Ordinamento] [int] NOT NULL,
	[IdDocumentoAiaStorico] [int] NULL,
 CONSTRAINT [PK_GEMMA_AIA_tblDocumenti] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblEventi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblEventi](
	[EventoID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NULL,
	[DataInizio] [datetime] NULL,
	[DataFine] [datetime] NULL,
	[Abilitato] [bit] NOT NULL,
	[TipoEventoID] [int] NOT NULL,
	[RaggruppamentoID] [int] NULL,
 CONSTRAINT [PK_GEMMA_AIA_tblEventi] PRIMARY KEY CLUSTERED 
(
	[EventoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblGruppi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblGruppi](
	[GruppoID] [int] NOT NULL,
	[RuoloID] [int] NOT NULL,
	[Nome] [varchar](100) NULL,
	[TerritorioID] [uniqueidentifier] NULL,
 CONSTRAINT [PK_GEMMA_AIA_tblGruppi] PRIMARY KEY CLUSTERED 
(
	[GruppoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblGruppiRaggruppamentiEsclusi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblGruppiRaggruppamentiEsclusi](
	[GruppoID] [int] NOT NULL,
	[RaggruppamentoID] [int] NOT NULL
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblPagamenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblPagamenti](
	[PagamentoID] [int] IDENTITY(1,1) NOT NULL,
	[OggettoProceduraID] [int] NOT NULL,
	[DataPagamento] [datetime] NULL,
	[Importo] [decimal](18, 2) NULL,
	[EstremiPagamento] [varchar](255) NULL,
	[NotaPagamento] [varchar](255) NULL,
	[TipoTariffaID] [int] NULL,
	[AnnoRiferimento] [varchar](255) NULL,
	[NomeFile] [varchar](256) NULL,
	[PercorsoFile] [varchar](512) NULL,
 CONSTRAINT [PK_Gemma_AIAtblPagamenti] PRIMARY KEY CLUSTERED 
(
	[PagamentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblPagamentiStoricoAia]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblPagamentiStoricoAia](
	[PagamentoID] [int] NOT NULL,
	[IDDocumento] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_AIAtblPagamentiStoricoAia] PRIMARY KEY CLUSTERED 
(
	[PagamentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblRaggruppamenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblRaggruppamenti](
	[RaggruppamentoID] [int] IDENTITY(1,1) NOT NULL,
	[GenitoreID] [int] NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NULL,
	[Descrizione] [varchar](256) NULL,
	[LivelloVisibilita] [int] NOT NULL,
	[Ordine] [int] NOT NULL,
	[IdClasseAiaStorico] [int] NULL,
 CONSTRAINT [PK_GEMMA_AIAtblRaggruppamenti] PRIMARY KEY CLUSTERED 
(
	[RaggruppamentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblRuoli]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblRuoli](
	[RuoloID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](50) NOT NULL,
	[Note] [varchar](255) NULL,
	[RaggruppamentoID] [int] NULL,
 CONSTRAINT [PK_GEMMA_AIA_tblRuoli] PRIMARY KEY CLUSTERED 
(
	[RuoloID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblTipiEvento]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblTipiEvento](
	[TipoEventoID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](50) NULL,
	[Nome_EN] [varchar](50) NULL,
	[RaggruppamentoID] [int] NULL,
 CONSTRAINT [PK_GEMMA_AIA_tblTipiEvento] PRIMARY KEY CLUSTERED 
(
	[TipoEventoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblTipoTariffa]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblTipoTariffa](
	[TipoTariffaID] [int] NOT NULL,
	[Descrizione] [varchar](250) NOT NULL,
 CONSTRAINT [PK_GEMMA_AIAtblTipoTariffa] PRIMARY KEY CLUSTERED 
(
	[TipoTariffaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_AIAtblUtenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_AIAtblUtenti](
	[UtenteID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Username] [varchar](50) NOT NULL,
	[Abilitato] [bit] NOT NULL,
 CONSTRAINT [PK_GEMMA_AIA_tblUtenti] PRIMARY KEY CLUSTERED 
(
	[UtenteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_ResponsabiliProcedimento]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_ResponsabiliProcedimento](
	[ResponsabileProcedimentoID] [tinyint] IDENTITY(1,1) NOT NULL,
	[Nome] [nvarchar](64) NOT NULL,
	[Telefono] [nvarchar](16) NOT NULL,
	[Email] [nvarchar](128) NOT NULL,
 CONSTRAINT [PK_GEMMA_ResponsabiliProcedimento] PRIMARY KEY CLUSTERED 
(
	[ResponsabileProcedimentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_SistemiGestAmbientale]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_SistemiGestAmbientale](
	[SistAmbID] [int] NOT NULL,
	[Nome] [varchar](250) NULL,
 CONSTRAINT [PK_GEMMA_SistemiAmbientali] PRIMARY KEY CLUSTERED 
(
	[SistAmbID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_STG_ProcedureDatiAmministrativi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_STG_ProcedureDatiAmministrativi](
	[ProceduraID] [int] NOT NULL,
	[DatoAmministrativoID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_STG_ProcedureDatiAmministrativi] PRIMARY KEY CLUSTERED 
(
	[ProceduraID] ASC,
	[DatoAmministrativoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_STG_ProcedureRaggruppamenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_STG_ProcedureRaggruppamenti](
	[ProceduraID] [int] NOT NULL,
	[RaggruppamentoID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_STG_ProcedureRaggruppamenti] PRIMARY KEY CLUSTERED 
(
	[ProceduraID] ASC,
	[RaggruppamentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_STG_RaggruppamentiEntita]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_STG_RaggruppamentiEntita](
	[EntitaID] [nvarchar](255) NOT NULL,
	[RaggruppamentoID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_STG_RaggruppamentiEntita] PRIMARY KEY CLUSTERED 
(
	[EntitaID] ASC,
	[RaggruppamentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_VASstgSettoreArgomento]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_VASstgSettoreArgomento](
	[ArgomentoID] [uniqueidentifier] NOT NULL,
	[SettoreID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_VASstgSettoreArgomento] PRIMARY KEY CLUSTERED 
(
	[ArgomentoID] ASC,
	[SettoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_VIAstgMacroTemiProgettiProcedure]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_VIAstgMacroTemiProgettiProcedure](
	[OggettoID] [int] NOT NULL,
	[ProceduraID] [int] NOT NULL,
	[MacrotemaID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_VIAstgMacroTemiProgettiProcedure] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC,
	[ProceduraID] ASC,
	[MacrotemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_VIAstgTipologiaArgomento]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_VIAstgTipologiaArgomento](
	[ArgomentoID] [uniqueidentifier] NOT NULL,
	[TipologiaID] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_VIAstgTipologiaArgomento] PRIMARY KEY CLUSTERED 
(
	[ArgomentoID] ASC,
	[TipologiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[GEMMA_VIAtblMacroTemi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[GEMMA_VIAtblMacroTemi](
	[MacroTemaID] [int] NOT NULL,
	[Nome] [varchar](250) NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_GEMMA_VIAtblMacroTemi] PRIMARY KEY CLUSTERED 
(
	[MacroTemaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_DocumentiArgomenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_DocumentiArgomenti](
	[DocumentoID] [int] NOT NULL,
	[ArgomentoID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_STG_DocumentiArgomenti] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC,
	[ArgomentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_DocumentiEntita]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_DocumentiEntita](
	[DocumentoID] [int] NOT NULL,
	[EntitaID] [int] NOT NULL,
	[RuoloEntitaID] [int] NOT NULL,
 CONSTRAINT [PK_STG_DocumentiRubrica_1] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC,
	[EntitaID] ASC,
	[RuoloEntitaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_OggettiImpiantiAttivitaIppc]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_OggettiImpiantiAttivitaIppc](
	[OggettoID] [int] NOT NULL,
	[AttivitaIppcID] [int] NOT NULL,
 CONSTRAINT [PK_STG_OggettiImpiantiAttivitaIppc] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC,
	[AttivitaIppcID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_OggettiLink]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_OggettiLink](
	[OggettoID] [int] NOT NULL,
	[LinkID] [int] NOT NULL,
	[TipoLinkID] [int] NOT NULL,
 CONSTRAINT [PK_STG_OggettiLink] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC,
	[LinkID] ASC,
	[TipoLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_OggettiProcedureAttributi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_OggettiProcedureAttributi](
	[OggettoProceduraID] [int] NOT NULL,
	[AttributoID] [int] NOT NULL,
	[Widget] [bit] NOT NULL,
 CONSTRAINT [PK_STG_OggettiProcedureAttributi] PRIMARY KEY CLUSTERED 
(
	[OggettoProceduraID] ASC,
	[AttributoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_OggettiProcedureEntita]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_OggettiProcedureEntita](
	[OggettoProceduraID] [int] NOT NULL,
	[EntitaID] [int] NOT NULL,
	[RuoloEntitaID] [int] NOT NULL,
 CONSTRAINT [PK_STG_OggettiProcedureEntita] PRIMARY KEY CLUSTERED 
(
	[OggettoProceduraID] ASC,
	[EntitaID] ASC,
	[RuoloEntitaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_OggettiTerritori]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_OggettiTerritori](
	[OggettoID] [int] NOT NULL,
	[TerritorioID] [uniqueidentifier] NOT NULL,
 CONSTRAINT [PK_STG_OggettiTerritori] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC,
	[TerritorioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_ProvvedimentiDocumenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_ProvvedimentiDocumenti](
	[ProvvedimentoID] [int] NOT NULL,
	[DocumentoID] [int] NOT NULL,
 CONSTRAINT [PK_STG_ProvvedimentiDocumenti] PRIMARY KEY CLUSTERED 
(
	[ProvvedimentoID] ASC,
	[DocumentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_UI_VociMenuTipiAttributi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_UI_VociMenuTipiAttributi](
	[VoceMenuID] [int] NOT NULL,
	[TipoAttributoID] [int] NOT NULL,
 CONSTRAINT [PK_STG_UI_VociMenuTipiAttributi] PRIMARY KEY CLUSTERED 
(
	[VoceMenuID] ASC,
	[TipoAttributoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_UI_VociMenuTipiProvvedimenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_UI_VociMenuTipiProvvedimenti](
	[VoceMenuID] [int] NOT NULL,
	[TipoProvvedimentoID] [int] NOT NULL,
 CONSTRAINT [pk_STG_UI_VociMenuTipiProvvedimenti] PRIMARY KEY CLUSTERED 
(
	[VoceMenuID] ASC,
	[TipoProvvedimentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_UI_VociMenuWidget]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_UI_VociMenuWidget](
	[VoceMenuID] [int] NOT NULL,
	[WidgetID] [int] NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_STG_UI_VociMenuWidget] PRIMARY KEY CLUSTERED 
(
	[VoceMenuID] ASC,
	[WidgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[STG_UtentiRuoliUtente]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[STG_UtentiRuoliUtente](
	[UtenteID] [int] NOT NULL,
	[RuoloUtenteID] [int] NOT NULL,
 CONSTRAINT [pk_UtenteIDRuoliUtenteID] PRIMARY KEY CLUSTERED 
(
	[UtenteID] ASC,
	[RuoloUtenteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_AmbitiProcedure]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_AmbitiProcedure](
	[AmbitoProceduraID] [int] NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [pk_TBL_AmbitiProcedure_AmbitoProceduraID] PRIMARY KEY CLUSTERED 
(
	[AmbitoProceduraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_AreeTipiProvvedimenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_AreeTipiProvvedimenti](
	[AreaTipoProvvedimentoID] [int] NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_TBL_AreeTipiProvvedimenti] PRIMARY KEY CLUSTERED 
(
	[AreaTipoProvvedimentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Argomenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Argomenti](
	[ArgomentoID] [uniqueidentifier] NOT NULL,
	[GenitoreID] [uniqueidentifier] NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TBL_Argomenti] PRIMARY KEY CLUSTERED 
(
	[ArgomentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_AttivitaIppc]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_AttivitaIppc](
	[AttivitaIppcID] [int] NOT NULL,
	[Codice] [varchar](10) NULL,
	[Categoria] [tinyint] NULL,
	[Livello] [tinyint] NULL,
	[Nome_IT] [varchar](1024) NULL,
	[Nome_EN] [varchar](1024) NULL,
	[RifNormativo] [varchar](50) NULL,
 CONSTRAINT [PK_TBL_AttivitaIppc] PRIMARY KEY CLUSTERED 
(
	[AttivitaIppcID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Attributi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Attributi](
	[AttributoID] [int] NOT NULL,
	[TipoAttributoID] [int] NOT NULL,
	[DatoAmministrativoID] [int] NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
	[Ordine] [int] NOT NULL,
	[MacroTipoOggettoID] [int] NOT NULL,
 CONSTRAINT [PK_TBL_Attributi] PRIMARY KEY CLUSTERED 
(
	[AttributoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_CategorieImpianti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_CategorieImpianti](
	[CategoriaImpiantoID] [int] NOT NULL,
	[Nome_IT] [varchar](1024) NOT NULL,
	[Nome_EN] [varchar](1024) NOT NULL,
	[Descrizione] [varchar](1024) NULL,
	[FileIcona] [varchar](32) NOT NULL,
 CONSTRAINT [PK_TBL_CategorieImpianti] PRIMARY KEY CLUSTERED 
(
	[CategoriaImpiantoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_CategorieNotizie]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_CategorieNotizie](
	[CategoriaNotiziaID] [int] NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TBL_CategorieNotizie] PRIMARY KEY CLUSTERED 
(
	[CategoriaNotiziaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ClassiAiaStorico]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ClassiAiaStorico](
	[RaggruppamentoID] [int] NOT NULL,
	[IdClasseDocumento] [int] NULL,
	[PercorsoClasse] [varchar](100) NULL,
	[PercorsoClasseTesto] [varchar](500) NULL,
	[EtichettaWeb] [varchar](256) NULL,
 CONSTRAINT [PK_TBL_ClassiAiaStorico] PRIMARY KEY CLUSTERED 
(
	[RaggruppamentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_DatiAmministrativi]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_DatiAmministrativi](
	[DatoAmministrativoID] [int] NOT NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NOT NULL,
	[Descrizione] [varchar](256) NOT NULL,
	[LivelloVisibilita] [int] NOT NULL,
	[Ordine] [int] NOT NULL,
	[TipoDati] [varchar](8) NOT NULL,
	[TipoDatoAmministrativo] [int] NOT NULL,
 CONSTRAINT [PK_TBL_DatiAmministrativi] PRIMARY KEY CLUSTERED 
(
	[DatoAmministrativoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Documenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Documenti](
	[DocumentoID] [int] IDENTITY(1,1) NOT NULL,
	[OggettoProceduraID] [int] NOT NULL,
	[RaggruppamentoID] [int] NOT NULL,
	[TipoFileID] [int] NOT NULL,
	[CodiceElaborato] [varchar](128) NOT NULL,
	[Titolo] [nvarchar](1024) NOT NULL,
	[Descrizione] [nvarchar](2048) NULL,
	[Scala] [varchar](64) NULL,
	[Tipologia] [varchar](32) NOT NULL,
	[Dimensione] [int] NOT NULL,
	[LivelloVisibilita] [int] NOT NULL,
	[NomeFile] [varchar](256) NOT NULL,
	[PercorsoFile] [varchar](512) NOT NULL,
	[DataPubblicazione] [datetime] NOT NULL,
	[DataStesura] [datetime] NULL,
	[LinguaDocumento] [varchar](32) NOT NULL,
	[LinguaMetadato] [varchar](32) NOT NULL,
	[Diritti] [varchar](256) NULL,
	[Ordinamento] [int] NOT NULL,
 CONSTRAINT [PK_TBL_Documenti] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_DocumentiAiaStorico]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_DocumentiAiaStorico](
	[DocumentoID] [int] NOT NULL,
	[IDDocumento] [int] NULL,
 CONSTRAINT [PK_TBL_DocumentiAiaStorico] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_DocumentiPortale]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_DocumentiPortale](
	[DocumentoPortaleID] [int] IDENTITY(1,1) NOT NULL,
	[TipoFileID] [int] NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
	[NomeFileOriginale] [varchar](128) NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
	[Dimensione] [int] NOT NULL,
 CONSTRAINT [pk_TBL_DocumentiPortale] PRIMARY KEY CLUSTERED 
(
	[DocumentoPortaleID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Email]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Email](
	[EmailID] [int] IDENTITY(1,1) NOT NULL,
	[Testo] [nvarchar](max) NOT NULL,
	[IndirizzoEmail] [varchar](128) NOT NULL,
	[Tipo] [varchar](16) NOT NULL,
	[DataInvio] [datetime] NOT NULL,
 CONSTRAINT [PK_TBL_Email] PRIMARY KEY CLUSTERED 
(
	[EmailID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Entita]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Entita](
	[EntitaID] [int] IDENTITY(1,1) NOT NULL,
	[CodiceFiscale] [varchar](16) NULL,
	[Nome] [varchar](256) NOT NULL,
	[Indirizzo] [varchar](128) NULL,
	[Cap] [varchar](16) NULL,
	[Citta] [varchar](64) NULL,
	[Provincia] [varchar](64) NULL,
	[Telefono] [varchar](64) NULL,
	[Fax] [varchar](64) NULL,
	[Email] [varchar](128) NULL,
	[Pec] [varchar](128) NULL,
	[SitoWeb] [varchar](128) NULL,
	[PuntoContatto] [varchar](128) NULL,
	[MacroTipoOggettoID] [int] NULL,
 CONSTRAINT [PK_TBL_Entita] PRIMARY KEY CLUSTERED 
(
	[EntitaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ExtraDocumenti]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ExtraDocumenti](
	[DocumentoID] [int] NOT NULL,
	[Riferimenti] [varchar](256) NULL,
	[Origine] [varchar](256) NULL,
	[Copertura] [varchar](512) NULL,
 CONSTRAINT [PK_TBL_ExtraDocumenti] PRIMARY KEY CLUSTERED 
(
	[DocumentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ExtraOggettiImpianto]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ExtraOggettiImpianto](
	[OggettoID] [int] NOT NULL,
	[ImpiantoAiaID] [int] NULL,
	[CategoriaImpiantoID] [int] NULL,
	[CapImpianto] [char](5) NULL,
	[IndirizzoImpianto] [varchar](255) NULL,
	[CompetenzaStataleStorico] [int] NULL,
	[StatoImpiantiID] [int] NOT NULL,
	[Regionale] [int] NOT NULL,
 CONSTRAINT [PK_TBL_ExtraOggettiImpianto] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ExtraOggettiPianoProgramma]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ExtraOggettiPianoProgramma](
	[OggettoID] [int] NOT NULL,
	[SettoreID] [int] NOT NULL,
 CONSTRAINT [PK_TBL_ExtraOggettiPianiProgrammi] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ExtraOggettiProceduraAia]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ExtraOggettiProceduraAia](
	[OggettoProceduraID] [int] NOT NULL,
	[StatoAiaID] [int] NOT NULL,
	[CodFascicolo] [varchar](50) NULL,
	[DomandaAiaID] [int] NULL,
	[ProceduraCollegataID] [int] NULL,
	[ProvvedimentoCollegatoID] [int] NULL,
 CONSTRAINT [PK_TBL_ExtraOggettiProceduraAia] PRIMARY KEY CLUSTERED 
(
	[OggettoProceduraID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ExtraOggettiProgetto]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ExtraOggettiProgetto](
	[OggettoID] [int] NOT NULL,
	[OperaID] [int] NOT NULL,
	[Cup] [varchar](15) NULL,
 CONSTRAINT [PK_TBL_ExtraOggetti] PRIMARY KEY CLUSTERED 
(
	[OggettoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_ExtraProvvedimentiAia]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_ExtraProvvedimentiAia](
	[ProvvedimentoID] [int] NOT NULL,
	[AutorizzazioneAiaID] [int] NULL,
	[DataScadenzaAutorizzazione] [datetime] NULL,
	[LivelloVisibilita] [int] NOT NULL,
 CONSTRAINT [PK_TBL_AutorizzazioniAiaStorico] PRIMARY KEY CLUSTERED 
(
	[ProvvedimentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_FasiProgettazione]    Script Date: 16/11/2020 10:00:42 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_FasiProgettazione](
	[FaseProgettazioneID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
 CONSTRAINT [PK_TBL_FasiProgettazione] PRIMARY KEY CLUSTERED 
(
	[FaseProgettazioneID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_FormatiImmagine]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_FormatiImmagine](
	[FormatoImmagineID] [int] NOT NULL,
	[Nome] [varchar](64) NOT NULL,
	[AltezzaMax] [int] NOT NULL,
	[AltezzaMin] [int] NOT NULL,
	[LarghezzaMax] [int] NOT NULL,
	[LarghezzaMin] [int] NOT NULL,
	[Abilitato] [bit] NULL,
 CONSTRAINT [PK_TBL_FormatiImmagine] PRIMARY KEY CLUSTERED 
(
	[FormatoImmagineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Immagini]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Immagini](
	[ImmagineID] [int] IDENTITY(1,1) NOT NULL,
	[ImmagineMasterID] [int] NOT NULL,
	[FormatoImmagineID] [int] NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
	[Altezza] [int] NOT NULL,
	[Larghezza] [int] NOT NULL,
	[NomeFile] [varchar](64) NOT NULL,
 CONSTRAINT [PK_TBL_Immagini] PRIMARY KEY CLUSTERED 
(
	[ImmagineID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Link]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Link](
	[LinkID] [int] IDENTITY(1,1) NOT FOR REPLICATION NOT NULL,
	[Nome] [varchar](255) NOT NULL,
	[Descrizione] [varchar](255) NOT NULL,
	[Indirizzo] [varchar](255) NOT NULL,
 CONSTRAINT [PK_TBL_Link] PRIMARY KEY CLUSTERED 
(
	[LinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_MacroTipiOggetto]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_MacroTipiOggetto](
	[MacroTipoOggettoID] [int] NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
	[NomeAbbreviato] [varchar](8) NOT NULL,
 CONSTRAINT [PK_TBL_MacroTipiOggetto] PRIMARY KEY CLUSTERED 
(
	[MacroTipoOggettoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_MacroTipologie]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_MacroTipologie](
	[MacrotipologiaID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](100) NOT NULL,
	[Nome_EN] [varchar](100) NULL,
 CONSTRAINT [PK_TBL_MacroTipologie] PRIMARY KEY CLUSTERED 
(
	[MacrotipologiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Notizie]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Notizie](
	[NotiziaID] [int] IDENTITY(1,1) NOT NULL,
	[CategoriaNotiziaID] [int] NOT NULL,
	[ImmagineID] [int] NULL,
	[Data] [datetime] NOT NULL,
	[Titolo_IT] [nvarchar](256) NOT NULL,
	[Titolo_EN] [nvarchar](256) NOT NULL,
	[TitoloBreve_IT] [nvarchar](256) NOT NULL,
	[TitoloBreve_EN] [nvarchar](256) NOT NULL,
	[Abstract_IT] [nvarchar](512) NOT NULL,
	[Abstract_EN] [nvarchar](512) NOT NULL,
	[Testo_IT] [nvarchar](max) NOT NULL,
	[Testo_EN] [nvarchar](max) NOT NULL,
	[Pubblicata] [bit] NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
	[Stato] [int] NOT NULL,
 CONSTRAINT [PK_TBL_Notizie] PRIMARY KEY CLUSTERED 
(
	[NotiziaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Opere]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Opere](
	[OperaID] [int] IDENTITY(1,1) NOT NULL,
	[TipologiaID] [int] NOT NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NOT NULL,
	[Alias_IT] [varchar](256) NOT NULL,
	[Alias_EN] [varchar](256) NOT NULL,
	[DataInserimento] [datetime] NULL,
 CONSTRAINT [PK_TBL_Opere] PRIMARY KEY CLUSTERED 
(
	[OperaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Provvedimenti]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Provvedimenti](
	[ProvvedimentoID] [int] IDENTITY(1,1) NOT NULL,
	[TipoProvvedimentoID] [int] NULL,
	[OggettoProceduraID] [int] NOT NULL,
	[EntitaID] [int] NOT NULL,
	[NumeroProtocollo] [varchar](50) NOT NULL,
	[Data] [datetime] NULL,
	[Oggetto_IT] [varchar](1024) NULL,
	[Oggetto_EN] [varchar](1024) NULL,
	[Esito] [varchar](128) NOT NULL,
 CONSTRAINT [PK_TBL_Provvedimenti] PRIMARY KEY CLUSTERED 
(
	[ProvvedimentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Raggruppamenti]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Raggruppamenti](
	[RaggruppamentoID] [int] NOT NULL,
	[GenitoreID] [int] NULL,
	[MacroTipoOggettoID] [int] NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NOT NULL,
	[Descrizione] [varchar](500) NULL,
	[LivelloVisibilita] [int] NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_TBL_Raggruppamenti] PRIMARY KEY CLUSTERED 
(
	[RaggruppamentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_RuoliEntita]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_RuoliEntita](
	[RuoloEntitaID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
 CONSTRAINT [PK_TBL_RuoliEntita] PRIMARY KEY CLUSTERED 
(
	[RuoloEntitaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_RuoliUtente]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_RuoliUtente](
	[RuoloUtenteID] [int] NOT NULL,
	[Codice] [varchar](16) NOT NULL,
	[Nome] [varchar](64) NOT NULL,
 CONSTRAINT [pk_TBL_RuoliUtente_RuoloUtenteID] PRIMARY KEY CLUSTERED 
(
	[RuoloUtenteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
UNIQUE NONCLUSTERED 
(
	[Codice] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Settori]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Settori](
	[SettoreID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](100) NOT NULL,
	[Nome_EN] [varchar](100) NULL,
 CONSTRAINT [PK_TBL_Settori] PRIMARY KEY CLUSTERED 
(
	[SettoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_StatiProceduraAIA]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_StatiProceduraAIA](
	[StatoAiaID] [int] NOT NULL,
	[Nome_IT] [varchar](1024) NOT NULL,
	[Nome_EN] [varchar](1024) NOT NULL,
	[NomeAreaRiservata] [nvarchar](255) NULL,
	[Ordine] [int] NULL,
 CONSTRAINT [PK_TBL_StatiProceduraAIA] PRIMARY KEY CLUSTERED 
(
	[StatoAiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_StatiProceduraVIPERA]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_StatiProceduraVIPERA](
	[ProSDeId] [int] NOT NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NOT NULL,
 CONSTRAINT [PK_TBL_StatoProceduraVIPERA] PRIMARY KEY CLUSTERED 
(
	[ProSDeId] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_StatoImpianti]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_StatoImpianti](
	[StatoImpiantiID] [int] NOT NULL,
	[Nome_IT] [varchar](1024) NOT NULL,
	[Nome_EN] [varchar](1024) NOT NULL,
 CONSTRAINT [PK_TBL_StatoImpianti] PRIMARY KEY CLUSTERED 
(
	[StatoImpiantiID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Territori]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Territori](
	[TerritorioID] [uniqueidentifier] NOT NULL,
	[GenitoreID] [uniqueidentifier] NULL,
	[TipologiaTerritorioID] [int] NOT NULL,
	[Nome] [varchar](70) NOT NULL,
	[CodiceIstat] [varchar](10) NULL,
	[LatitudineNord] [float] NULL,
	[LatitudineSud] [float] NULL,
	[LongitudineEst] [float] NULL,
	[LongitudineOvest] [float] NULL,
 CONSTRAINT [PK_TBL_Territori] PRIMARY KEY CLUSTERED 
(
	[TerritorioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_TipiAttributi]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_TipiAttributi](
	[TipoAttributoID] [int] NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_TBL_TipiAttributi] PRIMARY KEY CLUSTERED 
(
	[TipoAttributoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_TipiFile]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_TipiFile](
	[TipoFileID] [int] IDENTITY(1,1) NOT NULL,
	[FileIcona] [varchar](16) NOT NULL,
	[Estensione] [varchar](8) NOT NULL,
	[TipoMIME] [varchar](32) NOT NULL,
	[Software] [varchar](32) NOT NULL,
 CONSTRAINT [PK_TBL_TipiFile] PRIMARY KEY CLUSTERED 
(
	[TipoFileID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_TipiLink]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_TipiLink](
	[TipoLinkID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
 CONSTRAINT [PK_TBL_TipiLink] PRIMARY KEY CLUSTERED 
(
	[TipoLinkID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_TipiProvvedimenti]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_TipiProvvedimenti](
	[TipoProvvedimentoID] [int] IDENTITY(1,1) NOT NULL,
	[AreaTipoProvvedimentoID] [int] NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_TBL_TipiProvvedimenti] PRIMARY KEY CLUSTERED 
(
	[TipoProvvedimentoID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Tipologie]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Tipologie](
	[TipologiaID] [int] IDENTITY(1,1) NOT NULL,
	[MacroTipologiaID] [int] NOT NULL,
	[Nome_IT] [varchar](100) NOT NULL,
	[Nome_EN] [varchar](100) NOT NULL,
	[FileIcona] [varchar](32) NOT NULL,
 CONSTRAINT [PK_TBL_Tipologie] PRIMARY KEY CLUSTERED 
(
	[TipologiaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_TipologieTerritorio]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_TipologieTerritorio](
	[TipologiaTerritorioID] [int] IDENTITY(1,1) NOT NULL,
	[Nome] [varchar](24) NOT NULL,
	[Nome_EN] [varchar](24) NOT NULL,
	[MostraRicerca] [bit] NOT NULL,
 CONSTRAINT [PK_TBL_TipologieTerritorio] PRIMARY KEY CLUSTERED 
(
	[TipologiaTerritorioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_DatiAmbientaliHome]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_DatiAmbientaliHome](
	[DatoAmbientaleHomeID] [int] IDENTITY(1,1) NOT NULL,
	[ImmagineID] [int] NOT NULL,
	[Titolo_IT] [varchar](128) NOT NULL,
	[Titolo_EN] [varchar](128) NOT NULL,
	[Link] [varchar](256) NOT NULL,
	[Pubblicato] [bit] NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
 CONSTRAINT [PK_TBL_UI_DatiAmbientaliHome] PRIMARY KEY CLUSTERED 
(
	[DatoAmbientaleHomeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_Lingue]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_Lingue](
	[LinguaID] [int] NOT NULL,
	[Nome] [varchar](16) NOT NULL,
 CONSTRAINT [PK_TBL_UI_Lingue] PRIMARY KEY CLUSTERED 
(
	[LinguaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_OggettiCarosello]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_OggettiCarosello](
	[OggettoCaroselloID] [int] IDENTITY(1,1) NOT NULL,
	[TipoContenutoID] [int] NOT NULL,
	[ContenutoID] [int] NOT NULL,
	[ImmagineID] [int] NOT NULL,
	[Data] [datetime] NOT NULL,
	[Nome_IT] [varchar](1024) NOT NULL,
	[Nome_EN] [varchar](1024) NOT NULL,
	[Descrizione_IT] [varchar](1024) NOT NULL,
	[Descrizione_EN] [varchar](1024) NOT NULL,
	[LinkOggetto] [varchar](256) NULL,
	[LinkProgettoCartografico] [varchar](256) NULL,
	[Pubblicato] [bit] NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
 CONSTRAINT [PK_TBL_UI_OggettiCarosello] PRIMARY KEY CLUSTERED 
(
	[OggettoCaroselloID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_PagineStatiche]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_PagineStatiche](
	[PaginaStaticaID] [int] IDENTITY(1,1) NOT NULL,
	[VoceMenuID] [int] NOT NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
	[Testo_IT] [varchar](max) NULL,
	[Testo_EN] [varchar](max) NULL,
	[Visibile] [bit] NOT NULL,
 CONSTRAINT [PK_TBL_UI_PagineStatiche] PRIMARY KEY CLUSTERED 
(
	[PaginaStaticaID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_Variabili]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_Variabili](
	[Chiave] [varchar](32) NOT NULL,
	[Valore] [varchar](1024) NOT NULL,
 CONSTRAINT [PK_TBL_UI_Variabili] PRIMARY KEY CLUSTERED 
(
	[Chiave] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_VociDizionario]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_VociDizionario](
	[VoceDizionarioID] [int] NOT NULL,
	[Sezione] [varchar](32) NOT NULL,
	[Nome] [varchar](64) NOT NULL,
	[Valore_IT] [varchar](512) NOT NULL,
	[Valore_EN] [varchar](512) NOT NULL,
 CONSTRAINT [PK_TBL_UI_VociDizionario] PRIMARY KEY CLUSTERED 
(
	[VoceDizionarioID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_VociMenu]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_VociMenu](
	[VoceMenuID] [int] NOT NULL,
	[GenitoreID] [int] NOT NULL,
	[TipoMenu] [int] NOT NULL,
	[Nome_IT] [varchar](256) NOT NULL,
	[Nome_EN] [varchar](256) NOT NULL,
	[Descrizione_IT] [varchar](256) NOT NULL,
	[Descrizione_EN] [varchar](256) NOT NULL,
	[Sezione] [varchar](32) NOT NULL,
	[Voce] [varchar](32) NOT NULL,
	[Link] [bit] NOT NULL,
	[Editabile] [bit] NOT NULL,
	[VisibileFrontEnd] [bit] NOT NULL,
	[VisibileMappa] [bit] NOT NULL,
	[WidgetAbilitati] [bit] NOT NULL,
	[Ordine] [int] NOT NULL,
 CONSTRAINT [PK_TBL_UI_VociMenu] PRIMARY KEY CLUSTERED 
(
	[VoceMenuID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_UI_Widget]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_UI_Widget](
	[WidgetID] [int] IDENTITY(1,1) NOT NULL,
	[TipoWidget] [int] NOT NULL,
	[Nome_IT] [varchar](64) NOT NULL,
	[Nome_EN] [varchar](64) NOT NULL,
	[CategoriaNotiziaID] [int] NULL,
	[NumeroElementi] [int] NULL,
	[DataInserimento] [datetime] NOT NULL,
	[DataUltimaModifica] [datetime] NOT NULL,
	[VoceMenuID] [int] NULL,
	[Contenuto_IT] [varchar](max) NULL,
	[Contenuto_EN] [varchar](max) NULL,
	[MostraTitolo] [bit] NOT NULL,
	[NotiziaID] [int] NULL,
 CONSTRAINT [PK_TBL_UI_Widget] PRIMARY KEY CLUSTERED 
(
	[WidgetID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Utenti]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Utenti](
	[UtenteID] [int] IDENTITY(1,1) NOT NULL,
	[Ruolo] [int] NOT NULL,
	[NomeUtente] [varchar](32) NOT NULL,
	[Pswd] [varchar](128) NULL,
	[Abilitato] [bit] NOT NULL,
	[DataUltimoCambioPassword] [datetime] NULL,
	[DataUltimoLogin] [datetime] NULL,
	[Email] [varchar](100) NOT NULL,
	[Nome] [varchar](100) NOT NULL,
	[Cognome] [varchar](100) NOT NULL,
 CONSTRAINT [PK_TBL_Utenti] PRIMARY KEY CLUSTERED 
(
	[UtenteID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY],
 CONSTRAINT [UQ__TBL_Uten__A9D1053452C9DD8E] UNIQUE NONCLUSTERED 
(
	[Email] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_Valutatori]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_Valutatori](
	[ValutatoreID] [int] IDENTITY(1,1) NOT NULL,
	[Nome_IT] [varchar](128) NOT NULL,
	[Nome_EN] [varchar](128) NOT NULL,
	[Descrizione_IT] [varchar](256) NOT NULL,
	[Descrizione_EN] [varchar](256) NOT NULL,
 CONSTRAINT [PK_TBL_Commissioni] PRIMARY KEY CLUSTERED 
(
	[ValutatoreID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_WebEvents]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_WebEvents](
	[EventID] [uniqueidentifier] NOT NULL,
	[EventTimeUtc] [datetime] NOT NULL,
	[EventTime] [datetime] NOT NULL,
	[EventType] [varchar](256) NOT NULL,
	[EventSequence] [bigint] NOT NULL,
	[EventOccurrence] [bigint] NOT NULL,
	[EventCode] [int] NOT NULL,
	[EventDetailCode] [int] NOT NULL,
	[EventMessage] [nvarchar](1024) NULL,
	[MachineName] [varchar](256) NOT NULL,
	[ApplicationPath] [varchar](256) NULL,
	[ApplicationVirtualPath] [varchar](256) NULL,
	[RequestUrl] [nvarchar](1024) NULL,
	[RequestUserAgent] [nvarchar](1024) NULL,
	[RequestUrlReferrer] [nvarchar](1024) NULL,
	[PrincipalIdentityIsAuthenticated] [bit] NULL,
	[PrincipalIdentityName] [varchar](256) NULL,
	[UserHostAddress] [varchar](32) NULL,
	[ExceptionType] [varchar](256) NULL,
	[ExceptionMessage] [nvarchar](1024) NULL,
	[UtenteID] [int] NULL,
	[UtenteNomeUtente] [varchar](256) NULL,
	[IntEntityID] [int] NULL,
	[GuidEntityID] [uniqueidentifier] NULL,
	[WebEventTypeID] [int] NOT NULL,
	[Details] [nvarchar](max) NULL,
 CONSTRAINT [pk_TBL_WebEvents_EventID] PRIMARY KEY CLUSTERED 
(
	[EventID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY] TEXTIMAGE_ON [PRIMARY]
GO
/****** Object:  Table [dbo].[TBL_WebEventTypes]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE TABLE [dbo].[TBL_WebEventTypes](
	[WebEventTypeID] [int] NOT NULL,
	[Name] [nvarchar](128) NOT NULL,
 CONSTRAINT [pk_TBL_WebEventTypes_WebEventTypeID] PRIMARY KEY CLUSTERED 
(
	[WebEventTypeID] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
) ON [PRIMARY]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblDocumenti] ADD  CONSTRAINT [DF_GEMMA_AIAtblDocumenti_Dimensione]  DEFAULT ((0)) FOR [Dimensione]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblEventi] ADD  CONSTRAINT [DF_GEMMA_AIAtblEventi_Nome_IT]  DEFAULT ('') FOR [Nome_IT]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblRaggruppamenti] ADD  CONSTRAINT [DF_GEMMA_AIAtblRaggruppamenti_Nome_IT]  DEFAULT ('') FOR [Nome_IT]
GO
ALTER TABLE [dbo].[TBL_CategorieImpianti] ADD  CONSTRAINT [DF_TBL_CategorieImpianti_FileIcona]  DEFAULT ('') FOR [FileIcona]
GO
ALTER TABLE [dbo].[TBL_DocumentiPortale] ADD  CONSTRAINT [DF_TBL_DocumentiPortale_DataInserimento]  DEFAULT (getdate()) FOR [DataInserimento]
GO
ALTER TABLE [dbo].[TBL_DocumentiPortale] ADD  CONSTRAINT [DF_TBL_DocumentiPortale_DataUltimaModifica]  DEFAULT (getdate()) FOR [DataUltimaModifica]
GO
ALTER TABLE [dbo].[TBL_Entita] ADD  CONSTRAINT [DF_TBL_Entita_MacroTipoOggettoID]  DEFAULT ((1)) FOR [MacroTipoOggettoID]
GO
ALTER TABLE [dbo].[TBL_MacroTipologie] ADD  CONSTRAINT [DF_TBL_MacroTipologie_Nome_En]  DEFAULT ('') FOR [Nome_EN]
GO
ALTER TABLE [dbo].[TBL_Settori] ADD  CONSTRAINT [DF_TBL_Settori_Nome_En]  DEFAULT ('') FOR [Nome_EN]
GO
ALTER TABLE [dbo].[TBL_Tipologie] ADD  CONSTRAINT [DF_TBL_Tipologie_FileIcona]  DEFAULT ('') FOR [FileIcona]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggetti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_stgEventiOggetti_TBL_Oggetti] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_Oggetti] ([OggettoID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggetti] CHECK CONSTRAINT [FK_GEMMA_AIA_stgEventiOggetti_TBL_Oggetti]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggetti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_stgEventiOggetti_tblEventi] FOREIGN KEY([EventoID])
REFERENCES [dbo].[GEMMA_AIAtblEventi] ([EventoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggetti] CHECK CONSTRAINT [FK_GEMMA_AIA_stgEventiOggetti_tblEventi]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggettoProcedura]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_stgEventiOggettoProcedura_TBL_OggettiProcedure] FOREIGN KEY([OggettoProceduraID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggettoProcedura] CHECK CONSTRAINT [FK_GEMMA_AIA_stgEventiOggettoProcedura_TBL_OggettiProcedure]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggettoProcedura]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_stgEventiOggettoProcedura_tblEventi] FOREIGN KEY([EventoID])
REFERENCES [dbo].[GEMMA_AIAtblEventi] ([EventoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiOggettoProcedura] CHECK CONSTRAINT [FK_GEMMA_AIA_stgEventiOggettoProcedura_tblEventi]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiUtenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_stgEventiUtenti_tblEventi] FOREIGN KEY([EventoID])
REFERENCES [dbo].[GEMMA_AIAtblEventi] ([EventoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiUtenti] CHECK CONSTRAINT [FK_GEMMA_AIA_stgEventiUtenti_tblEventi]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiUtenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIAstgEventiUtenti_GEMMA_AIAtblUtenti] FOREIGN KEY([UtenteID])
REFERENCES [dbo].[GEMMA_AIAtblUtenti] ([UtenteID])
GO
ALTER TABLE [dbo].[GEMMA_AIAstgEventiUtenti] CHECK CONSTRAINT [FK_GEMMA_AIAstgEventiUtenti_GEMMA_AIAtblUtenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgGruppiUtenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_stgGruppiUtenti_tblGruppi] FOREIGN KEY([GruppoID])
REFERENCES [dbo].[GEMMA_AIAtblGruppi] ([GruppoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAstgGruppiUtenti] CHECK CONSTRAINT [FK_GEMMA_AIA_stgGruppiUtenti_tblGruppi]
GO
ALTER TABLE [dbo].[GEMMA_AIAstgGruppiUtenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_stgGruppiUtenti_tblUtenti] FOREIGN KEY([UtenteID])
REFERENCES [dbo].[GEMMA_AIAtblUtenti] ([UtenteID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAstgGruppiUtenti] CHECK CONSTRAINT [FK_GEMMA_AIA_stgGruppiUtenti_tblUtenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblDocumenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblDocumenti_tblEventi] FOREIGN KEY([EventoID])
REFERENCES [dbo].[GEMMA_AIAtblEventi] ([EventoID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAtblDocumenti] CHECK CONSTRAINT [FK_GEMMA_AIA_tblDocumenti_tblEventi]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblDocumenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblDocumenti_tblRaggruppamenti] FOREIGN KEY([RaggruppamentoID])
REFERENCES [dbo].[GEMMA_AIAtblRaggruppamenti] ([RaggruppamentoID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblDocumenti] CHECK CONSTRAINT [FK_GEMMA_AIA_tblDocumenti_tblRaggruppamenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblDocumenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIAtblDocumenti_GEMMA_AIAtblUtenti] FOREIGN KEY([UtenteID])
REFERENCES [dbo].[GEMMA_AIAtblUtenti] ([UtenteID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblDocumenti] CHECK CONSTRAINT [FK_GEMMA_AIAtblDocumenti_GEMMA_AIAtblUtenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblEventi]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblEventi_tblRaggruppamenti] FOREIGN KEY([RaggruppamentoID])
REFERENCES [dbo].[GEMMA_AIAtblRaggruppamenti] ([RaggruppamentoID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblEventi] CHECK CONSTRAINT [FK_GEMMA_AIA_tblEventi_tblRaggruppamenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblEventi]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblEventi_tblTipiEvento] FOREIGN KEY([TipoEventoID])
REFERENCES [dbo].[GEMMA_AIAtblTipiEvento] ([TipoEventoID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAtblEventi] CHECK CONSTRAINT [FK_GEMMA_AIA_tblEventi_tblTipiEvento]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppi]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblGruppi_TBL_Territori] FOREIGN KEY([TerritorioID])
REFERENCES [dbo].[TBL_Territori] ([TerritorioID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppi] CHECK CONSTRAINT [FK_GEMMA_AIA_tblGruppi_TBL_Territori]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppi]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblGruppi_tblRuoli] FOREIGN KEY([RuoloID])
REFERENCES [dbo].[GEMMA_AIAtblRuoli] ([RuoloID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppi] CHECK CONSTRAINT [FK_GEMMA_AIA_tblGruppi_tblRuoli]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppiRaggruppamentiEsclusi]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIAtblGruppiRaggruppamentiEsclusi_GEMMA_AIAtblGruppi] FOREIGN KEY([GruppoID])
REFERENCES [dbo].[GEMMA_AIAtblGruppi] ([GruppoID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppiRaggruppamentiEsclusi] CHECK CONSTRAINT [FK_GEMMA_AIAtblGruppiRaggruppamentiEsclusi_GEMMA_AIAtblGruppi]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppiRaggruppamentiEsclusi]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIAtblGruppiRaggruppamentiEsclusi_TBL_Raggruppamenti] FOREIGN KEY([RaggruppamentoID])
REFERENCES [dbo].[TBL_Raggruppamenti] ([RaggruppamentoID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblGruppiRaggruppamentiEsclusi] CHECK CONSTRAINT [FK_GEMMA_AIAtblGruppiRaggruppamentiEsclusi_TBL_Raggruppamenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblPagamentiStoricoAia]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIAtblPagamentiStoricoAia_GEMMA_AIAtblPagamenti] FOREIGN KEY([PagamentoID])
REFERENCES [dbo].[GEMMA_AIAtblPagamenti] ([PagamentoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[GEMMA_AIAtblPagamentiStoricoAia] CHECK CONSTRAINT [FK_GEMMA_AIAtblPagamentiStoricoAia_GEMMA_AIAtblPagamenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblRaggruppamenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblRaggruppamenti_tblRaggruppamenti] FOREIGN KEY([GenitoreID])
REFERENCES [dbo].[GEMMA_AIAtblRaggruppamenti] ([RaggruppamentoID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblRaggruppamenti] CHECK CONSTRAINT [FK_GEMMA_AIA_tblRaggruppamenti_tblRaggruppamenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblRuoli]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblRuoli_tblRaggruppamenti] FOREIGN KEY([RaggruppamentoID])
REFERENCES [dbo].[GEMMA_AIAtblRaggruppamenti] ([RaggruppamentoID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblRuoli] CHECK CONSTRAINT [FK_GEMMA_AIA_tblRuoli_tblRaggruppamenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblTipiEvento]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIA_tblTipiEvento_tblRaggruppamenti] FOREIGN KEY([RaggruppamentoID])
REFERENCES [dbo].[GEMMA_AIAtblRaggruppamenti] ([RaggruppamentoID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblTipiEvento] CHECK CONSTRAINT [FK_GEMMA_AIA_tblTipiEvento_tblRaggruppamenti]
GO
ALTER TABLE [dbo].[GEMMA_AIAtblUtenti]  WITH CHECK ADD  CONSTRAINT [FK_GEMMA_AIAtblUtenti_GEMMA_AIAtblUtenti] FOREIGN KEY([UtenteID])
REFERENCES [dbo].[GEMMA_AIAtblUtenti] ([UtenteID])
GO
ALTER TABLE [dbo].[GEMMA_AIAtblUtenti] CHECK CONSTRAINT [FK_GEMMA_AIAtblUtenti_GEMMA_AIAtblUtenti]
GO
ALTER TABLE [dbo].[STG_DocumentiArgomenti]  WITH CHECK ADD  CONSTRAINT [FK_STG_DocumentiArgomenti_TBL_Argomenti] FOREIGN KEY([ArgomentoID])
REFERENCES [dbo].[TBL_Argomenti] ([ArgomentoID])
GO
ALTER TABLE [dbo].[STG_DocumentiArgomenti] CHECK CONSTRAINT [FK_STG_DocumentiArgomenti_TBL_Argomenti]
GO
ALTER TABLE [dbo].[STG_DocumentiArgomenti]  WITH CHECK ADD  CONSTRAINT [FK_STG_DocumentiArgomenti_TBL_Documenti] FOREIGN KEY([DocumentoID])
REFERENCES [dbo].[TBL_Documenti] ([DocumentoID])
GO
ALTER TABLE [dbo].[STG_DocumentiArgomenti] CHECK CONSTRAINT [FK_STG_DocumentiArgomenti_TBL_Documenti]
GO
ALTER TABLE [dbo].[STG_DocumentiEntita]  WITH CHECK ADD  CONSTRAINT [FK_STG_DocumentiRubrica_TBL_Documenti] FOREIGN KEY([DocumentoID])
REFERENCES [dbo].[TBL_Documenti] ([DocumentoID])
GO
ALTER TABLE [dbo].[STG_DocumentiEntita] CHECK CONSTRAINT [FK_STG_DocumentiRubrica_TBL_Documenti]
GO
ALTER TABLE [dbo].[STG_DocumentiEntita]  WITH CHECK ADD  CONSTRAINT [FK_STG_DocumentiRubrica_TBL_Entita] FOREIGN KEY([EntitaID])
REFERENCES [dbo].[TBL_Entita] ([EntitaID])
GO
ALTER TABLE [dbo].[STG_DocumentiEntita] CHECK CONSTRAINT [FK_STG_DocumentiRubrica_TBL_Entita]
GO
ALTER TABLE [dbo].[STG_DocumentiEntita]  WITH CHECK ADD  CONSTRAINT [FK_STG_DocumentiRubrica_TBL_Ruoli] FOREIGN KEY([RuoloEntitaID])
REFERENCES [dbo].[TBL_RuoliEntita] ([RuoloEntitaID])
GO
ALTER TABLE [dbo].[STG_DocumentiEntita] CHECK CONSTRAINT [FK_STG_DocumentiRubrica_TBL_Ruoli]
GO
ALTER TABLE [dbo].[STG_OggettiImpiantiAttivitaIppc]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiImpiantiAttivitaIppc_TBL_AttivitaIppc] FOREIGN KEY([AttivitaIppcID])
REFERENCES [dbo].[TBL_AttivitaIppc] ([AttivitaIppcID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STG_OggettiImpiantiAttivitaIppc] CHECK CONSTRAINT [FK_STG_OggettiImpiantiAttivitaIppc_TBL_AttivitaIppc]
GO
ALTER TABLE [dbo].[STG_OggettiImpiantiAttivitaIppc]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiImpiantiAttivitaIppc_TBL_ExtraOggettiImpianto] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_ExtraOggettiImpianto] ([OggettoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STG_OggettiImpiantiAttivitaIppc] CHECK CONSTRAINT [FK_STG_OggettiImpiantiAttivitaIppc_TBL_ExtraOggettiImpianto]
GO
ALTER TABLE [dbo].[STG_OggettiLink]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiLink_TBL_Link1] FOREIGN KEY([LinkID])
REFERENCES [dbo].[TBL_Link] ([LinkID])
GO
ALTER TABLE [dbo].[STG_OggettiLink] CHECK CONSTRAINT [FK_STG_OggettiLink_TBL_Link1]
GO
ALTER TABLE [dbo].[STG_OggettiLink]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiLink_TBL_Oggetti1] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_Oggetti] ([OggettoID])
GO
ALTER TABLE [dbo].[STG_OggettiLink] CHECK CONSTRAINT [FK_STG_OggettiLink_TBL_Oggetti1]
GO
ALTER TABLE [dbo].[STG_OggettiLink]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiLink_TBL_TipiLink] FOREIGN KEY([TipoLinkID])
REFERENCES [dbo].[TBL_TipiLink] ([TipoLinkID])
GO
ALTER TABLE [dbo].[STG_OggettiLink] CHECK CONSTRAINT [FK_STG_OggettiLink_TBL_TipiLink]
GO
ALTER TABLE [dbo].[STG_OggettiProcedureAttributi]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiProcedureAttributi_TBL_Attributi] FOREIGN KEY([AttributoID])
REFERENCES [dbo].[TBL_Attributi] ([AttributoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STG_OggettiProcedureAttributi] CHECK CONSTRAINT [FK_STG_OggettiProcedureAttributi_TBL_Attributi]
GO
ALTER TABLE [dbo].[STG_OggettiProcedureAttributi]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiProcedureAttributi_TBL_OggettiProcedure] FOREIGN KEY([OggettoProceduraID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STG_OggettiProcedureAttributi] CHECK CONSTRAINT [FK_STG_OggettiProcedureAttributi_TBL_OggettiProcedure]
GO
ALTER TABLE [dbo].[STG_OggettiProcedureEntita]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiProcedureEntita_TBL_OggettiProcedure] FOREIGN KEY([OggettoProceduraID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STG_OggettiProcedureEntita] CHECK CONSTRAINT [FK_STG_OggettiProcedureEntita_TBL_OggettiProcedure]
GO
ALTER TABLE [dbo].[STG_OggettiProcedureEntita]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiRubrica_TBL_Rubrica] FOREIGN KEY([EntitaID])
REFERENCES [dbo].[TBL_Entita] ([EntitaID])
GO
ALTER TABLE [dbo].[STG_OggettiProcedureEntita] CHECK CONSTRAINT [FK_STG_OggettiRubrica_TBL_Rubrica]
GO
ALTER TABLE [dbo].[STG_OggettiProcedureEntita]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiRubrica_TBL_Ruoli] FOREIGN KEY([RuoloEntitaID])
REFERENCES [dbo].[TBL_RuoliEntita] ([RuoloEntitaID])
GO
ALTER TABLE [dbo].[STG_OggettiProcedureEntita] CHECK CONSTRAINT [FK_STG_OggettiRubrica_TBL_Ruoli]
GO
ALTER TABLE [dbo].[STG_OggettiTerritori]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiTerritori_TBL_Oggetti] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_Oggetti] ([OggettoID])
GO
ALTER TABLE [dbo].[STG_OggettiTerritori] CHECK CONSTRAINT [FK_STG_OggettiTerritori_TBL_Oggetti]
GO
ALTER TABLE [dbo].[STG_OggettiTerritori]  WITH CHECK ADD  CONSTRAINT [FK_STG_OggettiTerritori_TBL_Territori] FOREIGN KEY([TerritorioID])
REFERENCES [dbo].[TBL_Territori] ([TerritorioID])
GO
ALTER TABLE [dbo].[STG_OggettiTerritori] CHECK CONSTRAINT [FK_STG_OggettiTerritori_TBL_Territori]
GO
ALTER TABLE [dbo].[STG_ProvvedimentiDocumenti]  WITH CHECK ADD  CONSTRAINT [FK_STG_ProvvedimentiDocumenti_TBL_Documenti] FOREIGN KEY([DocumentoID])
REFERENCES [dbo].[TBL_Documenti] ([DocumentoID])
GO
ALTER TABLE [dbo].[STG_ProvvedimentiDocumenti] CHECK CONSTRAINT [FK_STG_ProvvedimentiDocumenti_TBL_Documenti]
GO
ALTER TABLE [dbo].[STG_ProvvedimentiDocumenti]  WITH CHECK ADD  CONSTRAINT [FK_STG_ProvvedimentiDocumenti_TBL_Provvedimenti] FOREIGN KEY([ProvvedimentoID])
REFERENCES [dbo].[TBL_Provvedimenti] ([ProvvedimentoID])
GO
ALTER TABLE [dbo].[STG_ProvvedimentiDocumenti] CHECK CONSTRAINT [FK_STG_ProvvedimentiDocumenti_TBL_Provvedimenti]
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiAttributi]  WITH CHECK ADD  CONSTRAINT [FK_STG_UI_VociMenuTipiAttributi_TBL_TipiAttributi] FOREIGN KEY([TipoAttributoID])
REFERENCES [dbo].[TBL_TipiAttributi] ([TipoAttributoID])
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiAttributi] CHECK CONSTRAINT [FK_STG_UI_VociMenuTipiAttributi_TBL_TipiAttributi]
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiAttributi]  WITH CHECK ADD  CONSTRAINT [FK_STG_UI_VociMenuTipiAttributi_TBL_UI_VociMenu] FOREIGN KEY([VoceMenuID])
REFERENCES [dbo].[TBL_UI_VociMenu] ([VoceMenuID])
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiAttributi] CHECK CONSTRAINT [FK_STG_UI_VociMenuTipiAttributi_TBL_UI_VociMenu]
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiProvvedimenti]  WITH CHECK ADD  CONSTRAINT [fk_STG_UI_VociMenuTipiProvvedimenti_TBL_TipiProvvedimenti_TipoProvvedimentoID] FOREIGN KEY([TipoProvvedimentoID])
REFERENCES [dbo].[TBL_TipiProvvedimenti] ([TipoProvvedimentoID])
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiProvvedimenti] CHECK CONSTRAINT [fk_STG_UI_VociMenuTipiProvvedimenti_TBL_TipiProvvedimenti_TipoProvvedimentoID]
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiProvvedimenti]  WITH CHECK ADD  CONSTRAINT [fk_STG_UI_VociMenuTipiProvvedimenti_TBL_UI_VociMenu_VoceMenuID] FOREIGN KEY([VoceMenuID])
REFERENCES [dbo].[TBL_UI_VociMenu] ([VoceMenuID])
GO
ALTER TABLE [dbo].[STG_UI_VociMenuTipiProvvedimenti] CHECK CONSTRAINT [fk_STG_UI_VociMenuTipiProvvedimenti_TBL_UI_VociMenu_VoceMenuID]
GO
ALTER TABLE [dbo].[STG_UI_VociMenuWidget]  WITH CHECK ADD  CONSTRAINT [fk_STG_UI_VociMenuWidget_TBL_UI_VociMenu_VoceMenuID] FOREIGN KEY([VoceMenuID])
REFERENCES [dbo].[TBL_UI_VociMenu] ([VoceMenuID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STG_UI_VociMenuWidget] CHECK CONSTRAINT [fk_STG_UI_VociMenuWidget_TBL_UI_VociMenu_VoceMenuID]
GO
ALTER TABLE [dbo].[STG_UI_VociMenuWidget]  WITH CHECK ADD  CONSTRAINT [fk_STG_UI_VociMenuWidget_TBL_UI_Widget_WidgetID] FOREIGN KEY([WidgetID])
REFERENCES [dbo].[TBL_UI_Widget] ([WidgetID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[STG_UI_VociMenuWidget] CHECK CONSTRAINT [fk_STG_UI_VociMenuWidget_TBL_UI_Widget_WidgetID]
GO
ALTER TABLE [dbo].[STG_UtentiRuoliUtente]  WITH CHECK ADD  CONSTRAINT [fk_STG_UtentiRuoliUtente_TBL_RuoliUtente_RuoloUtenteID] FOREIGN KEY([RuoloUtenteID])
REFERENCES [dbo].[TBL_RuoliUtente] ([RuoloUtenteID])
GO
ALTER TABLE [dbo].[STG_UtentiRuoliUtente] CHECK CONSTRAINT [fk_STG_UtentiRuoliUtente_TBL_RuoliUtente_RuoloUtenteID]
GO
ALTER TABLE [dbo].[STG_UtentiRuoliUtente]  WITH CHECK ADD  CONSTRAINT [fk_STG_UtentiRuoliUtente_TBL_Utenti_UtenteID] FOREIGN KEY([UtenteID])
REFERENCES [dbo].[TBL_Utenti] ([UtenteID])
GO
ALTER TABLE [dbo].[STG_UtentiRuoliUtente] CHECK CONSTRAINT [fk_STG_UtentiRuoliUtente_TBL_Utenti_UtenteID]
GO
ALTER TABLE [dbo].[TBL_Attributi]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Attributi_TBL_TipiAttributi] FOREIGN KEY([TipoAttributoID])
REFERENCES [dbo].[TBL_TipiAttributi] ([TipoAttributoID])
GO
ALTER TABLE [dbo].[TBL_Attributi] CHECK CONSTRAINT [FK_TBL_Attributi_TBL_TipiAttributi]
GO
ALTER TABLE [dbo].[TBL_ClassiAiaStorico]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ClassiAiaStorico_TBL_Raggruppamenti] FOREIGN KEY([RaggruppamentoID])
REFERENCES [dbo].[TBL_Raggruppamenti] ([RaggruppamentoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ClassiAiaStorico] CHECK CONSTRAINT [FK_TBL_ClassiAiaStorico_TBL_Raggruppamenti]
GO
ALTER TABLE [dbo].[TBL_Documenti]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Documenti_TBL_OggettiProcedure] FOREIGN KEY([OggettoProceduraID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
GO
ALTER TABLE [dbo].[TBL_Documenti] CHECK CONSTRAINT [FK_TBL_Documenti_TBL_OggettiProcedure]
GO
ALTER TABLE [dbo].[TBL_Documenti]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Documenti_TBL_Raggruppamenti] FOREIGN KEY([RaggruppamentoID])
REFERENCES [dbo].[TBL_Raggruppamenti] ([RaggruppamentoID])
GO
ALTER TABLE [dbo].[TBL_Documenti] CHECK CONSTRAINT [FK_TBL_Documenti_TBL_Raggruppamenti]
GO
ALTER TABLE [dbo].[TBL_Documenti]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Documenti_TBL_TipiFile] FOREIGN KEY([TipoFileID])
REFERENCES [dbo].[TBL_TipiFile] ([TipoFileID])
GO
ALTER TABLE [dbo].[TBL_Documenti] CHECK CONSTRAINT [FK_TBL_Documenti_TBL_TipiFile]
GO
ALTER TABLE [dbo].[TBL_DocumentiAiaStorico]  WITH CHECK ADD  CONSTRAINT [FK_TBL_DocumentiAiaStorico_TBL_Documenti] FOREIGN KEY([DocumentoID])
REFERENCES [dbo].[TBL_Documenti] ([DocumentoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_DocumentiAiaStorico] CHECK CONSTRAINT [FK_TBL_DocumentiAiaStorico_TBL_Documenti]
GO
ALTER TABLE [dbo].[TBL_DocumentiPortale]  WITH CHECK ADD  CONSTRAINT [fk_TBL_DocumentiPortale_TBL_TipiFile_TipoFileID] FOREIGN KEY([TipoFileID])
REFERENCES [dbo].[TBL_TipiFile] ([TipoFileID])
GO
ALTER TABLE [dbo].[TBL_DocumentiPortale] CHECK CONSTRAINT [fk_TBL_DocumentiPortale_TBL_TipiFile_TipoFileID]
GO
ALTER TABLE [dbo].[TBL_Entita]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Entita_TBL_MacroTipiOggetto] FOREIGN KEY([MacroTipoOggettoID])
REFERENCES [dbo].[TBL_MacroTipiOggetto] ([MacroTipoOggettoID])
ON UPDATE CASCADE
GO
ALTER TABLE [dbo].[TBL_Entita] CHECK CONSTRAINT [FK_TBL_Entita_TBL_MacroTipiOggetto]
GO
ALTER TABLE [dbo].[TBL_ExtraDocumenti]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraDocumenti_TBL_Documenti] FOREIGN KEY([DocumentoID])
REFERENCES [dbo].[TBL_Documenti] ([DocumentoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraDocumenti] CHECK CONSTRAINT [FK_TBL_ExtraDocumenti_TBL_Documenti]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiImpianto_TBL_CategorieImpianti] FOREIGN KEY([CategoriaImpiantoID])
REFERENCES [dbo].[TBL_CategorieImpianti] ([CategoriaImpiantoID])
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto] CHECK CONSTRAINT [FK_TBL_ExtraOggettiImpianto_TBL_CategorieImpianti]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiImpianto_TBL_Oggetti] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_Oggetti] ([OggettoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto] CHECK CONSTRAINT [FK_TBL_ExtraOggettiImpianto_TBL_Oggetti]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiImpianto_TBL_StatoImpianti] FOREIGN KEY([StatoImpiantiID])
REFERENCES [dbo].[TBL_StatoImpianti] ([StatoImpiantiID])
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto] CHECK CONSTRAINT [FK_TBL_ExtraOggettiImpianto_TBL_StatoImpianti]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiPianoProgramma]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiPianoProgramma_TBL_Oggetti] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_Oggetti] ([OggettoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiPianoProgramma] CHECK CONSTRAINT [FK_TBL_ExtraOggettiPianoProgramma_TBL_Oggetti]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiPianoProgramma]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiPianoProgramma_TBL_Settori] FOREIGN KEY([SettoreID])
REFERENCES [dbo].[TBL_Settori] ([SettoreID])
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiPianoProgramma] CHECK CONSTRAINT [FK_TBL_ExtraOggettiPianoProgramma_TBL_Settori]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_OggettiProcedure] FOREIGN KEY([OggettoProceduraID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia] CHECK CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_OggettiProcedure]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_OggettiProcedureCollegata] FOREIGN KEY([ProceduraCollegataID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia] CHECK CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_OggettiProcedureCollegata]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_Provvedimenti] FOREIGN KEY([ProvvedimentoCollegatoID])
REFERENCES [dbo].[TBL_Provvedimenti] ([ProvvedimentoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia] CHECK CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_Provvedimenti]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_StatiProceduraAIA] FOREIGN KEY([StatoAiaID])
REFERENCES [dbo].[TBL_StatiProceduraAIA] ([StatoAiaID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProceduraAia] CHECK CONSTRAINT [FK_TBL_ExtraOggettiProceduraAia_TBL_StatiProceduraAIA]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProgetto]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiProgetto_TBL_Oggetti] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_Oggetti] ([OggettoID])
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProgetto] CHECK CONSTRAINT [FK_TBL_ExtraOggettiProgetto_TBL_Oggetti]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProgetto]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ExtraOggettiProgetto_TBL_Opere] FOREIGN KEY([OperaID])
REFERENCES [dbo].[TBL_Opere] ([OperaID])
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiProgetto] CHECK CONSTRAINT [FK_TBL_ExtraOggettiProgetto_TBL_Opere]
GO
ALTER TABLE [dbo].[TBL_ExtraProvvedimentiAia]  WITH CHECK ADD  CONSTRAINT [FK_TBL_AutorizzazioniAiaStorico_TBL_Provvedimenti] FOREIGN KEY([ProvvedimentoID])
REFERENCES [dbo].[TBL_Provvedimenti] ([ProvvedimentoID])
ON UPDATE CASCADE
ON DELETE CASCADE
GO
ALTER TABLE [dbo].[TBL_ExtraProvvedimentiAia] CHECK CONSTRAINT [FK_TBL_AutorizzazioniAiaStorico_TBL_Provvedimenti]
GO
ALTER TABLE [dbo].[TBL_Immagini]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Immagini_TBL_FormatiImmagine] FOREIGN KEY([FormatoImmagineID])
REFERENCES [dbo].[TBL_FormatiImmagine] ([FormatoImmagineID])
GO
ALTER TABLE [dbo].[TBL_Immagini] CHECK CONSTRAINT [FK_TBL_Immagini_TBL_FormatiImmagine]
GO
ALTER TABLE [dbo].[TBL_Notizie]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Notizie_TBL_CategorieNotizie] FOREIGN KEY([CategoriaNotiziaID])
REFERENCES [dbo].[TBL_CategorieNotizie] ([CategoriaNotiziaID])
GO
ALTER TABLE [dbo].[TBL_Notizie] CHECK CONSTRAINT [FK_TBL_Notizie_TBL_CategorieNotizie]
GO
ALTER TABLE [dbo].[TBL_Notizie]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Notizie_TBL_Immagini] FOREIGN KEY([ImmagineID])
REFERENCES [dbo].[TBL_Immagini] ([ImmagineID])
GO
ALTER TABLE [dbo].[TBL_Notizie] CHECK CONSTRAINT [FK_TBL_Notizie_TBL_Immagini]
GO
ALTER TABLE [dbo].[TBL_Oggetti]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Oggetti_TBL_TipiOggetto] FOREIGN KEY([TipoOggettoID])
REFERENCES [dbo].[TBL_TipiOggetto] ([TipoOggettoID])
GO
ALTER TABLE [dbo].[TBL_Oggetti] CHECK CONSTRAINT [FK_TBL_Oggetti_TBL_TipiOggetto]
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure]  WITH CHECK ADD  CONSTRAINT [FK_TBL_OggettiProcedure_TBL_FasiProgettazione] FOREIGN KEY([FaseProgettazioneID])
REFERENCES [dbo].[TBL_FasiProgettazione] ([FaseProgettazioneID])
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure] CHECK CONSTRAINT [FK_TBL_OggettiProcedure_TBL_FasiProgettazione]
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure]  WITH CHECK ADD  CONSTRAINT [FK_TBL_OggettiProcedure_TBL_Oggetti] FOREIGN KEY([OggettoID])
REFERENCES [dbo].[TBL_Oggetti] ([OggettoID])
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure] CHECK CONSTRAINT [FK_TBL_OggettiProcedure_TBL_Oggetti]
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure]  WITH CHECK ADD  CONSTRAINT [FK_TBL_OggettiProcedure_TBL_Procedure] FOREIGN KEY([ProceduraID])
REFERENCES [dbo].[TBL_Procedure] ([ProceduraID])
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure] CHECK CONSTRAINT [FK_TBL_OggettiProcedure_TBL_Procedure]
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure]  WITH CHECK ADD  CONSTRAINT [FK_TBL_OggettiProcedure_TBL_Valutatori] FOREIGN KEY([ValutatoreID])
REFERENCES [dbo].[TBL_Valutatori] ([ValutatoreID])
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure] CHECK CONSTRAINT [FK_TBL_OggettiProcedure_TBL_Valutatori]
GO
ALTER TABLE [dbo].[TBL_Opere]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Opere_TBL_Tipologie] FOREIGN KEY([TipologiaID])
REFERENCES [dbo].[TBL_Tipologie] ([TipologiaID])
GO
ALTER TABLE [dbo].[TBL_Opere] CHECK CONSTRAINT [FK_TBL_Opere_TBL_Tipologie]
GO
ALTER TABLE [dbo].[TBL_Procedure]  WITH CHECK ADD  CONSTRAINT [fk_TBL_Procedure_TBL_AmbitiProcedure_AmbitoProceduraID] FOREIGN KEY([AmbitoProceduraID])
REFERENCES [dbo].[TBL_AmbitiProcedure] ([AmbitoProceduraID])
GO
ALTER TABLE [dbo].[TBL_Procedure] CHECK CONSTRAINT [fk_TBL_Procedure_TBL_AmbitiProcedure_AmbitoProceduraID]
GO
ALTER TABLE [dbo].[TBL_Provvedimenti]  WITH CHECK ADD  CONSTRAINT [fk_TBL_Provvedimenti_TBL_OggettiProcedure_OggettoProceduraID] FOREIGN KEY([OggettoProceduraID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
GO
ALTER TABLE [dbo].[TBL_Provvedimenti] CHECK CONSTRAINT [fk_TBL_Provvedimenti_TBL_OggettiProcedure_OggettoProceduraID]
GO
ALTER TABLE [dbo].[TBL_Provvedimenti]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Provvedimenti_TBL_TipiProvvedimenti] FOREIGN KEY([TipoProvvedimentoID])
REFERENCES [dbo].[TBL_TipiProvvedimenti] ([TipoProvvedimentoID])
GO
ALTER TABLE [dbo].[TBL_Provvedimenti] CHECK CONSTRAINT [FK_TBL_Provvedimenti_TBL_TipiProvvedimenti]
GO
ALTER TABLE [dbo].[TBL_Territori]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Territori_TBL_TipologieTerritorio] FOREIGN KEY([TipologiaTerritorioID])
REFERENCES [dbo].[TBL_TipologieTerritorio] ([TipologiaTerritorioID])
GO
ALTER TABLE [dbo].[TBL_Territori] CHECK CONSTRAINT [FK_TBL_Territori_TBL_TipologieTerritorio]
GO
ALTER TABLE [dbo].[TBL_TipiOggetto]  WITH CHECK ADD  CONSTRAINT [FK_TBL_TipiOggetto_TBL_MacroTipiOggetto] FOREIGN KEY([MacroTipoOggettoID])
REFERENCES [dbo].[TBL_MacroTipiOggetto] ([MacroTipoOggettoID])
GO
ALTER TABLE [dbo].[TBL_TipiOggetto] CHECK CONSTRAINT [FK_TBL_TipiOggetto_TBL_MacroTipiOggetto]
GO
ALTER TABLE [dbo].[TBL_TipiProvvedimenti]  WITH CHECK ADD  CONSTRAINT [FK_TBL_TipiProvvedimenti_TBL_AreeTipiProvvedimenti] FOREIGN KEY([AreaTipoProvvedimentoID])
REFERENCES [dbo].[TBL_AreeTipiProvvedimenti] ([AreaTipoProvvedimentoID])
GO
ALTER TABLE [dbo].[TBL_TipiProvvedimenti] CHECK CONSTRAINT [FK_TBL_TipiProvvedimenti_TBL_AreeTipiProvvedimenti]
GO
ALTER TABLE [dbo].[TBL_Tipologie]  WITH CHECK ADD  CONSTRAINT [FK_TBL_Tipologie_TBL_MacroTipologie] FOREIGN KEY([MacroTipologiaID])
REFERENCES [dbo].[TBL_MacroTipologie] ([MacrotipologiaID])
GO
ALTER TABLE [dbo].[TBL_Tipologie] CHECK CONSTRAINT [FK_TBL_Tipologie_TBL_MacroTipologie]
GO
ALTER TABLE [dbo].[TBL_UI_DatiAmbientaliHome]  WITH CHECK ADD  CONSTRAINT [FK_TBL_UI_DatiAmbientaliHome_TBL_Immagini] FOREIGN KEY([ImmagineID])
REFERENCES [dbo].[TBL_Immagini] ([ImmagineID])
GO
ALTER TABLE [dbo].[TBL_UI_DatiAmbientaliHome] CHECK CONSTRAINT [FK_TBL_UI_DatiAmbientaliHome_TBL_Immagini]
GO
ALTER TABLE [dbo].[TBL_UI_OggettiCarosello]  WITH CHECK ADD  CONSTRAINT [FK_TBL_UI_OggettiCarosello_TBL_Immagini] FOREIGN KEY([ImmagineID])
REFERENCES [dbo].[TBL_Immagini] ([ImmagineID])
GO
ALTER TABLE [dbo].[TBL_UI_OggettiCarosello] CHECK CONSTRAINT [FK_TBL_UI_OggettiCarosello_TBL_Immagini]
GO
ALTER TABLE [dbo].[TBL_UI_PagineStatiche]  WITH CHECK ADD  CONSTRAINT [FK_TBL_UI_PagineStatiche_TBL_UI_VociMenu] FOREIGN KEY([VoceMenuID])
REFERENCES [dbo].[TBL_UI_VociMenu] ([VoceMenuID])
GO
ALTER TABLE [dbo].[TBL_UI_PagineStatiche] CHECK CONSTRAINT [FK_TBL_UI_PagineStatiche_TBL_UI_VociMenu]
GO
ALTER TABLE [dbo].[TBL_UI_Widget]  WITH CHECK ADD  CONSTRAINT [FK_TBL_UI_Widget_TBL_Notizie] FOREIGN KEY([NotiziaID])
REFERENCES [dbo].[TBL_Notizie] ([NotiziaID])
GO
ALTER TABLE [dbo].[TBL_UI_Widget] CHECK CONSTRAINT [FK_TBL_UI_Widget_TBL_Notizie]
GO
ALTER TABLE [dbo].[TBL_ValoriDatiAmministrativi]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ValoriDatiAmministrativi_TBL_DatiAmministrativi] FOREIGN KEY([DatoAmministrativoID])
REFERENCES [dbo].[TBL_DatiAmministrativi] ([DatoAmministrativoID])
GO
ALTER TABLE [dbo].[TBL_ValoriDatiAmministrativi] CHECK CONSTRAINT [FK_TBL_ValoriDatiAmministrativi_TBL_DatiAmministrativi]
GO
ALTER TABLE [dbo].[TBL_ValoriDatiAmministrativi]  WITH CHECK ADD  CONSTRAINT [FK_TBL_ValoriDatiAmministrativi_TBL_OggettiProcedure] FOREIGN KEY([OggettoProceduraID])
REFERENCES [dbo].[TBL_OggettiProcedure] ([OggettoProceduraID])
GO
ALTER TABLE [dbo].[TBL_ValoriDatiAmministrativi] CHECK CONSTRAINT [FK_TBL_ValoriDatiAmministrativi_TBL_OggettiProcedure]
GO
ALTER TABLE [dbo].[TBL_WebEvents]  WITH NOCHECK ADD  CONSTRAINT [fk_TBL_WebEvents_TBL_WebEventTypes_WebEventTypeID] FOREIGN KEY([WebEventTypeID])
REFERENCES [dbo].[TBL_WebEventTypes] ([WebEventTypeID])
GO
ALTER TABLE [dbo].[TBL_WebEvents] CHECK CONSTRAINT [fk_TBL_WebEvents_TBL_WebEventTypes_WebEventTypeID]
GO
ALTER TABLE [dbo].[TBL_Documenti]  WITH CHECK ADD  CONSTRAINT [CK_TBL_Documenti_DataStesura] CHECK  (([dbo].[CK_IsDocumentoAiaRegionale]([OggettoProceduraID]) IS NOT NULL OR [DataStesura] IS NOT NULL))
GO
ALTER TABLE [dbo].[TBL_Documenti] CHECK CONSTRAINT [CK_TBL_Documenti_DataStesura]
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto]  WITH CHECK ADD  CONSTRAINT [CK_TBL_ExtraOggettiImpianto_CategoriaImpiantoID] CHECK  (([Regionale]=(1) OR [CategoriaImpiantoID] IS NOT NULL))
GO
ALTER TABLE [dbo].[TBL_ExtraOggettiImpianto] CHECK CONSTRAINT [CK_TBL_ExtraOggettiImpianto_CategoriaImpiantoID]
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure]  WITH CHECK ADD  CONSTRAINT [CK_TBL_OggettiProcedure_ProcedureID] CHECK  (([dbo].[CK_IsOggettoProceduraAiaRegionale]([OggettoID],[AIAID]) IS NOT NULL OR [ProceduraID] IS NOT NULL))
GO
ALTER TABLE [dbo].[TBL_OggettiProcedure] CHECK CONSTRAINT [CK_TBL_OggettiProcedure_ProcedureID]
GO
ALTER TABLE [dbo].[TBL_Provvedimenti]  WITH CHECK ADD  CONSTRAINT [CK_TBL_Provvedimenti_Data] CHECK  (([dbo].[CK_IsProvvedimentoAiaRegionale]([OggettoProceduraID]) IS NOT NULL OR [Data] IS NOT NULL))
GO
ALTER TABLE [dbo].[TBL_Provvedimenti] CHECK CONSTRAINT [CK_TBL_Provvedimenti_Data]
GO
ALTER TABLE [dbo].[TBL_Provvedimenti]  WITH CHECK ADD  CONSTRAINT [CK_TBL_Provvedimenti_TipoProvvedimentoID] CHECK  (([dbo].[CK_IsProvvedimentoAiaRegionale]([OggettoProceduraID]) IS NOT NULL OR [TipoProvvedimentoID] IS NOT NULL))
GO
ALTER TABLE [dbo].[TBL_Provvedimenti] CHECK CONSTRAINT [CK_TBL_Provvedimenti_TipoProvvedimentoID]
GO
/****** Object:  StoredProcedure [dbo].[FT_SP_AggiornaCataloghi]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[FT_SP_AggiornaCataloghi]
      -- Add the parameters for the stored procedure here
AS
BEGIN
      -- SET NOCOUNT ON added to prevent extra result sets from
      -- interfering with SELECT statements.
      SET NOCOUNT ON;

    -- Insert statements for procedure here
    TRUNCATE TABLE dbo.FTL_Documenti;
    
      INSERT INTO dbo.FTL_Documenti 
      SELECT D.DocumentoID, D.Titolo, ISNULL(D.Descrizione, ''), D.CodiceElaborato, 
            O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, 
      dbo.FT_FN_ConcatenaArgomentiPerDocumento_IT(D.DocumentoID), 
      dbo.FT_FN_ConcatenaArgomentiPerDocumento_EN(D.DocumentoID), 
          dbo.FT_FN_ConcatenaAutoriPerDocumento(D.DocumentoID), 
           dbo.FT_FN_ConcatenaProponentiPerOggetto(O.OggettoID) 
      FROM dbo.TBL_Documenti D 
            INNER JOIN dbo.TBL_OggettiProcedure OP ON OP.OggettoProceduraID = D.OggettoProceduraID 
            INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
            INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
      WHERE T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL
      
    TRUNCATE TABLE dbo.FTL_Oggetti;
      -- VIA
      INSERT INTO FTL_Oggetti
      SELECT O.OggettoID
              ,O.Nome_IT
              ,O.Nome_EN
              ,O.Descrizione_IT
              ,O.Descrizione_EN
              ,OP.Nome_IT
              ,OP.Nome_EN
              ,dbo.FT_FN_ConcatenaTerritoriPerOggetto(O.OggettoID) 
              ,dbo.FT_FN_ConcatenaProponentiPerOggetto(O.OggettoID)
        FROM dbo.TBL_Oggetti AS O INNER JOIN
        dbo.TBL_ExtraOggettiProgetto AS EOP ON O.OggettoID = EOP.OggettoID INNER JOIN
        dbo.TBL_Opere AS OP ON OP.OperaID = EOP.OperaID
        WHERE O.TipoOggettoID = 1
      -- VAS
      INSERT INTO FTL_Oggetti
      SELECT O.OggettoID
              ,O.Nome_IT
              ,O.Nome_EN
              ,O.Descrizione_IT
              ,O.Descrizione_EN
              ,S.Nome_IT
              ,S.Nome_EN
              ,dbo.FT_FN_ConcatenaTerritoriPerOggetto(O.OggettoID) 
              ,dbo.FT_FN_ConcatenaProponentiPerOggetto(O.OggettoID)
        FROM dbo.TBL_Oggetti AS O INNER JOIN
            dbo.TBL_ExtraOggettiPianoProgramma AS E ON E.OggettoID = O.OggettoID INNER JOIN
            dbo.TBL_Settori AS S ON S.SettoreID = E.SettoreID
        WHERE O.TipoOggettoID = 2 OR O.TipoOggettoID = 3
      -- AIA
      INSERT INTO FTL_Oggetti
      SELECT distinct  O.OggettoID
              ,O.Nome_IT
              ,O.Nome_EN
              ,O.Descrizione_IT
              ,O.Descrizione_EN
              ,C.Nome_IT
              ,C.Nome_EN
              ,dbo.FT_FN_ConcatenaTerritoriPerOggetto(O.OggettoID) 
              ,dbo.FT_FN_ConcatenaProponentiPerOggetto(O.OggettoID)
              
        FROM dbo.TBL_Oggetti AS O INNER JOIN
            dbo.TBL_ExtraOggettiImpianto AS I ON I.OggettoID = O.OggettoID 
            INNER JOIN dbo.TBL_CategorieImpianti AS C ON C.CategoriaImpiantoID = I.CategoriaImpiantoID
            left join dbo.TBL_OggettiProcedure as OP on op.OggettoID = o.OggettoID 
        WHERE O.TipoOggettoID = 4 and op.AIAID IS not NULL
        
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaDettaglioDocumento]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaDettaglioDocumento] 
	@DocumentoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Documento
	SELECT D.DocumentoID, D.TipoFileID, OP.ProceduraID, O.TipoOggettoID, D.CodiceElaborato, D.Titolo, D.Descrizione, D.Tipologia,
		D.Scala, D.Diritti, D.LinguaDocumento, D.Dimensione, O.OggettoID, O.Nome_IT, O.Nome_EN, 
		XD.Riferimenti, XD.Origine, XD.Copertura, D.DataPubblicazione, D.DataStesura
	FROM dbo.TBL_Documenti AS D LEFT OUTER JOIN 
		dbo.TBL_ExtraDocumenti AS XD ON D.DocumentoID = XD.DocumentoID INNER JOIN
		dbo.TBL_OggettiProcedure AS OP ON D.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN
		dbo.TBL_Oggetti AS O ON OP.OggettoID = O.OggettoID
	WHERE D.DocumentoID = @DocumentoID AND D.LivelloVisibilita = 1;
		
	-- Entita
	SELECT E.EntitaID, E.Nome, STG.RuoloEntitaID
	FROM dbo.TBL_Entita AS E INNER JOIN
		dbo.STG_DocumentiEntita AS STG ON E.EntitaID = STG.EntitaID
	WHERE STG.DocumentoID = @DocumentoID;
	
	-- Raggruppamenti (ricorsivo)
	WITH Raggruppamenti (RaggruppamentoID, GenitoreID, Nome_IT, NOME_EN, Ordine, Ct)
	AS
	(
		-- Ancora
		SELECT R.RaggruppamentoID, R.GenitoreID, R.Nome_IT, R.Nome_EN, R.Ordine, 0
		FROM dbo.TBL_Raggruppamenti AS R INNER JOIN 
			dbo.TBL_Documenti AS D ON D.RaggruppamentoID = R.RaggruppamentoID
		WHERE D.DocumentoID = @DocumentoID
		UNION ALL
		-- Select ricorsiva
		SELECT R.RaggruppamentoID, R.GenitoreID, R.Nome_IT, R.Nome_EN, R.Ordine, Ct +1 
		FROM dbo.TBL_Raggruppamenti AS R
		INNER JOIN Raggruppamenti AS RT
			ON R.RaggruppamentoID = RT.GenitoreID
	)
	-- Select risultati raggruppamenti
	SELECT RaggruppamentoID, GenitoreID, Nome_IT, NOME_EN, Ordine 
	FROM Raggruppamenti
	ORDER BY Ct DESC

	-- Argomenti
	SELECT A.ArgomentoID, A.Nome_IT, A.Nome_EN 
	FROM dbo.TBL_Argomenti AS A INNER JOIN 
		dbo.STG_DocumentiArgomenti AS SDA ON SDA.ArgomentoID = A.ArgomentoID
	WHERE SDA.DocumentoID = @DocumentoID
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaDocumentazioneOggettoBase]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaDocumentazioneOggettoBase]
	@OggettoID int, 
	@OggettoProceduraID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Dettagli
	SELECT O.OggettoID, O.TipoOggettoID, O.Nome_IT, O.Nome_EN
	FROM dbo.TBL_Oggetti AS O
	WHERE O.OggettoID = @OggettoID

	
	-- Procedure Collegate (ProceduraCollegata)
	--SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, ISNULL(SPV.ProSDeId, 0) AS ProSDeId, 
	--	OP.DataInserimento, ISNULL(D.NumeroDocumenti, 0) AS NumeroDocumenti, OP.ViperaID 
	--FROM dbo.TBL_OggettiProcedure AS OP LEFT OUTER JOIN 
	--	Vipera.dbo.vipProgetti AS VP ON OP.ViperaID = VP.ProId LEFT OUTER JOIN 
	--	dbo.TBL_StatiProceduraVIPERA AS SPV ON VP.ProSDeId = SPV.ProSDeId LEFT OUTER JOIN
	--	(SELECT COUNT(DocumentoID) AS NumeroDocumenti, OggettoProceduraID FROM dbo.TBL_Documenti WHERE LivelloVisibilita = 1 GROUP BY OggettoProceduraID) AS D ON OP.OggettoProceduraID = D.OggettoProceduraID
	--WHERE OP.OggettoProceduraID = @OggettoProceduraID

	-- Procedure Collegate (ProceduraCollegata)
	SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID,  coalesce(ISNULL(SPV.ProSDeId, EPA.StatoAiaID),0) AS ProSDeId, 
		OP.DataInserimento, ISNULL(D.NumeroDocumenti, 0) AS NumeroDocumenti, 
		Coalesce(CONVERT(VARCHAR(12),OP.ViperaID), OP.AiaID) as ViperaAIAID  
	FROM dbo.TBL_OggettiProcedure AS OP 
	LEFT OUTER JOIN Vipera.dbo.vipProgetti AS VP ON OP.ViperaID = VP.ProId 
	LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON VP.ProSDeId = SPV.ProSDeId 
	LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
    LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID 
	LEFT OUTER JOIN
		(SELECT COUNT(DocumentoID) AS NumeroDocumenti, OggettoProceduraID FROM dbo.TBL_Documenti WHERE LivelloVisibilita = 1 GROUP BY OggettoProceduraID) AS D ON OP.OggettoProceduraID = D.OggettoProceduraID
	WHERE OP.OggettoID = @OggettoID
		AND OP.OggettoProceduraID = @OggettoProceduraID
	ORDER BY OP.DataInserimento DESC;


	----Dati amministrativi
	--SELECT TOP (999999) VDA.OggettoProceduraID, OP.ProceduraID, 
	--	VDA.ValoreBooleano, VDA.ValoreData, 
	--	VDA.ValoreNumero, VDA.ValoreTesto, 
	--	DA.DatoAmministrativoID, OP.ViperaID 	
	--FROM dbo.TBL_DatiAmministrativi AS DA INNER JOIN
	--	dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.DatoAmministrativoID = DA.DatoAmministrativoID INNER JOIN 
	--	dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = VDA.OggettoProceduraID
	--WHERE 
	--	OP.OggettoProceduraID = @OggettoProceduraID 
	--AND
	--	DA.LivelloVisibilita = 1	
	--ORDER BY OP.DataInserimento DESC, DA.Ordine ASC;
	
	--Dati amministrativi
	SELECT TOP (999999) VDA.OggettoProceduraID, OP.ProceduraID, 
		VDA.ValoreBooleano, VDA.ValoreData, 
		VDA.ValoreNumero, VDA.ValoreTesto, 
		DA.DatoAmministrativoID, Coalesce(CONVERT(VARCHAR(12),OP.ViperaID), OP.AiaID) as ViperaAIAID 
	FROM dbo.TBL_DatiAmministrativi AS DA 
		INNER JOIN dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.DatoAmministrativoID = DA.DatoAmministrativoID 
		INNER JOIN dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = VDA.OggettoProceduraID
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
        LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID
	WHERE OP.OggettoID = @OggettoID 
		AND DA.LivelloVisibilita = 1
		AND OP.OggettoProceduraID = @OggettoProceduraID
	ORDER BY OP.DataInserimento DESC, DA.Ordine ASC;
	
	---- Stato procedura, da mischiare con i dati amministrativi
	--SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, 280 AS DatoAmministrativoID, 
	--	SPV.ProSDeId, OP.ViperaID 
	--FROM dbo.TBL_OggettiProcedure AS OP INNER JOIN 
	--	Vipera.dbo.vipProgetti AS VP ON OP.ViperaID = VP.ProId INNER JOIN 
	--	dbo.TBL_StatiProceduraVIPERA AS SPV ON VP.ProSDeId = SPV.ProSDeId 
	--WHERE OP.OggettoProceduraID = @OggettoProceduraID;
	
	-- Stato procedura, da mischiare con i dati amministrativi
	SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, 280 AS DatoAmministrativoID, 
		ISNULL(SPV.ProSDeId, 0) AS ProSDeId, CONVERT(VARCHAR(12),OP.ViperaID) as ViperaID 
	FROM dbo.TBL_OggettiProcedure AS OP 
		INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		LEFT OUTER JOIN Vipera.dbo.vipProgetti AS VP ON OP.ViperaID = VP.ProId 
		LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON VP.ProSDeId = SPV.ProSDeId 
	WHERE OP.OggettoID =  @OggettoID and T.MacroTipoOggettoID in (1 , 2)
		AND OP.OggettoProceduraID = @OggettoProceduraID 
	UNION
	SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, 2035 AS DatoAmministrativoID, 
		ISNULL(SPA.StatoAiaID, 0) AS ProSDeId, Coalesce(CONVERT(VARCHAR(12),OP.ViperaID), OP.AiaID) as ViperaAIAID 
	FROM dbo.TBL_OggettiProcedure AS OP 
		INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
        LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID         
	WHERE OP.OggettoID =  @OggettoID and T.MacroTipoOggettoID = 3 
		AND OP.OggettoProceduraID = @OggettoProceduraID
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaDocumentazioneOggettoVas]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaDocumentazioneOggettoVas]
	-- Add the parameters for the stored procedure here
	@OggettoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Dati in comune con gli oggetti VAS
	EXEC SP_RecuperaDocumentazioneOggettoBase @OggettoID	
	
	-- Settore
	SELECT EOP.SettoreID
	FROM dbo.TBL_ExtraOggettiPianoProgramma AS EOP 
	WHERE EOP.OggettoID = @OggettoID

END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaDocumentazioneOggettoVia]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaDocumentazioneOggettoVia]
	-- Add the parameters for the stored procedure here
	@OggettoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Dati in comune con gli oggetti VAS
	EXEC SP_RecuperaDocumentazioneOggettoBase @OggettoID	
	
	-- Opera
	SELECT O.OperaID, O.TipologiaID, O.Nome_IT, O.Nome_EN
	FROM dbo.TBL_ExtraOggettiProgetto AS EOP INNER JOIN 
		dbo.TBL_Opere AS O ON EOP.OperaID = O.OperaID
	WHERE EOP.OggettoID = @OggettoID	

END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaDocumenti]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaDocumenti]
     -- Add the parameters for the stored procedure here
     @MacroTipoOggettoID int,
     @OggettoProceduraID int,
     @RaggruppamentoID int,
     @Lingua varchar(2),
     @TestoRicerca nvarchar(128),
     @OrderBy nvarchar(32),
     @OrderDirection nvarchar(4),
     @StartRowNum int,
     @EndRowNum int,
     @FiltroData bit
     
AS
BEGIN
	-- NORMALIZZAZIONE DATI
	 IF @OrderBy IS NULL 
		SET @OrderBy = N''

	 IF @OrderDirection IS NULL
		SET @OrderDirection = N''

     -- SET NOCOUNT ON added to prevent extra result sets from
     -- interfering with SELECT statements.
     SET NOCOUNT ON;
     DECLARE @TotalRowCount int;
     DECLARE @Query_Base nvarchar(4000);
     DECLARE @QueryCount nvarchar(4000);
     DECLARE @Query nvarchar(4000);
     DECLARE @FTL_Join nvarchar(4000);
     DECLARE @TipiOggettoID nvarchar(8);
     DECLARE @OrderByStatement nvarchar(64);
     DECLARE @FilterByDate nvarchar(64);
     DECLARE @FilterByDataStesura nvarchar(64);
	 DECLARE @QueryRaggruppamentoID nvarchar(200);
	
	
	
     IF (@MacroTipoOggettoID = 1)
         SET @TipiOggettoID = '1';
     ELSE IF (@MacroTipoOggettoID = 2)
         SET @TipiOggettoID = '2,3';
	 ELSE IF (@MacroTipoOggettoID = 3)
         SET @TipiOggettoID = '4';
	
     IF (@TestoRicerca <> N'')
         SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryDocumenti(@Lingua));
     ELSE
         SET @FTL_Join = N''
	 
	 IF (@FiltroData = 1)
		BEGIN
			SET @FilterByDate = ' AND Data >= ''20170516''';
			SET @FilterByDataStesura = ' AND D.DataStesura >= ''20170516''';
		END
	 ELSE
		BEGIN
			SET @FilterByDate = '';
			SET @FilterByDataStesura = '';
		END
			
     IF @FTL_Join <> N''
		BEGIN
			SET @OrderByStatement = N'FTL.RANK DESC'
		END
	 ELSE
		BEGIN
			IF @OrderDirection = N''
				SET @OrderDirection = 'ASC'

			IF @OrderBy = N''
				BEGIN
					SET @OrderByStatement = N'R.Ordine ASC, D.Ordinamento ' + @OrderDirection
				END
			ELSE
				BEGIN
					SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection
				END
		END
	 
	If @RaggruppamentoID = 141
		BEGIN	
			SET @QueryRaggruppamentoID = '(D.RaggruppamentoID IN (396,399,484,141,144,229)) AND '
		END
	ELSE
		BEGIN
			SET @QueryRaggruppamentoID = '((D.RaggruppamentoID = @RaggruppamentoID) OR (@RaggruppamentoID IS NULL)) AND '
		END
		
		
     -- Insert statements for procedure here
     SET @Query_Base = N'FROM dbo.TBL_Documenti AS D INNER JOIN ' +
     N'    dbo.TBL_Raggruppamenti AS R ON R.RaggruppamentoID = D.RaggruppamentoID INNER JOIN ' +
     N'    dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = D.OggettoProceduraID INNER JOIN ' +
     N'    dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID  ' + @FTL_JOIN +
     N'    INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID ' +
     N'WHERE O.TipoOggettoID IN (' + @TipiOggettoID + ') AND ' +
     N'    ((D.OggettoProceduraID = @OggettoProceduraID) OR (@OggettoProceduraID IS NULL)) AND ' +
     @QueryRaggruppamentoID +
     N'    (D.LivelloVisibilita = 1)' +
     N'   AND (T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL) ' + @FilterByDataStesura;

     SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' + @OrderByStatement + N') AS RN, ' +
     N'D.DocumentoID, D.RaggruppamentoID, D.TipoFileID, D.Titolo, D.CodiceElaborato, D.Scala, D.Dimensione, ' + 
	 N'O.Nome_IT AS NomeOggetto_IT, O.Nome_EN AS NomeOggetto_EN, D.DataStesura AS Data, ' + 
	 N'dbo.FN_DataScadenzaPresentazioneOsservazioniPerOggettoProcedura(OP.OggettoProceduraID) AS DataScadenzaPresentazioneOsservazioni, ' + 
	 N'OP.OggettoID, D.OggettoProceduraID ' + -- campi
     @Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

     SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;

     IF (@FTL_Join <> '')
       BEGIN
          EXECUTE sp_executesql @Query
          , N'@OggettoProceduraID int, @RaggruppamentoID int, @TestoRicerca nvarchar(128),
          @StartRowNum int, @EndRowNum int'
          , @OggettoProceduraID = @OggettoProceduraID
          , @RaggruppamentoID = @RaggruppamentoID
          , @TestoRicerca = @TestoRicerca
          , @StartRowNum = @StartRowNum
          , @EndRowNum = @EndRowNum;

          EXECUTE sp_executesql @QueryCount
          , N'@OggettoProceduraID int, @RaggruppamentoID int, @TestoRicerca nvarchar(128),
          @TotalRowCount int OUTPUT'
          , @OggettoProceduraID = @OggettoProceduraID
          , @RaggruppamentoID = @RaggruppamentoID
          , @TestoRicerca = @TestoRicerca
          , @TotalRowCount = @TotalRowCount OUTPUT;
       END
     ELSE
       BEGIN
          EXECUTE sp_executesql @Query
          , N'@OggettoProceduraID int, @RaggruppamentoID int,
          @StartRowNum int, @EndRowNum int'
          , @OggettoProceduraID = @OggettoProceduraID
          , @RaggruppamentoID = @RaggruppamentoID
          , @StartRowNum = @StartRowNum
          , @EndRowNum = @EndRowNum;

          EXECUTE sp_executesql @QueryCount
          , N'@OggettoProceduraID int, @RaggruppamentoID int,
          @TotalRowCount int OUTPUT'
          , @OggettoProceduraID = @OggettoProceduraID
          , @RaggruppamentoID = @RaggruppamentoID
          , @TotalRowCount = @TotalRowCount OUTPUT;
       END

       SELECT @TotalRowCount;
       
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaDocumentiEvento]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO


CREATE PROCEDURE [dbo].[SP_RecuperaDocumentiEvento]
     -- Add the parameters for the stored procedure here
     @EventoID int,
     @RaggruppamentoID int = NULL,
     @Lingua varchar(2) = N'IT',
     @TestoRicerca nvarchar(128) = N'',
     @StartRowNum int = 0,
     @EndRowNum int = 999999999
     
AS
BEGIN

     -- SET NOCOUNT ON added to prevent extra result sets from
     -- interfering with SELECT statements.
     SET NOCOUNT ON;
     DECLARE @TotalRowCount int;
     DECLARE @Query_Base nvarchar(4000);
     DECLARE @QueryCount nvarchar(4000);
     DECLARE @Query nvarchar(4000);
     DECLARE @FiltroTestoRicerca nvarchar(4000) = N'';
     DECLARE @OrderByStatement nvarchar(64) = N'ASC';
	 DECLARE @QueryRaggruppamentoID nvarchar(200);
	
	
     IF (@TestoRicerca <> N'')
         SET @FiltroTestoRicerca = N' AND D.Titolo Like ''%'+@TestoRicerca+'%''';
	 
	 SET @QueryRaggruppamentoID = ' AND (D.RaggruppamentoID = @RaggruppamentoID OR @RaggruppamentoID IS NULL) '
		
     SET @Query_Base = N'FROM dbo.GEMMA_AIAtblEventi E ' +
     N'    INNER JOIN dbo.GEMMA_AIAtblDocumenti D ON D.EventoID = E.EventoID ' +
     N'    INNER JOIN dbo.GEMMA_AIAtblRaggruppamenti R ON R.RaggruppamentoID = D.RaggruppamentoID ' +
     N'WHERE (E.EventoID = @EventoID)' +
     @QueryRaggruppamentoID +
     N'   AND (D.LivelloVisibilita = 1)' +
     @FiltroTestoRicerca

     SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY R.Ordine, D.Ordinamento) AS RN, ' +
     N'D.DocumentoID, D.RaggruppamentoID, D.Titolo, D.Dimensione, D.DataPubblicazione AS Data, ' +
     N'E.Nome_IT, E.Nome_EN, D.NomeFile ' +
     @Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

     SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;

PRINT @Query
     EXECUTE sp_executesql @Query
      , N'@EventoID int, @RaggruppamentoID int, @StartRowNum int, @EndRowNum int'
      , @EventoID = @EventoID
      , @RaggruppamentoID = @RaggruppamentoID
      , @StartRowNum = @StartRowNum
      , @EndRowNum = @EndRowNum;

PRINT @QueryCount
     EXECUTE sp_executesql @QueryCount
      , N'@EventoID int, @RaggruppamentoID int, @TotalRowCount int OUTPUT'
      , @EventoID = @EventoID
      , @RaggruppamentoID = @RaggruppamentoID
      , @TotalRowCount = @TotalRowCount OUTPUT;
	
  	  SELECT @TotalRowCount;
       
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaInfoOggettoAIA]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[SP_RecuperaInfoOggettoAIA]
	-- Add the parameters for the stored procedure here
	@OggettoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Dati in comune con gli oggetti AIA
	EXEC SP_RecuperaInfoOggettoBase @OggettoID	
	
	-- Categoria/Stato/AttivitaIPPC
	SELECT 
		EOI.CategoriaImpiantoID, EOI.StatoImpiantiID, CapImpianto, IndirizzoImpianto,
		AI.AttivitaIppcID 
		  ,AI.[Codice]
		  ,AI.[Nome_IT]
		  ,AI.[Nome_EN]		
	FROM dbo.TBL_ExtraOggettiImpianto AS EOI 
	LEFT JOIN dbo.STG_OggettiImpiantiAttivitaIppc AS SOIA on SOIA.OggettoID = EOI.OggettoID 
	LEFT JOIN dbo.TBL_AttivitaIppc AS AI on ai.AttivitaIppcID = SOIA.AttivitaIppcID
	WHERE EOI.OggettoID = @OggettoID

/*
	-- Gruppi di Lavoro / Commissioni / Conferenze dei Servizi
	SELECT E.EventoID, E.Nome_IT, E.Nome_EN, E.DataInizio, E.DataFine, E.TipoEventoID
	FROM GEMMA_AIAtblEventi E
	INNER JOIN GEMMA_AIAstgEventiOggetti EO ON EO.EventoID = E.EventoID
	WHERE EO.OggettoID = @OggettoID AND E.Abilitato = 1
*/		
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaInfoOggettoBase]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaInfoOggettoBase]
	@OggettoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @OggettoProceduraID int;

	SET @OggettoProceduraID = (SELECT dbo.FN_UltimoOggettoProceduraIDPerOggetto(@OggettoID));

	-- Dettagli
	SELECT O.OggettoID, O.TipoOggettoID, O.Nome_IT, O.Nome_EN, 
		O.Descrizione_IT, O.Descrizione_EN, O.ImmagineLocalizzazione, 
		dbo.FN_DataScadenzaPresentazioneOsservazioni(@OggettoID) AS ScadenzaPresentazioneOsservazioni, 
		OP.OggettoProceduraID
	FROM dbo.TBL_Oggetti AS O INNER JOIN
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID 
	WHERE O.OggettoID = @OggettoID AND OP.UltimaProcedura = 1

	-- Entita
	SELECT E.EntitaID, E.Nome, SPE.RuoloEntitaID,
		   E.CodiceFiscale,E.Indirizzo,E.Cap,E.Citta,E.Provincia,E.SitoWeb 
	FROM dbo.TBL_Entita AS E INNER JOIN
		dbo.STG_OggettiProcedureEntita AS SPE ON SPE.EntitaID = E.EntitaID INNER JOIN
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = SPE.OggettoProceduraID
	WHERE OP.OggettoProceduraID = @OggettoProceduraID;

	-- Link
	SELECT L.LinkID, L.Nome, L.Descrizione, L.Indirizzo, SOL.TipoLinkID
	FROM dbo.TBL_Link AS L INNER JOIN
		dbo.STG_OggettiLink AS SOL ON SOL.LinkID = L.LinkID
	WHERE SOL.OggettoID = @OggettoID;

	--Territori
	SELECT T.TerritorioID, T.GenitoreID, T.TipologiaTerritorioID, 
		T.Nome, T.CodiceIstat
	FROM dbo.TBL_Territori AS T INNER JOIN 
		dbo.STG_OggettiTerritori AS SOT ON SOT.TerritorioID = T.TerritorioID
	WHERE SOT.OggettoID = @OggettoID;

	-- Procedure Collegate (ProceduraCollegata)
	SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID,  coalesce(ISNULL(SPV.ProSDeId, EPA.StatoAiaID),0) AS ProSDeId, 
		OP.DataInserimento, ISNULL(D.NumeroDocumenti, 0) AS NumeroDocumenti, 
		Coalesce(CONVERT(VARCHAR(12),OP.ViperaID), OP.AiaID) as ViperaAIAID  
	FROM dbo.TBL_OggettiProcedure AS OP 
	LEFT OUTER JOIN Vipera.dbo.vipProgetti AS VP ON OP.ViperaID = VP.ProId 
	LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON VP.ProSDeId = SPV.ProSDeId 
	LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
    LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID 
	LEFT OUTER JOIN
		(SELECT COUNT(DocumentoID) AS NumeroDocumenti, OggettoProceduraID FROM dbo.TBL_Documenti WHERE LivelloVisibilita = 1 GROUP BY OggettoProceduraID) AS D ON OP.OggettoProceduraID = D.OggettoProceduraID
	WHERE OP.OggettoID = @OggettoID
	ORDER BY OP.DataInserimento DESC;

	--Dati amministrativi
	SELECT TOP (999999) VDA.OggettoProceduraID, OP.ProceduraID, 
		VDA.ValoreBooleano, VDA.ValoreData, 
		VDA.ValoreNumero, VDA.ValoreTesto, 
		DA.DatoAmministrativoID, Coalesce(CONVERT(VARCHAR(12),OP.ViperaID), OP.AiaID) as ViperaAIAID 
	FROM dbo.TBL_DatiAmministrativi AS DA 
		INNER JOIN dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.DatoAmministrativoID = DA.DatoAmministrativoID 
		INNER JOIN dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = VDA.OggettoProceduraID
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
        LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID
	WHERE OP.OggettoID = @OggettoID 
		AND DA.LivelloVisibilita = 1
	ORDER BY OP.DataInserimento DESC, DA.Ordine ASC;
	
	-- Stato procedura, da mischiare con i dati amministrativi
	SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, 280 AS DatoAmministrativoID, 
		ISNULL(SPV.ProSDeId, 0) AS ProSDeId, CONVERT(VARCHAR(12),OP.ViperaID) as ViperaID 
	FROM dbo.TBL_OggettiProcedure AS OP 
		INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		LEFT OUTER JOIN Vipera.dbo.vipProgetti AS VP ON OP.ViperaID = VP.ProId 
		LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON VP.ProSDeId = SPV.ProSDeId 
	WHERE OP.OggettoID =  @OggettoID and T.MacroTipoOggettoID in (1 , 2) 
	UNION
	SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, 2035 AS DatoAmministrativoID, 
		ISNULL(SPA.StatoAiaID, 0) AS ProSDeId, Coalesce(CONVERT(VARCHAR(12),OP.ViperaID), OP.AiaID) as ViperaAIAID 
	FROM dbo.TBL_OggettiProcedure AS OP 
		INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
		INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
        LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID         
	WHERE OP.OggettoID =  @OggettoID and T.MacroTipoOggettoID = 3 
	
	--SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, 280 AS DatoAmministrativoID, 
	--	ISNULL(SPV.ProSDeId, 0) AS ProSDeId, OP.ViperaID 
	--FROM dbo.TBL_OggettiProcedure AS OP 
	--	INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
	--	INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
	--	LEFT OUTER JOIN Vipera.dbo.vipProgetti AS VP ON OP.ViperaID = VP.ProId 
	--	LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON VP.ProSDeId = SPV.ProSDeId 
	--WHERE OP.OggettoID = @OggettoID and T.MacroTipoOggettoID in (1 , 2) 
	--UNION
	--SELECT TOP (999999) OP.OggettoProceduraID, OP.ProceduraID, NULL AS DatoAmministrativoID, 
	--	ISNULL(SPA.StatoAiaID, 0) AS ProSDeId, OP.AIAID 
	--FROM dbo.TBL_OggettiProcedure AS OP 
	--	INNER JOIN dbo.TBL_Oggetti O ON O.OggettoID = OP.OggettoID
	--	INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
	--	LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
 --       LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID         
	--WHERE OP.OggettoID = @OggettoID and T.MacroTipoOggettoID = 3 ;


END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaInfoOggettoVas]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaInfoOggettoVas]
	-- Add the parameters for the stored procedure here
	@OggettoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
    -- Dati in comune con gli oggetti VAS
	EXEC SP_RecuperaInfoOggettoBase @OggettoID	
	
	-- Settore
	SELECT EOP.SettoreID
	FROM dbo.TBL_ExtraOggettiPianoProgramma AS EOP 
	WHERE EOP.OggettoID = @OggettoID
	

END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaInfoOggettoVia]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaInfoOggettoVia]
	-- Add the parameters for the stored procedure here
	@OggettoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @OperaID int
	SET @OperaID = (SELECT OperaID FROM dbo.TBL_ExtraOggettiProgetto WHERE OggettoID = @OggettoID)
	
    -- Dati in comune con gli oggetti VAS
	EXEC SP_RecuperaInfoOggettoBase @OggettoID	
	
	-- Opera, Cup
	SELECT O.OperaID, O.TipologiaID, O.Nome_IT, O.Nome_EN, EOP.Cup
	FROM dbo.TBL_ExtraOggettiProgetto AS EOP INNER JOIN 
		dbo.TBL_Opere AS O ON EOP.OperaID = O.OperaID
	WHERE EOP.OggettoID = @OggettoID
	
	-- Altri progetti
	SELECT O.OggettoID, OP.ProceduraID, O.TipoOggettoID, O.Nome_IT, O.Nome_EN, E.Nome, OP.OggettoProceduraID 
	FROM dbo.TBL_Oggetti AS O INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN 
		dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN 
		dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID INNER JOIN 
		dbo.TBL_ExtraOggettiProgetto AS EOP ON EOP.OggettoID = O.OggettoID
	WHERE OP.UltimaProcedura = 1 AND EOP.OperaID = @OperaID AND EOP.OggettoID <> @OggettoID
	

END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaNotizie]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaNotizie]
	-- Add the parameters for the stored procedure here
	@Lingua varchar(2), 
	@TestoRicerca nvarchar(128), 
	@CategoriaNotiziaID int, 
	@AnnoCorrente int, 
	@CercaAnnoCorrente bit, 
	@Pubblicata bit, 
	@Stato int, 
	@OrderBy nvarchar(32),
	@OrderDirection nvarchar(4),
	@StartRowNum int,
	@EndRowNum int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalRowCount int;
	DECLARE @Query_Base nvarchar(4000);
	DECLARE @QueryCount nvarchar(4000);
	DECLARE @Query nvarchar(4000);
	DECLARE @FTL_Join nvarchar(4000);
	DECLARE @OrderByStatement nvarchar(64);
	
	IF (@TestoRicerca <> N'')
		SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryNotizie(@Lingua));
	ELSE
		SET @FTL_Join = N''

	IF (@OrderBy IS NULL OR @OrderBy = N'')
		IF @FTL_Join = N''
			BEGIN
				SET @OrderBy = N'N.Data'
				SET @OrderDirection = N'DESC'
			END
		ELSE
			BEGIN
				SET @OrderBy = N'FTL.RANK'
				SET @OrderDirection = N'DESC'
			END

	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
	
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	
    -- Insert statements for procedure here
     SET @Query_Base = N'FROM dbo.TBL_Notizie AS N ' + @FTL_JOIN +
     N'WHERE ' +
     N'    ((N.CategoriaNotiziaID = @CategoriaNotiziaID) OR (@CategoriaNotiziaID IS NULL)) AND ' +
     N'    ((@CercaAnnoCorrente IS NULL) OR (@CercaAnnoCorrente = 1 AND YEAR(N.Data) = @AnnoCorrente) OR (@CercaAnnoCorrente = 0 AND YEAR(N.Data) < @AnnoCorrente)) AND ' +
     N'    ((N.Pubblicata = @Pubblicata) OR (@Pubblicata IS NULL)) AND ' +
     N'    ((N.Stato = @Stato) OR (@Stato IS NULL))';

     SET @Query = N'SELECT * FROM (SELECT ' +
     N'N.NotiziaID, N.CategoriaNotiziaID, N.ImmagineID, N.Data, N.Titolo_IT, N.Titolo_EN, N.TitoloBreve_IT, 
		N.TitoloBreve_EN, N.Abstract_IT, N.Abstract_EN, N.Testo_IT, N.Testo_EN, N.Pubblicata, N.DataInserimento, 
		N.DataUltimaModifica, N.Stato, ROW_NUMBER() OVER (ORDER BY ' + @OrderByStatement + N') AS RN ' + -- campi
     @Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

     SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;


	   --SELECT @Query;
	   --SELECT @QueryCount;

     IF (@FTL_Join <> '')
       BEGIN
          EXECUTE sp_executesql @Query
          , N'@CategoriaNotiziaID int, @CercaAnnoCorrente bit, @AnnoCorrente int, @Pubblicata bit, @Stato int, @TestoRicerca nvarchar(128),
          @StartRowNum int, @EndRowNum int'
          , @CategoriaNotiziaID = @CategoriaNotiziaID
          , @CercaAnnoCorrente = @CercaAnnoCorrente
          , @AnnoCorrente = @AnnoCorrente
          , @Pubblicata = @Pubblicata
          , @Stato = @Stato
          , @TestoRicerca = @TestoRicerca
          , @StartRowNum = @StartRowNum
          , @EndRowNum = @EndRowNum;

          EXECUTE sp_executesql @QueryCount
          , N'@CategoriaNotiziaID int, @CercaAnnoCorrente bit, @AnnoCorrente int, @Pubblicata bit, @Stato int, @TestoRicerca nvarchar(128),
          @TotalRowCount int OUTPUT'
          , @CategoriaNotiziaID = @CategoriaNotiziaID
          , @CercaAnnoCorrente = @CercaAnnoCorrente
          , @AnnoCorrente = @AnnoCorrente
          , @Pubblicata = @Pubblicata
          , @Stato = @Stato
          , @TestoRicerca = @TestoRicerca
          , @TotalRowCount = @TotalRowCount OUTPUT;
       END
     ELSE
       BEGIN
          EXECUTE sp_executesql @Query
          , N'@CategoriaNotiziaID int, @CercaAnnoCorrente bit, @AnnoCorrente int, @Pubblicata bit, @Stato int,
          @StartRowNum int, @EndRowNum int'
          , @CategoriaNotiziaID = @CategoriaNotiziaID
          , @CercaAnnoCorrente = @CercaAnnoCorrente
          , @AnnoCorrente = @AnnoCorrente
          , @Pubblicata = @Pubblicata
          , @Stato = @Stato
          , @StartRowNum = @StartRowNum
          , @EndRowNum = @EndRowNum;

          EXECUTE sp_executesql @QueryCount
          , N'@CategoriaNotiziaID int, @CercaAnnoCorrente bit, @AnnoCorrente int, @Pubblicata bit, @Stato int,
          @TotalRowCount int OUTPUT'
          , @CategoriaNotiziaID = @CategoriaNotiziaID
          , @CercaAnnoCorrente = @CercaAnnoCorrente
          , @AnnoCorrente = @AnnoCorrente
          , @Pubblicata = @Pubblicata
          , @Stato = @Stato
          , @TotalRowCount = @TotalRowCount OUTPUT;
       END
       SELECT @TotalRowCount;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggetti]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

-- CREATE STORED
CREATE PROCEDURE [dbo].[SP_RecuperaOggetti]

	-- Add the parameters for the stored procedure here
	@Lingua varchar(2), 
	@TestoRicerca nvarchar(128), 
	@OrderBy nvarchar(32),
	@OrderDirection nvarchar(4),
	@StartRowNum int,
	@EndRowNum int,
	@ElencoViperaAiaID nvarchar(100)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalRowCount int;
	DECLARE @Query_Base nvarchar(4000);
	DECLARE @QueryCount nvarchar(4000);
	DECLARE @Query nvarchar(4000);
	DECLARE @FTL_Join nvarchar(4000);
	DECLARE @OrderByStatement nvarchar(64);
	
	IF (@TestoRicerca <> N'')
		SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryOggetti(@Lingua));
	ELSE
		SET @FTL_Join = N''

	IF (@OrderBy IS NULL OR @OrderBy = N'')
		IF @FTL_Join = N''
			BEGIN
				SET @OrderBy = N'OP.DataInserimento'
				SET @OrderDirection = N'DESC'
			END
		ELSE
			BEGIN
				SET @OrderBy = N'FTL.RANK'
				SET @OrderDirection = N'DESC'
			END

	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
	
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	
    -- Insert statements for procedure here
	SET @Query_Base = N'FROM dbo.TBL_Oggetti AS O INNER JOIN ' +
	N'	dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN ' +
	N'	dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN ' + 
	N'	dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID ' + @FTL_JOIN + 
	N'WHERE ' +
	N'	(SOPE.RuoloEntitaID IN (1,10)) AND ' +  -- Proponente/Gestore
	N'	(OP.UltimaProcedura = 1)';  -- Ultima Procedura
	
	-- Filtro per ID procedura VIPERA o AIA richiamato dalla HP
	IF (@ElencoViperaAiaID <>  N'')
		SET @Query_Base = @Query_Base + ' AND (OP.OggettoID in ('+ @ElencoViperaAiaID +')) '
		
	SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
	+ @OrderByStatement + N') AS RN, ' + 
	'O.OggettoID, O.TipoOggettoID, OP.ProceduraID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, E.Nome, OP.OggettoProceduraID, Coalesce(CONVERT(VARCHAR(12),OP.ViperaID), OP.AiaID) as ViperaAIAID  ' + -- campi
	@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

	SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;

	IF (@FTL_Join <> '' OR @ElencoViperaAiaID <>  N'')
	  BEGIN
		 EXECUTE sp_executesql @Query
		 , N'@TestoRicerca nvarchar(128), 
		 @StartRowNum int, @EndRowNum int'
		 , @TestoRicerca = @TestoRicerca
		 , @StartRowNum = @StartRowNum
		 , @EndRowNum = @EndRowNum;

		 EXECUTE sp_executesql @QueryCount
		 , N'@TestoRicerca nvarchar(128), 
		 @TotalRowCount int OUTPUT'
		 , @TestoRicerca = @TestoRicerca
		 , @TotalRowCount = @TotalRowCount OUTPUT;
	  END
	  
	  SELECT @TotalRowCount;	  
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiAia]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaOggettiAia]
-- Add the parameters for the stored procedure here
@ProceduraID int, 
@TipologiaID int, 
@AttributoID int, 
@Lingua varchar(2), 
@TestoRicerca nvarchar(128), 
@OrderBy nvarchar(32),
@OrderDirection nvarchar(4),
@StartRowNum int,
@EndRowNum int
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
DECLARE @TotalRowCount int;
DECLARE @Campi nvarchar(512);
DECLARE @Query_Base nvarchar(4000);
DECLARE @QueryCount nvarchar(4000);
DECLARE @Query nvarchar(4000);
DECLARE @FTL_Join nvarchar(4000);
DECLARE @TipiOggettoID nvarchar(8);
DECLARE @OrderByStatement nvarchar(64);
DECLARE @UltimaProcedura bit;
SET @Campi = 'O.OggettoID, O.TipoOggettoID, OP.ProceduraID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, E.Nome, OP.OggettoProceduraID ';
-- OggettiVia
SET @TipiOggettoID = N'4';
-- Ultima procedura a false per la ricerca per procedura o per attributo
IF @ProceduraID IS NOT NULL OR @AttributoID IS NOT NULL
SET @UltimaProcedura = NULL
ELSE
SET @UltimaProcedura = 1
IF (@TestoRicerca <> N'')
SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryOggetti(@Lingua));
ELSE
SET @FTL_Join = N''
IF (@OrderBy IS NULL OR @OrderBy = N'')
IF @FTL_Join = N''
BEGIN
SET @OrderBy = N'OP.DataInserimento'
SET @OrderDirection = N'DESC'
END
ELSE
BEGIN
SET @OrderBy = N'FTL.RANK'
SET @OrderDirection = N'DESC'
END
IF (@OrderDirection IS NULL OR @OrderDirection = N'')
SET @OrderDirection = 'ASC'
SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
-- Insert statements for procedure here
SET @Query_Base = N'FROM dbo.TBL_Oggetti AS O INNER JOIN  dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID ' + 
  N'INNER JOIN  dbo.TBL_ExtraOggettiImpianto AS EO ON EO.OggettoID = O.OggettoID  ' +
  N'INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID ' +
  N'INNER JOIN  dbo.TBL_CategorieImpianti AS OPE ON OPE.CategoriaImpiantoID = EO.CategoriaImpiantoID ' +
  N'INNER JOIN  dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = OP.OggettoProceduraID  ' +
  N'INNER JOIN  dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID ' +
  N'LEFT OUTER JOIN  dbo.STG_OggettiProcedureAttributi AS OPA ON OP.OggettoProceduraID = OPA.OggettoProceduraID ' + @FTL_JOIN + 
N'WHERE O.TipoOggettoID IN (' + @TipiOggettoID + ') AND ' +
N' ((OP.ProceduraID = @ProceduraID) OR (@ProceduraID = 0) OR (@ProceduraID IS NULL)) AND ' + -- Procedura
N' ((OPE.CategoriaImpiantoID = @TipologiaID) OR (@TipologiaID IS NULL)) AND ' + -- Categoria
N' ((OPA.AttributoID = @AttributoID) OR (@AttributoID IS NULL)) AND ' + -- Attributo
N' (SOPE.RuoloEntitaID = 10) AND ' + -- Gestore
N' ((OP.UltimaProcedura = @UltimaProcedura) OR (@UltimaProcedura IS NULL)) AND ' + -- Ultima Procedura
N' (T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL)' + 
--N' (O.TipoOggettoID = ' + @TipiOggettoID + ')' + -- TipoOggetto
N'GROUP BY ' + @OrderBy + ', ' + @Campi;
SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
+ @OrderByStatement + N') AS RN, ' + @Campi + -- campi
@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';
SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) FROM (SELECT ' + @Campi + @Query_Base + ') T';
IF (@FTL_Join <> '')
BEGIN
EXECUTE sp_executesql @Query
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, @TestoRicerca nvarchar(128), 
@StartRowNum int, @EndRowNum int'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @TestoRicerca = @TestoRicerca
, @StartRowNum = @StartRowNum
, @EndRowNum = @EndRowNum;
EXECUTE sp_executesql @QueryCount
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, @TestoRicerca nvarchar(128), 
@TotalRowCount int OUTPUT'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @TestoRicerca = @TestoRicerca
, @TotalRowCount = @TotalRowCount OUTPUT;
END
ELSE
BEGIN
EXECUTE sp_executesql @Query
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, 
@StartRowNum int, @EndRowNum int'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @StartRowNum = @StartRowNum
, @EndRowNum = @EndRowNum;
EXECUTE sp_executesql @QueryCount
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, 
@TotalRowCount int OUTPUT'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @TotalRowCount = @TotalRowCount OUTPUT;
END
SELECT @TotalRowCount;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiHome]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaOggettiHome]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

SELECT * FROM (
    -- Valutazione Impatto Ambientale (VIA)
	SELECT 
		O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, VDA.ValoreData AS DataScadenzaPresentazione, O.ImmagineLocalizzazione, 
		dbo.FN_SintesiNonTecnicaID(OP.OggettoProceduraID, O.TipoOggettoID) AS DocumentoID, OPR.TipologiaID AS TipologiaOperaID, O.TipoOggettoID, 1 as TipoElenco,
		null as CategoriaImpiantoID, E.Nome as ProponenteGestore
	FROM
		dbo.TBL_Oggetti AS O INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN
		dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN 
		dbo.TBL_ExtraOggettiProgetto AS EOP ON EOP.OggettoID = O.OggettoID INNER JOIN 
		dbo.TBL_Opere AS OPR ON OPR.OperaID = EOP.OperaID INNER JOIN 
		dbo.TBL_DatiAmministrativi AS DA ON DA.DatoAmministrativoID = VDA.DatoAmministrativoID
		LEFT JOIN STG_OggettiProcedureEntita OPE on OPE.OggettoProceduraID = OP.OggettoProceduraID AND OPE.RuoloEntitaID = 1
		LEFT JOIN dbo.TBL_Entita E ON E.EntitaID  = OPE.EntitaID 
	WHERE (OP.ProceduraID IN (3, 14, 110)) AND 
          (DA.TipoDatoAmministrativo = 2) AND 
          (DATEADD(SECOND, 86399, CAST(CAST(VDA.ValoreData AS date) AS datetime)) > GETDATE()) AND 
          (O.TipoOggettoID = 1)
   -- ORDER BY DataScadenzaPresentazione
UNION ALL
    -- Verifica Assoggettabilità a VIA (VIA)
	SELECT 
		O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, VDA.ValoreData AS DataScadenzaPresentazione, O.ImmagineLocalizzazione, 
		dbo.FN_SintesiNonTecnicaID(OP.OggettoProceduraID, O.TipoOggettoID) AS DocumentoID, OPR.TipologiaID AS TipologiaOperaID, O.TipoOggettoID, 2 as TipoElenco,
		null as CategoriaImpiantoID, E.Nome as ProponenteGestore
	FROM dbo.TBL_Oggetti AS O INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN
		dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN 
		dbo.TBL_ExtraOggettiProgetto AS EOP ON EOP.OggettoID = O.OggettoID INNER JOIN 
		dbo.TBL_Opere AS OPR ON OPR.OperaID = EOP.OperaID INNER JOIN 
		dbo.TBL_DatiAmministrativi AS DA ON DA.DatoAmministrativoID = VDA.DatoAmministrativoID
		LEFT JOIN STG_OggettiProcedureEntita OPE on OPE.OggettoProceduraID = OP.OggettoProceduraID AND OPE.RuoloEntitaID = 1
		LEFT JOIN dbo.TBL_Entita E ON E.EntitaID  = OPE.EntitaID 
	WHERE (OP.ProceduraID = 5) AND 
          (DA.TipoDatoAmministrativo = 2) AND 
          (DATEADD(SECOND, 86399, CAST(CAST(VDA.ValoreData AS date) AS datetime)) > GETDATE()) AND 
          (O.TipoOggettoID = 1)
   -- ORDER BY DataScadenzaPresentazione
UNION ALL
    -- Piani/Programmi in consultazione (VAS)
	SELECT 
		O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, VDA.ValoreData AS DataScadenzaPresentazione, O.ImmagineLocalizzazione, 
		dbo.FN_SintesiNonTecnicaID(OP.OggettoProceduraID, O.TipoOggettoID) AS DocumentoID, 0 AS TipologiaOperaID, O.TipoOggettoID,  3 as TipoElenco,
		null as CategoriaImpiantoID, E.Nome as ProponenteGestore
	FROM dbo.TBL_Oggetti AS O INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN
		dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN 
		dbo.TBL_DatiAmministrativi AS DA ON DA.DatoAmministrativoID = VDA.DatoAmministrativoID
		LEFT JOIN STG_OggettiProcedureEntita OPE on OPE.OggettoProceduraID = OP.OggettoProceduraID AND OPE.RuoloEntitaID = 1
		LEFT JOIN dbo.TBL_Entita E ON E.EntitaID  = OPE.EntitaID 
	WHERE (OP.UltimaProcedura = 1) AND 
          (DA.TipoDatoAmministrativo = 2) AND 
          (DATEADD(SECOND, 86399, CAST(CAST(VDA.ValoreData AS date) AS datetime)) > GETDATE()) AND 
          (O.TipoOggettoID IN (2, 3))
  
UNION ALL
    -- AIA
	SELECT
		O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, 
		VDA.ValoreData AS DataScadenzaPresentazione, O.ImmagineLocalizzazione, 
		dbo.FN_SintesiNonTecnicaID(OP.OggettoProceduraID, O.TipoOggettoID) AS DocumentoID,
		0 AS TipologiaOperaID, O.TipoOggettoID,  7 as TipoElenco,
		dbo.TBL_CategorieImpianti.CategoriaImpiantoID, E.Nome as ProponenteGestore
	FROM dbo.TBL_Oggetti AS O 
		INNER JOIN 
			dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID 
		INNER JOIN
			dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID 
		INNER JOIN 
			dbo.TBL_DatiAmministrativi AS DA ON DA.DatoAmministrativoID = VDA.DatoAmministrativoID
		 INNER JOIN
            dbo.TBL_ExtraOggettiImpianto ON O.OggettoID = dbo.TBL_ExtraOggettiImpianto.OggettoID 
         INNER JOIN
            dbo.TBL_CategorieImpianti ON dbo.TBL_ExtraOggettiImpianto.CategoriaImpiantoID = dbo.TBL_CategorieImpianti.CategoriaImpiantoID	            
		INNER JOIN dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID
		LEFT JOIN STG_OggettiProcedureEntita OPE on OPE.OggettoProceduraID = OP.OggettoProceduraID AND OPE.RuoloEntitaID = 10
		LEFT JOIN dbo.TBL_Entita E ON E.EntitaID  = OPE.EntitaID             
	WHERE (OP.ProceduraID in (201,202,203,204,205)) 
		and	
          (DA.TipoDatoAmministrativo = 2) 
         AND 
          (O.TipoOggettoID = 4 )
         AND 
			(DATEADD(SECOND, 86399, CAST(CAST(VDA.ValoreData AS date) AS datetime)) > GETDATE()) 
		
		-- COndizione per escludere i provvedimenti Regionali AIA
		AND
			(T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL)
    
    ) lista
    
    ORDER BY DataScadenzaPresentazione


END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiPerProceduraInCorso]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaOggettiPerProceduraInCorso]
	-- Add the parameters for the stored procedure here
	@MacroTipoOggettoID int, 
	@parametro int = 0, 
	@concluse bit = 0, 
	@Lingua varchar(2), 
	@TestoRicerca nvarchar(128), 
	@OrderBy nvarchar(32),
	@OrderDirection nvarchar(4),
	@StartRowNum int,
	@EndRowNum int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @sqlQ nvarchar(MAX)

	IF @MacroTipoOggettoID = 1
		SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA(@parametro ,0, @concluse)
		
	IF @MacroTipoOggettoID = 2
		SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVAS(@parametro ,0, @concluse)
		
	IF @MacroTipoOggettoID = 3	
		SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA(@parametro ,0, @concluse)

	DECLARE @TotalRowCount int;
	DECLARE @Query_Base nvarchar(MAX);
	DECLARE @QueryCount nvarchar(MAX);
	DECLARE @Query nvarchar(4000);
	DECLARE @FTL_Join nvarchar(4000);
	DECLARE @TipiOggettoID nvarchar(8);
	DECLARE @OrderByStatement nvarchar(64);
	DECLARE @UltimaProcedura bit;

		IF (@TestoRicerca <> N'')
		SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryOggetti(@Lingua));
	ELSE
		SET @FTL_Join = N''

	IF (@OrderBy IS NULL OR @OrderBy = N'')
		IF @FTL_Join = N''
			BEGIN
				SET @OrderBy = N'O.ValoreData'
				SET @OrderDirection = N'DESC'
			END
		ELSE
			BEGIN
				SET @OrderBy = N'FTL.RANK'
				SET @OrderDirection = N'DESC'
			END

	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
	
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	
    -- Insert statements for procedure here
	SET @Query_Base = N'FROM (' + @sqlQ + ') AS O INNER JOIN ' +
	N'	dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = O.OggettoProceduraID INNER JOIN ' + 
	N'	dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID ' + @FTL_JOIN + 
	N'WHERE (SOPE.RuoloEntitaID IN (1,10))';
	
--select @Query_Base
			
	SET @Query = N'SELECT TOP(9999999) * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
	+ @OrderByStatement + N') AS RN, ' + 
	'O.OggettoID, O.Nome_IT, O.Nome_EN, E.Nome, O.ValoreData, O.ViperaAIAID, O.OggettoProceduraID, O.TipoOggettoID, ' +
	' O.StatoProceduraID '
	
		
	SET @Query =  @Query + @Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';
    
--select @Query
    
	SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;

--select @QueryCount

	IF (@FTL_Join <> '')
	  BEGIN
		 EXECUTE sp_executesql @Query
		 , N'@TestoRicerca nvarchar(128), 
		 @StartRowNum int, @EndRowNum int'

		 , @TestoRicerca = @TestoRicerca
		 , @StartRowNum = @StartRowNum
		 , @EndRowNum = @EndRowNum;

		 EXECUTE sp_executesql @QueryCount
		 , N'@TestoRicerca nvarchar(128), 
		 @TotalRowCount int OUTPUT'
		 , @TestoRicerca = @TestoRicerca
		 , @TotalRowCount = @TotalRowCount OUTPUT;
	  END
	ELSE
	  BEGIN
		 EXECUTE sp_executesql @Query
		 , N'@StartRowNum int, @EndRowNum int'
		 , @StartRowNum = @StartRowNum
		 , @EndRowNum = @EndRowNum;

		 EXECUTE sp_executesql @QueryCount
		 , N'@TotalRowCount int OUTPUT'
		 , @TotalRowCount = @TotalRowCount OUTPUT;
	  END
	  
	  SELECT @TotalRowCount;

END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiPerTerritorioVia]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaOggettiPerTerritorioVia]
	-- Add the parameters for the stored procedure here
	@TerritorioID uniqueidentifier, 
	@OrderBy nvarchar(32),
	@OrderDirection nvarchar(4),
	@StartRowNum int,
	@EndRowNum int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalRowCount int;
	DECLARE @Query_Base nvarchar(4000);
	DECLARE @QueryCount nvarchar(4000);
	DECLARE @Query nvarchar(4000);
	--DECLARE @FTL_Join nvarchar(512);
	DECLARE @TipiOggettoID nvarchar(8);
	DECLARE @OrderByStatement nvarchar(64);
	DECLARE @UltimaProcedura bit;
	
	-- OggettiVia
	SET @TipiOggettoID = N'1';
	
	-- Ultima procedura a false per la ricerca per procedura
	--IF @ProceduraID IS NOT NULL
	--	SET @UltimaProcedura = 0
	--ELSE
	--	SET @UltimaProcedura = 1
	
	--IF (@TestoRicerca <> N'')
	--	SET @FTL_Join = CASE WHEN @Lingua = N'EN' THEN
	--	 N' INNER JOIN FREETEXTTABLE(dbo.FTL_Oggetti, (Nome_EN, Descrizione_EN), @TestoRicerca, LANGUAGE N''English'') AS FTL ON O.OggettoID = FTL.[KEY] '
	--	 ELSE
	--	 N' INNER JOIN FREETEXTTABLE(dbo.FTL_Oggetti, (Nome_IT, Descrizione_IT), @TestoRicerca, LANGUAGE N''Italian'') AS FTL ON O.OggettoID = FTL.[KEY] '
	--	 END
	--ELSE
	--	SET @FTL_Join = N''

	SET @OrderBy = N'OP.DataInserimento'
	SET @OrderDirection = N'DESC'

	--IF (@OrderBy IS NULL OR @OrderBy = N'')
	--	IF @FTL_Join = N''
	--		BEGIN
	--			SET @OrderBy = N'OP.DataInserimento'
	--			SET @OrderDirection = N'DESC'
	--		END
	--	ELSE
	--		BEGIN
	--			SET @OrderBy = N'FTL.RANK'
	--			SET @OrderDirection = N'DESC'
	--		END

	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
	
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	
    -- Insert statements for procedure here
	SET @Query_Base = N'FROM dbo.TBL_Oggetti AS O INNER JOIN ' +
	N'	dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN ' +
	N'	dbo.STG_OggettiTerritori AS S ON S.OggettoID = O.OggettoID INNER JOIN ' + 
	N'	dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID ' + 
	N'WHERE O.TipoOggettoID IN (' + @TipiOggettoID + ') AND ' +
	N'	(S.TerritorioID = @TerritorioID) AND ' +  -- Territorio
	N'	(SOPE.RuoloEntitaID = 1) AND ' +  -- Proponente
	N'	(O.TipoOggettoID = ' + @TipiOggettoID + ')'; -- TipoOggetto
		
	SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
	+ @OrderByStatement + N') AS RN, ' + 
	'O.OggettoID, O.TipoOggettoID, OP.ProceduraID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, E.Nome ' + -- campi
	@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

	SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;

	 EXECUTE sp_executesql @Query
	 , N'@TerritorioID uniqueidentifier, 
	 @StartRowNum int, @EndRowNum int'
	 , @TerritorioID = @TerritorioID
	 , @StartRowNum = @StartRowNum
	 , @EndRowNum = @EndRowNum;

	 EXECUTE sp_executesql @QueryCount
	 , N'@TerritorioID uniqueidentifier, 
	 @TotalRowCount int OUTPUT'
	 , @TerritorioID = @TerritorioID
	 , @TotalRowCount = @TotalRowCount OUTPUT;

	--IF (@FTL_Join <> '')
	--  BEGIN
	--	 EXECUTE sp_executesql @Query
	--	 , N'@ProceduraID int, @TipologiaID int, @TestoRicerca nvarchar(128), 
	--	 @StartRowNum int, @EndRowNum int'
	--	 , @ProceduraID = @ProceduraID
	--	 , @TipologiaID = @TipologiaID
	--	 , @TestoRicerca = @TestoRicerca
	--	 , @StartRowNum = @StartRowNum
	--	 , @EndRowNum = @EndRowNum;

	--	 EXECUTE sp_executesql @QueryCount
	--	 , N'@ProceduraID int, @TipologiaID int, @TestoRicerca nvarchar(128), 
	--	 @TotalRowCount int OUTPUT'
	--	 , @ProceduraID = @ProceduraID
	--	 , @TipologiaID = @TipologiaID
	--	 , @TestoRicerca = @TestoRicerca
	--	 , @TotalRowCount = @TotalRowCount OUTPUT;
	--  END
	--ELSE
	--  BEGIN
	--	 EXECUTE sp_executesql @Query
	--	 , N'@ProceduraID int, @TipologiaID int, 
	--	 @StartRowNum int, @EndRowNum int'
	--	 , @ProceduraID = @ProceduraID
	--	 , @TipologiaID = @TipologiaID
	--	 , @StartRowNum = @StartRowNum
	--	 , @EndRowNum = @EndRowNum;

	--	 EXECUTE sp_executesql @QueryCount
	--	 , N'@ProceduraID int, @TipologiaID int, 
	--	 @TotalRowCount int OUTPUT'
	--	 , @ProceduraID = @ProceduraID
	--	 , @TipologiaID = @TipologiaID
	--	 , @TotalRowCount = @TotalRowCount OUTPUT;
	--  END
	  
	  SELECT @TotalRowCount;
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiTerritorio]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaOggettiTerritorio]
	-- Add the parameters for the stored procedure here
	@MacroTipoOggettoID int,
	@TipologiaTerritorioID int, 
	@TestoRicerca nvarchar(128), 
	@OrderBy nvarchar(32),
	@OrderDirection nvarchar(4),
	@StartRowNum int,
	@EndRowNum int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalRowCount int;
	DECLARE @Campi nvarchar(512);
	DECLARE @Query_Base nvarchar(4000);
	DECLARE @QueryCount nvarchar(4000);
	DECLARE @Query nvarchar(4000);
	DECLARE @FTL_Join nvarchar(4000);
	DECLARE @TipiOggettoID nvarchar(8);
	DECLARE @OrderByStatement nvarchar(64);
	DECLARE @UltimaProcedura bit;
	DECLARE @Testo nvarchar(250);
	
	SET @Campi = 'O.OggettoID, O.TipoOggettoID, OP.ProceduraID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, E.Nome, OP.OggettoProceduraID, OGG.Territori ';
	
	IF @MacroTipoOggettoID = 1 SET @TipiOggettoID = N'1';
	IF @MacroTipoOggettoID = 2 SET @TipiOggettoID = N'2,3';
	IF @MacroTipoOggettoID = 3 SET @TipiOggettoID = N'4';
	
	-- Ultima procedura
	SET @UltimaProcedura = 1
	
	-- Like testo ricerca
	SET @Testo = (SELECT dbo.FN_ApplicaCriterio(@TestoRicerca, 0));
	
	IF (@TestoRicerca <> N'')
	BEGIN
		SET @FTL_Join = N' INNER JOIN (SELECT [KEY], [RANK]' +
			'	FROM FREETEXTTABLE(dbo.FTL_Oggetti, (Territori), @TestoRicerca, LANGUAGE N''Italian'') ' +
			') AS FTL ON O.OggettoID = FTL.[KEY] ';
	END
	ELSE
		SET @FTL_Join = N''

	IF (@OrderBy IS NULL OR @OrderBy = N'')
		IF @FTL_Join = N''
			BEGIN
				SET @OrderBy = N'OP.DataInserimento'
				SET @OrderDirection = N'DESC'
			END
		ELSE
			BEGIN
				SET @OrderBy = N'OP.DataInserimento'
				SET @OrderDirection = N'DESC'
			END

	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
	
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	
    -- Insert statements for procedure here
	SET @Query_Base = N'FROM dbo.TBL_Oggetti AS O INNER JOIN ' +
	N'	dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN ' +
	N'	dbo.STG_OggettiTerritori AS SOT ON O.OggettoID = SOT.OggettoID INNER JOIN ' + 
	N'	dbo.TBL_Territori AS T ON T.TerritorioID = SOT.TerritorioID INNER JOIN ' + 
	N'	dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN ' + 
	N'	dbo.FTL_Oggetti AS OGG ON OGG.OggettoID = O.OggettoID INNER JOIN ' + 
	N'	dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID ' + 
	N'WHERE O.TipoOggettoID IN (' + @TipiOggettoID + ') AND ' +
	N'	((T.TipologiaTerritorioID = @TipologiaTerritorioID) OR (@TipologiaTerritorioID IS NULL)) AND ' +  -- Tipologia territorio
	N'	(T.Nome LIKE @Testo) AND ' +  -- Nome territorio
	N'	(SOPE.RuoloEntitaID  IN (1,10)) AND ' +  -- Proponente / Gestore
	N'	((OP.UltimaProcedura = @UltimaProcedura) OR (@UltimaProcedura IS NULL)) AND ' +  -- Ultima Procedura
	N'	(O.TipoOggettoID = ' + @TipiOggettoID + ')' +  -- TipoOggetto
	N'GROUP BY ' + @OrderBy + ', ' + @Campi;
		
	SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
	+ @OrderByStatement + N') AS RN, ' + @Campi + -- campi
	@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

	SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) FROM (SELECT ' + @Campi + @Query_Base + ') T';

	--SELECT @Query;

	IF (@Testo <> '')
	  BEGIN
		 EXECUTE sp_executesql @Query
		 , N'@TipologiaTerritorioID int, @UltimaProcedura bit, @Testo nvarchar(256), 
		 @StartRowNum int, @EndRowNum int'
		 , @TipologiaTerritorioID = @TipologiaTerritorioID
		 , @UltimaProcedura = @UltimaProcedura
		 , @Testo = @Testo
		 , @StartRowNum = @StartRowNum
		 , @EndRowNum = @EndRowNum;

		 EXECUTE sp_executesql @QueryCount
		 , N'@TipologiaTerritorioID int, @UltimaProcedura bit, @Testo nvarchar(256), 
		 @TotalRowCount int OUTPUT'
		 , @TipologiaTerritorioID = @TipologiaTerritorioID
		 , @UltimaProcedura = @UltimaProcedura
		 , @Testo = @Testo
		 , @TotalRowCount = @TotalRowCount OUTPUT;
	  END
	ELSE
	  BEGIN
		 EXECUTE sp_executesql @Query
		 , N'@TipologiaTerritorioID int, @UltimaProcedura bit, 
		 @StartRowNum int, @EndRowNum int'
		 , @TipologiaTerritorioID = @TipologiaTerritorioID
		 , @UltimaProcedura = @UltimaProcedura
		 , @StartRowNum = @StartRowNum
		 , @EndRowNum = @EndRowNum;

		 EXECUTE sp_executesql @QueryCount
		 , N'@TipologiaTerritorioID int, @UltimaProcedura bit, 
		 @TotalRowCount int OUTPUT'
		 , @TipologiaTerritorioID = @TipologiaTerritorioID
		 , @UltimaProcedura = @UltimaProcedura
		 , @TotalRowCount = @TotalRowCount OUTPUT;
	  END
	  
	  SELECT @TotalRowCount;
	  
	  --SELECT @Testo;
	  
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiVas]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaOggettiVas]
-- Add the parameters for the stored procedure here
@ProceduraID int, 
@SettoreID int, 
@AttributoID int, 
@Lingua varchar(2), 
@TestoRicerca nvarchar(128), 
@OrderBy nvarchar(32),
@OrderDirection nvarchar(4),
@StartRowNum int,
@EndRowNum int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalRowCount int;
	DECLARE @Campi nvarchar(512);
	DECLARE @Query_Base nvarchar(4000);
	DECLARE @QueryCount nvarchar(4000);
	DECLARE @Query nvarchar(4000);
	DECLARE @FTL_Join nvarchar(4000);
	DECLARE @TipiOggettoID nvarchar(8);
	DECLARE @OrderByStatement nvarchar(64);
	DECLARE @UltimaProcedura bit;
	SET @Campi = 'O.OggettoID, O.TipoOggettoID, OP.ProceduraID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, E.Nome, OP.OggettoProceduraID ';
	-- OggettiVas
	SET @TipiOggettoID = N'2,3';
	-- Ultima procedura a false per la ricerca per procedura o per attributo
	IF @ProceduraID IS NOT NULL OR @AttributoID IS NOT NULL
		SET @UltimaProcedura = NULL
	ELSE
		SET @UltimaProcedura = 1

	IF (@TestoRicerca <> N'')
		SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryOggetti(@Lingua));
	ELSE
		SET @FTL_Join = N''
	IF (@OrderBy IS NULL OR @OrderBy = N'')
		IF @FTL_Join = N''
		BEGIN
			SET @OrderBy = N'OP.DataInserimento'
			SET @OrderDirection = N'DESC'
		END
		ELSE
		BEGIN
			SET @OrderBy = N'FTL.RANK'
			SET @OrderDirection = N'DESC'
		END
	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
		
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	-- Insert statements for procedure here
	SET @Query_Base = N'FROM dbo.TBL_Oggetti AS O INNER JOIN ' +
	N' dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN ' +
	N' dbo.TBL_ExtraOggettiPianoProgramma AS EO ON EO.OggettoID = O.OggettoID INNER JOIN ' + 
	N' dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN ' + 
	N' dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID LEFT OUTER JOIN ' + 
	N' dbo.STG_OggettiProcedureAttributi AS OPA ON OP.OggettoProceduraID = OPA.OggettoProceduraID ' + @FTL_JOIN + 
	N'WHERE O.TipoOggettoID IN (' + @TipiOggettoID + ') AND ' +
	N' ((OP.ProceduraID = @ProceduraID) OR (@ProceduraID = 0) OR (@ProceduraID IS NULL)) AND ' + -- Procedura
	N' ((EO.SettoreID = @SettoreID) OR (@SettoreID IS NULL)) AND ' + -- Settore
	N' ((OPA.AttributoID = @AttributoID) OR (@AttributoID IS NULL)) AND ' + -- Attributo
	N' (SOPE.RuoloEntitaID = 1) AND ' + -- Proponente
	N' ((OP.UltimaProcedura = @UltimaProcedura) OR (@UltimaProcedura IS NULL)) AND ' + -- Ultima Procedura
	N' (O.TipoOggettoID IN (' + @TipiOggettoID + '))' + -- TipoOggetto
	N'GROUP BY ' + @OrderBy + ', ' + @Campi;
	SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
	+ @OrderByStatement + N') AS RN, ' + @Campi + -- campi
	@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';
	SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) FROM (SELECT ' + @Campi + @Query_Base + ') T';
	IF (@FTL_Join <> '')
	BEGIN
		EXECUTE sp_executesql @Query
		, N'@ProceduraID int, @SettoreID int, @AttributoID int, @UltimaProcedura bit, @TestoRicerca nvarchar(128), 
		@StartRowNum int, @EndRowNum int'
		, @ProceduraID = @ProceduraID
		, @SettoreID = @SettoreID
		, @AttributoID = @AttributoID
		, @UltimaProcedura = @UltimaProcedura
		, @TestoRicerca = @TestoRicerca
		, @StartRowNum = @StartRowNum
		, @EndRowNum = @EndRowNum;
		EXECUTE sp_executesql @QueryCount
		, N'@ProceduraID int, @SettoreID int, @AttributoID int, @UltimaProcedura bit, @TestoRicerca nvarchar(128), 
		@TotalRowCount int OUTPUT'
		, @ProceduraID = @ProceduraID
		, @SettoreID = @SettoreID
		, @AttributoID = @AttributoID
		, @UltimaProcedura = @UltimaProcedura
		, @TestoRicerca = @TestoRicerca
		, @TotalRowCount = @TotalRowCount OUTPUT;
	END
	ELSE
	BEGIN
		EXECUTE sp_executesql @Query
		, N'@ProceduraID int, @SettoreID int, @AttributoID int, @UltimaProcedura bit, 
		@StartRowNum int, @EndRowNum int'
		, @ProceduraID = @ProceduraID
		, @SettoreID = @SettoreID
		, @AttributoID = @AttributoID
		, @UltimaProcedura = @UltimaProcedura
		, @StartRowNum = @StartRowNum
		, @EndRowNum = @EndRowNum;
		EXECUTE sp_executesql @QueryCount
		, N'@ProceduraID int, @SettoreID int, @AttributoID int, @UltimaProcedura bit, 
		@TotalRowCount int OUTPUT'
		, @ProceduraID = @ProceduraID
		, @SettoreID = @SettoreID
		, @AttributoID = @AttributoID
		, @UltimaProcedura = @UltimaProcedura
		, @TotalRowCount = @TotalRowCount OUTPUT;
	END
	SELECT @TotalRowCount;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiVia]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaOggettiVia]
-- Add the parameters for the stored procedure here
@ProceduraID int, 
@TipologiaID int, 
@AttributoID int, 
@Lingua varchar(2), 
@TestoRicerca nvarchar(128), 
@OrderBy nvarchar(32),
@OrderDirection nvarchar(4),
@StartRowNum int,
@EndRowNum int
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
-- interfering with SELECT statements.
SET NOCOUNT ON;
DECLARE @TotalRowCount int;
DECLARE @Campi nvarchar(512);
DECLARE @Query_Base nvarchar(4000);
DECLARE @QueryCount nvarchar(4000);
DECLARE @Query nvarchar(4000);
DECLARE @FTL_Join nvarchar(4000);
DECLARE @TipiOggettoID nvarchar(8);
DECLARE @OrderByStatement nvarchar(64);
DECLARE @UltimaProcedura bit;
SET @Campi = 'O.OggettoID, O.TipoOggettoID, OP.ProceduraID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, E.Nome, OP.OggettoProceduraID ';
-- OggettiVia
SET @TipiOggettoID = N'1';
-- Ultima procedura a false per la ricerca per procedura o per attributo
IF @ProceduraID IS NOT NULL OR @AttributoID IS NOT NULL
SET @UltimaProcedura = NULL
ELSE
SET @UltimaProcedura = 1
IF (@TestoRicerca <> N'')
SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryOggetti(@Lingua));
ELSE
SET @FTL_Join = N''
IF (@OrderBy IS NULL OR @OrderBy = N'')
IF @FTL_Join = N''
BEGIN
SET @OrderBy = N'OP.DataInserimento'
SET @OrderDirection = N'DESC'
END
ELSE
BEGIN
SET @OrderBy = N'FTL.RANK'
SET @OrderDirection = N'DESC'
END
IF (@OrderDirection IS NULL OR @OrderDirection = N'')
SET @OrderDirection = 'ASC'
SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
-- Insert statements for procedure here
SET @Query_Base = N'FROM dbo.TBL_Oggetti AS O INNER JOIN ' +
N' dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN ' +
N' dbo.TBL_ExtraOggettiProgetto AS EO ON EO.OggettoID = O.OggettoID INNER JOIN ' + 
N' dbo.TBL_Opere AS OPE ON OPE.OperaID = EO.OperaID INNER JOIN ' + 
N' dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN ' + 
N' dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID LEFT OUTER JOIN ' + 
N' dbo.STG_OggettiProcedureAttributi AS OPA ON OP.OggettoProceduraID = OPA.OggettoProceduraID ' + @FTL_JOIN + 
N'WHERE O.TipoOggettoID IN (' + @TipiOggettoID + ') AND ' +
N' ((OP.ProceduraID = @ProceduraID) OR (@ProceduraID = 0) OR (@ProceduraID IS NULL)) AND ' + -- Procedura
N' ((OPE.TipologiaID = @TipologiaID) OR (@TipologiaID IS NULL)) AND ' + -- Tipologia
N' ((OPA.AttributoID = @AttributoID) OR (@AttributoID IS NULL)) AND ' + -- Attributo
N' (SOPE.RuoloEntitaID = 1) AND ' + -- Proponente
N' ((OP.UltimaProcedura = @UltimaProcedura) OR (@UltimaProcedura IS NULL)) AND ' + -- Ultima Procedura
N' (O.TipoOggettoID = ' + @TipiOggettoID + ')' + -- TipoOggetto
N'GROUP BY ' + @OrderBy + ', ' + @Campi;
SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
+ @OrderByStatement + N') AS RN, ' + @Campi + -- campi
@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';
SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) FROM (SELECT ' + @Campi + @Query_Base + ') T';
IF (@FTL_Join <> '')
BEGIN
EXECUTE sp_executesql @Query
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, @TestoRicerca nvarchar(128), 
@StartRowNum int, @EndRowNum int'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @TestoRicerca = @TestoRicerca
, @StartRowNum = @StartRowNum
, @EndRowNum = @EndRowNum;
EXECUTE sp_executesql @QueryCount
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, @TestoRicerca nvarchar(128), 
@TotalRowCount int OUTPUT'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @TestoRicerca = @TestoRicerca
, @TotalRowCount = @TotalRowCount OUTPUT;
END
ELSE
BEGIN
EXECUTE sp_executesql @Query
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, 
@StartRowNum int, @EndRowNum int'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @StartRowNum = @StartRowNum
, @EndRowNum = @EndRowNum;
EXECUTE sp_executesql @QueryCount
, N'@ProceduraID int, @TipologiaID int, @AttributoID int, @UltimaProcedura bit, 
@TotalRowCount int OUTPUT'
, @ProceduraID = @ProceduraID
, @TipologiaID = @TipologiaID
, @AttributoID = @AttributoID
, @UltimaProcedura = @UltimaProcedura
, @TotalRowCount = @TotalRowCount OUTPUT;
END
SELECT @TotalRowCount;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaOggettiViaSpaziale]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaOggettiViaSpaziale]
	-- Add the parameters for the stored procedure here
	@xMax float, 
	@yMax float, 
	@xMin float, 
	@yMin float, 
	@OrderBy nvarchar(32),
	@OrderDirection nvarchar(4),
	@StartRowNum int,
	@EndRowNum int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalRowCount int;
	DECLARE @Query_Base nvarchar(4000);
	DECLARE @QueryCount nvarchar(4000);
	DECLARE @Query nvarchar(4000);
	DECLARE @TipiOggettoID nvarchar(8);
	DECLARE @OrderByStatement nvarchar(64);
	DECLARE @UltimaProcedura bit;
	
	-- OggettiVia
	SET @TipiOggettoID = N'1';
	
	IF (@OrderBy IS NULL OR @OrderBy = N'')
	BEGIN
		SET @OrderBy = N'OP.DataInserimento'
		SET @OrderDirection = N'DESC'
	END

	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
	
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	
    -- Insert statements for procedure here
	SET @Query_Base = N'FROM dbo.TBL_Oggetti AS O INNER JOIN ' +
	N'	dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN ' +
	N'	dbo.STG_OggettiProcedureEntita AS SOPE ON SOPE.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN ' + 
	N'	dbo.TBL_Entita AS E ON E.EntitaID = SOPE.EntitaID ' + 
	N'WHERE  ((LongitudineEst <= @xMax AND LongitudineOvest >= @xMin ' +
	N' AND LatitudineNord <= @yMax AND LatitudineSud >= @yMin) ' +
	N' OR ' +
	N' ( ' +
	N' (LongitudineEst >= @xMax AND LongitudineOvest <= @xMax) ' +
	N' AND (LongitudineOvest <= @xMin AND LongitudineEst >= @xMin) ' +
	N' ) ' +
	N' AND ' +
	N' ( ' +
	N' (LatitudineNord >= @yMax AND LatitudineSud <= @yMax) ' +
	N' AND (LatitudineSud <= @yMin AND LatitudineNord >= @yMin) ' +
	N' )) AND ' +
	N'	(SOPE.RuoloEntitaID = 1) AND ' +  -- Proponente
	N'	(OP.UltimaProcedura = 1) AND ' +  -- Ultima Procedura
	N'	(O.TipoOggettoID = ' + @TipiOggettoID + ')'; -- TipoOggetto
		
	SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
	+ @OrderByStatement + N') AS RN, ' + 
	'O.OggettoID, O.TipoOggettoID, OP.ProceduraID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, E.Nome, OP.OggettoProceduraID ' + -- campi
	@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

	SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;

	 --SELECT @Query
	 EXECUTE sp_executesql @Query
	 , N'@xMax float, @yMax float, @xMin float, @yMin float, 
	 @StartRowNum int, @EndRowNum int'
	 , @xMax = @xMax
	 , @yMax = @yMax
	 , @xMin = @xMin
	 , @yMin = @yMin
	 , @StartRowNum = @StartRowNum
	 , @EndRowNum = @EndRowNum;

	 EXECUTE sp_executesql @QueryCount
	 , N'@xMax float, @yMax float, @xMin float, @yMin float, 
	 @TotalRowCount int OUTPUT'
	 , @xMax = @xMax
	 , @yMax = @yMax
	 , @xMin = @xMin
	 , @yMin = @yMin
	 , @TotalRowCount = @TotalRowCount OUTPUT;
	  
	  SELECT @TotalRowCount;
END


GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaPagineStatiche]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaPagineStatiche]
	-- Add the parameters for the stored procedure here
	@Lingua varchar(2), 
	@TestoRicerca nvarchar(128), 
	@OrderBy nvarchar(32),
	@OrderDirection nvarchar(4),
	@StartRowNum int,
	@EndRowNum int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @TotalRowCount int;
	DECLARE @Query_Base nvarchar(4000);
	DECLARE @QueryCount nvarchar(4000);
	DECLARE @Query nvarchar(4000);
	DECLARE @FTL_Join nvarchar(4000);
	DECLARE @OrderByStatement nvarchar(64);
	
	IF (@TestoRicerca <> N'')
		SET @FTL_Join = (SELECT dbo.FT_FN_CreaQueryPagineStatiche(@Lingua));
	ELSE
		SET @FTL_Join = N''

	IF (@OrderBy IS NULL OR @OrderBy = N'')
		IF @FTL_Join = N''
			BEGIN
				SET @OrderBy = N'VM.Ordine'
				SET @OrderDirection = N'DESC'
			END
		ELSE
			BEGIN
				SET @OrderBy = N'FTL.RANK'
				SET @OrderDirection = N'DESC'
			END

	IF (@OrderDirection IS NULL OR @OrderDirection = N'')
		SET @OrderDirection = 'ASC'
	
	SET @OrderByStatement = @OrderBy + N' ' + @OrderDirection;
	
    -- Insert statements for procedure here
	SET @Query_Base = N'FROM dbo.TBL_UI_VociMenu AS VM INNER JOIN dbo.FTL_PagineStatiche AS FT_PS ON FT_PS.VoceMenuID = VM.VoceMenuID ' + @FTL_JOIN 
		
	SET @Query = N'SELECT * FROM (SELECT ROW_NUMBER() OVER (ORDER BY ' 
	+ @OrderByStatement + N') AS RN, ' + 
	'VM.VoceMenuID, FT_PS.Testo_IT, FT_PS.Testo_EN ' + -- campi
	@Query_Base + N') AS R WHERE RN > @StartRowNum AND RN <= @EndRowNum';

	SET @QueryCount = N'SELECT @TotalRowCount = COUNT(*) ' + @Query_Base;

	IF (@FTL_Join <> '')
	  BEGIN
		 EXECUTE sp_executesql @Query
		 , N'@TestoRicerca nvarchar(128), 
		 @StartRowNum int, @EndRowNum int'
		 , @TestoRicerca = @TestoRicerca
		 , @StartRowNum = @StartRowNum
		 , @EndRowNum = @EndRowNum;

		 EXECUTE sp_executesql @QueryCount
		 , N'@TestoRicerca nvarchar(128), 
		 @TotalRowCount int OUTPUT'
		 , @TestoRicerca = @TestoRicerca
		 , @TotalRowCount = @TotalRowCount OUTPUT;
	  END
	ELSE
	  BEGIN
		 EXECUTE sp_executesql @Query
		 , N'@StartRowNum int, @EndRowNum int'
		 , @StartRowNum = @StartRowNum
		 , @EndRowNum = @EndRowNum;

		 EXECUTE sp_executesql @QueryCount
		 , N'@TotalRowCount int OUTPUT'
		 , @TotalRowCount = @TotalRowCount OUTPUT;
	  END
	  
	  SELECT @TotalRowCount;
END

GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaProvvedimentiHome]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaProvvedimentiHome]

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Valutazione Impatto Ambientale (VIA)
	SELECT O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, P.Data AS DataProvvedimento, 
	O.ImmagineLocalizzazione, P.ProvvedimentoID, OPR.TipologiaID AS TipologiaOperaID, O.TipoOggettoID
	FROM dbo.TBL_Oggetti AS O INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN
		dbo.TBL_Provvedimenti AS P ON P.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN
		dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN 
		dbo.TBL_ExtraOggettiProgetto AS EOP ON EOP.OggettoID = O.OggettoID INNER JOIN 
		dbo.TBL_Opere AS OPR ON OPR.OperaID = EOP.OperaID
	WHERE ((OP.ProceduraID IN (3, 4, 24, 25, 26, 29)) AND (VDA.DatoAmministrativoID = 63) AND (DATEADD(day, - 60, GETDATE()) <= P.Data) OR
		  (OP.ProceduraID IN (3, 4, 24, 25, 26, 29)) AND (VDA.DatoAmministrativoID = 627) AND (DATEADD(day, - 60, GETDATE()) <= P.Data) OR
		  (OP.ProceduraID IN (14, 15, 23, 27, 28, 30)) AND (VDA.DatoAmministrativoID = 137) AND (DATEADD(day, - 60, GETDATE()) <= P.Data) OR
		  (OP.ProceduraID IN (14, 15, 23, 27, 28, 30)) AND (VDA.DatoAmministrativoID = 138) AND (DATEADD(day, - 60, GETDATE()) <= P.Data)) AND
		  (O.TipoOggettoID = 1)
    ORDER BY DataProvvedimento DESC

    -- Verifica Assoggettabilità a VIA (VIA)
	SELECT O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, P.Data AS DataProvvedimento, 
	O.ImmagineLocalizzazione, P.ProvvedimentoID, OPR.TipologiaID AS TipologiaOperaID, O.TipoOggettoID
	FROM dbo.TBL_Oggetti AS O INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN
		dbo.TBL_Provvedimenti AS P ON P.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN
		dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN 
		dbo.TBL_ExtraOggettiProgetto AS EOP ON EOP.OggettoID = O.OggettoID INNER JOIN 
		dbo.TBL_Opere AS OPR ON OPR.OperaID = EOP.OperaID
	WHERE ((OP.ProceduraID IN (5, 6, 33, 34, 57)) AND (VDA.DatoAmministrativoID = 483) AND (DATEADD(day, - 60, GETDATE()) <= P.Data) OR
		  (OP.ProceduraID IN (5, 6, 33, 34, 57)) AND (VDA.DatoAmministrativoID = 627) AND (DATEADD(day, - 60, GETDATE()) <= P.Data)) AND
		  (O.TipoOggettoID = 1)
    ORDER BY DataProvvedimento DESC

    -- Valutazione Ambientale Strategica (VAS)
	SELECT O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, P.Data AS DataProvvedimento, 
	O.ImmagineLocalizzazione, P.ProvvedimentoID, 0 AS TipologiaOperaID, O.TipoOggettoID
	FROM dbo.TBL_Oggetti AS O INNER JOIN 
		dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN
		dbo.TBL_Provvedimenti AS P ON P.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN
		dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID
	WHERE ((OP.ProceduraID = 102) AND (VDA.DatoAmministrativoID = 1028) AND (DATEADD(day, - 60, GETDATE()) <= P.Data) OR
		  (OP.ProceduraID = 102) AND (VDA.DatoAmministrativoID = 627) AND (DATEADD(day, - 60, GETDATE()) <= P.Data)) AND
		  (O.TipoOggettoID IN (2, 3))
    ORDER BY DataProvvedimento DESC
    
    
    --  (AIA)
	--SELECT O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, P.Data AS DataProvvedimento, 
	--O.ImmagineLocalizzazione, P.ProvvedimentoID, 0 AS TipologiaOperaID, O.TipoOggettoID
	--FROM dbo.TBL_Oggetti AS O INNER JOIN 
	--	dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID INNER JOIN
	--	dbo.TBL_Provvedimenti AS P ON P.OggettoProceduraID = OP.OggettoProceduraID INNER JOIN
	--	dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID
	--WHERE
	--	(
	--		(OP.ProceduraID in (201,202,203,204,205)) 
	--	AND 
	--		(VDA.DatoAmministrativoID = 2010) AND (DATEADD(day, - 45, GETDATE()) <= P.Data) 
	--	AND
	--		(O.TipoOggettoID = 4)
	--	)	
 --   ORDER BY DataProvvedimento DESC
 
 	SELECT  O.OggettoID, O.Nome_IT, O.Nome_EN, O.Descrizione_IT, O.Descrizione_EN, P.Data AS DataProvvedimento, 
			O.ImmagineLocalizzazione, P.ProvvedimentoID, 0 AS TipologiaOperaID, O.TipoOggettoID
	FROM    
			dbo.TBL_Oggetti AS O 
	INNER JOIN 
			dbo.TBL_TipiOggetto T ON T.TipoOggettoID = O.TipoOggettoID                					
	INNER JOIN
			dbo.TBL_OggettiProcedure AS OP ON OP.OggettoID = O.OggettoID 
	INNER JOIN
			dbo.TBL_ValoriDatiAmministrativi AS VDA ON VDA.OggettoProceduraID = OP.OggettoProceduraID 
    LEFT OUTER JOIN
			dbo.TBL_Provvedimenti AS P ON P.OggettoProceduraID = OP.OggettoProceduraID                      	
	WHERE
		(
			(OP.ProceduraID in (201,202,203,204,205)) 
		AND 
			(VDA.DatoAmministrativoID = 2010) 
		AND 
			(DATEADD(day, - 45, GETDATE()) <= VDA.ValoreData) 
		AND
			(O.TipoOggettoID = 4)
		AND
			(T.MacroTipoOggettoID <> 3 OR OP.AIAID IS NOT NULL)		
		)	
    ORDER BY DataProvvedimento DESC 
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaRaggruppamentiDocumentazione]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaRaggruppamentiDocumentazione]
	@OggettoProceduraID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	WITH Raggruppamenti (RaggruppamentoID, GenitoreID, Nome_IT, NOME_EN, Ordine, Figli)
	AS
	(
		-- Ancora
		SELECT R.RaggruppamentoID, R.GenitoreID, R.Nome_IT, R.Nome_EN, R.Ordine, 
				dbo.FN_FigliRaggruppamento(R.RaggruppamentoID) AS Figli
		FROM dbo.TBL_Raggruppamenti AS R INNER JOIN
			dbo.TBL_Documenti AS D ON D.RaggruppamentoID = R.RaggruppamentoID INNER JOIN 
			dbo.TBL_OggettiProcedure AS OP ON OP.OggettoProceduraID = D.OggettoProceduraID 
		WHERE OP.OggettoProceduraID = @OggettoProceduraID AND D.LivelloVisibilita = 1 
		UNION ALL
		-- Select ricorsiva
		SELECT R.RaggruppamentoID, R.GenitoreID, R.Nome_IT, R.Nome_EN, R.Ordine, 
				dbo.FN_FigliRaggruppamento(R.RaggruppamentoID) AS Figli
		FROM dbo.TBL_Raggruppamenti AS R
		INNER JOIN Raggruppamenti AS RT
			ON R.RaggruppamentoID = RT.GenitoreID
	)
	-- Select risultati raggruppamenti
	SELECT DISTINCT RaggruppamentoID, GenitoreID, Nome_IT, NOME_EN, Ordine, Figli 
	FROM Raggruppamenti
	ORDER BY Ordine ASC
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaRaggruppamentiDocumentazioneEvento]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaRaggruppamentiDocumentazioneEvento]
	@EventoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	;WITH Raggruppamenti 
	AS
	(
		SELECT R.RaggruppamentoID, R.GenitoreID, R.Nome_IT, R.Nome_EN, R.Ordine
			,0 AS Livello, CAST(R.RaggruppamentoID AS varchar(50)) AS Percorso,	CAST(R.Nome_IT AS varchar(500)) as Percorso2
		FROM GEMMA_AIAtblRaggruppamenti R
		WHERE R.RaggruppamentoID IN (
			SELECT COALESCE(E.RaggruppamentoID, TE.RaggruppamentoID)
			FROM GEMMA_AIAtblEventi E INNER JOIN GEMMA_AIAtblTipiEvento TE ON TE.TipoEventoID = E.TipoEventoID
			WHERE E.EventoID = @EventoID
		)
		UNION ALL
		SELECT R1.RaggruppamentoID, R1.GenitoreID, R1.Nome_IT, R1.Nome_EN, R1.Ordine
			,Livello + 1, CAST(Percorso + '-' + CAST(R1.RaggruppamentoID AS varchar(50)) AS varchar(50))
			,CAST(Percorso2 + ' - ' + CAST(R1.Nome_IT AS varchar(500)) AS varchar(500))
		FROM GEMMA_AIAtblRaggruppamenti R1
		INNER JOIN Raggruppamenti R2 ON R2.RaggruppamentoID = R1.GenitoreID
	)
	
	SELECT DISTINCT RaggruppamentoID, GenitoreID, Nome_IT, NOME_EN, Ordine, 
		(SELECT COUNT(*) FROM GEMMA_AIAtblRaggruppamenti WHERE GenitoreID = R.RaggruppamentoID) as Figli
	FROM GEMMA_AIAtblRaggruppamenti R
	WHERE R.RaggruppamentoID IN ( SELECT GenitoreID FROM Raggruppamenti )
	UNION
	SELECT DISTINCT RaggruppamentoID, GenitoreID, Nome_IT, NOME_EN, Ordine, 
		(SELECT COUNT(*) FROM GEMMA_AIAtblRaggruppamenti WHERE GenitoreID = R.RaggruppamentoID) as Figli
	FROM Raggruppamenti R
	WHERE R.RaggruppamentoID IN (SELECT RaggruppamentoID FROM GEMMA_AIAtblDocumenti WHERE EventoID = @EventoID AND LivelloVisibilita = 1)
	ORDER BY Ordine ASC
	
	
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaReportGrafici]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaReportGrafici]
 @TipoReport int
 AS
 BEGIN
 	SET NOCOUNT ON;

	/****** RECUPERA TIPOLOGIA ******/

	IF (@TipoReport = 1)
	 SELECT Nome_IT, TipologiaID, ISNULL([1],0) AS [1], ISNULL([2],0) AS [2], ISNULL([9],0) AS [9]
                                    FROM
                                    (SELECT TBL_Tipologie.TipologiaID, TBL_Tipologie.Nome_IT,-- TBL_MacroTipologie.Nome_IT,
                                    TBL_Provvedimenti.TipoProvvedimentoID, COUNT(*) AS TOT FROM TBL_Tipologie 
                                    INNER JOIN TBL_Opere ON TBL_Tipologie.TipologiaID = TBL_Opere.TipologiaID 
                                    INNER JOIN TBL_ExtraOggettiProgetto ON TBL_Opere.OperaID = TBL_ExtraOggettiProgetto.OperaID 
                                    INNER JOIN TBL_Provvedimenti 
                                    INNER JOIN TBL_OggettiProcedure ON TBL_Provvedimenti.OggettoProceduraID = TBL_OggettiProcedure.OggettoProceduraID 
                                    INNER JOIN TBL_Oggetti ON TBL_OggettiProcedure.OggettoID = TBL_Oggetti.OggettoID ON TBL_ExtraOggettiProgetto.OggettoID = TBL_Oggetti.OggettoID 
                                    INNER JOIN TBL_MacroTipologie ON TBL_Tipologie.MacroTipologiaID = TBL_MacroTipologie.MacrotipologiaID 
                                    INNER JOIN TBL_TipiProvvedimenti ON TBL_Provvedimenti.TipoProvvedimentoID = TBL_TipiProvvedimenti.TipoProvvedimentoID 
                                    WHERE (TBL_Provvedimenti.TipoProvvedimentoID IN (1, 2, 9)) 
                                    GROUP BY TBL_Tipologie.TipologiaID, TBL_Tipologie.Nome_IT, TBL_MacroTipologie.Nome_IT, 
                                    TBL_Provvedimenti.TipoProvvedimentoID) AS DATI
                                    PIVOT
                                    (
                                    SUM(TOT) FOR TipoProvvedimentoID IN ([1], [2], [9])
                                    )
                                    AS PivotRisultato ORDER BY Nome_IT

/****** RECUPERA TIPO PROVVEDIMENTO ******/

    ELSE IF (@TipoReport =2)
	  SELECT Anno, [1], [2], [9] FROM (SELECT YEAR(Data) AS Anno, TipoProvvedimentoID,ProvvedimentoID FROM  TBL_Provvedimenti
       WHERE TipoProvvedimentoID IN (1, 2, 9) ) AS Dati
       PIVOT
       (
      COUNT (ProvvedimentoID) FOR TipoProvvedimentoID IN ([1], [2], [9])) AS PivotRisultato ORDER BY Anno

/****** RECUPERA ESITO VIA ******/

	ELSE IF (@TipoReport=3)
	  SELECT Esito, Percentuale FROM
      (
       SELECT (Count(P.ProvvedimentoID)* 100 / (Select Count(*) From TBL_Provvedimenti WHERE 
       TipoProvvedimentoID IN (1,2))) Percentuale, P.Esito FROM TBL_Provvedimenti AS P
       WHERE TipoProvvedimentoID IN (1,2) GROUP BY P.Esito) T WHERE T.Percentuale > 0

/****** RECUPERA ESITO VIA LO ******/
	
	ELSE IF (@TipoReport=4)
	  SELECT Esito, Percentuale FROM
       (
        SELECT (Count(P.ProvvedimentoID)* 100 / (Select Count(*) From TBL_Provvedimenti WHERE 
        TipoProvvedimentoID IN (9))) Percentuale, P.Esito FROM TBL_Provvedimenti AS P
        WHERE TipoProvvedimentoID IN (9) GROUP BY P.Esito) T WHERE T.Percentuale > 0
  END

GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaReportProcedure]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

CREATE PROCEDURE [dbo].[SP_RecuperaReportProcedure]
	-- Add the parameters for the stored procedure here
	@Anno int, 
	@ProceduraID int, 
	@MostraInCorso bit, 
	@MostraAvviate bit, 
	@MostraConcluse bit
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT DISTINCT 
	--	O.TipoOggettoID, 
	--	OP.OggettoProceduraID, 
	--	--OP.ProceduraID, 
	--	--OP.OggettoID, 
	--	OP.ViperaID, 
	--	O.Nome_IT, 
	--	O.Nome_EN, 
	--	E.Nome AS Proponente, 
	--	OP.DataInserimento, 
	--	ISNULL(SPV.ProSDeId, 0) AS StatoProceduraViperaID, 
	--	P.Data AS DataProvvedimento, P.NumeroProtocollo, P.Esito, 
	--	-- VAS
	--	EOPP.SettoreID, 
	--	-- VIA
	--	OPERE.TipologiaID 
	--FROM dbo.TBL_OggettiProcedure AS OP
	--	INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID
	--	LEFT OUTER JOIN dbo.STG_OggettiProcedureEntita AS OPE ON OPE.OggettoProceduraID = OP.OggettoProceduraID AND OPE.RuoloEntitaID = 1
	--	INNER JOIN dbo.TBL_Entita AS E ON E.EntitaID = OPE.EntitaID
	--	LEFT OUTER JOIN VIPERA.dbo.vipProgetti AS V ON V.ProId = OP.ViperaID
	--	LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON V.ProSDeId = SPV.ProSDeId
	--	LEFT OUTER JOIN 
	--	(SELECT P1. OggettoProceduraID, P1.DataProvvedimento AS Data, P2.NumeroProtocollo, P2.Esito FROM 
	--		(SELECT OggettoProceduraID, MIN(Data) AS DataProvvedimento FROM dbo.TBL_Provvedimenti WHERE YEAR(Data) = @Anno GROUP BY OggettoProceduraID) AS P1
	--			LEFT OUTER JOIN dbo.TBL_Provvedimenti AS P2 ON P1.OggettoProceduraID = P2.OggettoProceduraID AND P1.DataProvvedimento = P2.Data
	--	) AS P ON P.OggettoProceduraID = OP.OggettoProceduraID
	--	-- VAS
	--	LEFT OUTER JOIN dbo.TBL_ExtraOggettiPianoProgramma AS EOPP ON EOPP.OggettoID = O.OggettoID
	--	-- VIA
	--	LEFT OUTER JOIN dbo.TBL_ExtraOggettiProgetto AS EOP ON EOP.OggettoID = O.OggettoID
	--	LEFT OUTER JOIN dbo.TBL_Opere AS OPERE ON OPERE.OperaID = EOP.OperaID
	SELECT DISTINCT 
		O.TipoOggettoID, 
		OP.OggettoProceduraID, 
		--OP.ProceduraID, 
		--OP.OggettoID, 
		--Case when Len(OP.ViperaID) > 0 Then OP.ViperaID Else OP.AiaID end as VIperaAiaID, 		
		--Coalesce(OP.ViperaID,OP.AiaID) as VIperaAiaID, 
		CONVERT(VARCHAR(12),
			CASE
			WHEN IsNumeric(OP.ViperaID) = 1 THEN CONVERT(VARCHAR(12),OP.ViperaID)
			ELSE CONVERT(VARCHAR(12), OP.AiaID) END) as VIperaAiaID,
			
		--OP.ViperaID,
		--OP.AiaID,
		O.Nome_IT, 
		O.Nome_EN, 
		E.Nome AS [Proponente/gestore], 
		OP.DataInserimento, 
		Coalesce(SPV.ProSDeId,SPA.StatoAiaID,0) AS StatoProceduraViperaAiaID, 
		--ISNULL(SPV.ProSDeId, 0) AS StatoProceduraViperaID, 
		--ISNULL(SPA.StatoAiaID, 0) AS StatoProceduraAiaID, 
		P.Data AS DataProvvedimento, P.NumeroProtocollo, P.Esito, 
		-- VAS
		EOPP.SettoreID, 
		-- VIA
		OPERE.TipologiaID,
		-- AIA
		EOI.CategoriaImpiantoID
	FROM dbo.TBL_OggettiProcedure AS OP
		INNER JOIN dbo.TBL_Oggetti AS O ON O.OggettoID = OP.OggettoID
		LEFT OUTER JOIN dbo.STG_OggettiProcedureEntita AS OPE ON OPE.OggettoProceduraID = OP.OggettoProceduraID AND OPE.RuoloEntitaID in (1,10) 
		INNER JOIN dbo.TBL_Entita AS E ON E.EntitaID = OPE.EntitaID
		LEFT OUTER JOIN VIPERA.dbo.vipProgetti AS V ON V.ProId = OP.ViperaID
		LEFT OUTER JOIN dbo.TBL_StatiProceduraVIPERA AS SPV ON V.ProSDeId = SPV.ProSDeId
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiProceduraAia AS EPA ON EPA.OggettoProceduraID = OP.OggettoProceduraID 
        LEFT OUTER JOIN dbo.TBL_StatiProceduraAIA AS SPA ON SPA.StatoAiaID = EPA.StatoAiaID 
		LEFT OUTER JOIN 
		(SELECT P1. OggettoProceduraID, P1.DataProvvedimento AS Data, P2.NumeroProtocollo, P2.Esito FROM 
			(SELECT OggettoProceduraID, MIN(Data) AS DataProvvedimento FROM dbo.TBL_Provvedimenti WHERE YEAR(Data) = @Anno GROUP BY OggettoProceduraID) AS P1
				LEFT OUTER JOIN dbo.TBL_Provvedimenti AS P2 ON P1.OggettoProceduraID = P2.OggettoProceduraID AND P1.DataProvvedimento = P2.Data
		) AS P ON P.OggettoProceduraID = OP.OggettoProceduraID
		-- VAS
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiPianoProgramma AS EOPP ON EOPP.OggettoID = O.OggettoID
		-- VIA
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiProgetto AS EOP ON EOP.OggettoID = O.OggettoID
		LEFT OUTER JOIN dbo.TBL_Opere AS OPERE ON OPERE.OperaID = EOP.OperaID
		-- AIA
		LEFT OUTER JOIN dbo.TBL_ExtraOggettiImpianto AS EOI ON EOI.OggettoID = O.OggettoID 
        LEFT OUTER JOIN dbo.TBL_CategorieImpianti AS CI ON Ci.CategoriaImpiantoID = EOI.CategoriaImpiantoID
	WHERE 
		--(P.TipoProvvedimentoID IN (1,2,9) OR P.TipoProvvedimentoID IS NULL)
		--AND 
		OP.ProceduraID = @ProceduraID 
		--AND ((@MostraConcluse = 1 AND YEAR(P.Data) = @Anno) OR @MostraConcluse = 0)
		AND (P.Data IS NULL OR YEAR(P.Data) = @Anno)
		AND 
		(
			(
			-- AVVIATE ?
				@MostraAvviate = 1
				AND -- CLAUSOLA IN
				OP.OggettoProceduraID IN 
				(SELECT OggettoProceduraID FROM dbo.FN_TBL_ProcedureAvviate(@Anno))
			)
			OR
			(
			-- IN CORSO?
				@MostraInCorso = 1
				AND -- CLAUSOLA IN
				OP.OggettoProceduraID IN 
				(SELECT OggettoProceduraID FROM dbo.FN_TBL_ProcedureInCorso(@Anno))
			)
			OR
			(
			-- CONCLUSE?
				@MostraConcluse = 1
				--AND YEAR(P.Data) = @Anno
				AND -- CLAUSOLA IN
				OP.OggettoProceduraID IN 
				(SELECT OggettoProceduraID FROM dbo.FN_TBL_ProcedureConcluse(@Anno))
			)
		)
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaStatisticheProcedure]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaStatisticheProcedure]
	-- Add the parameters for the stored procedure here 
	@anno int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT P.ProceduraID, 
		ISNULL(InCorso.Conteggio, 0) AS InCorso, 
		ISNULL(Avviate.Conteggio, 0) AS Avviate, 
		ISNULL(Concluse.Conteggio, 0) AS Concluse 
	FROM dbo.TBL_Procedure AS P
	INNER JOIN dbo.TBL_AmbitiProcedure AS AP ON AP.AmbitoProceduraID = P.AmbitoProceduraID
	LEFT OUTER JOIN
	(
		-- IN CORSO
		SELECT F.ProceduraID, COUNT(F.OggettoProceduraID) AS Conteggio
		FROM dbo.FN_TBL_ProcedureInCorso(@anno) F GROUP BY F.ProceduraID
	) AS InCorso ON P.ProceduraID = InCorso.ProceduraID
	LEFT OUTER JOIN 
	(
		-- AVVIATE
		SELECT F.ProceduraID, COUNT(F.OggettoProceduraID) AS Conteggio
		FROM dbo.FN_TBL_ProcedureAvviate(@Anno) F GROUP BY F.ProceduraID
	) AS Avviate ON Avviate.ProceduraID = P.ProceduraID
	LEFT OUTER JOIN 
	(
		-- CONCLUSE
		SELECT F.ProceduraID, COUNT(F.OggettoProceduraID) AS Conteggio
		FROM dbo.FN_TBL_ProcedureConcluse(@Anno) F GROUP BY F.ProceduraID
	) AS Concluse ON Concluse.ProceduraID = P.ProceduraID
	WHERE P.ProceduraID NOT IN (1,11,23)
	ORDER BY AP.Ordine, P.Ordine
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaStatoProcedure]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaStatoProcedure]
	-- Add the parameters for the stored procedure here
	@Concluse bit, 
	@MacroTipoOggettoID int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @sqlQ nvarchar(MAX)
	DECLARE @statoProcedure TABLE
	(
	  Conteggio int,
	  ProceduraID int, 
	  Parametro int
	)
	
		IF (@MacroTipoOggettoID = 1)
		BEGIN
	
			-- Ripetere il blocco per creare l'elenco delle procedure
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (11 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ

			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (1 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ

			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (9 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
				
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA(2 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ

			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA(7 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ

			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (10 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ

			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (3 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
	
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (4 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (5 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVIA (6 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
			
		END
	IF (@MacroTipoOggettoID = 2)
		BEGIN
			-- Ripetere il blocco per creare l'elenco delle procedure
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVAS (8 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
					
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureVAS (7 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
		END
		
	IF (@MacroTipoOggettoID = 3)
		BEGIN
			-- Ripetere il blocco per creare l'elenco delle procedure
			 /* PROCEDURE:
				201		AIA per nuova installazione
				202		Prima AIA per installazione esistente
				203		Rinnovo AIA
				204		Riesame AIA
				205		AIA per modifica sostanziale
				206		Aggiornamento AIA per modifica non sostanziale
				207		Verifica adempimenti prescrizioni
			*/
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA (201 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA (202 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ		
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA (203 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ		
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA (204 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ		
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA (205 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ		
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA (206 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ		
			
			SET @sqlQ = dbo.FN_CreaQueryStatoProcedureAIA (207 ,1, @concluse)
			INSERT INTO @statoProcedure (Conteggio, ProceduraID, Parametro)
			EXEC sp_executesql @sqlQ		
		
		END
		
	-- ALLA FINE SELEZIONO DALLA VARIABILE TABELLA
	
	SELECT * FROM @statoProcedure
END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaTerritoriVas_R_Territorio]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaTerritoriVas_R_Territorio] 
	-- Add the parameters for the stored procedure here
	@testo nvarchar(50),
	@criterio int,
	@tipologia int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @stringaRicerca nvarchar(256)

	SET @stringaRicerca = dbo.FN_ApplicaCriterio(@testo, @criterio);
 
	DECLARE @risultati table (
	TerritorioID uniqueidentifier NOT NULL,
	TipologiaTerritorioID int NOT NULL, 
	GenitoreID uniqueidentifier NULL,
	Nome varchar(70) NULL, 
	CodiceIstat varchar(10) NULL
	);

	-- Prima ricorsione per prendere tutti i figli dei genitori associati
	-- Andiamo in giù
	WITH TerritoriF (TerritorioID, TipologiaTerritorioID, GenitoreID, Nome, CodiceIstat)
	AS
	(
		-- Ancora
		SELECT T.TerritorioID, T.TipologiaTerritorioID, T.GenitoreID, T.Nome, T.CodiceIstat
		FROM dbo.TBL_Territori AS T INNER JOIN
			dbo.STG_OggettiTerritori AS S ON T.TerritorioID = S.TerritorioID INNER JOIN
			dbo.TBL_Oggetti AS O ON S.OggettoID = O.OggettoID
		WHERE O.TipoOggettoID = 2 OR O.TipoOggettoID = 3 
		UNION ALL
		-- Select ricorsiva
		SELECT T.TerritorioID, T.TipologiaTerritorioID, T.GenitoreID, T.Nome, T.CodiceIstat
		FROM dbo.TBL_Territori AS T
		INNER JOIN TerritoriF AS RT
			ON T.GenitoreID = RT.TerritorioID
	)
	INSERT INTO @risultati 
	SELECT DISTINCT TerritorioID, TipologiaTerritorioID, GenitoreID, Nome, CodiceIstat
	FROM TerritoriF
	ORDER BY Nome ASC;

	WITH Territori (TerritorioID, TipologiaTerritorioID, GenitoreID, Nome, CodiceIstat, Selezionato)
	AS
	(
		-- Ancora
		SELECT T.TerritorioID, T.TipologiaTerritorioID, T.GenitoreID, T.Nome, T.CodiceIstat, 1 AS Selezionato
		FROM @risultati AS T 
		WHERE (T.Nome LIKE @stringaRicerca) AND 
			((T.TipologiaTerritorioID = @tipologia) OR (@tipologia IS NULL))
		UNION ALL
		-- Select ricorsiva
		SELECT T.TerritorioID, T.TipologiaTerritorioID, T.GenitoreID, T.Nome, T.CodiceIstat, 0 AS Selezionato
		FROM dbo.TBL_Territori AS T
		INNER JOIN Territori AS RT
			ON T.TerritorioID = RT.GenitoreID
	)
	-- Select risultati territori
	SELECT DISTINCT TerritorioID, TipologiaTerritorioID, GenitoreID, Nome, CodiceIstat, Selezionato
	FROM Territori
	ORDER BY Nome ASC


END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaTerritoriVia_R_Territorio]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaTerritoriVia_R_Territorio] 
	-- Add the parameters for the stored procedure here
	@testo nvarchar(50),
	@criterio int,
	@tipologia int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @stringaRicerca nvarchar(256)

	SET @stringaRicerca = dbo.FN_ApplicaCriterio(@testo, @criterio);

	WITH Territori (TerritorioID, TipologiaTerritorioID, GenitoreID, Nome, CodiceIstat, Selezionato)
	AS
	(
		-- Ancora
		SELECT T.TerritorioID, T.TipologiaTerritorioID, T.GenitoreID, T.Nome, T.CodiceIstat, 1 AS Selezionato
		FROM dbo.TBL_Territori AS T INNER JOIN
			dbo.STG_OggettiTerritori AS S ON T.TerritorioID = S.TerritorioID INNER JOIN
			dbo.TBL_Oggetti AS O ON S.OggettoID = O.OggettoID
		WHERE (T.Nome LIKE @stringaRicerca) AND 
			((T.TipologiaTerritorioID = @tipologia) OR (@tipologia IS NULL)) AND
			(O.TipoOggettoID = 1)
		UNION ALL
		-- Select ricorsiva
		SELECT T.TerritorioID, T.TipologiaTerritorioID, T.GenitoreID, T.Nome, T.CodiceIstat, 0 AS Selezionato
		FROM dbo.TBL_Territori AS T
		INNER JOIN Territori AS RT
			ON T.TerritorioID = RT.GenitoreID
	)
	-- Select risultati territori
	SELECT TerritorioID, TipologiaTerritorioID, GenitoreID, Nome, CodiceIstat, SUM(Selezionato)
	FROM Territori
	GROUP BY TerritorioID, TipologiaTerritorioID, GenitoreID, Nome, CodiceIstat 
	ORDER BY Nome ASC

END
GO
/****** Object:  StoredProcedure [dbo].[SP_RecuperaTerritoriVia_R_Tipologia]    Script Date: 16/11/2020 10:00:43 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO
CREATE PROCEDURE [dbo].[SP_RecuperaTerritoriVia_R_Tipologia] 
	-- Add the parameters for the stored procedure here
	@testo nvarchar(50),
	@criterio int,
	@tipologia int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	DECLARE @risultati table (
		[ID] [uniqueidentifier] NOT NULL,
		[GenitoreID] [uniqueidentifier] NULL,
		[Tipologia] [nvarchar](254) NULL,
		[Nome] [nvarchar](254) NULL,
		[Selezionato] [bit] NOT NULL, 
		[Ordine] [int] NULL
	);

	DECLARE @idRadice uniqueidentifier
	SET @idRadice = newid();
	DECLARE @idRegioni uniqueidentifier
	SET @idRegioni = newid();
	DECLARE @idProvince uniqueidentifier
	SET @idProvince = newid();
	DECLARE @idComuni uniqueidentifier
	SET @idComuni = newid();
	DECLARE @idMari uniqueidentifier
	SET @idMari = newid();
	

	--inserisco il nodo radice
	INSERT INTO @risultati (ID, GenitoreID, Tipologia, Nome, Selezionato, Ordine) VALUES 
					  (@idRadice, NULL, 'Tipologia', 'Italia', 0, 0)
	--inserisco le tipologie
	INSERT INTO @risultati (ID, GenitoreID, Tipologia, Nome, Selezionato, Ordine) VALUES
	(@idRegioni, @idRadice, 'Tipologia', 'Regioni', 0, 1)

	INSERT INTO @risultati (ID, GenitoreID, Tipologia, Nome, Selezionato, Ordine) VALUES
	(@idProvince, @idRadice, 'Tipologia', 'Province', 0, 2)

	INSERT INTO @risultati (ID, GenitoreID, Tipologia, Nome, Selezionato, Ordine) VALUES
	(@idComuni, @idRadice, 'Tipologia', 'Comuni', 0, 3)

	INSERT INTO @risultati (ID, GenitoreID, Tipologia, Nome, Selezionato, Ordine) VALUES
	(@idMari, @idRadice, 'Tipologia', 'Mari', 0, 4)

	--inserisco i territori
	INSERT INTO @risultati (ID, GenitoreID, Tipologia, Nome, Selezionato) 
	(SELECT T.TerritorioID, 
	  CASE WHEN TipologiaTerritorioID = 2 THEN @idRegioni ELSE
	    CASE WHEN TipologiaTerritorioID = 3 THEN @idProvince ELSE 
	      CASE WHEN TipologiaTerritorioID = 4 THEN @idComuni ELSE @idMari END
	     END
	  END
	, 'Territorio', T.Nome, 1 FROM 
	dbo.TBL_Territori AS T INNER JOIN dbo.STG_OggettiTerritori AS S
	ON T.TerritorioID = S.TerritorioID 
	WHERE (T.Nome LIKE dbo.FN_ApplicaCriterio(@testo, @criterio)) AND ((T.TipologiaTerritorioID = @tipologia) OR (@tipologia < 0)))

	SELECT DISTINCT * FROM @risultati ORDER BY Ordine ASC, Nome ASC
END
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'DocumentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'Titolo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Codice elaborato documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'CodiceElaborato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in italiano dell''oggetto cui è collegato il documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'NomeOggetto_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in inglese dell''oggetto cui è collegato il documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'NomeOggetto_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in italiano dell''oggetto cui è collegato il documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'DescrizioneOggetto_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in inglese dell''oggetto cui è collegato il documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'DescrizioneOggetto_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concatenazione degli argomenti in italiano del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'Argomenti_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concatenazione degli argomenti in inglese del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'Argomenti_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Autore del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'Autore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Proponente dell''oggetto cui è collegato il documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti', @level2type=N'COLUMN',@level2name=N'Proponente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente i dati per la ricerca in FULL-TEXT sui documenti degli oggetti (TBL_Documenti)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Documenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie', @level2type=N'COLUMN',@level2name=N'NotiziaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo in italiano della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie', @level2type=N'COLUMN',@level2name=N'Titolo_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo in inglese della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie', @level2type=N'COLUMN',@level2name=N'Titolo_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Abstract in italiano della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie', @level2type=N'COLUMN',@level2name=N'Abstract_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Abstract in inglese della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie', @level2type=N'COLUMN',@level2name=N'Abstract_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo in italiano della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie', @level2type=N'COLUMN',@level2name=N'Testo_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo in inglese della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie', @level2type=N'COLUMN',@level2name=N'Testo_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente i dati per la ricerca in FULL-TEXT sulle notizie (TBL_Notizie)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Notizie'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in italiano dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in inglese dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in italiano dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'Descrizione_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in inglese dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'Descrizione_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome dell''opera in italiano cui fa parte l''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'NomeOpera_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome dell''opera in inglese cui fa parte l''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'NomeOpera_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Concatenazione dei Territori collegati all''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'Territori'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Proponente dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti', @level2type=N'COLUMN',@level2name=N'Proponente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente i dati per la ricerca in FULL-TEXT sugli oggetti (TBL_Oggetti)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_Oggetti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK della voce di menu cui è collegata la pagina statica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche', @level2type=N'COLUMN',@level2name=N'VoceMenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in italiano della pagina statica -> il nome della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in inglese della pagina statica -> il nome della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in italiano della pagina statica -> Descrizione della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Descrizione_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in inglese della pagina statica -> Descrizione della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Descrizione_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo in italiano della pagina statica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Testo_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo in inglese della pagina statica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Testo_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente i dati per la ricerca in FULL-TEXT sulle pagine statiche (TBL_UI_PagineStatiche)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'FTL_PagineStatiche'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK categoria impianto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAstgCategoriaImpArgomento', @level2type=N'COLUMN',@level2name=N'CategoriaImpiantoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAstgEventiOggetti', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK oggetto procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAstgEventiOggettoProcedura', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblDocumenti', @level2type=N'COLUMN',@level2name=N'Titolo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome file documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblDocumenti', @level2type=N'COLUMN',@level2name=N'NomeFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Percorso file relativo alla cartella dell''oggetto del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblDocumenti', @level2type=N'COLUMN',@level2name=N'PercorsoFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data pubblicazione del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblDocumenti', @level2type=N'COLUMN',@level2name=N'DataPubblicazione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Livello visibilita documento (0: non visibile, 1: visibile)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblDocumenti', @level2type=N'COLUMN',@level2name=N'LivelloVisibilita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dimensione documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblDocumenti', @level2type=N'COLUMN',@level2name=N'Dimensione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblEventi', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblEventi', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Territori' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblGruppi', @level2type=N'COLUMN',@level2name=N'TerritorioID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblPagamenti', @level2type=N'COLUMN',@level2name=N'PagamentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_OggettiProcedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblPagamenti', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome file documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblPagamenti', @level2type=N'COLUMN',@level2name=N'NomeFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Percorso file relativo alla cartella dell''oggetto del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblPagamenti', @level2type=N'COLUMN',@level2name=N'PercorsoFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblPagamentiStoricoAia', @level2type=N'COLUMN',@level2name=N'PagamentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblRaggruppamenti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblRaggruppamenti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblTipiEvento', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblTipiEvento', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cognome utente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblUtenti', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se l''utente è abilitato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'GEMMA_AIAtblUtenti', @level2type=N'COLUMN',@level2name=N'Abilitato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Documenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_DocumentiArgomenti', @level2type=N'COLUMN',@level2name=N'DocumentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Argomenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_DocumentiArgomenti', @level2type=N'COLUMN',@level2name=N'ArgomentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra documenti ed argomenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_DocumentiArgomenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Documenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_DocumentiEntita', @level2type=N'COLUMN',@level2name=N'DocumentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Entita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_DocumentiEntita', @level2type=N'COLUMN',@level2name=N'EntitaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_RuoliEntita -> definisce il rapporto tra documento ed entità: autore, responsabile metadato, ecc...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_DocumentiEntita', @level2type=N'COLUMN',@level2name=N'RuoloEntitaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra documenti ed entità' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_DocumentiEntita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_Oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiImpiantiAttivitaIppc', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_AttivitaIppc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiImpiantiAttivitaIppc', @level2type=N'COLUMN',@level2name=N'AttivitaIppcID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiLink', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiLink', @level2type=N'COLUMN',@level2name=N'LinkID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipiLink -> definisce la natura del link: Sito Web di interesse, Proggetto cartografico, ecc...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiLink', @level2type=N'COLUMN',@level2name=N'TipoLinkID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra gli oggetti ed i link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiLink'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_OggettiProcedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureAttributi', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Attributi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureAttributi', @level2type=N'COLUMN',@level2name=N'AttributoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se il collegamento deve apparire nel widget delle consultazioni transfrontaliere' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureAttributi', @level2type=N'COLUMN',@level2name=N'Widget'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra gli oggetti procedure e gli attributi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureAttributi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_OggettiProcedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureEntita', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Entita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureEntita', @level2type=N'COLUMN',@level2name=N'EntitaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_RuoliEntita -> definisce il rapporto tra oggetto procedura ed entità: proponente, Autorità procedente, ecc...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureEntita', @level2type=N'COLUMN',@level2name=N'RuoloEntitaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra gli oggetti procedure ed entità' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiProcedureEntita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiTerritori', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Territori' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiTerritori', @level2type=N'COLUMN',@level2name=N'TerritorioID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra documenti ed argomenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_OggettiTerritori'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Provvedimenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_ProvvedimentiDocumenti', @level2type=N'COLUMN',@level2name=N'ProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Documenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_ProvvedimentiDocumenti', @level2type=N'COLUMN',@level2name=N'DocumentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra provvedimenti e documenti, indica quali documenti sono parte del provvedimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_ProvvedimentiDocumenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_UI_VociMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuTipiAttributi', @level2type=N'COLUMN',@level2name=N'VoceMenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipiAttributi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuTipiAttributi', @level2type=N'COLUMN',@level2name=N'TipoAttributoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra voci menu e tipi attributi, utilizzata per collegare direttamente le voci menu alle consultazioni transfrontaliere e alle proceudre integrate' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuTipiAttributi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_UI_VociMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuTipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'VoceMenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipiProvvedimenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuTipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'TipoProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra voci menu e tipi provvedimenti, utilizzata per collegare direttamente le voci menu ai tipi provvedimenti per le pagine di elenco' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuTipiProvvedimenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_UI_VociMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuWidget', @level2type=N'COLUMN',@level2name=N'VoceMenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_UI_Widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuWidget', @level2type=N'COLUMN',@level2name=N'WidgetID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento del widget nella relativa voce menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuWidget', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra voci menu e widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UI_VociMenuWidget'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Utenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UtentiRuoliUtente', @level2type=N'COLUMN',@level2name=N'UtenteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_RuoliUtente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UtentiRuoliUtente', @level2type=N'COLUMN',@level2name=N'RuoloUtenteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di collegamento tra utenti e ruoli' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'STG_UtentiRuoliUtente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK ambito procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AmbitiProcedure', @level2type=N'COLUMN',@level2name=N'AmbitoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano dell''ambito procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AmbitiProcedure', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese dell''ambito procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AmbitiProcedure', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AmbitiProcedure', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente gli ambiti delle procedure (Valutazione Ambientale Strategica, Valutazione Impatto Ambientale, Valutazione Impatto ambientale (Legge obiettivo)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AmbitiProcedure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK area' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AreeTipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'AreaTipoProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano dell''area' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AreeTipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese dell''area' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AreeTipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AreeTipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente gli le aree dei diversi tipi di provvedimento (Valutazione Ambientale Strategica, Valutazione Impatto Ambientale, Valutazione Impatto ambientale (Legge obiettivo)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AreeTipiProvvedimenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK argomento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Argomenti', @level2type=N'COLUMN',@level2name=N'ArgomentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificativo del genitore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Argomenti', @level2type=N'COLUMN',@level2name=N'GenitoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano dell''argomento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Argomenti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese dell''argomento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Argomenti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente argomenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Argomenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK AttivitaIppcID' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AttivitaIppc', @level2type=N'COLUMN',@level2name=N'AttivitaIppcID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Codifica delle attivita ippc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AttivitaIppc', @level2type=N'COLUMN',@level2name=N'Codice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Categoria di appartenenza' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AttivitaIppc', @level2type=N'COLUMN',@level2name=N'Categoria'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Livello gerarchico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AttivitaIppc', @level2type=N'COLUMN',@level2name=N'Livello'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano attivita ippc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AttivitaIppc', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese attivita ippc' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_AttivitaIppc', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Attributi', @level2type=N'COLUMN',@level2name=N'AttributoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipiAttributi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Attributi', @level2type=N'COLUMN',@level2name=N'TipoAttributoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano dell''attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Attributi', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese dell''attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Attributi', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Attributi', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Macro tipo oggetto cui l''attributo si riferisce' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Attributi', @level2type=N'COLUMN',@level2name=N'MacroTipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente gli attributi che possono essere assegnati agli oggetti procedure. Sono utilizzati per le procedure integrate e coordinate e le consultazioni transfrontaliere.' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Attributi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK categoria impianto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieImpianti', @level2type=N'COLUMN',@level2name=N'CategoriaImpiantoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano categoria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieImpianti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese categoria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieImpianti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in italiano categoria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieImpianti', @level2type=N'COLUMN',@level2name=N'Descrizione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome del file per l''icona della categoria (utilizzata in Home)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieImpianti', @level2type=N'COLUMN',@level2name=N'FileIcona'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente le categorie degli impianti AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieImpianti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK categoria notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieNotizie', @level2type=N'COLUMN',@level2name=N'CategoriaNotiziaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano della categoria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieNotizie', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese della categoria' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieNotizie', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente le categorie delle notizie' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_CategorieNotizie'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK raggruppamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ClassiAiaStorico', @level2type=N'COLUMN',@level2name=N'RaggruppamentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK dato amministrativo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi', @level2type=N'COLUMN',@level2name=N'DatoAmministrativoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano del dato amministrativo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese del dato amministrativo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Livello visibilita del dato amministrativo - non utilizzato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi', @level2type=N'COLUMN',@level2name=N'LivelloVisibilita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione del tipo di dati base che serve per rappresentare il dato amministrativo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi', @level2type=N'COLUMN',@level2name=N'TipoDati'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipologia del dato amministrativo, le tipologie dovrebbero essere: data chiusura procedura, data scadenza presentazione osservazioni, ecc.. (stato attuale: parzialmente supportato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi', @level2type=N'COLUMN',@level2name=N'TipoDatoAmministrativo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente i dati amministrativi disponibili' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DatiAmministrativi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'DocumentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_OggettiProcedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Raggruppamenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'RaggruppamentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipiFile' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'TipoFileID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Codice elaborato documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'CodiceElaborato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'Titolo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'Descrizione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Scala documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'Scala'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipologia documento (R: testuale, D: grafico)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'Tipologia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dimensione documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'Dimensione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Livello visibilita documento (0: non visibile, 1: visibile)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'LivelloVisibilita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome file documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'NomeFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Percorso file relativo alla cartella dell''oggetto del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'PercorsoFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data pubblicazione del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'DataPubblicazione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data stesura del documento - Consentito NULL solo per AIA Regionali' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'DataStesura'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lingua del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'LinguaDocumento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Lingua del metadato del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'LinguaMetadato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Diritti del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'Diritti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti', @level2type=N'COLUMN',@level2name=N'Ordinamento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente i documenti associati ad oggetti e provvedimenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Documenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiAiaStorico', @level2type=N'COLUMN',@level2name=N'DocumentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'DocumentoPortaleID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipiFile' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'TipoFileID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese del documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'NomeFileOriginale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultima modifica documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'DataUltimaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Dimensione documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale', @level2type=N'COLUMN',@level2name=N'Dimensione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente i documenti del portale, non relativi ad oggetti specifici' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_DocumentiPortale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK email' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Email', @level2type=N'COLUMN',@level2name=N'EmailID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo email' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Email', @level2type=N'COLUMN',@level2name=N'Testo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indirizzo email del mittente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Email', @level2type=N'COLUMN',@level2name=N'IndirizzoEmail'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo (Cittadino o Proponente)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Email', @level2type=N'COLUMN',@level2name=N'Tipo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data invio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Email', @level2type=N'COLUMN',@level2name=N'DataInvio'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente le comunicazioni ricevute tramite i form delle pagine Spazio per il cittadino e Spazio per il proponente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK entita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'EntitaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'CodiceFiscale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indirizzo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Indirizzo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Cap'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Citta' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Citta'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Provincia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Provincia'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Telefono' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Telefono'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Fax' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Fax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Pec' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'Pec'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'SitoWeb' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'SitoWeb'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PuntoContatto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'PuntoContatto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_MacroTipiOggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita', @level2type=N'COLUMN',@level2name=N'MacroTipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente le entità coinvolte negli oggetti e documenti del portale: persone fisiche, aziende, ecc...' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Entita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Documenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraDocumenti', @level2type=N'COLUMN',@level2name=N'DocumentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Riferimenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraDocumenti', @level2type=N'COLUMN',@level2name=N'Riferimenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Origine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraDocumenti', @level2type=N'COLUMN',@level2name=N'Origine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Copertura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraDocumenti', @level2type=N'COLUMN',@level2name=N'Copertura'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella informazioni aggiuntive per i documenti (TBL_Documenti)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraDocumenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_Oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Storico ID Impianto AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'ImpiantoAiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_CategorieImpianti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'CategoriaImpiantoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cap impianto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'CapImpianto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indirizzo impianto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'IndirizzoImpianto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Vaolre storico competenza impianto 0=Regionale, 1=Statale,2=Transfrontaliera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'CompetenzaStataleStorico'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_StatoImpianti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'StatoImpiantiID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Flag Impianto utilizzato per i provvedimenti regionali (0 = non regionale, 1 = regionale)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto', @level2type=N'COLUMN',@level2name=N'Regionale'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di estensione di TBL_Oggetti per quanto riguarda gli impianti AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiImpianto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiPianoProgramma', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Settori' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiPianoProgramma', @level2type=N'COLUMN',@level2name=N'SettoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di estensione di TBL_Oggetti per quanto riguarda i piani e programmi (VAS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiPianoProgramma'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK oggetto procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProceduraAia', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK stato procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProceduraAia', @level2type=N'COLUMN',@level2name=N'StatoAiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID Pratica spiga' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProceduraAia', @level2type=N'COLUMN',@level2name=N'CodFascicolo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Storico id domanda AIA ' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProceduraAia', @level2type=N'COLUMN',@level2name=N'DomandaAiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id procedura collegata AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProceduraAia', @level2type=N'COLUMN',@level2name=N'ProceduraCollegataID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Id provvedimento collegato AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProceduraAia', @level2type=N'COLUMN',@level2name=N'ProvvedimentoCollegatoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProgetto', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Opere' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProgetto', @level2type=N'COLUMN',@level2name=N'OperaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'CUP (Codice Unico di Progetto)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProgetto', @level2type=N'COLUMN',@level2name=N'Cup'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella di estensione di TBL_Oggetti per quanto riguarda i progetti (VIA)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraOggettiProgetto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK provvedimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraProvvedimentiAia', @level2type=N'COLUMN',@level2name=N'ProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Storico id autorizzazione AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraProvvedimentiAia', @level2type=N'COLUMN',@level2name=N'AutorizzazioneAiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data scadenza autorizzazione' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraProvvedimentiAia', @level2type=N'COLUMN',@level2name=N'DataScadenzaAutorizzazione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Livello visibilita provvedimento (0: non visibile, 1: visibile)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ExtraProvvedimentiAia', @level2type=N'COLUMN',@level2name=N'LivelloVisibilita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK fasi progettazione' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FasiProgettazione', @level2type=N'COLUMN',@level2name=N'FaseProgettazioneID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano della fase progettazione' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FasiProgettazione', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese della fase progettazione' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FasiProgettazione', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario delle fasi di progettazione' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FasiProgettazione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK formato immagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine', @level2type=N'COLUMN',@level2name=N'FormatoImmagineID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome del formato immagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Altezza massima consentita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine', @level2type=N'COLUMN',@level2name=N'AltezzaMax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Altezza minima consentita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine', @level2type=N'COLUMN',@level2name=N'AltezzaMin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Larghezza massima consentita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine', @level2type=N'COLUMN',@level2name=N'LarghezzaMax'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Larghezza minima consentita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine', @level2type=N'COLUMN',@level2name=N'LarghezzaMin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Abilitato (si/no)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine', @level2type=N'COLUMN',@level2name=N'Abilitato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei formati immagine che possono essere creati dal back end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_FormatiImmagine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK immagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'ImmagineID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificativo dell''immagine cui è figlia quella corrente (0 = immagine master)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'ImmagineMasterID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_FormatiImmagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'FormatoImmagineID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano dell''immagine (generalmente utilizzato per gli attributi title)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese dell''immagine (generalmente utilizzato per gli attributi title)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento immagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultima modifica immagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'DataUltimaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Altezza immagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'Altezza'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Larghezza immagine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'Larghezza'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome file sorgente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini', @level2type=N'COLUMN',@level2name=N'NomeFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella delle immagini utilizzate nel portale, gestite da back end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Immagini'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Link', @level2type=N'COLUMN',@level2name=N'LinkID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Link', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Link', @level2type=N'COLUMN',@level2name=N'Descrizione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indirizzo link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Link', @level2type=N'COLUMN',@level2name=N'Indirizzo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dei link che vengono associati agli oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK macro tipo oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipiOggetto', @level2type=N'COLUMN',@level2name=N'MacroTipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano del macro tipo oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipiOggetto', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese del macro tipo oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipiOggetto', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome abbreviato del macro tipo oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipiOggetto', @level2type=N'COLUMN',@level2name=N'NomeAbbreviato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente le macrotipologie di oggetti (VIA e VAS)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipiOggetto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK macrotipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipologie', @level2type=N'COLUMN',@level2name=N'MacrotipologiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano della macrotipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipologie', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese della macrotipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipologie', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario contenente le macrotipologie che contengono le tipologie di opere' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_MacroTipologie'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'NotiziaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_CategorieNotizie' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'CategoriaNotiziaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Immagini' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'ImmagineID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Data'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo italiano della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Titolo_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo inglese della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Titolo_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo Breve italiano della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'TitoloBreve_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo Breve inglese della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'TitoloBreve_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Abstract italiano della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Abstract_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Abstract inglese della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Abstract_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo italiano della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Testo_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo inglese della notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Testo_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se la notizia è visibile su front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Pubblicata'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultima modifica notizia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'DataUltimaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Stato della notizia (bozza, in revisione, pronta per la pubblicazione' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie', @level2type=N'COLUMN',@level2name=N'Stato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella delle notizie del portale, inserite da back end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Notizie'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_TipiOggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'TipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in italiano oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'Descrizione_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in inglese oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'Descrizione_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bounding box - Latitudine Nord' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'LatitudineNord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bounding box - Latitudine Sud' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'LatitudineSud'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bounding box - Longitudine Est' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'LongitudineEst'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Bounding box - Longitudine Ovest' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'LongitudineOvest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Immagine Localizzazione oggetto (url per il visulalizzatore oppure nome file pdf)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti', @level2type=N'COLUMN',@level2name=N'ImmagineLocalizzazione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella in cui si trovano gli oggetti (piani, programmi, progetti)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Oggetti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK oggetto procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_Oggetti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'OggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_Procedura - Consentito NULL solo per AIA Regionali' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'ProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_FasiProgettazione' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'FaseProgettazioneID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_Valutatori' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'ValutatoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se è l''ultima procedura per l''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'UltimaProcedura'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inizio della procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Collegamento con VIPERA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'ViperaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Storico IDMATTM  AIA - NULL per le procedure AIA regionali' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure', @level2type=N'COLUMN',@level2name=N'AIAID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella in cui si trovano gli oggetti collegati alle procedure attraverso le quali sono passati' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_OggettiProcedure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere', @level2type=N'COLUMN',@level2name=N'OperaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK TBL_Tipologie' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere', @level2type=N'COLUMN',@level2name=N'TipologiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Alias in italiano opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere', @level2type=N'COLUMN',@level2name=N'Alias_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Alias in inglese opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere', @level2type=N'COLUMN',@level2name=N'Alias_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data creazione opera' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella in cui si trovano le opere di cui fanno parte i progetti VIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Opere'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Procedure', @level2type=N'COLUMN',@level2name=N'ProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Macro Tipo Oggetto cui fa riferimento la procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Procedure', @level2type=N'COLUMN',@level2name=N'MacroTipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Procedure', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Procedure', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_AmbitiProcedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Procedure', @level2type=N'COLUMN',@level2name=N'AmbitoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Procedure', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario delle procedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Procedure'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK provvedimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'ProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipiProvvedimenti - Consentito NULL solo per AIA Regionali' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'TipoProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_OggettiProcedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione dell''entità che rappresenta il proponente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'EntitaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numero protocollo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'NumeroProtocollo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data - Consentito NULL solo per AIA Regionali' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'Data'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Oggetto del provvedimento in italiano' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'Oggetto_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Oggetto del provvedimento in inglese' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'Oggetto_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Esito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti', @level2type=N'COLUMN',@level2name=N'Esito'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella in cui si trovano i provvedimenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Provvedimenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK raggruppamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'RaggruppamentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Raggruppamento genitore del corrente (0 = root)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'GenitoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'MacroTipoOggetto cui è assimilabile il raggruppamento (no FK)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'MacroTipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano raggruppamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese raggruppamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione raggruppamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'Descrizione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Livello visibilità raggruppamento (non utilizzato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'LivelloVisibilita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordine raggruppamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei raggruppamenti in cui può essere inserito un documento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Raggruppamenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK ruolo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliEntita', @level2type=N'COLUMN',@level2name=N'RuoloEntitaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano ruolo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliEntita', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese ruolo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliEntita', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei ruoli che può assumere una entità di TBL_Entita' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliEntita'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK ruolo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliUtente', @level2type=N'COLUMN',@level2name=N'RuoloUtenteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Codice ruolo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliUtente', @level2type=N'COLUMN',@level2name=N'Codice'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome ruolo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliUtente', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei ruoli che può assumere un utente di TBL_Utenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_RuoliUtente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK settore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Settori', @level2type=N'COLUMN',@level2name=N'SettoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano settore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Settori', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese settore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Settori', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei settori dei piani e programmi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Settori'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK stato procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraAIA', @level2type=N'COLUMN',@level2name=N'StatoAiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano stato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraAIA', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese stato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraAIA', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in area riservata' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraAIA', @level2type=N'COLUMN',@level2name=N'NomeAreaRiservata'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraAIA', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente gli stati delle domande/procedure AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraAIA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK stato procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraVIPERA', @level2type=N'COLUMN',@level2name=N'ProSDeId'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano stato procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraVIPERA', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese stato procedura' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraVIPERA', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario e traduzione degli stati procedura che si trovano in VIPERA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatiProceduraVIPERA'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK stato impianti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatoImpianti', @level2type=N'COLUMN',@level2name=N'StatoImpiantiID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano stato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatoImpianti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese stato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatoImpianti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contenente gli stati degli impianti AIA' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_StatoImpianti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK territorio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'TerritorioID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ID del genitore del record' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'GenitoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_TipologieTerritorio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'TipologiaTerritorioID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome territorio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Codice ISTAT territorio' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'CodiceIstat'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Latitudine nord bounding box' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'LatitudineNord'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Latitudine sud bounding box' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'LatitudineSud'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Longitudine est bounding box' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'LongitudineEst'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Longitudine ovest bounding box' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori', @level2type=N'COLUMN',@level2name=N'LongitudineOvest'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dei territori' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Territori'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK tipo attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiAttributi', @level2type=N'COLUMN',@level2name=N'TipoAttributoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano tipo attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiAttributi', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese tipo attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiAttributi', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiAttributi', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei tipi di attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiAttributi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK tipo file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiFile', @level2type=N'COLUMN',@level2name=N'TipoFileID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome del file icona che viene visualizzato su front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiFile', @level2type=N'COLUMN',@level2name=N'FileIcona'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Estensione del tipo file' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiFile', @level2type=N'COLUMN',@level2name=N'Estensione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipo MIME (non utilizzato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiFile', @level2type=N'COLUMN',@level2name=N'TipoMIME'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Software utilizzato per aprire il file (non utilizzato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiFile', @level2type=N'COLUMN',@level2name=N'Software'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei tipi di attributo' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiFile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK tipo link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiLink', @level2type=N'COLUMN',@level2name=N'TipoLinkID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano tipo link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiLink', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese tipo link' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiLink', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei tipi link (sito web,  progetto cartografico, ...)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiLink'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK tipo oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiOggetto', @level2type=N'COLUMN',@level2name=N'TipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_MacroTipiOggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiOggetto', @level2type=N'COLUMN',@level2name=N'MacroTipoOggettoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano tipo oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiOggetto', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese tipo oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiOggetto', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei tipi oggetto (TBL_Oggetti)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiOggetto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK tipo provvedimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'TipoProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_AreeTipiProvvedimenti' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'AreaTipoProvvedimentoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano tipo provvedimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese tipo provvedimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'ordine' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiProvvedimenti', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei tipi provvedimento (TBL_Provvedimenti)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipiProvvedimenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK tipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Tipologie', @level2type=N'COLUMN',@level2name=N'TipologiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_MacroTipologie' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Tipologie', @level2type=N'COLUMN',@level2name=N'MacroTipologiaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano tipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Tipologie', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese tipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Tipologie', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome del file per l''icona della tipologia (utilizzata in Home)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Tipologie', @level2type=N'COLUMN',@level2name=N'FileIcona'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario delle tipologie di opera (TBL_Opere)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Tipologie'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK tipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipologieTerritorio', @level2type=N'COLUMN',@level2name=N'TipologiaTerritorioID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome italiano tipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipologieTerritorio', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome inglese tipologia' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipologieTerritorio', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se la tipologia deve essere visibile nella ricerca (non utilizzato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipologieTerritorio', @level2type=N'COLUMN',@level2name=N'MostraRicerca'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario delle tipologie di territorio (TBL_Territori)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_TipologieTerritorio'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK dato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'DatoAmbientaleHomeID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Immagini' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'ImmagineID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo italiano dato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'Titolo_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo inglese dato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'Titolo_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link cui punta il dato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'Link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se pubblico o no' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'Pubblicato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultima modifica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome', @level2type=N'COLUMN',@level2name=N'DataUltimaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella in cui si trovano i dati ambientali presenti nella home del front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_DatiAmbientaliHome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK lingua' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Lingue', @level2type=N'COLUMN',@level2name=N'LinguaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome lingua' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Lingue', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario delle lingue disponibili (non utilizzata)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Lingue'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'OggettoCaroselloID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipologia di contenuto (1 = TBL_Oggetti)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'TipoContenutoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Identificativo contenuto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'ContenutoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_Immagini' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'ImmagineID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'Data'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in italiano dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in inglese dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in italiano dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'Descrizione_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in inglese dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'Descrizione_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link dell''oggetto' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'LinkOggetto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Link del progetto cartografico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'LinkProgettoCartografico'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se l''elemento è pubblicato su front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'Pubblicato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultima modifica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello', @level2type=N'COLUMN',@level2name=N'DataUltimaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dove si trovano gli oggetti che sono visualizzati nel carosello in home del front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_OggettiCarosello'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK pagina' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'PaginaStaticaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_UI_VociMenu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'VoceMenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultima modifica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'DataUltimaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in italiano della pagina statica -> il nome della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in inglese della pagina statica -> il nome della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo in italiano della pagina statica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Testo_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo in inglese della pagina statica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Testo_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Visibilità pagina statica (non utilizzato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche', @level2type=N'COLUMN',@level2name=N'Visibile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dove si trovano i contenuti delle pagine statiche visibili su front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_PagineStatiche'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK variabile' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Variabili', @level2type=N'COLUMN',@level2name=N'Chiave'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'valore variabile' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Variabili', @level2type=N'COLUMN',@level2name=N'Valore'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario di variabili utilizzate in front end (link dei siti VA regionali, ecc..)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Variabili'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK voce dizionario' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociDizionario', @level2type=N'COLUMN',@level2name=N'VoceDizionarioID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'sezione delle voci (utilizzata solo per organizzazione)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociDizionario', @level2type=N'COLUMN',@level2name=N'Sezione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome della voce, utilizzato per il recupero da front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociDizionario', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valore italiano della voce' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociDizionario', @level2type=N'COLUMN',@level2name=N'Valore_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Valore inglese della voce' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociDizionario', @level2type=N'COLUMN',@level2name=N'Valore_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei testi di piccole dimensioni utilizzati sul front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociDizionario'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK voce menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'VoceMenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Voce menu genitore di quella corrente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'GenitoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipologia del menu (0 = menu superiore, 1 = menu principale)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'TipoMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'nome italiano della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in inglese voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'descrizione in italiano della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Descrizione_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Testo in inglese della voce di menu' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Descrizione_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Sezione del sito (Controller)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Sezione'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'voce di menu (Action)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Voce'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se la voce deve avere un link o no' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Link'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se la voce è di una pagina statica o no' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Editabile'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se la voce è visibile a front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'VisibileFrontEnd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se la voce è visibile nella mappa del sito' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'VisibileMappa'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se la pagina può contenere widget o no' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'WidgetAbilitati'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ordinamento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu', @level2type=N'COLUMN',@level2name=N'Ordine'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella in cui si trovano le voci che costruiscono i menu di navigazione sul front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_VociMenu'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'WidgetID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tipologia di widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'TipoWidget'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo italiano del widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Titolo inglese del widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Categoria delle notizie che devono apparire nel widget (applicabile ai widget notizia)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'CategoriaNotiziaID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Numero di elementi che vanno a popolare il widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'NumeroElementi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data inserimento' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'DataInserimento'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultima modifica' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'DataUltimaModifica'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Voce menu linkata nel widget' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'VoceMenuID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contenuto italiano (applicabile ai widget HTML)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'Contenuto_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Contenuto inglese (applicabile ai widget HTML)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'Contenuto_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se deve essere mostrato il titolo a front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget', @level2type=N'COLUMN',@level2name=N'MostraTitolo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella in cui si trovano i widget visibili sul front end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_UI_Widget'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK utente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'UtenteID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Ruolo (non più utilizzato)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'Ruolo'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome utente per il login' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'NomeUtente'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Password (hash)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'Pswd'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Indicazione se l''utente è abilitato' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'Abilitato'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultimo cambio password (se impostata a NULL è obbligatorio cambiare la password)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'DataUltimoCambioPassword'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Data ultimo login' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'DataUltimoLogin'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Email utente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'Email'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome utente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'Nome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Cognome utente' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti', @level2type=N'COLUMN',@level2name=N'Cognome'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella degli utenti del back end' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Utenti'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_OggettiProcedure' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ValoriDatiAmministrativi', @level2type=N'COLUMN',@level2name=N'OggettoProceduraID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'FK verso TBL_DatiAmministrativi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ValoriDatiAmministrativi', @level2type=N'COLUMN',@level2name=N'DatoAmministrativoID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'valore testuale' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ValoriDatiAmministrativi', @level2type=N'COLUMN',@level2name=N'ValoreTesto'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'valore data' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ValoriDatiAmministrativi', @level2type=N'COLUMN',@level2name=N'ValoreData'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'valore numerico' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ValoriDatiAmministrativi', @level2type=N'COLUMN',@level2name=N'ValoreNumero'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'valore booleano' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ValoriDatiAmministrativi', @level2type=N'COLUMN',@level2name=N'ValoreBooleano'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella contentente i valori dei dati amministrativi' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_ValoriDatiAmministrativi'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'PK valutatore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Valutatori', @level2type=N'COLUMN',@level2name=N'ValutatoreID'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in italiano del valutatore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Valutatori', @level2type=N'COLUMN',@level2name=N'Nome_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Nome in inglese del valutatore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Valutatori', @level2type=N'COLUMN',@level2name=N'Nome_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in italiano del valutatore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Valutatori', @level2type=N'COLUMN',@level2name=N'Descrizione_IT'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Descrizione in inglese del valutatore' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Valutatori', @level2type=N'COLUMN',@level2name=N'Descrizione_EN'
GO
EXEC sys.sp_addextendedproperty @name=N'MS_Description', @value=N'Tabella dizionario dei valutatori (TBL_OggettiProcedure)' , @level0type=N'SCHEMA',@level0name=N'dbo', @level1type=N'TABLE',@level1name=N'TBL_Valutatori'
GO

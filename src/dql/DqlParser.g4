parser grammar DqlParser    ;

options { tokenVocab=DqlLexer; }

startRule :
	selectAllFromTable
;

selectAllFromTable: 
	SELECT 
	ALL 
	FROM 
	TABLE_NAME
	SEMI_COLON
;


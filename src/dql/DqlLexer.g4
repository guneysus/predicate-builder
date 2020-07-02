lexer grammar DqlLexer;

SELECT			:	'select'	;
ALL				:	'*'			;
SEMI_COLON		:	';'			;
FROM			:	'from'		;



TABLE_NAME	: 
	([A-Z][A-Za-z]+)
	;


WS: [\n\t\r]+ -> skip;

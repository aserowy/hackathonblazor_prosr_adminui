grammar Prosr1;

// token
SYNTAX : 'syntax';
PACKAGE : 'package';
MESSAGE : 'message';
HUB : 'hub';
ACTION : 'action';
CALLS : 'calls';
ON : 'on';
REPEATED : 'repeated';
TYPE : 'string' | 'bool' | 'int32';
IDENT : [a-zA-Z]([a-zA-Z0-9] | '_')+;
NUMBER : [0-9]+;
WHITESPACE : [ \r\n\t]+ -> skip;

// parser rules
content : syntax definition* EOF;
syntax : SYNTAX '=' quote 'prosr1' quote';';
definition : (pkg | hub | message);

pkg : PACKAGE fullIdent ';';
hub : HUB hubIdent '{' (sending | returning)+ '}';
sending : ACTION sendingIdent'('sendingMessageIdent')' (CALLS returningIdent'('returningMessageIdent')' ON sendingTarget)? ';';
sendingMessageIdent : (messageIdent)?;
sendingTarget : ('caller' | 'all');
returning : CALLS returningIdent'('returningMessageIdent')' ON returningTarget ';';
returningMessageIdent : (messageIdent)?;
returningTarget : 'all';

message : MESSAGE messageIdent '{' (field)* '}';
field : (REPEATED)? typeIdent fieldIdent '=' NUMBER';';
typeIdent : (packageIdent messageIdent | TYPE);

// literals & identifier
quote : '\'' | '"';

fieldIdent : IDENT;
fullIdent : IDENT ('.' IDENT)*;
hubIdent : IDENT;
messageIdent : IDENT;
// packageIdent : ('.')? (IDENT '.')*;
packageIdent : (IDENT '.')*;
returningIdent : IDENT;
sendingIdent : IDENT;
using Antlr4.Runtime;
using Antlr4.Runtime.Misc;
using System;
using System.Collections.Generic;
using System.Text;
using Antlr4.Runtime.Atn;
using Antlr4.Runtime.Sharpen;
using Serilog;
using Serilog.Core;
using Antlr4.Runtime.Dfa;

namespace dql
{
    public class Dql
    {
        public (DqlParser.StartRuleContext, string) Parse(string input)
        {
            var stream = new AntlrInputStream(input);
            var lexer = new DqlLexer(stream);
            var tokens = new CommonTokenStream(lexer);
            var parser = new DqlParser(tokens);
            parser.BuildParseTree = true;
            parser.TrimParseTree = true;

            DqlParser.StartRuleContext startRuleContext = parser.startRule();

            var listener = new DefaultListener(parser);
            var errorListener = new DefaultErrorListener();

            parser.AddParseListener(listener);
            parser.AddErrorListener(errorListener);
            Antlr4.Runtime.Tree.ParseTreeWalker.Default.Walk(listener, startRuleContext);

            return (startRuleContext, startRuleContext.ToStringTree(parser));
        }
    }
}
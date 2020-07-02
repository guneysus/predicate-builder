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

    public class DefaultListener : DqlParserBaseListener {
        private readonly DqlParser parser;

        public DefaultListener(DqlParser parser)
        {
            this.parser = parser;
        }
    }
}
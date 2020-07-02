using Antlr4.Runtime;
using System;
using System.Runtime.CompilerServices;
using Antlr4.Runtime.Tree.Pattern;

namespace dql
{
    public static class LogHelper
    {
        public static void Trace(object message = null, [CallerMemberName] string memberName = "")
        {
            DefaultErrorListener.Logger.Debug($"[{DateTime.Now.ToString("G")}] @{memberName} {message}");
        }

        public static void Debug(ParseTreePattern parseTree, Parser parser, [CallerMemberName] string memberName = "")
        {
            DefaultErrorListener.Logger.Debug($"[{DateTime.Now.ToString("G")}] @{memberName} {parseTree.PatternTree.ToStringTree()}");
        }

        public static void Error(object message = null, [CallerMemberName] string memberName = "")
        {
            DefaultErrorListener.Logger.Error($"[{DateTime.Now.ToString("G")}] @{memberName} {message}");
        }
    }
}
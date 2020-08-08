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
using NorthwindApp;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using Northwind.Domain.Entities;

namespace dql
{

    public class DefaultListener : DqlParserBaseListener
    {
        public NorthwindDbContext db { get; }

        private readonly DqlParser parser;

        public DefaultListener(DqlParser parser, NorthwindDbContext db)
        {
            this.db = db;
            this.parser = parser;
        }

        public override void EnterSelectAllFromTable([NotNull] DqlParser.SelectAllFromTableContext context)
        {
            var tableName = context.TABLE_NAME().GetText();

            const string assemblyName = "NorthwindApp";
            const string @namespace = "Northwind.Domain.Entities";

            string assemblyQualifiedName = $"{string.Join('.', @namespace, tableName)}, {assemblyName}";
            var type = Type.GetType(assemblyQualifiedName);

            var set = typeof(NorthwindDbContext).GetMethod("Set");

            var genericSet = set.MakeGenericMethod(type);
            try
            {
                var result = genericSet.Invoke(db, new object[] { });

                Expression<Func<Product, bool>> predicate = x => x.Id < 2;

                // Where<TSource>(this IQueryable<TSource> source, Expression<Func<TSource, bool>> predicate)
                MethodInfo where = typeof(Queryable)
                        .GetMethods()
                        .First(x => x.Name == "Where")
                        ;

                var whereGeneric = where.MakeGenericMethod(type);

                var filtered = whereGeneric.Invoke(null, new object[] { result, predicate });

                // new string[] { }.AsQueryable().Where);
            }
            catch (Exception ex)
            {
                throw;
            }

            base.EnterSelectAllFromTable(context);
        }
    }

    public static class ReflectionExtensions
    {

    }

}
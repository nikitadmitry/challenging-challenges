using System;
using AutoMapper;

namespace Shared.Framework.Automapper
{
    public static class AutoMapperExtensions
    {
        /// <summary>
        /// Extension for apply Ignore for all members, mapping properties can be configured after this extension
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDest"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDest>
            IgnoreAllUnmapped<TSource, TDest>(this IMappingExpression<TSource, TDest> expression)
        {
            expression.ForAllMembers(opt => opt.Ignore());
            return expression;
        }
    }
}
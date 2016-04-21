using System;
using System.Linq;
using System.Linq.Expressions;

namespace Shared.Framework.Extensions
{
    public static class ExpressionExtentions
    {
        /// <summary>
        /// Builds full expression from root expression and child expression.
        /// Usage example:
        /// <c>
        ///     Expression{Func{TParent, TChild}} rootExpression = root => root.Child; 
        ///     var result = rootExpression.Sub(child => child.Method(args));
        ///     // result: root => root.Child.Method(args);
        /// </c>
        /// </summary>
        /// <typeparam name="TRoot">The type of the root object.</typeparam>
        /// <typeparam name="TChild">The type of the child object.</typeparam>
        /// <typeparam name="TChildMember">The type of the child member.</typeparam>
        /// <param name="expression">The root expression.</param>
        /// <param name="childExpression">The child expression.</param>
        public static Expression<Func<TRoot, TChildMember>> Sub<TRoot, TChild, TChildMember>(
            this Expression<Func<TRoot, TChild>> expression,
            Expression<Func<TChild, TChildMember>> childExpression)
        {
            var replaceVisitor = new ReplaceVisitor(childExpression.Parameters.First(), expression.Body);
            var newChildExpression = replaceVisitor.Visit(childExpression.Body);
            return Expression.Lambda<Func<TRoot, TChildMember>>(newChildExpression, expression.Parameters);
        }

        private class ReplaceVisitor : ExpressionVisitor
        {
            private readonly Expression from;
            private readonly Expression to;

            public ReplaceVisitor(Expression from, Expression to)
            {
                this.from = from;
                this.to = to;
            }

            public override Expression Visit(Expression node)
            {
                return node == from ? to : base.Visit(node);
            }
        }
    }
}
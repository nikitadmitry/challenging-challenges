using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace Shared.Framework.DataSource
{
    /// <summary>
    /// Represents expression converter
    /// </summary>
    public class ExpressionConverter
    {
        /// <summary>
        /// Converting expression
        /// </summary>
        /// <param name="expr">Expression to convert.</param>
        /// <param name="propertyName">Name of the property.</param>
        /// <typeparam name="TEntity">Entity used in expression.</typeparam>
        /// <typeparam name="TPropertyType">Property type to convert.</typeparam>
        /// <returns></returns>
        public static Expression<Func<TEntity, TPropertyType>> Convert<TEntity, TPropertyType>(Expression<Func<TEntity, object>> expr, String propertyName)
        {
            var substitutues = new Dictionary<Expression, Expression>();
            var oldParam = expr.Parameters[0];
            var newParam = Expression.Parameter(typeof(TEntity), oldParam.Name);

            substitutues.Add(oldParam, newParam);
            var body = (expr.Body is UnaryExpression ? ((UnaryExpression)expr.Body).Operand : expr.Body);
            Expression convertNode = ConvertNode(body, substitutues, propertyName);
            Expression conversion = Expression.Convert(convertNode, typeof(TPropertyType));

            return Expression.Lambda<Func<TEntity, TPropertyType>>(conversion, newParam);
        }

        private static Expression ConvertNode(Expression node, IDictionary<Expression, Expression> subst, String propertyName)
        {
            if (node == null) return null;
            if (subst.ContainsKey(node)) return subst[node];

            switch (node.NodeType)
            {
                case ExpressionType.Constant:
                    return node;
                case ExpressionType.MemberAccess:
                    {
                        var me = (MemberExpression)node;
                        var newNode = ConvertNode(me.Expression, subst, propertyName);
                        return Expression.MakeMemberAccess(newNode, newNode.Type.GetMember(propertyName).Single());
                    }
                case ExpressionType.Equal:
                    {
                        var be = (BinaryExpression)node;
                        return Expression.MakeBinary(be.NodeType, ConvertNode(be.Left, subst, propertyName), ConvertNode(be.Right, subst, propertyName), be.IsLiftedToNull, be.Method);
                    }
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var me1 = (MemberExpression)node;
                    var newNode1 = ConvertNode(me1.Expression, subst, propertyName);
                    return Expression.MakeMemberAccess(newNode1, newNode1.Type.GetMember(propertyName).Single());
                default:
                    throw new NotSupportedException(node.NodeType.ToString());
            }
        }
    }
}

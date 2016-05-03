using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Shared.Framework.Utilities
{
    /// <summary>
    /// Represents helper methods for expressions
    /// </summary>
    public static class ExpressionUtility
    {
        /// <summary>
        /// Gets member info from expression
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static MemberInfo GetLambdaExpressionMemberInfo(LambdaExpression expression)
        {
            MemberInfo member;

            var body = expression.Body as MemberExpression;

            if (body != null)
            {
                member = body.Member;
            }
            else
            {
                var op = ((UnaryExpression)expression.Body).Operand;

                member = ((MemberExpression)op).Member;
            }

            return member;
        }

        private static MemberInfo GetMemberInfo<T, U>(Expression<Func<T, U>> expression)
        {
            var member = expression.Body as MemberExpression;
            if (member != null)
            {
                return member.Member;
            }
            var method = expression.Body as MethodCallExpression;
            if (method != null)
            {
                return method.Method;
            }

            throw new ArgumentException("Expression is not a member access", "expression");
        }

        /// <summary>
        /// Utility method used to get the member/method name of the expression. It can be used for both method expression and property expressions
        /// </summary>
        /// <typeparam name="TClass"></typeparam>
        /// <typeparam name="TProperty"></typeparam>
        /// <param name="expression"></param>
        /// <returns></returns>
        public static string GetNameByExpression<TClass, TProperty>(Expression<Func<TClass, TProperty>> expression)
        {
            return GetMemberInfo(expression).Name;
        }

        public static bool AreLambdaExpressionsMemberNamesEqual(LambdaExpression expression1, LambdaExpression expression2)
        {
            var memberInfo1 = GetLambdaExpressionMemberInfo(expression1);
            var memberInfo2 = GetLambdaExpressionMemberInfo(expression2);

            if (memberInfo1 == null && memberInfo2 != null || memberInfo1 != null && memberInfo2 == null)
            {
                return false;
            }

            if (memberInfo1 == memberInfo2)
            {
                return true;
            }

            return memberInfo1.Name.Equals(memberInfo2.Name);
        }
    }
}
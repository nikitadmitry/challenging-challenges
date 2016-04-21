using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq.Expressions;
using System.Reflection;
using Shared.Framework.DataSource;
using Shared.Framework.Utilities;

namespace Data.Common.Query.FilterExpressions
{
    /// <summary>
    /// Provides base functionality for filter expression providers
    /// </summary>
    public abstract class BaseFilterExpressionProvider : IFilterExpressionProvider
    {

        protected Dictionary<FilterOperator, Func<Expression, PropertyInfo, object, Expression>>
            ExpressionCollection;

        protected Func<Expression, PropertyInfo, object, Expression> DefaultExpressionHandler;

        /// <summary>
        /// Initializes a new instance of the <see cref="BaseFilterExpressionProvider"/> class.
        /// </summary>
        protected BaseFilterExpressionProvider()
        {
            ExpressionCollection =
                new Dictionary<FilterOperator, Func<Expression, PropertyInfo, object, Expression>>();

            DefaultExpressionHandler = (memberExpression, propertyInfo, value) =>
            {
                var constant = GetConstantWithNullableCastHandle(memberExpression, propertyInfo, value);
                Expression compExpr = Expression.Equal(memberExpression, constant);
                return compExpr;
            };
        }

        /// <summary>
        /// Creates the part of the full filter expression which can be joined with other parts by logic operator.
        /// </summary>
        /// <param name="argument">The argument.</param>
        /// <param name="property">The property.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">The value.</param>
        public Expression CreateConditionExpression(ParameterExpression argument, LambdaExpression property,
            FilterOperator filterOperator, object value)
        {
            var propertyInfo = GetLambdaExpressionMemberInfo(property) as PropertyInfo;

            var memberExpression = Expression.Property(argument, propertyInfo);
            return GetExpression(filterOperator, memberExpression, propertyInfo, value);
        }

        /// <summary>
        /// Creates the filter expression from the specified member expression, comparison operator and filter value.
        /// </summary>
        /// <typeparam name="T">Type of entity.</typeparam>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="value">The filter value.</param>
        public Expression<Func<T, bool>> CreateFilterExpression<T>(LambdaExpression memberExpression,
            FilterOperator filterOperator, object value)
        {
            var argExpr = Expression.Parameter(typeof(T), "p");

            var compExpr = CreateConditionExpression(argExpr, memberExpression, filterOperator, value);

            return Expression.Lambda<Func<T, bool>>(compExpr, argExpr);
        }

        /// <summary>
        /// Determines whether this filter expression provider 
        /// contains filter expression for the specified filter operator.
        /// </summary>
        /// <param name="filterOperator">The filter operator.</param>
        /// <returns>
        ///   <c>true</c> if this filter expression provider contains 
        /// filter expression for the specified filter operator; 
        /// otherwise, <c>false</c>.
        /// </returns>
        public virtual bool ContainsFilterExpressionFor(FilterOperator filterOperator)
        {
            return ExpressionCollection.ContainsKey(filterOperator);
        }


        /// <summary>
        /// Gets the lambda expression member info.
        /// </summary>
        /// <param name="expression">The expression.</param>
        protected MemberInfo GetLambdaExpressionMemberInfo(LambdaExpression expression)
        {
            MemberInfo memberInfo;

            var body = expression.Body as MemberExpression;

            if (body != null)
            {
                memberInfo = body.Member;
            }
            else
            {
                var operand = ((UnaryExpression)expression.Body).Operand;

                memberInfo = ((MemberExpression)operand).Member;
            }

            return memberInfo;
        }

        /// <summary>
        /// Gets the comparison expression for the specified entity member and filter operator.
        /// </summary>
        /// <param name="filterOperator">The filter operator.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="value">The value.</param>
        /// <returns>
        /// The comparison expression if this instance of filter expression provider contains 
        /// comparison expression for the specified filter operator; otherwise returns default expression.
        /// </returns>
        protected Expression GetExpression(FilterOperator filterOperator, Expression memberExpression,
            PropertyInfo propertyInfo, object value)
        {
            var expressionHandler = ContainsFilterExpressionFor(filterOperator)
                ? ExpressionCollection[filterOperator]
                : DefaultExpressionHandler;

            return expressionHandler.Invoke(memberExpression, propertyInfo, value);
        }

        /// <summary>
        /// Gets the constant expression containing the specified value.
        /// </summary>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="value">The filter value.</param>
        protected Expression GetConstant(PropertyInfo propertyInfo, object value)
        {
            Type propertyType = Nullable.GetUnderlyingType(propertyInfo.PropertyType) ?? propertyInfo.PropertyType;

            Expression constantExpression = parametrizationHandlersTable.ContainsKey(propertyType)
                ? parametrizationHandlersTable[propertyType](value, propertyType)
                : GetDefaultParamterizedExpression(value, propertyInfo.PropertyType);

            return constantExpression;
        }

        private static readonly Dictionary<Type, Func<object, Type, Expression>> parametrizationHandlersTable =
            new Dictionary<Type, Func<object, Type, Expression>>
            {
                { typeof(Guid), GetGuidParamterizedExpression },
                { typeof(Byte), GetNumericParamterizedExpression<Byte> },
                { typeof(Int16), GetNumericParamterizedExpression<Int16> },
                { typeof(Int32), GetNumericParamterizedExpression<Int32> },
                { typeof(Int64), GetNumericParamterizedExpression<Int64> },
                { typeof(String), GetSimpleParamterizedExpression<String> },
                { typeof(DateTime), GetSimpleParamterizedExpression<DateTime> },
                { typeof(Single), GetSimpleParamterizedExpression<Single> },
                { typeof(Double), GetSimpleParamterizedExpression<Double> },
                { typeof(Decimal), GetSimpleParamterizedExpression<Decimal> },
                { typeof(Boolean), GetSimpleParamterizedExpression<Boolean> }
            };

        private Expression GetDefaultParamterizedExpression(object value, Type propertyType)
        {
            CheckVerificationForValue(value, propertyType);

            if (value != null)
            {
                value = GetExpressionValue(value, propertyType);

                Expression result = Expression.Property(Expression.Constant(new { Value = value }), "Value");

                bool isTargetPropertyNotNullableEnum = propertyType.IsEnum && Nullable.GetUnderlyingType(propertyType) == null;
                return isTargetPropertyNotNullableEnum
                    ? Expression.Convert(result, value.GetType())
                    : result;
            }

            return Expression.Constant(value);
        }

        private static void CheckVerificationForValue(object value, Type propertyType)
        {
            Contract.Requires<ArgumentException>(
                value == null ||
                ObjectTransformer.CheckIEnumerableInterface(value.GetType()) ||
                value.GetType().IsEnum ||
                CanAndShouldBeConvertedToEnum(value, propertyType) ||
                CanAndShouldBeConvertedToEnumForNullable(value, propertyType)
                , "value");
        }

        private static object GetExpressionValue(object value, Type propertyType)
        {
            if (CheckForIntOrByte(value, propertyType))
            {
                value = Enum.ToObject(propertyType, value);
            }

            else if (CheckForIntOrByteWithNullable(value, propertyType))
            {
                value = Enum.ToObject(Nullable.GetUnderlyingType(propertyType), value);
            }

            else if (CheckForFloatOrDouble(value, propertyType))
            {
                value = Enum.ToObject(propertyType, Int32.Parse(value.ToString()));
            }

            else if (CheckForFloatOrDoubleWithNullable(value, propertyType))
            {
                value = Enum.ToObject(Nullable.GetUnderlyingType(propertyType), Int32.Parse(value.ToString()));
            }
            return value;
        }

        private static bool IsIntOrByte(object value)
        {
            return value is Int32 || value is Byte;
        }

        private static bool CanAndShouldBeConvertedToEnum(object value, Type propertyType)
        {
            var checkForIntOrByte = CheckForIntOrByte(value, propertyType);
            var checkForFloatOrDouble = CheckForFloatOrDouble(value, propertyType);

            return checkForIntOrByte || checkForFloatOrDouble;
        }

        private static bool CheckForFloatOrDouble(object value, Type propertyType)
        {
            var checkForFloatOrDouble = IsFloatOrDouble(value) && propertyType.IsEnum;
            return checkForFloatOrDouble;
        }

        private static bool CheckForIntOrByte(object value, Type propertyType)
        {
            var checkForIntOrByte = IsIntOrByte(value) && propertyType.IsEnum;
            return checkForIntOrByte;
        }

        private static bool IsFloatOrDouble(object value)
        {
            return (value is Single || value is Double);
        }

        private static bool CanAndShouldBeConvertedToEnumForNullable(object value, Type propertyType)
        {
            var checkForIntOrByte = CheckForIntOrByteWithNullable(value, propertyType);
            var checkForFloatOrDouble = CheckForFloatOrDoubleWithNullable(value, propertyType);

            return checkForIntOrByte || checkForFloatOrDouble;
        }

        private static bool CheckForFloatOrDoubleWithNullable(object value, Type propertyType)
        {
            var checkForFloatOrDouble = IsFloatOrDouble(value) && Nullable.GetUnderlyingType(propertyType).IsEnum;
            return checkForFloatOrDouble;
        }

        private static bool CheckForIntOrByteWithNullable(object value, Type propertyType)
        {
            var checkForIntOrByte = IsIntOrByte(value) && Nullable.GetUnderlyingType(propertyType).IsEnum;
            return checkForIntOrByte;
        }
        
        private static Expression GetSimpleParamterizedExpression<T>(object value, Type propertyType)
        {
            if (value is T)
            {
                return Expression.Property(Expression.Constant(new { Value = (T)value }), "Value");
            }

            return Expression.Constant(value);
        }

        private static Expression GetNumericParamterizedExpression<T>(object value, Type propertyType)
        {
            value = ConvertDoubleConstantIntoTarget(value, propertyType);
            return GetSimpleParamterizedExpression<T>(value, propertyType);
        }

        private static Expression GetGuidParamterizedExpression(object value, Type propertyType)
        {
            Expression paramterizedExpression;

            if (value is Guid || value is String)
            {
                var guid = Guid.Parse(value.ToString());
                paramterizedExpression = Expression.Property(Expression.Constant(new { Value = guid }), "Value");
            }
            else
            {
                paramterizedExpression = value != null
                    ? Expression.Constant(value, value.GetType())
                    : Expression.Constant(null);
            }

            return paramterizedExpression;
        }

        private static object ConvertDoubleConstantIntoTarget(object value, Type type)
        {
            if (value is double)
            {
                return Convert.ChangeType(value, type);
            }

            return value;
        }

        /// <summary>
        /// Gets the constant expression containing the specified value with nullable cast handle.
        /// </summary>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="value">The value.</param>
        protected Expression GetConstantWithNullableCastHandle(Expression memberExpression,
            PropertyInfo propertyInfo, object value)
        {
            var constant = GetConstant(propertyInfo, value);
            NullableCastHandle(ref memberExpression, ref constant);

            return constant;
        }

        /// <summary>
        /// Casts one of the expressions to the type of the other expression if one of the expressions 
        /// is nullable and the other one isn't nullable.
        /// </summary>
        /// <param name="firstExpression">The first expression.</param>
        /// <param name="secondExpression">The second expression.</param>
        protected void NullableCastHandle(ref Expression firstExpression, ref Expression secondExpression)
        {
            if (IsCollection(firstExpression.Type) || IsCollection(secondExpression.Type))
            {
                return;
            }

            //EF can't cast nullable type to primitive types
            if (IsNullableType(firstExpression.Type) && !IsNullableType(secondExpression.Type))
            {
                secondExpression = Expression.Convert(secondExpression, firstExpression.Type);
            }
            else if (!IsNullableType(firstExpression.Type) && IsNullableType(secondExpression.Type))
            {
                firstExpression = Expression.Convert(firstExpression, secondExpression.Type);
            }
        }

        /// <summary>
        /// Determines whether the specified type is nullable type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns>
        ///   <c>true</c> if the specified type is nullable type; otherwise, <c>false</c>.
        /// </returns>
        protected bool IsNullableType(Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        /// <summary>
        /// Includes the handling of the situation when the value will be compared with null 
        /// to the comparison expression.
        /// </summary>
        /// <param name="comparisonExpression">The comparison expression.</param>
        /// <param name="memberExpression">The member expression.</param>
        /// <param name="propertyInfo">The property info.</param>
        /// <param name="value">The filter value.</param>
        protected Expression HandleNullComparison(Expression comparisonExpression, Expression memberExpression,
            PropertyInfo propertyInfo, object value)
        {
            if ((IsNullableType(memberExpression.Type) || memberExpression.Type == typeof(String)) && value != null)
            {
                var nullConstant = GetConstant(propertyInfo, null);
                var nullComparison = Expression.Equal(memberExpression, nullConstant);
                comparisonExpression = Expression.OrElse(nullComparison, comparisonExpression);
            }
            return comparisonExpression;
        }

        private bool IsCollection(Type type)
        {
            return type.GetInterface("IEnumerable") != null;
        }
    }
}

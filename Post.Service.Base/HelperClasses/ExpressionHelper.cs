using System;
using System.Linq.Expressions;

namespace Post.Service.Base.HelperClasses
{
    public static class ExpressionHelper
    {
        public static Expression<Func<T, bool>> CombineAnd<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null && right == null) throw new ArgumentException("At least one argument must not be null");
            if (left == null) return right;
            if (right == null) return left;

            var parameter = Expression.Parameter(typeof(T), "p");
            var combined = new ParameterReplacer(parameter).Visit(Expression.AndAlso(left.Body, right.Body));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        public static Expression<Func<T, bool>> CombineOR<T>(Expression<Func<T, bool>> left, Expression<Func<T, bool>> right)
        {
            if (left == null && right == null) throw new ArgumentException("At least one argument must not be null");
            if (left == null) return right;
            if (right == null) return left;

            var parameter = Expression.Parameter(typeof(T), "p");
            var combined = new ParameterReplacer(parameter).Visit(Expression.OrElse(left.Body, right.Body));
            return Expression.Lambda<Func<T, bool>>(combined, parameter);
        }

        class ParameterReplacer : ExpressionVisitor
        {
            readonly ParameterExpression parameter;

            internal ParameterReplacer(ParameterExpression parameter)
            {
                this.parameter = parameter;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return parameter;
            }
        }
    }

    
}

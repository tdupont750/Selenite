using System;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;

namespace Common.Extensions
{
    public static class PropertyChangeExtensions
    {
        public static PropertyChangingEventHandler NotifyPropertyChanging<T>(this PropertyChangingEventHandler handler, Expression<Func<T>> propertyExpression)
        {
            if (handler == null)
                return handler;

            if (propertyExpression == null)
                return handler;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                var unary = propertyExpression.Body as UnaryExpression;
                if (unary == null)
                    return handler;
                body = unary.Operand as MemberExpression;

                if (body == null)
                    return handler;
            }

            if (body.Member as PropertyInfo != null)
                if (body.Expression as ConstantExpression != null)
                    handler(((ConstantExpression)body.Expression).Value, new PropertyChangingEventArgs(body.Member.Name));

            return handler;
        }

        public static PropertyChangedEventHandler NotifyPropertyChanged<T>(this PropertyChangedEventHandler handler, Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                return handler;

            var body = propertyExpression.Body as MemberExpression;
            if (body == null)
            {
                var unary = propertyExpression.Body as UnaryExpression;
                if (unary == null)
                    return handler;
                body = unary.Operand as MemberExpression;

                if (body == null)
                    return handler;
            }
            if (body.Member as PropertyInfo != null)
                if (body.Expression as ConstantExpression != null)
                    if (handler != null)
                        handler(((ConstantExpression)body.Expression).Value, new PropertyChangedEventArgs(body.Member.Name));

            return handler;
        }
    }
}

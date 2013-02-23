using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;

namespace Selenite.Client.ViewModels
{
    public abstract class ViewModelBase : INotifyPropertyChanged, INotifyPropertyChanging
    {
        #region Property Get/Set

        private readonly IDictionary<string, object> _valueDictionary = new Dictionary<string, object>();

        /// <summary>
        /// Gets a Property Value based on an Expression
        /// </summary>
        protected virtual T Get<T>(Expression<Func<T>> propertyExpression)
        {
            if (propertyExpression == null)
                return default(T);

            var propertyName = GetPropertyName(propertyExpression);

            if (string.IsNullOrWhiteSpace(propertyName))
                return default(T);

            object propertyValue;
            return _valueDictionary.TryGetValue(propertyName, out propertyValue) ? (T)propertyValue : default(T);
        }

        /// <summary>
        /// Sets a property value based on an expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">The value.</param>
        /// <param name="propertyExpression">Primary Property To Update</param>
        /// <param name="secondaryExpressions">Additional Properties to notify of change</param>
        protected virtual void Set<T>(T value, Expression<Func<T>> propertyExpression, params Expression<Func<object>>[] secondaryExpressions)
        {
            if (propertyExpression == null)
                return;

            var propertyName = GetPropertyName(propertyExpression);

            if (string.IsNullOrWhiteSpace(propertyName))
                return;

            // notify self
            OnNotifyPropertyChanging(propertyName);

            // notify linked
            foreach (var prop in secondaryExpressions)
                PropertyChanging.NotifyPropertyChanging(prop);

            // set value
            _valueDictionary[propertyName] = value;

            // notify self
            OnNotifyPropertyChanged(propertyName);

            // notify linked
            foreach (var prop in secondaryExpressions)
                PropertyChanged.NotifyPropertyChanged(prop);
        }

        #endregion

        #region Extract Property Name from Expression

        /// <summary>
        /// Gets the name of the property.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expression">The expression.</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetPropertyName<T>(Expression<Func<T>> expression)
        {
            if (expression == null)
                return string.Empty;

            // faster than walking the expression
            var expressionString = expression.ToString();

            return expressionString.LastIndexOf(".") < expressionString.Length ? expressionString.Substring(expressionString.LastIndexOf(".") + 1) : string.Empty;
        }

        #endregion

        #region Implementation of INotifyPropertyChanged

        /// <summary>
        /// Occurs when a property value changes.
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion

        #region Implementation of INotifyPropertyChanging

        /// <summary>
        /// Occurs when a property value is changing.
        /// </summary>
        public event PropertyChangingEventHandler PropertyChanging;

        #endregion

        #region Protected Event Notifiers

        /// <summary>
        /// Called when [notify property changing].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnNotifyPropertyChanging(string propertyName)
        {
            if (PropertyChanging != null)
                PropertyChanging(this, new PropertyChangingEventArgs(propertyName));
        }

        /// <summary>
        /// Called when [notify property changed].
        /// </summary>
        /// <param name="propertyName">Name of the property.</param>
        protected virtual void OnNotifyPropertyChanged(string propertyName)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}
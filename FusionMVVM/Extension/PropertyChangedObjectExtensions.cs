using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using FusionMVVM.Common;

namespace FusionMVVM.Extension
{
    public static class PropertyChangedObjectExtensions
    {
        /// <summary>
        /// Raises the PropertyChanged event if the value has changed.
        /// </summary>
        /// <param name="source"></param>
        /// <param name="expression"></param>
        /// <param name="backingField"></param>
        /// <param name="value"></param>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        public static void RaiseIfPropertyChanged<TSource, TValue>(this TSource source, Expression<Func<TSource, TValue>> expression, ref TValue backingField, TValue value) where TSource : PropertyChangedObject
        {
            if (EqualityComparer<TValue>.Default.Equals(backingField, value) == false)
            {
                // The property value has changed.
                var body = expression.Body as MemberExpression;
                if (body != null)
                {
                    // Set backing field and call OnPropertyChanged.
                    backingField = value;
                    source.OnPropertyChanged(body.Member.Name);
                }
            }
        }
    }
}

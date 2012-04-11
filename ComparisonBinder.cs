/**
 * This file is part of BindingTools.
 * Copyright (C) 2012 Saulo Vallory <me@saulovallory.com>
 * 
 * BindingTools is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *  
 * BindingTools is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with Edmx Typist. If not, see <http://www.gnu.org/licenses/>.
 */
namespace BindingTools
{
	using System;
	using System.ComponentModel;
	using System.Linq.Expressions;
	using System.Reflection;

	class ComparisonBinder<TSource, TValue> : ExpressionBinder<bool> where TSource : INotifyPropertyChanged
	{
		private readonly TSource instance;
		private readonly TValue comparisonValue;
		private readonly PropertyInfo pInfo;

		public ComparisonBinder(TSource instance, Expression<Func<TSource, TValue>> property, TValue comparisonValue)
			: base(null,null)
		{
			pInfo = GetPropertyInfo(property);

			this.instance = instance;
			this.comparisonValue = comparisonValue;

			Getter = GetValue;
			Setter = SetValue;

			instance.PropertyChanged += SourcePropertyChanged;
			WatchingProps.Add(pInfo.Name);
		}

		private bool GetValue()
		{
			return comparisonValue.Equals(pInfo.GetValue(instance, null));
		}

		private void SetValue(bool value)
		{
			if (value)
			{
				pInfo.SetValue(instance, comparisonValue, null);
			}
		}
	}
}

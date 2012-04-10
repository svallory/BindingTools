namespace EdmxTypist.BindingTools
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

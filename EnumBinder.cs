using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EdmxTypist.BindingTools
{
	using System.ComponentModel;
	using System.Reflection;

	public class BindableEnumItem<T> where T : struct
	{
		public T Value { get; set; }
		public string Label { get; set; }
	}

	public static class EnumBinder
	{
		public static List<BindableEnumItem<TE>> Bind<TE>() where TE : struct
		{
			var list = new List<BindableEnumItem<TE>>();

			var values = Enum.GetValues(typeof(TE)).Cast<TE>();

			foreach (var val in values)
			{
				list.Add(new BindableEnumItem<TE>() { Value = val, Label = EnumBinder.GetDescription(val) });
			}

			return list;
		}

		private static string GetDescription<TE>(TE enumObj) where TE : struct
		{
			FieldInfo fieldInfo = enumObj.GetType().GetField(enumObj.ToString());

			var desc = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false).FirstOrDefault() as DescriptionAttribute;

			if (desc == null)
				return enumObj.ToString();

			return desc.Description;
		}
	}
}

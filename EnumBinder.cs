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
	using System.ComponentModel;
	using System.Reflection;
	using System;
	using System.Collections.Generic;
	using System.Linq;

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

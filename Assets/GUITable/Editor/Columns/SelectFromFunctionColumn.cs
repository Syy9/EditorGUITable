using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorGUITable
{
	
	/// <summary>
	/// This class represents a column that will draw entries using the given function from the element's serialized property.
	/// This allows to build any type of table entry.
	/// </summary>
	public class SelectFromFunctionColumn : SelectorColumn
	{
		public Func<SerializedProperty, TableEntry> selector;
		public SelectFromFunctionColumn (Func<SerializedProperty, TableEntry> selector, string title, params TableColumnOption[] options) : base (title, options)
		{
			this.selector = selector;
		}
		public SelectFromFunctionColumn (Func<SerializedProperty, TableEntry> selector, string title, float width, params TableColumnOption[] options) : base (title, width, options)
		{
			this.selector = selector;
		}

		public override TableEntry GetEntry (SerializedProperty elementProperty)
		{
			return selector (elementProperty);
		}
	}


}
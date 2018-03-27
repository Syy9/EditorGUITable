using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorGUITable
{
	
	/// <summary>
	/// This class adds a property and a selector to a column.
	/// This will be used to automatically draw the entries for this column in some versions of <see cref="GUITable.DrawTable"/>
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
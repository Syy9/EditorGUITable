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
	public class SelectorColumn : PropertyColumn
	{
		public Func<SerializedProperty, TableEntry> selector;
		public SelectorColumn (Func<SerializedProperty, TableEntry> selector, string propertyName, string name, params TableColumnOption[] options) : base (propertyName, name, options)
		{
			this.selector = selector;
		}
	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// Internal Use Only.
	/// This class adds a property to a column.
	/// This will be used to automatically draw the entries for this column in some versions of <see cref="GUITable.DrawTable"/>
	/// </summary>
	public class SelectFromPropertyNameColumn : SelectorColumn
	{
		public string propertyName;
		public SelectFromPropertyNameColumn (string propertyName, string title, params TableColumnOption[] options) : base (title, options)
		{
			this.propertyName = propertyName;
		}

		public override TableEntry GetEntry (SerializedProperty elementProperty)
		{
			return new PropertyEntry (elementProperty.FindPropertyRelative (propertyName));
		}
	}

}
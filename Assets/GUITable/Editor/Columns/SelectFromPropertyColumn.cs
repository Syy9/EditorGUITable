using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This class represents a column that will draw Property Cells from the given property name, 
	/// relative to the collection element's serialized property.
	/// </summary>
	public class SelectFromPropertyNameColumn : SelectorColumn
	{
		public string propertyName;
		public SelectFromPropertyNameColumn (string propertyName, string title, params TableColumnOption[] options) : base (title, options)
		{
			this.propertyName = propertyName;
		}

		public override TableCell GetCell (SerializedProperty elementProperty)
		{
			return new PropertyCell (elementProperty.FindPropertyRelative (propertyName));
		}
	}

}
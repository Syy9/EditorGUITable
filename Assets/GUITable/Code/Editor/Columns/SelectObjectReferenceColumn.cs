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
	public class SelectObjectReferenceColumn : SelectorColumn
	{
		public SelectObjectReferenceColumn (string title, params TableColumnOption[] options) : base (title, options)
		{
		}

		public override TableCell GetCell (SerializedProperty elementProperty)
		{
			if (elementProperty.propertyType == SerializedPropertyType.ObjectReference)
			{
				return new ObjectCell (elementProperty);
			}
			else
			{
				Debug.LogErrorFormat ("The property {0} is not an object reference. Cannot use SelectObjectReferenceColumn on it.");
				return null;
			}
		}
	
	}

}
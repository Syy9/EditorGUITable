using System;
using System.Linq;
using System.Reflection;
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
	public class SelectFromCellTypeColumn : SelectorColumn
	{
		public string propertyName;

		public Type cellType;

		public SelectFromCellTypeColumn (string propertyName, Type cellType, string title, params TableColumnOption[] options) : base (title, options)
		{
			this.propertyName = propertyName;
			this.cellType = cellType;
		}

		public override TableCell GetCell (SerializedProperty elementProperty)
		{
			if (!cellType.IsSubclassOf (typeof (TableCell)))
			{
				Debug.LogErrorFormat ("Type {0} is not a TableCell type", cellType);
				return null;
			}
			ConstructorInfo c = cellType.GetConstructors ().FirstOrDefault (ci => ci.GetParameters ().Length == 1 && ci.GetParameters ()[0].ParameterType == typeof (SerializedProperty));
			if (c == default (ConstructorInfo))
			{
				Debug.LogErrorFormat ("Type {0} doesn't have a constructor taking a SerializedProperty as parameter.", cellType);
				return null;
			}
			return (TableCell) c.Invoke (new object[] {elementProperty.FindPropertyRelative (propertyName)});
		}
	}

}
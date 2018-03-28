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
	/// This class represents a column that will draw Property Entries from the given property name, 
	/// relative to the collection element's serialized property.
	/// </summary>
	public class SelectFromEntryTypeColumn : SelectorColumn
	{
		public string propertyName;

		public Type entryType;

		public SelectFromEntryTypeColumn (string propertyName, Type entryType, string title, params TableColumnOption[] options) : base (title, options)
		{
			this.propertyName = propertyName;
			this.entryType = entryType;
		}

		public override TableEntry GetEntry (SerializedProperty elementProperty)
		{
			if (!entryType.IsSubclassOf (typeof (TableEntry)))
			{
				Debug.LogErrorFormat ("Type {0} is not a TableEntry type", entryType);
				return null;
			}
			ConstructorInfo c = entryType.GetConstructors ().FirstOrDefault (ci => ci.GetParameters ().Length == 1 && ci.GetParameters ()[0].ParameterType == typeof (SerializedProperty));
			if (c == default (ConstructorInfo))
			{
				Debug.LogErrorFormat ("Type {0} doesn't have a constructor taking a SerializedProperty as parameter.", entryType);
				return null;
			}
			return (TableEntry) c.Invoke (new object[] {elementProperty.FindPropertyRelative (propertyName)});
		}
	}

}
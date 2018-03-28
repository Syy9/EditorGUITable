using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// Base class for all table entries.
	/// DrawEntry needs to be overriden to draw the entry for the cell.
	/// Use this to customize the table however needed.
	/// CompareTo can be overriden to customize the sorting.
	/// comparingValue is used as a fallback for sorting any types of entries, even different types.
	/// </summary>
	public abstract class TableEntry : System.IComparable
	{
		/// <summary>
		/// Draws the entry using GUILayout.
		/// </summary>
		/// <param name="width">Width.</param>
		/// <param name="height">Height.</param>
		public abstract void DrawEntryLayout (float width, float height);

		/// <summary>
		/// Draws the entry using GUI (without GUILayout).
		/// </summary>
		/// <param name="rect">Rect.</param>
		public abstract void DrawEntry (Rect rect);

		public abstract string comparingValue { get; }

		public virtual int CompareTo (object other) 
		{ 
			TableEntry otherEntry = (TableEntry) other;
			if (otherEntry == null)
				throw new ArgumentException ("Object is not a TableEntry");
			return comparingValue.CompareTo ( otherEntry.comparingValue );
		}

		public static string GetPropertyValueAsString (SerializedProperty sp)
		{
			if (sp != null)
			{
				switch (sp.propertyType)
				{
					case SerializedPropertyType.String:
					case SerializedPropertyType.Character:
						return sp.stringValue.ToString ();
					case SerializedPropertyType.Float:
						return sp.doubleValue.ToString ();
					case SerializedPropertyType.Integer:
					case SerializedPropertyType.LayerMask:
					case SerializedPropertyType.ArraySize:
						return sp.intValue.ToString ();
					case SerializedPropertyType.Enum:
						return sp.enumValueIndex.ToString ();
					case SerializedPropertyType.Boolean:
						return sp.boolValue.ToString ();
					case SerializedPropertyType.ObjectReference:
						return (sp.objectReferenceValue == null) ? "" : sp.objectReferenceValue.name.ToString ();
					case SerializedPropertyType.ExposedReference:
						return (sp.exposedReferenceValue == null) ? "" : sp.exposedReferenceValue.name.ToString ();
				}
			}
			return "";
		}

	}

}

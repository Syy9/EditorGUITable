using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// Base class for all table cells.
	/// DrawCell needs to be overriden to draw the cell.
	/// Use this to customize the table however needed.
	/// CompareTo can be overriden to customize the sorting.
	/// comparingValue is used as a fallback for sorting any types of cells, even different types.
	/// </summary>
	public abstract class TableCell : System.IComparable
	{
		
		/// <summary>
		/// Draws the cell using GUI (without GUILayout).
		/// </summary>
		/// <param name="rect">Rect.</param>
		public abstract void DrawCell (Rect rect);

		public abstract string comparingValue { get; }

		public virtual int CompareTo (object other) 
		{ 
			TableCell otherCell = (TableCell) other;
			if (otherCell == null)
				throw new ArgumentException ("Object is not a TableCell");
			return comparingValue.CompareTo ( otherCell.comparingValue );
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

		public static int CompareTwoSerializedProperties (SerializedProperty sp1, SerializedProperty sp2)
		{

			if (sp1 == null && sp2 == null)
				return 0;
			else if (sp2 == null)
				return 1;
			else if (sp1 == null)
				return -1;

			if (sp1.propertyType != sp2.propertyType)
				return GetPropertyValueAsString (sp1).CompareTo(GetPropertyValueAsString (sp2));
			
			switch (sp1.propertyType)
			{
				case SerializedPropertyType.String:
				case SerializedPropertyType.Character:
					return sp1.stringValue.CompareTo (sp2.stringValue);
				case SerializedPropertyType.Float:
					return sp1.doubleValue.CompareTo (sp2.doubleValue);
				case SerializedPropertyType.Integer:
				case SerializedPropertyType.LayerMask:
				case SerializedPropertyType.ArraySize:
					return sp1.intValue.CompareTo (sp2.intValue);
				case SerializedPropertyType.Enum:
					return sp1.enumValueIndex.CompareTo (sp2.enumValueIndex);
				case SerializedPropertyType.Boolean:
					return sp1.boolValue.CompareTo (sp2.boolValue);
				case SerializedPropertyType.ObjectReference:
					return ((sp1.objectReferenceValue == null) ? "" : sp1.objectReferenceValue.name).CompareTo ((sp2.objectReferenceValue == null) ? "" : sp2.objectReferenceValue.name);
				case SerializedPropertyType.ExposedReference:
					return ((sp1.exposedReferenceValue == null) ? "" : sp1.exposedReferenceValue.name).CompareTo ((sp2.exposedReferenceValue == null) ? "" : sp2.exposedReferenceValue.name);
			}
			return 0;
		}

	}

}

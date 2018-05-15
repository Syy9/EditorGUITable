using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This cell class just uses EditorGUILayout.PropertyField to draw a given property.
	/// This is the basic way to use GUITable. It will draw the properties the same way Unity would by default.
	/// </summary>
	public class PropertyCell : TableCell
	{
		SerializedProperty sp;
		SerializedObject so;
		string propertyPath;

		public override void DrawCell (Rect rect)
		{
			if (sp != null)
			{
				EditorGUI.PropertyField (rect, sp, GUIContent.none);
				so.ApplyModifiedProperties ();
			}
			else
			{
				Debug.LogWarningFormat ("Property not found: {0} -> {1}", so.targetObject.name, propertyPath);
			}
		}

		public override string comparingValue
		{
			get
			{
				return GetPropertyValueAsString (sp);
			}
		}

		public override int CompareTo (object other)
		{
			
			TableCell otherCell = (TableCell) other;
			if (otherCell == null)
				throw new ArgumentException ("Object is not a TableCell");

			PropertyCell otherPropCell = (PropertyCell) other;
			if (otherPropCell == null)
				return base.CompareTo(other);

			SerializedProperty otherSp = otherPropCell.sp;

			return CompareTwoSerializedProperties (sp, otherSp);
		}

		public PropertyCell (SerializedProperty property)
		{
			this.sp = property;
			this.so = property.serializedObject;
			this.propertyPath = property.propertyPath;
		}

		public PropertyCell (SerializedObject so, string propertyPath)
		{
			this.so = so;
			this.propertyPath = propertyPath;
			this.sp = so.FindProperty(propertyPath);
		}

	}

}

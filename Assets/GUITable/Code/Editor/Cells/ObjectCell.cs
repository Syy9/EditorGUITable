using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This entry class just uses EditorGUILayout.ObjectField to draw a given property.
	/// </summary>
	public class ObjectCell : TableCell
	{
		SerializedProperty sp;
		SerializedObject so;
		string propertyPath;

		public override void DrawCell (Rect rect)
		{
			if (sp != null)
			{
				EditorGUI.ObjectField (rect, sp, GUIContent.none);
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

		public ObjectCell (SerializedProperty property)
		{
			this.sp = property;
			this.so = property.serializedObject;
			this.propertyPath = property.propertyPath;
		}

		public ObjectCell (SerializedObject so, string propertyPath)
		{
			this.so = so;
			this.propertyPath = propertyPath;
			this.sp = so.FindProperty(propertyPath);
		}

	}

}

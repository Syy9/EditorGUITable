using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This entry class just uses EditorGUILayout.PropertyField to draw a given property.
	/// This is the basic way to use GUITable. It will draw the properties the same way Unity would by default.
	/// </summary>
	public class PropertyEntry : TableEntry
	{
		SerializedProperty sp;
		SerializedObject so;
		string propertyPath;

		public override void DrawEntryLayout (float width, float height)
		{
			if (sp != null)
			{
				EditorGUILayout.PropertyField (sp, GUIContent.none, GUILayout.Width (width), GUILayout.Height (height));
				so.ApplyModifiedProperties ();
			}
			else
			{
				Debug.LogWarningFormat ("Property not found: {0} -> {1}", so.targetObject.name, propertyPath);
				GUILayout.Space (width + 4f);
			}
		}

		public override void DrawEntry (Rect rect)
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

		public PropertyEntry (SerializedProperty property)
		{
			this.sp = property;
			this.so = property.serializedObject;
			this.propertyPath = property.propertyPath;
		}

		public PropertyEntry (SerializedObject so, string propertyPath)
		{
			this.so = so;
			this.propertyPath = propertyPath;
			this.sp = so.FindProperty(propertyPath);
		}

	}

}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{

	/// <summary>
	/// This entry class just uses EditorGUILayout.PropertyField to draw a given property.
	/// This is the basic way to use GUITable. It will draw the properties the same way Unity would by default.
	/// </summary>
	public class PropertyEntry : TableEntry
	{
		SerializedObject so;
		string propertyName;

		public override void DrawEntry (float width, float height)
		{
			SerializedProperty sp = so.FindProperty (propertyName);
			if (sp != null)
			{
				EditorGUILayout.PropertyField (sp, GUIContent.none, GUILayout.Width (width), GUILayout.Height (height));
				so.ApplyModifiedProperties ();
			}
			else
			{
				Debug.LogWarningFormat ("Property not found: {0} -> {1}", so.targetObject.name, propertyName);
				GUILayout.Space (width + 4f);
			}
		}

		public override string comparingValue
		{
			get
			{
				SerializedProperty sp = so.FindProperty (propertyName);
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
							return sp.objectReferenceValue.name.ToString ();
						case SerializedPropertyType.ExposedReference:
							return sp.exposedReferenceValue.name.ToString ();
					}
				}
				return "";
			}
		}

		public override int CompareTo (object other)
		{
			TableEntry otherEntry = (TableEntry) other;
			if (otherEntry == null)
				throw new ArgumentException ("Object is not a GUITableEntry");

			PropertyEntry otherPropEntry = (PropertyEntry) other;
			if (otherPropEntry == null)
				return base.CompareTo(other);

			SerializedProperty sp = so.FindProperty (propertyName);
			SerializedProperty otherSp = otherPropEntry.so.FindProperty (otherPropEntry.propertyName);

			if (sp.propertyType != otherSp.propertyType)
				return base.CompareTo(other);

			if (sp != null)
			{

				switch (sp.propertyType)
				{
					case SerializedPropertyType.String:
					case SerializedPropertyType.Character:
						return sp.stringValue.CompareTo (otherSp.stringValue);
					case SerializedPropertyType.Float:
						return sp.doubleValue.CompareTo (otherSp.doubleValue);
					case SerializedPropertyType.Integer:
					case SerializedPropertyType.LayerMask:
					case SerializedPropertyType.ArraySize:
						return sp.intValue.CompareTo (otherSp.intValue);
					case SerializedPropertyType.Enum:
						return sp.enumValueIndex.CompareTo (otherSp.enumValueIndex);
					case SerializedPropertyType.Boolean:
						return sp.boolValue.CompareTo (otherSp.boolValue);
					case SerializedPropertyType.ObjectReference:
						return sp.objectReferenceValue.name.CompareTo (otherSp.objectReferenceValue.name);
					case SerializedPropertyType.ExposedReference:
						return sp.exposedReferenceValue.name.CompareTo (otherSp.exposedReferenceValue.name);
				}
			}
			return 0;
		}

		public PropertyEntry (SerializedObject so, string propertyName)
		{
			this.so = so;
			this.propertyName = propertyName;
		}
	}

}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Text.RegularExpressions;

namespace EditorGUITable
{

	/// <summary>
	/// Drawer for the Table Attribute.
	/// See the TableAttribute class documentation for the limitations of this attribute.
	/// </summary>
	[CustomPropertyDrawer(typeof(TableAttribute))]
	public class TableDrawer : PropertyDrawer
	{

		protected GUITableState tableState;

		Rect lastRect;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			return EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			TableAttribute tableAttribute = (TableAttribute) attribute;

			//Check that it is a collection
			Match match = Regex.Match(property.propertyPath, "^([a-zA-Z0-9_]*).Array.data\\[([0-9]*)\\]$");
			if (!match.Success)
			{
				EditorGUI.LabelField(position, label.text, "Use the Table attribute with a collection.");
				return;
			}

			string collectionPath = match.Groups[1].Value;

			// Check that it's the first element
			string index = match.Groups[2].Value;

			if (index != "0")
				return;
			// Sometimes GetLastRect returns 0, so we keep the last relevant value
			if (GUILayoutUtility.GetLastRect().width > 1f)
				lastRect = GUILayoutUtility.GetLastRect();

			SerializedProperty collectionProperty = property.serializedObject.FindProperty(collectionPath);

			EditorGUI.indentLevel = 0;

			Rect r = new Rect(position.x + 15f, position.y, position.width - 15f, lastRect.height);

			tableState = DrawTable (r, collectionProperty, label, tableAttribute);
		}

		/// <summary>
		/// Full Version Only
		/// </summary>
		protected virtual GUITableState DrawTable (Rect rect, SerializedProperty collectionProperty, GUIContent label, TableAttribute tableAttribute)
		{
			return null;
		}
		

	}

}
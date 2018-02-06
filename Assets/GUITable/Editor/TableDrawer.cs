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

		GUITableState tableState;

		Rect lastRect;

		public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
		{
			//Check that it is a collection
			Match match = Regex.Match(property.propertyPath, "^([a-zA-Z0-9_]*).Array.data\\[([0-9]*)\\]$");
			if (!match.Success)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			// Check that it's the first element
			string index = match.Groups[2].Value;

			if (index != "0")
				return EditorGUIUtility.singleLineHeight;
			
			return 2 * EditorGUIUtility.singleLineHeight;
		}

		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			TableAttribute tableAttribute = (TableAttribute) attribute;
			if (tableAttribute.properties == null)
			{
				OnGUIAuto (position, property, label);
			}
			else
			{
				OnGUIPropList (position, property, label, tableAttribute.properties);
			}
		}

		void OnGUIAuto(Rect position, SerializedProperty property, GUIContent label)
		{
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
			EditorGUI.indentLevel = 0;
			if (GUILayoutUtility.GetLastRect().width > 1f)
				lastRect = GUILayoutUtility.GetLastRect();
			Rect r = new Rect(position.x + 15f, position.y, position.width, lastRect.height);
			tableState = GUITable.DrawTable(r, tableState, property.serializedObject.FindProperty(collectionPath), GUITableOption.AllowScrollView(false));
		}

		void OnGUIPropList(Rect position, SerializedProperty property, GUIContent label, string[] properties)
		{
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
			EditorGUI.indentLevel = 0;
			if (GUILayoutUtility.GetLastRect().width > 1f)
				lastRect = GUILayoutUtility.GetLastRect();
			Rect r = new Rect(position.x + 15f, position.y, position.width, lastRect.height);
			tableState = GUITable.DrawTable(r, tableState, property.serializedObject.FindProperty(collectionPath), properties.ToList(), GUITableOption.AllowScrollView(false));
		}

	}

}
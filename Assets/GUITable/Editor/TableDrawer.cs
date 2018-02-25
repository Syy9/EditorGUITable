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
			DrawTable (position, property, label, tableAttribute);
		}

		void DrawTable (Rect position, SerializedProperty property, GUIContent label, TableAttribute tableAttribute)
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
			if (tableAttribute.properties == null && tableAttribute.widths == null)
				tableState = GUITable.DrawTable(r, tableState, property.serializedObject.FindProperty(collectionPath), GUITableOption.AllowScrollView(false));
			else if (tableAttribute.widths == null)
				tableState = GUITable.DrawTable(r, tableState, property.serializedObject.FindProperty(collectionPath), tableAttribute.properties.ToList(), GUITableOption.AllowScrollView(false));
			else
				tableState = GUITable.DrawTable(r, tableState, property.serializedObject.FindProperty(collectionPath), GetPropertyColumns(tableAttribute), GUITableOption.AllowScrollView(false));
		}

		static List<PropertyColumn> GetPropertyColumns (TableAttribute tableAttribute)
		{
			List<PropertyColumn> res = new List<PropertyColumn>();
			for (int i = 0 ; i < tableAttribute.properties.Length ; i++)
			{
				if (i >= tableAttribute.widths.Length)
					res.Add(new PropertyColumn(tableAttribute.properties[i], tableAttribute.properties[i]));
				else
					res.Add(new PropertyColumn(tableAttribute.properties[i], tableAttribute.properties[i], TableColumn.Width(tableAttribute.widths[i])));
			}
			return res;
		}

	}

}
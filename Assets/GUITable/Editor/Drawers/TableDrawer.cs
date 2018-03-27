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
			//Check that it is a collection
			Match match = Regex.Match(property.propertyPath, "^([a-zA-Z0-9_]*).Array.data\\[([0-9]*)\\]$");
			if (!match.Success)
			{
				return EditorGUIUtility.singleLineHeight;
			}

			// Check that it's the first element
			string index = match.Groups[2].Value;

			if (index != "0")
				return EditorGUIUtility.singleLineHeight + 2;
			
			return EditorGUIUtility.singleLineHeight + 2 + GetRequiredAdditionalHeight ();
		}

		protected virtual float GetRequiredAdditionalHeight ()
		{
			return 1f * EditorGUIUtility.singleLineHeight;
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

		protected virtual GUITableState DrawTable (Rect rect, SerializedProperty collectionProperty, GUIContent label, TableAttribute tableAttribute)
		{
			if (tableAttribute.properties == null && tableAttribute.widths == null)
				return GUITable.DrawTable(rect, tableState, collectionProperty, GUITableOption.AllowScrollView(false));
			else if (tableAttribute.widths == null)
				return GUITable.DrawTable(rect, tableState, collectionProperty, tableAttribute.properties.ToList(), GUITableOption.AllowScrollView(false));
			else
				return GUITable.DrawTable(rect, tableState, collectionProperty, GetPropertyColumns(tableAttribute), GUITableOption.AllowScrollView(false));
		}

		protected static List<SelectorColumn> GetPropertyColumns (TableAttribute tableAttribute)
		{
			List<SelectorColumn> res = new List<SelectorColumn>();
			for (int i = 0 ; i < tableAttribute.properties.Length ; i++)
			{
				if (i >= tableAttribute.widths.Length)
					res.Add(new SelectFromPropertyNameColumn(tableAttribute.properties[i], tableAttribute.properties[i]));
				else
					res.Add(new SelectFromPropertyNameColumn(tableAttribute.properties[i], tableAttribute.properties[i], TableColumn.Width(tableAttribute.widths[i])));
			}
			return res;
		}

	}

}
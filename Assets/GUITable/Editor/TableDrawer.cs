using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;
using System.Text.RegularExpressions;

/// <summary>
/// Drawer for the Table Attribute.
/// See the TableAttribute class documentation for the limitations of this attribute.
/// </summary>
[CustomPropertyDrawer(typeof(TableAttribute))]
public class TableDrawer : PropertyDrawer
{

	GUITableState tableState;

	Rect lastRect;

	public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
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

		if (GUILayoutUtility.GetLastRect().width > 1f)
			lastRect = GUILayoutUtility.GetLastRect();
		Rect r = new Rect(lastRect.x + 15f, lastRect.y + 35f, lastRect.width, lastRect.height);
		GUILayout.BeginArea(r);
		EditorGUI.indentLevel = 0;
		tableState = GUITable.DrawTable(property.serializedObject.FindProperty(collectionPath), tableState);
		GUILayout.EndArea();
		GUILayout.Space(30f);
	}
}

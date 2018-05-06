using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

public class CustomColumnsWindow : EditorWindow 
{

	SerializedObject serializedObject;

	GUITableState tableState;


	void OnEnable ()
	{
		tableState = new GUITableState("tableState3");
	}

	void OnGUI () 
	{

		GUILayout.Label ("Customize the columns (right-click to hide optional columns)", EditorStyles.boldLabel);

		DrawCustomColumns ();

	}

	void DrawCustomColumns ()
	{
		SerializedObject serializedObject = new SerializedObject(SimpleExample.Instance);
		List<SelectorColumn> propertyColumns = new List<SelectorColumn>()
		{
			new SelectFromPropertyNameColumn("stringProperty", "String", TableColumn.Width(60f)),
			new SelectFromPropertyNameColumn("floatProperty", "Float", TableColumn.Width(50f), TableColumn.Optional(true)),
			new SelectFromPropertyNameColumn("objectProperty", "Object", TableColumn.Width(50f), TableColumn.EnabledTitle(false), TableColumn.Optional(true)),
		};

		tableState = GUITableLayout.DrawTable (tableState, serializedObject.FindProperty("simpleObjects"), propertyColumns);
	}

}

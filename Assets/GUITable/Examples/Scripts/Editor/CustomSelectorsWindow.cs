using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

public class CustomSelectorsWindow : EditorWindow 
{

	SerializedObject serializedObject;

	GUITableState tableState;


	void OnEnable ()
	{
		tableState = new GUITableState("tableState4");
	}

	void OnGUI () 
	{

		GUILayout.Label ("Customize the columns and the selector function for entries", EditorStyles.boldLabel);

		DrawCustomColumnsWithSelector ();

	}

	void DrawCustomColumnsWithSelector ()
	{
		SerializedObject serializedObject = new SerializedObject(SimpleExample.Instance);

		List<SelectorColumn> selectorColumns = new List<SelectorColumn>()
		{
			new SelectorColumn(prop => new LabelEntry(prop.stringValue), "stringProperty", "String", TableColumn.Width(60f)),
			new SelectorColumn(prop => new LabelEntry(prop.floatValue.ToString()), "floatProperty", "Float", TableColumn.Width(50f), TableColumn.Optional(true)),
			new SelectorColumn(prop => new LabelEntry(prop.objectReferenceValue.name), "objectProperty", "Object", TableColumn.Width(110f), TableColumn.EnabledTitle(false), TableColumn.Optional(true)),
		};

		tableState = GUITableLayout.DrawTable (tableState, serializedObject.FindProperty("simpleObjects"), selectorColumns);
	}

}

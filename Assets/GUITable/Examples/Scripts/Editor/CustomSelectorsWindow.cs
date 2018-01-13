using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

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
			new SelectorColumn(prop => new LabelEntry(prop.stringValue), "stringProperty", "String", 60f),
			new SelectorColumn(prop => new LabelEntry(prop.floatValue.ToString()), "floatProperty", "Float", 50f) {optional = true},
			new SelectorColumn(prop => new LabelEntry(prop.objectReferenceValue.name), "objectProperty", "Object", 110f) {enabledTitle = false, optional = true},
		};

		tableState = GUITable.DrawTable (serializedObject.FindProperty("simpleObjects"), selectorColumns, tableState);
	}

}

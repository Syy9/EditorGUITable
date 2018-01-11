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

	public static CustomSelectorsWindow ShowWindow (SerializedObject serializedObject)
	{
		CustomSelectorsWindow window = EditorWindow.GetWindow<CustomSelectorsWindow>();
		window.serializedObject = serializedObject;
		window.Show();
		return window;
	}


	void DrawCustomColumnsWithSelector ()
	{

		List<SelectorColumn> selectorColumns = new List<SelectorColumn>()
		{
			new SelectorColumn(prop => new LabelEntry(prop.stringValue), "stringProperty", "String", 60f),
			new SelectorColumn(prop => new LabelEntry(prop.floatValue.ToString()), "floatProperty", "Float", 50f) {optional = true},
			new SelectorColumn(prop => new LabelEntry(prop.objectReferenceValue.name), "objectProperty", "Object", 110f) {enabledTitle = false, optional = true},
		};

		tableState = GUITable.DrawTable (selectorColumns, serializedObject, "simpleObjects", tableState);
	}

}

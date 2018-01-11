using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

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

	public static CustomColumnsWindow ShowWindow (SerializedObject serializedObject)
	{
		CustomColumnsWindow window = EditorWindow.GetWindow<CustomColumnsWindow>();
		window.serializedObject = serializedObject;
		window.Show();
		return window;
	}

	void DrawCustomColumns ()
	{
		List<PropertyColumn> propertyColumns = new List<PropertyColumn>()
		{
			new PropertyColumn("stringProperty", "String", 60f),
			new PropertyColumn("floatProperty", "Float", 50f) {optional = true},
			new PropertyColumn("objectProperty", "Object", 110f) {enabledTitle = false, optional = true},
		};

		tableState = GUITable.DrawTable (serializedObject.FindProperty("simpleObjects"), propertyColumns, tableState);
	}

}

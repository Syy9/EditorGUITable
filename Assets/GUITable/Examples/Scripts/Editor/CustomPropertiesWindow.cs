using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

public class CustomPropertiesWindow : EditorWindow 
{

	SerializedObject serializedObject;

	GUITableState tableState;


	void OnEnable ()
	{
		tableState = new GUITableState("tableState2");
	}

	void OnGUI () 
	{

		GUILayout.Label ("Customize the properties to display", EditorStyles.boldLabel);

		DrawCustomProperties ();

	}

	public static CustomPropertiesWindow ShowWindow (SerializedObject serializedObject)
	{
		CustomPropertiesWindow window = EditorWindow.GetWindow<CustomPropertiesWindow>();
		window.serializedObject = serializedObject;
		window.Show();
		return window;
	}


	void DrawCustomProperties ()
	{
		tableState = GUITable.DrawTable (serializedObject.FindProperty("simpleObjects"), new List<string>(){"floatProperty", "objectProperty"}, tableState);
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

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

	void DrawCustomProperties ()
	{
		SerializedObject serializedObject = new SerializedObject(SimpleExample.Instance);
		tableState = GUITableLayout.DrawTable (tableState, serializedObject.FindProperty("simpleObjects"), new List<string>(){"floatProperty", "objectProperty"});
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(SimpleExample))]
public class SimpleExampleEditor : Editor 
{

	GUITableState tableState;

	bool reorderable = true;

	void OnEnable ()
	{
		tableState = new GUITableState("tableState");
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Default display", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (serializedObject.FindProperty("simpleObjects"), true);

		GUILayout.Space (20f);

		GUILayout.Label ("Table display", EditorStyles.boldLabel);
		DrawObjectsTable ();
	}

	void DrawSimple ()
	{
		reorderable = EditorGUILayout.Toggle ("Reorderable", reorderable);
		tableState = GUITableLayout.DrawTable (
			tableState, 
			serializedObject.FindProperty("simpleObjects"), 
			GUITableOption.Reorderable(reorderable));
	}

	void DrawObjectsTable ()
	{

		GUILayout.Label ("Simply Display the Whole list (click to sort, drag to resize)", EditorStyles.boldLabel);

		DrawSimple ();

		GUILayout.Space (20f);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Customize the properties", EditorStyles.boldLabel, GUILayout.Width(170f));
		if (GUILayout.Button("Window Example", GUILayout.Width (120f)))
			EditorWindow.GetWindow<CustomPropertiesWindow>().Show();
		GUILayout.EndHorizontal ();

		GUILayout.Space (10f);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Customize the columns", EditorStyles.boldLabel, GUILayout.Width(170f));
		if (GUILayout.Button("Window Example", GUILayout.Width (120f)))
			EditorWindow.GetWindow<CustomColumnsWindow>().Show();
		GUILayout.EndHorizontal ();

		GUILayout.Space (10f);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Customize the selectors", EditorStyles.boldLabel, GUILayout.Width(170f));
		if (GUILayout.Button("Window Example", GUILayout.Width (120f)))
			EditorWindow.GetWindow<CustomSelectorsWindow>().Show();
		GUILayout.EndHorizontal ();

		GUILayout.Space (10f);

		GUILayout.BeginHorizontal ();
		GUILayout.Label ("Customize the cells", EditorStyles.boldLabel, GUILayout.Width(170f));
		if (GUILayout.Button("Window Example", GUILayout.Width (120f)))
			EditorWindow.GetWindow<CustomCellsWindow>().Show();
		GUILayout.EndHorizontal ();

		GUILayout.FlexibleSpace();

	}

}

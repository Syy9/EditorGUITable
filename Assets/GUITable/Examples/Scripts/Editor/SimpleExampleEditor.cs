using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(SimpleExample))]
public class SimpleExampleEditor : Editor 
{

	GUITableState tableState;

	public override void OnInspectorGUI ()
	{
		tableState = GUITableLayout.DrawTable (tableState, serializedObject.FindProperty("simpleObjects"));
	}

}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(LiteExample))]
public class LiteExampleEditor : Editor 
{

	GUITableState tableState;

	void OnEnable ()
	{
		tableState = new GUITableState("tableState_Lite");
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Default display", EditorStyles.boldLabel);

		base.OnInspectorGUI();

		GUILayout.Space (20f);

		GUILayout.Label ("Table display", EditorStyles.boldLabel);

		tableState = GUITableLayout.DrawTable (tableState, serializedObject.FindProperty ("simpleObjects"));

		GUILayout.Space (20f);

	}

}

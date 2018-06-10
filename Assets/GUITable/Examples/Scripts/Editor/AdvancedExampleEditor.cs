using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(AdvancedExample))]
public class AdvancedExampleEditor : Editor 
{

	GUITableState tableState;

	void OnEnable ()
	{
		tableState = new GUITableState("tableState_Advanced");
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Default display", EditorStyles.boldLabel);

		base.OnInspectorGUI();

		GUILayout.Space (20f);

		GUILayout.Label ("Table display", EditorStyles.boldLabel);

		if (GUILayout.Button("Show Window"))
		{
			AdvancedExampleWindow.Init();
		}
	}

}

public class SpawnersCell : TableCell
{

	SerializedProperty sp;
	SerializedObject so;

	public override void DrawCell (Rect rect)
	{
		sp.intValue = EditorGUI.MaskField (rect, sp.intValue, AdvancedExample.Instance.spawners.Select(s => s.name).ToArray());
		so.ApplyModifiedProperties();
	}

	public override string comparingValue {
		get {
			return string.Empty;
		}
	}

	public SpawnersCell (SerializedObject so, string propertyName)
	{
		sp = so.FindProperty(propertyName);
		this.so = so;
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ActionExample))]
public class ActionExampleEditor : Editor 
{

	GUITableState tableState = new GUITableState();

	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Default display", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (serializedObject.FindProperty("simpleObjects"), true);
		GUILayout.Space (10f);
		GUILayout.Label ("Table display", EditorStyles.boldLabel);
		DrawObjectsTable ();
	}

	void DrawObjectsTable ()
	{

		List<GUITableColumn> columns = new List<GUITableColumn>()
		{
			new GUITableColumn("String", 60f),
			new GUITableColumn("Float", 50f),
			new GUITableColumn("Object", 110f),
			new GUITableColumn("", 50f) { enabledTitle = false },
		};

		List<List<GUITableEntry>> rows = new List<List<GUITableEntry>>();

		ActionExample targetObject = (ActionExample) target;

		for (int i = 0 ; i < targetObject.simpleObjects.Count ; i++)
		{
			ActionExample.SimpleObject entry = targetObject.simpleObjects[i];
			rows.Add (new List<GUITableEntry>()
			{
				new LabelEntry (entry.stringProperty),
				new PropertyEntry (serializedObject, string.Format("simpleObjects.Array.data[{0}].floatProperty", i)),
				new PropertyEntry (serializedObject, string.Format("simpleObjects.Array.data[{0}].objectProperty", i)),
				new ActionEntry ("Reset", () => entry.Reset() ),
			});
		}

		tableState = GUITable.Table (columns, rows, tableState);

	}

}

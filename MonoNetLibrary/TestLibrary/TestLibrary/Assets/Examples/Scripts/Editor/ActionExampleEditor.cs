using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

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

		List<TableColumn> columns = new List<TableColumn>()
		{
			new TableColumn("String", TableColumn.Width(60f)),
			new TableColumn("Float", TableColumn.Width(50f)),
			new TableColumn("Object", TableColumn.Width(110f)),
			new TableColumn("", TableColumn.Width(50f), TableColumn.EnabledTitle(false))
		};

		List<List<TableCell>> rows = new List<List<TableCell>>();

		ActionExample targetObject = (ActionExample) target;

		for (int i = 0 ; i < targetObject.simpleObjects.Count ; i++)
		{
			ActionExample.SimpleObject entry = targetObject.simpleObjects[i];
			rows.Add (new List<TableCell>()
			{
				new LabelCell (entry.stringProperty),
				new PropertyCell (serializedObject, string.Format("simpleObjects.Array.data[{0}].floatProperty", i)),
				new PropertyCell (serializedObject, string.Format("simpleObjects.Array.data[{0}].objectProperty", i)),
				new ActionCell ("Reset", () => entry.Reset() ),
			});
		}

		tableState = GUITableLayout.DrawTable (tableState, columns, rows);

	}

}

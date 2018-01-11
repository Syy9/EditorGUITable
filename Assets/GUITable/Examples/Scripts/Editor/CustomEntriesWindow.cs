using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

public class CustomEntriesWindow : EditorWindow 
{

	SerializedObject serializedObject;

	GUITableState tableState;


	void OnEnable ()
	{
		tableState = new GUITableState("tableState5");
	}

	void OnGUI () 
	{

		GUILayout.Label ("Customize the entries", EditorStyles.boldLabel);

		DrawCustomEntries ();

	}

	void DrawCustomEntries ()
	{
		SerializedObject serializedObject = new SerializedObject(SimpleExample.Instance);
		
		List<TableColumn> columns = new List<TableColumn>()
		{
			new TableColumn("String", 60f),
			new TableColumn("Float", 50f),
			new TableColumn("Object", 110f),
			new TableColumn("", 100f) {enabledTitle = false},
		};

		List<List<TableEntry>> rows = new List<List<TableEntry>>();

		SimpleExample targetObject = (SimpleExample) serializedObject.targetObject;

		for (int i = 0 ; i < targetObject.simpleObjects.Count ; i++)
		{
			SimpleExample.SimpleObject entry = targetObject.simpleObjects[i];
			rows.Add (new List<TableEntry>()
			{
				new LabelEntry (entry.stringProperty),
				new PropertyEntry (serializedObject, string.Format("simpleObjects.Array.data[{0}].floatProperty", i)),
				new PropertyEntry (serializedObject, string.Format("simpleObjects.Array.data[{0}].objectProperty", i)),
				new ActionEntry ("Reset", () => entry.Reset ()),
			});
		}

		tableState = GUITable.DrawTable (columns, rows, tableState);
	}

}

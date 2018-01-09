using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

[CustomEditor(typeof(SimpleExample))]
public class SimpleExampleEditor : Editor 
{

	GUITableState tableState;
	GUITableState tableState2;
	GUITableState tableState3;
	GUITableState tableState4;

	Vector2 scrollPos;

	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Default display", EditorStyles.boldLabel);
		EditorGUILayout.PropertyField (serializedObject.FindProperty("simpleObjects"), true);

		GUILayout.Space (10f);

		GUILayout.Label ("Table display", EditorStyles.boldLabel);
		DrawObjectsTable ();

		GUILayout.Space (10f);

		if (GUILayout.Button ("Draw in a Window"))
		{

		}
	}

	void DrawSimple ()
	{
		tableState = GUITable.DrawTable (serializedObject, "simpleObjects", tableState);
	}

	void DrawCustomProperties ()
	{
		tableState2 = GUITable.DrawTable (serializedObject, "simpleObjects", new List<string>(){"floatProperty", "objectProperty"}, tableState2);
	}

	void DrawCustomColumns ()
	{
		List<GUITable.PropertyColumn> propertyColumns = new List<GUITable.PropertyColumn>()
		{
			new GUITable.PropertyColumn("stringProperty", new GUITableColumn("String", 60f)),
			new GUITable.PropertyColumn("floatProperty", new GUITableColumn("Float", 50f) {optional = true}),
			new GUITable.PropertyColumn("objectProperty", new GUITableColumn("Object", 110f) {enabledTitle = false, optional = true}),
		};

		tableState3 = GUITable.DrawTable (propertyColumns, serializedObject, "simpleObjects", tableState3);
	}

	void DrawCustomEntries ()
	{
		List<GUITableColumn> columns = new List<GUITableColumn>()
		{
			new GUITableColumn("String", 60f),
			new GUITableColumn("Float", 50f),
			new GUITableColumn("Object", 110f),
			new GUITableColumn("", 100f) {enabledTitle = false},
		};

		List<List<GUITableEntry>> rows = new List<List<GUITableEntry>>();

		SimpleExample targetObject = (SimpleExample) target;

		for (int i = 0 ; i < targetObject.simpleObjects.Count ; i++)
		{
			SimpleExample.SimpleObject entry = targetObject.simpleObjects[i];
			rows.Add (new List<GUITableEntry>()
			{
				new LabelEntry (entry.stringProperty),
				new PropertyEntry (serializedObject, string.Format("simpleObjects.Array.data[{0}].floatProperty", i)),
				new PropertyEntry (serializedObject, string.Format("simpleObjects.Array.data[{0}].objectProperty", i)),
				new ActionEntry ("Reset", () => entry.Reset ()),
			});
		}

		tableState4 = GUITable.DrawTable (columns, rows, tableState4);
	}

	void DrawObjectsTable ()
	{

		GUILayout.Label ("Simply Display the Whole list (click to sort, drag to resize)", EditorStyles.boldLabel);

		DrawSimple ();

		GUILayout.Label ("Select the properties to display", EditorStyles.boldLabel);

		DrawCustomProperties ();

		GUILayout.Label ("Customize the columns (right-click to hide optional columns)", EditorStyles.boldLabel);

		DrawCustomColumns ();

		GUILayout.Label ("Customize the entries", EditorStyles.boldLabel);

		DrawCustomEntries ();

	}

}

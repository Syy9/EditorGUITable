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
	GUITableState tableState5;

	Vector2 scrollPos;

	void OnEnable ()
	{
		tableState = new GUITableState("tableState");
		tableState2 = new GUITableState("tableState2");
		tableState3 = new GUITableState("tableState3");
		tableState4 = new GUITableState("tableState4");
		tableState5 = new GUITableState("tableState5");
	}

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
		tableState = GUITable.DrawTable (serializedObject.FindProperty("simpleObjects"), tableState);
	}

	void DrawCustomProperties ()
	{
		tableState2 = GUITable.DrawTable (serializedObject.FindProperty("simpleObjects"), new List<string>(){"floatProperty", "objectProperty"}, tableState2);
	}

	void DrawCustomColumns ()
	{
		List<PropertyColumn> propertyColumns = new List<PropertyColumn>()
		{
			new PropertyColumn("stringProperty", "String", 60f),
			new PropertyColumn("floatProperty", "Float", 50f) {optional = true},
			new PropertyColumn("objectProperty", "Object", 110f) {enabledTitle = false, optional = true},
		};

		tableState3 = GUITable.DrawTable (serializedObject.FindProperty("simpleObjects"), propertyColumns, tableState3);
	}

	void DrawCustomColumnsWithSelector ()
	{

		List<SelectorColumn> selectorColumns = new List<SelectorColumn>()
		{
			new SelectorColumn(prop => new LabelEntry(prop.stringValue), "stringProperty", "String", 60f),
			new SelectorColumn(prop => new LabelEntry(prop.floatValue.ToString()), "floatProperty", "Float", 50f) {optional = true},
			new SelectorColumn(prop => new LabelEntry(prop.objectReferenceValue.name), "objectProperty", "Object", 110f) {enabledTitle = false, optional = true},
		};

		tableState4 = GUITable.DrawTable (selectorColumns, serializedObject, "simpleObjects", tableState4);
	}

	void DrawCustomEntries ()
	{
		List<TableColumn> columns = new List<TableColumn>()
		{
			new TableColumn("String", 60f),
			new TableColumn("Float", 50f),
			new TableColumn("Object", 110f),
			new TableColumn("", 100f) {enabledTitle = false},
		};

		List<List<TableEntry>> rows = new List<List<TableEntry>>();

		SimpleExample targetObject = (SimpleExample) target;

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

		tableState5 = GUITable.DrawTable (columns, rows, tableState5);
	}

	void DrawObjectsTable ()
	{

		GUILayout.Label ("Simply Display the Whole list (click to sort, drag to resize)", EditorStyles.boldLabel);

		DrawSimple ();

		GUILayout.Label ("Select the properties to display", EditorStyles.boldLabel);

		DrawCustomProperties ();

		GUILayout.Label ("Customize the columns (right-click to hide optional columns)", EditorStyles.boldLabel);

		DrawCustomColumns ();

		GUILayout.Label ("Customize the columns and the selector function for entries", EditorStyles.boldLabel);

		DrawCustomColumnsWithSelector ();

		GUILayout.Label ("Customize the entries", EditorStyles.boldLabel);

		DrawCustomEntries ();

	}

}

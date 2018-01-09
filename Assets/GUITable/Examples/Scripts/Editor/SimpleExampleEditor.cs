using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

[CustomEditor(typeof(SimpleExample))]
public class SimpleExampleEditor : Editor 
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
			});
		}

		tableState = GUITable.Table (columns, rows, tableState);

		List<GUITable.PropertyColumn> propertyColumns = new List<GUITable.PropertyColumn>()
		{
			new GUITable.PropertyColumn("stringProperty", new GUITableColumn("String", 60f)),
			new GUITable.PropertyColumn("floatProperty", new GUITableColumn("Float", 50f)),
			new GUITable.PropertyColumn("objectProperty", new GUITableColumn("Object", 110f)),
		};

		tableState = GUITable.Table (propertyColumns, serializedObject, "simpleObjects", tableState);

		tableState = GUITable.Table (serializedObject, "simpleObjects", new List<string>(){"stringProperty", "floatProperty", "objectProperty"}, tableState);

	}

}

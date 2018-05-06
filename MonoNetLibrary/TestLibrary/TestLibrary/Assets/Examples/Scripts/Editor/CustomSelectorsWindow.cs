using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

public class CustomSelectorsWindow : EditorWindow 
{

	SerializedObject serializedObject;

	GUITableState tableState;


	void OnEnable ()
	{
		tableState = new GUITableState("tableState4");
	}

	void OnGUI () 
	{

		GUILayout.Label ("Customize the columns and the selector function for cells", EditorStyles.boldLabel);

		DrawCustomColumnsWithSelector ();

	}

	void DrawCustomColumnsWithSelector ()
	{
		SerializedObject serializedObject = new SerializedObject(SimpleExample.Instance);

		List<SelectorColumn> selectorColumns = new List<SelectorColumn>()
		{
			new SelectFromFunctionColumn(prop => new LabelCell(prop.FindPropertyRelative("stringProperty").stringValue), "String", TableColumn.Width(60f)),
			new SelectFromFunctionColumn(prop => new LabelCell(prop.FindPropertyRelative("floatProperty").floatValue.ToString()), "Float", TableColumn.Width(50f), TableColumn.Optional(true)),
			new SelectFromFunctionColumn(prop => new LabelCell(prop.FindPropertyRelative("objectProperty").objectReferenceValue.name), "Object", TableColumn.Width(110f), TableColumn.EnabledTitle(false), TableColumn.Optional(true)),
		};

		tableState = GUITableLayout.DrawTable (tableState, serializedObject.FindProperty("simpleObjects"), selectorColumns);
	}

}

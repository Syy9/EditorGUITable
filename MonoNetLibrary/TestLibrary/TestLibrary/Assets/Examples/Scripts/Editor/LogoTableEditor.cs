using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

[CustomEditor(typeof(LogoTable))]
public class LogoTableEditor : Editor 
{

	GUITableState tableState;

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI();
		SerializedObject serializedObject = new SerializedObject(LogoTable.Instance);

		List<TableColumn> columns = new List<TableColumn>()
		{
			new TableColumn("G", 28f),
			new TableColumn("U", 22f),
			new TableColumn("I     ", 35f),
		};

		List<List<TableCell>> rows = new List<List<TableCell>>();

		LogoTable targetObject = LogoTable.Instance;

		for (int i = 0 ; i < targetObject.logoLines.Count ; i++)
		{
			rows.Add (new List<TableCell>()
			{
				new PropertyCell (serializedObject, string.Format("logoLines.Array.data[{0}].letter1", i)),
				new PropertyCell (serializedObject, string.Format("logoLines.Array.data[{0}].letter2", i)),
				new PropertyCell (serializedObject, string.Format("logoLines.Array.data[{0}].color", i)),
			});
		}

		tableState = GUITableLayout.DrawTable (tableState, columns, rows);
	}

}

using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

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

		List<List<TableEntry>> rows = new List<List<TableEntry>>();

		LogoTable targetObject = LogoTable.Instance;

		for (int i = 0 ; i < targetObject.logoLines.Count ; i++)
		{
			rows.Add (new List<TableEntry>()
			{
				new PropertyEntry (serializedObject, string.Format("logoLines.Array.data[{0}].letter1", i)),
				new PropertyEntry (serializedObject, string.Format("logoLines.Array.data[{0}].letter2", i)),
				new PropertyEntry (serializedObject, string.Format("logoLines.Array.data[{0}].color", i)),
			});
		}

		tableState = GUITable.DrawTable (columns, rows, tableState);
	}

}

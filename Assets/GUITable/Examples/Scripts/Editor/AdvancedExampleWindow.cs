using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using EditorGUITable;

public class AdvancedExampleWindow : EditorWindow 
{

	SerializedObject serializedObject;

	GUITableState tableState;


	void OnEnable ()
	{
//		tableState = new GUITableState("tableState2");
	}

	public static void Init ()
	{
		AdvancedExampleWindow window = EditorWindow.GetWindow<AdvancedExampleWindow>();
		window.position = new Rect(Screen.currentResolution.width / 2 - 300, Screen.currentResolution.height / 2 - 200, 600, 400);
		window.titleContent = new GUIContent("Enemies Window");
		window.Show();
	}

	void OnGUI () 
	{
		EditorStyles.boldLabel.alignment = TextAnchor.MiddleCenter;
		GUILayout.Label ("Enemies", EditorStyles.boldLabel);

		DrawObjectsTable ();

	}
	void DrawObjectsTable ()
	{

		SerializedObject serializedObject = new SerializedObject(AdvancedExample.Instance);

		List<TableColumn> columns = new List<TableColumn>()
		{
			new TableColumn("Name", TableColumn.Width(60f)),
			new TableColumn("Prefab", TableColumn.Width(50f), TableColumn.EnabledCells(false), TableColumn.Optional(true)),
			new TableColumn("Type", TableColumn.Width(50f), TableColumn.Optional(true)),
			new TableColumn("Health", TableColumn.Width(50f)),
			new TableColumn("Speed", TableColumn.Width(50f)),
			new TableColumn("Color", TableColumn.Width(50f), TableColumn.Optional(true)),
			new TableColumn("Can Swim", TableColumn.Width(30f), TableColumn.Optional(true)),
			new TableColumn("Spawners", TableColumn.Width(450f), TableColumn.Optional(true)),
			new TableColumn("Intro (shared by type)", TableColumn.Width(110f), TableColumn.Optional(true)),
			new TableColumn("Instantiation", TableColumn.Width(110f), TableColumn.Optional(true))
		};

		List<List<TableCell>> rows = new List<List<TableCell>>();

		AdvancedExample targetObject = (AdvancedExample) serializedObject.targetObject;

		for (int i = 0 ; i < targetObject.enemies.Count ; i++)
		{
			Enemy enemy = targetObject.enemies[i];
			int sentenceIndex = targetObject.introSentences.FindIndex(s => s.enemyType == enemy.type);
			rows.Add (new List<TableCell>()
			{
				new LabelCell (enemy.name),
				new PropertyCell (serializedObject, string.Format("enemies.Array.data[{0}]", i)),
				new PropertyCell (new SerializedObject(enemy), "type"),
				new PropertyCell (new SerializedObject(enemy), "health"),
				new PropertyCell (new SerializedObject(enemy), "speed"),
				new PropertyCell (new SerializedObject(enemy), "color"),
				new PropertyCell (new SerializedObject(enemy), "canSwim"),
				new SpawnersCell (new SerializedObject(enemy), "spawnersMask"),
				new PropertyCell (serializedObject, string.Format("introSentences.Array.data[{0}].sentence", sentenceIndex)),
				new ActionCell ("Instantiate", () => enemy.Instantiate ()),
			});
		}

		tableState = GUITableLayout.DrawTable (tableState, columns, rows);
	}

}

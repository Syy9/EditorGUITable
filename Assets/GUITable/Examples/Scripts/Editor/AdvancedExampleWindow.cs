using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

public class AdvancedExampleWindow : EditorWindow 
{

	SerializedObject serializedObject;

	GUITableState tableState;


	void OnEnable ()
	{
		tableState = new GUITableState("tableState2");
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
			new TableColumn("Name", 60f),
			new TableColumn("Prefab", 50f){enabledEntries = false, optional = true},
			new TableColumn("Type", 50f) {optional = true},
			new TableColumn("Health", 50f),
			new TableColumn("Speed", 50f),
			new TableColumn("Color", 50f) {optional = true},
			new TableColumn("Can Swim", 30f) {optional = true},
			new TableColumn("Spawners", 450f) {optional = true},
			new TableColumn("Intro (shared by type)", 110f) {optional = true},
			new TableColumn("Instantiation", 110f) {optional = true}
		};

		List<List<TableEntry>> rows = new List<List<TableEntry>>();

		AdvancedExample targetObject = (AdvancedExample) serializedObject.targetObject;

		for (int i = 0 ; i < targetObject.enemies.Count ; i++)
		{
			Enemy enemy = targetObject.enemies[i];
			int sentenceIndex = targetObject.introSentences.FindIndex(s => s.enemyType == enemy.type);
			rows.Add (new List<TableEntry>()
			{
				new LabelEntry (enemy.name),
				new PropertyEntry (serializedObject, string.Format("enemies.Array.data[{0}]", i)),
				new PropertyEntry (new SerializedObject(enemy), "type"),
				new PropertyEntry (new SerializedObject(enemy), "health"),
				new PropertyEntry (new SerializedObject(enemy), "speed"),
				new PropertyEntry (new SerializedObject(enemy), "color"),
				new PropertyEntry (new SerializedObject(enemy), "canSwim"),
				new SpawnersEntry (new SerializedObject(enemy), "spawnersMask"),
				new PropertyEntry (serializedObject, string.Format("introSentences.Array.data[{0}].sentence", sentenceIndex)),
				new ActionEntry ("Instantiate", () => enemy.Instantiate ()),
			});
		}

		tableState = GUITable.DrawTable (columns, rows, tableState);
	}

}

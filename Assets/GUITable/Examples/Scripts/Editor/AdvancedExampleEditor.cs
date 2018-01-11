using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using GUIExtensions;

[CustomEditor(typeof(AdvancedExample))]
public class AdvancedExampleEditor : Editor 
{

	GUITableState tableState;

	void OnEnable ()
	{
		tableState = new GUITableState("tableState_Advanced");
	}

	public override void OnInspectorGUI ()
	{
		GUILayout.Label ("Default display", EditorStyles.boldLabel);

		base.OnInspectorGUI();

//		EditorGUILayout.PropertyField (serializedObject.FindProperty("enemies"), true);
//		serializedObject.ApplyModifiedProperties();

		GUILayout.Space (20f);

		GUILayout.Label ("Table display", EditorStyles.boldLabel);
		DrawObjectsTable ();
	}

	void DrawObjectsTable ()
	{

		SerializedObject serializedObject = new SerializedObject(AdvancedExample.Instance);

		List<TableColumn> columns = new List<TableColumn>()
		{
			new TableColumn("Name", 60f),
			new TableColumn("Prefab", 50f),
			new TableColumn("Type", 50f),
			new TableColumn("Health", 110f),
			new TableColumn("Speed", 110f),
			new TableColumn("Color", 110f),
			new TableColumn("Swims", 110f),
			new TableColumn("Spawners", 110f),
			new TableColumn("Intro Sentence", 110f),
			new TableColumn("Instantiation", 110f)
		};

		List<List<TableEntry>> rows = new List<List<TableEntry>>();

		AdvancedExample targetObject = (AdvancedExample) serializedObject.targetObject;

		for (int i = 0 ; i < targetObject.enemies.Count ; i++)
		{
			Enemy entry = targetObject.enemies[i];
			rows.Add (new List<TableEntry>()
			{
				new LabelEntry (entry.name),
				new PropertyEntry (serializedObject, string.Format("enemies.Array.data[{0}]", i)),
				new PropertyEntry (serializedObject, string.Format("enemies.Array.data[{0}].type", i)),
				new PropertyEntry (serializedObject, string.Format("enemies.Array.data[{0}].health", i)),
				new PropertyEntry (serializedObject, string.Format("enemies.Array.data[{0}].speed", i)),
				new PropertyEntry (serializedObject, string.Format("enemies.Array.data[{0}].color", i)),
				new PropertyEntry (serializedObject, string.Format("enemies.Array.data[{0}].canSwim", i)),
				new SpawnersEntry (),
				new PropertyEntry (serializedObject, string.Format("introSentences.Array.data[{0}].sentence", 0)),
				new ActionEntry ("Instantiate", () => entry.Instantiate ()),
			});
		}

		tableState = GUITable.DrawTable (columns, rows, tableState);

	}

}

public class EnemyTypeEntry : TableEntry
{

	EnemyType enemyType;

	public EnemyTypeEntry (EnemyType enemyType)
	{
		this.enemyType = enemyType;
	}

	public override void DrawEntry (float width, float height)
	{
		throw new System.NotImplementedException ();
	}

	public override string comparingValue {
		get {
			return enemyType.ToString();
		}
	}
}

public class SpawnersEntry : TableEntry
{

	public override void DrawEntry (float width, float height)
	{
		EditorGUILayout.MaskField (0, AdvancedExample.Instance.spawners.Select(s => s.name).ToArray(), GUILayout.Width(width), GUILayout.Height(height));
	}

	public override string comparingValue {
		get {
			return string.Empty;
		}
	}
}

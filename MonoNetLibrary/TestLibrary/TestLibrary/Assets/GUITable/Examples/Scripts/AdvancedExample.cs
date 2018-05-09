using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AdvancedExample : MonoBehaviour {

	private static AdvancedExample instance = null;
	public static AdvancedExample Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<AdvancedExample>();
			return instance;
		}
	}

	public List<Enemy> enemies;

	[System.Serializable]
	public class IntroSentence
	{
		public EnemyType enemyType;
		public string sentence;
	}
	[System.Serializable]
	public class Spawner
	{
		public string name;
		public Vector2 position;
	}

	public List<IntroSentence> introSentences = new List<IntroSentence>();

	public List<Spawner> spawners = new List<Spawner>();

	void OnGUI ()
	{
		GUILayout.Label ("Select the AdvancedExample scene object to visualize the table in the editor");
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleExample : MonoBehaviour {

	private static SimpleExample instance = null;
	public static SimpleExample Instance
	{
		get
		{
			if (instance == null)
				instance = FindObjectOfType<SimpleExample>();
			return instance;
		}
	}

	[System.Serializable]
	public class SimpleObject
	{
		public string stringProperty;
		public float floatProperty;
		public GameObject objectProperty;
		public void Reset ()
		{
			stringProperty = "";
			floatProperty = 0f;
			objectProperty = null;
		}
	}

	public List<SimpleObject> simpleObjects;

	void OnGUI ()
	{
		GUILayout.Label ("Select the SimpleExample scene object to visualize the table in the inspector");
	}

}

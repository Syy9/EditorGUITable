using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LiteExample : MonoBehaviour 
{

	[System.Serializable]
	public class SimpleObject
	{
		public string stringProperty;
		public float floatProperty;
		public GameObject objectProperty;
	}

	public List<SimpleObject> simpleObjects;

	void OnGUI ()
	{
		GUILayout.Label ("Select the LiteExample scene object to visualize the table in the inspector");
	}

}

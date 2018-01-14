using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GUIExtensions;


public class PropertyAttributeExample : MonoBehaviour {
	
	[System.Serializable]
	public class SimpleObject
	{
		public string stringProperty;
		public float floatProperty;
		public GameObject objectProperty;
		public Vector2 v2Property;
	}

	[Table]
	public List<SimpleObject> simpleObjects;

	void OnGUI ()
	{
		GUILayout.Label ("Select the PropertyAttribute scene object to visualize the table in the inspector");
	}

}

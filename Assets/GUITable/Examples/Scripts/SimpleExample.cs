using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleExample : MonoBehaviour {
	
	[System.Serializable]
	public class SimpleObject
	{
		public string stringProperty;
		public float floatProperty;
		public GameObject objectProperty;
	}

	public List<SimpleObject> simpleObjects;
	

}

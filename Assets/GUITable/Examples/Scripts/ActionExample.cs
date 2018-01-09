using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ActionExample : MonoBehaviour {
	
	[System.Serializable]
	public class SimpleObject
	{
		public string stringProperty;
		public float floatProperty;
		public GameObject objectProperty;

		public void Reset ()
		{
			this.stringProperty = "";
			this.floatProperty = 0f;
			this.objectProperty = null;
		}
	}

	public List<SimpleObject> simpleObjects;
	

}

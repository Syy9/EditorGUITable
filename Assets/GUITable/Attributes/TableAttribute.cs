using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorGUITable
{

	/// <summary>
	/// Attribute that automatically draws a collection as a table
	/// 
	/// Example:
	/// 
	/// <code>
	///public class TableAttributeExample : MonoBehaviour {
	///
	///	[System.Serializable]
	///	public class SimpleObject
	///	{
	///		public string stringProperty;
	///		public float floatProperty;
	///	}
	///
	///	[TableAttribute]
	///	public List<SimpleObject> simpleObjects;
	///
	///}
	/// 
	/// </code>
	/// 
	/// </summary>
	public class TableAttribute : PropertyAttribute
	{

		public string[] properties = null;

		public TableAttribute () 
		{
		}

		public TableAttribute (params string[] properties)
		{
			this.properties = properties;
		}

	}

}
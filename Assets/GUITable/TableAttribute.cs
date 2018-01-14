using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUIExtensions
{

	/// <summary>
	/// Attribute that automatically draws a collection as a table
	/// 
	/// IMPORTANT NOTE:
	/// Because of the way Unity works internally, all code using GUILayout
	/// will not work in a Property Drawer, unless the containing MonoBehaviour has 
	/// a CustomEditor. GUITable uses GUILayout, so you need to add a CustomEditor to
	/// the MonoBehaviour in which you want to use the Table Attribute.
	/// 
	/// Example:
	/// 
	/// public class TableAttributeExample : MonoBehaviour {
	///
	///[System.Serializable]
	///public class SimpleObject
	///{
	///	public string stringProperty;
	///	public float floatProperty;
	///}
	///
	///[TableAttribute]
	///public List<SimpleObject> simpleObjects;
	///
	///}
	/// 
	/// This will not work unless you have this class in an Editor folder:
	/// 
	/// using UnityEditor;
	///[CustomEditor(typeof(TableAttributeExample))]
	///public class TableAttributeExampleEditor : Editor { }
	/// 
	/// You don't need anything in this class, it just needs to be here.
	/// 
	/// </summary>
	public class TableAttribute : PropertyAttribute
	{

	}

}
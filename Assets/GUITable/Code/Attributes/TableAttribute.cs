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
		public float[] widths = null;

		/// <summary>
		/// This attribute will display the collection in a table, instead of the classic Unity list.
		/// </summary>
		public TableAttribute () 
		{
		}

		/// <summary>
		/// This attribute will display the collection's chosen properties in a table, instead of the classic Unity list.
		/// </summary>
		/// <param name="properties"> The properties to display in the table </param>
		public TableAttribute (params string[] properties)
		{
			this.properties = properties;
		}

		/// <summary>
		/// This attribute will display the collection's chosen properties in a table, with the chosen column sizes, instead of the classic Unity list.
		/// </summary>
		/// <param name="properties"> The properties to display in the table</param>
		/// <param name="widths"> The widths of the table's columns</param>
		public TableAttribute (string[] properties, float[] widths)
		{
			this.properties = properties;
			this.widths = widths;
		}

	}

}
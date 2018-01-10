using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{

	/// <summary>
	/// Internal Use Only.
	/// This class adds a property to a column.
	/// This will be used to automatically draw the entries for this column in some versions of <see cref="GUITable.DrawTable"/>
	/// </summary>
	public class PropertyColumn : TableColumn
	{
		public string propertyName;
		public PropertyColumn (string propertyName, string name, float width) : base (name, width)
		{
			this.propertyName = propertyName;
		}
	}

}
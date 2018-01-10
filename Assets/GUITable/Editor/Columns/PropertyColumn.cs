using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{

	public class PropertyColumn : TableColumn
	{
		public string propertyName;
		public PropertyColumn (string propertyName, string name, float width) : base (name, width)
		{
			this.propertyName = propertyName;
		}
	}

}
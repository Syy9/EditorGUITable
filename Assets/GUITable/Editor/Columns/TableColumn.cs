using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{

	public class TableColumn
	{
		public string name { get; private set; }
		public float width { get; private set; }
		public bool enabledEntries = true;
		public bool isSortable = true;
		public bool enabledTitle = true;
		public bool optional = false;
		public bool visibleByDefault = true;
		public TableColumn (string name, float width, bool enabled = true, bool isSortable = true, bool enabledTitle = true, bool optional = false, bool visibleByDefault = true)
		{
			this.name = name;
			this.width = width;
			this.enabledEntries = enabled;
			this.isSortable = isSortable;
			this.enabledTitle = enabledTitle;
			this.optional = optional;
			this.visibleByDefault = visibleByDefault;
		}
	}

}
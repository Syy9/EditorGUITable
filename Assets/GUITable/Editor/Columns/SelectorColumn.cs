using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{

	public class SelectorColumn : PropertyColumn
	{
		public Func<SerializedProperty, TableEntry> selector;
		public SelectorColumn (Func<SerializedProperty, TableEntry> selector, string propertyName, string name, float width) : base (propertyName, name, width)
		{
			this.selector = selector;
		}
	}

}
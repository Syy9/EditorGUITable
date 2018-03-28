using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This class represents a column that can automatically get an entry from an element's serialized property.
	/// It is abstract so there can be several ways to get this entry.
	/// </summary>
	public abstract class SelectorColumn : TableColumn
	{

		public SelectorColumn (string title, params TableColumnOption[] options) : base (title, options) {}

		public SelectorColumn (string title, float width, params TableColumnOption[] options) : base (title, width, options) {}

		public abstract TableEntry GetEntry (SerializedProperty elementProperty);

	}

}
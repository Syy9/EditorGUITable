using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This class adds a property and a selector to a column.
	/// This will be used to automatically draw the entries for this column in some versions of <see cref="GUITable.DrawTable"/>
	/// </summary>
	public abstract class SelectorColumn : TableColumn
	{

		public SelectorColumn (string title, params TableColumnOption[] options) : base (title, options) {}

		public SelectorColumn (string title, float width, params TableColumnOption[] options) : base (title, width, options) {}

		public abstract TableEntry GetEntry (SerializedProperty elementProperty);

	}

}
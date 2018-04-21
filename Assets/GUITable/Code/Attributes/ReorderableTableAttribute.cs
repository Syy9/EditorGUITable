using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EditorGUITable
{

	/// <summary>
	/// Attribute that automatically draws a collection as a table with the Reorderable option enabled
	/// </summary>
	public class ReorderableTableAttribute : TableAttribute
	{

		public ReorderableTableAttribute () {}

		public ReorderableTableAttribute (params string[] properties) : base (properties) {}

		public ReorderableTableAttribute (string[] properties, float[] widths) : base (properties, widths) {}

	}

}
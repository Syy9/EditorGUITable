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
		/// <summary>
		/// This attribute will display the collection in a reorderable table, instead of the classic Unity list.
		/// </summary>
		public ReorderableTableAttribute () {}

		/// <summary>
		/// This attribute will display the collection's chosen properties in a reorderable table, instead of the classic Unity list.
		/// </summary>
		/// <param name="properties"> The properties to display in the table </param>
		public ReorderableTableAttribute (params string[] properties) : base (properties) {}

		/// <summary>
		/// This attribute will display the collection's chosen properties in a reorderable table, with the chosen column sizes, instead of the classic Unity list.
		/// </summary>
		/// <param name="properties"> The properties to display in the table</param>
		/// <param name="widths"> The widths of the table's columns</param>
		public ReorderableTableAttribute (string[] properties, float[] widths) : base (properties, widths) {}

	}

}
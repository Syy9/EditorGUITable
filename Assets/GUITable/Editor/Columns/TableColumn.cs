using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{

	/// <summary>
	/// Base class for all table columns.
	/// It only takes a title and a width in the constructor, but other settings are available to customize the column.
	/// </summary>
	public class TableColumn
	{
		public string title { get; private set; }
		public float width { get; private set; }
		/// <summary>
		/// Defines if the entries are enabled (interactable) or disabled (grayed out). Default: true.
		/// </summary>
		public bool enabledEntries = true;
		/// <summary>
		/// Defines if the column is sortable.
		/// </summary>
		public bool isSortable = true;
		/// <summary>
		/// Defines if the title is enabled (interactable) or disabled (grayed out). Default: true.
		/// </summary>
		public bool enabledTitle = true;
		/// <summary>
		/// Defines if the column can be hidden by right-clicking the column titles bar. Default: false.
		/// </summary>
		public bool optional = false;
		/// <summary>
		/// Defines if the column is visible by default. If this is false, and optional is false too. The column can never be viewed. Default: true.
		/// </summary>
		public bool visibleByDefault = true;

		/// <summary>
		/// Initializes a column with its title and width.
		/// Edit the other public properties for more settings.
		/// </summary>
		/// <param name="title">The column title.</param>
		/// <param name="width">The column width.</param>
		public TableColumn (string title, float width)
		{
			this.title = title;
			this.width = width;
		}
	}

}
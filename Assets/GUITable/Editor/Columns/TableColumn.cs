using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// Base class for all table columns.
	/// It only takes a title and a width in the constructor, but other settings are available to customize the column.
	/// </summary>
	public class TableColumn
	{
		
		public string title { get; private set; }

		public TableColumnEntry entry;

		/// <summary>
		/// Initializes a column with its title and options.
		/// Edit the other public properties for more settings.
		/// </summary>
		/// <param name="title">The column title.</param>
		/// <param name="options">The column options.</param>
		public TableColumn (string title, params TableColumnOption[] options)
		{
			this.title = title;
			this.entry = new TableColumnEntry(options);
		}

		[System.Obsolete ("Use TableColumn(title, options) instead, with TableColumn.Width() to set the width")]
		public TableColumn (string title, float width) : this (title, TableColumn.Width(width))
		{
		}

		public static TableColumnOption ExpandWidth (bool enable)
		{
			return new TableColumnOption (TableColumnOption.Type.ExpandWidth, enable);
		}

		public static TableColumnOption MinWidth (float value)
		{
			return new TableColumnOption (TableColumnOption.Type.MinWidth, value);
		}

		public static TableColumnOption MaxWidth (float value)
		{
			return new TableColumnOption (TableColumnOption.Type.MaxWidth, value);
		}

		public static TableColumnOption Width (float value)
		{
			return new TableColumnOption (TableColumnOption.Type.Width, value);
		}

		public static TableColumnOption Resizeable (bool value)
		{
			return new TableColumnOption (TableColumnOption.Type.Resizeable, value);
		}

		public static TableColumnOption Sortable (bool value)
		{
			return new TableColumnOption (TableColumnOption.Type.Sortable, value);
		}

		public static TableColumnOption EnabledTitle (bool value)
		{
			return new TableColumnOption (TableColumnOption.Type.EnabledTitle, value);
		}

		public static TableColumnOption EnabledEntries (bool value)
		{
			return new TableColumnOption (TableColumnOption.Type.EnabledEntries, value);
		}

		public static TableColumnOption Optional (bool value)
		{
			return new TableColumnOption (TableColumnOption.Type.Optional, value);
		}

		public static TableColumnOption VisibleByDefault (bool value)
		{
			return new TableColumnOption (TableColumnOption.Type.VisibleByDefault, value);
		}
	}

}
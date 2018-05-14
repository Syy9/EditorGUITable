using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace EditorGUITable
{

	/// <summary>
	/// Main Class of the Table Plugin.
	/// This contains static functions to draw a table, from the most basic 
	/// to the most customizable, using GUILayout functions.
	/// </summary>
	public static class GUITableLayout
	{

		/// <summary>
		/// Draw a table just from the collection's property.
		/// This will create columns for all the visible members in the elements' class,
		/// similar to what Unity would show in the classic vertical collection display, but as a table instead.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="tableState">The Table state.</param>
		public static GUITableState DrawTable (
			GUITableState tableState,
			SerializedProperty collectionProperty)
		{
			List<string> properties = new List<string> ();
			string firstElementPath = collectionProperty.propertyPath + ".Array.data[0]";
			foreach (SerializedProperty prop in collectionProperty.serializedObject.FindProperty (firstElementPath))
			{
				string subPropName = prop.propertyPath.Substring (firstElementPath.Length + 1);
				// Avoid drawing properties more than 1 level deep
				if (!subPropName.Contains ("."))
					properties.Add (subPropName);
			}
			List<SelectorColumn> columns = properties.Select (prop => (SelectorColumn) new SelectFromPropertyNameColumn (
				 prop, ObjectNames.NicifyVariableName (prop))).ToList ();
			List<List<TableCell>> rows = new List<List<TableCell>> ();
			for (int i = 0 ; i < collectionProperty.arraySize ; i++)
			{
				SerializedProperty sp = collectionProperty.serializedObject.FindProperty (string.Format ("{0}.Array.data[{1}]", collectionProperty.propertyPath, i));
				List<TableCell> row = new List<TableCell> ();
				foreach (SelectorColumn col in columns)
				{
					row.Add (col.GetCell (sp));
				}
				rows.Add (row);
			}

			List<TableColumn> tableColumns = columns.Select (c => c as TableColumn).ToList ();
			GUITableEntry tableEntry = new GUITableEntry (new GUITableOption[] { });

			if (tableState == null)
			{
				tableState = new GUITableState ();
				tableState.CheckState (tableColumns, tableEntry, float.MaxValue);
			}

			Rect position = GUILayoutUtility.GetRect (
				tableEntry.allowScrollView ? Screen.width / EditorGUIUtility.pixelsPerPoint - 40 : tableState.totalWidth,
				(tableEntry.rowHeight + (tableEntry.reorderable ? 5 : 0)) * rows.Count + EditorGUIUtility.singleLineHeight * (tableEntry.reorderable ? 2 : 1));
			if (Event.current.type == EventType.Layout)
				return tableState;
			else
				return GUITable.DrawTable (position, tableState, tableColumns, rows, collectionProperty, new GUITableOption[] { });
		}
			

		#region FULL_VERSION


		/// <summary>
		/// Full Version Only.
		/// Draw a table just from the collection's property.
		/// This will create columns for all the visible members in the elements' class,
		/// similar to what Unity would show in the classic vertical collection display, but as a table instead.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="options">The Table options.</param>
		public static GUITableState DrawTable (
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			params GUITableOption[] options) 
		{
			List<string> properties = new List<string>();
			string firstElementPath = collectionProperty.propertyPath + ".Array.data[0]";
			foreach (SerializedProperty prop in collectionProperty.serializedObject.FindProperty(firstElementPath))
			{
				string subPropName = prop.propertyPath.Substring(firstElementPath.Length + 1);
				// Avoid drawing properties more than 1 level deep
				if (!subPropName.Contains("."))
					properties.Add (subPropName);
			}
			return DrawTable (tableState, collectionProperty, properties, SetDemoVersionOption (options));
		}

		/// <summary>
		/// Full Version Only.
		/// Draw a table using just the paths of the properties to display.
		/// This will create columns automatically using the property name as title, and will create
		/// PropertyCell instances for each element.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="properties">The paths (names) of the properties to display.</param>
		/// <param name="options">The Table options.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<string> properties, 
			params GUITableOption[] options) 
		{
			List<SelectorColumn> columns = properties.Select(prop => (SelectorColumn) new SelectFromPropertyNameColumn(
				prop, ObjectNames.NicifyVariableName (prop))).ToList();

			return DrawTable (tableState, collectionProperty, columns, SetDemoVersionOption (options));
		}

		/// <summary>
		/// Full Version Only.
		/// Draw a table from the columns' settings, the path for the corresponding properties and a selector function
		/// that takes a SerializedProperty and returns the TableCell to put in the corresponding cell.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="columns">The Selector Columns.</param>
		/// <param name="options">The Table options.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<SelectorColumn> columns, 
			params GUITableOption[] options) 
		{
			GUITableEntry tableEntry = new GUITableEntry (options);
			List<List<TableCell>> rows = new List<List<TableCell>>();
			for (int i = 0 ; i < collectionProperty.arraySize ; i++)
			{
				SerializedProperty sp = collectionProperty.serializedObject.FindProperty (string.Format ("{0}.Array.data[{1}]", collectionProperty.propertyPath, i));
				if (tableEntry.filter != null && !tableEntry.filter (sp))
					continue;
				List <TableCell> row = new List<TableCell>();
				foreach (SelectorColumn col in columns)
				{
					row.Add ( col.GetCell (sp));
				}
				rows.Add(row);
			}
			return DrawTable (tableState, columns.Select((col) => (TableColumn) col).ToList(), rows, collectionProperty, SetDemoVersionOption (options));
		}

		/// <summary>
		/// Full Version Only.
		/// Draw a table completely manually.
		/// Each cell has to be created and given as parameter in cells.
		/// A collectionProperty is needed for reorderable tables. Use an overload with a collectionProperty.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="cells">The Cells as a list of rows.</param>
		/// <param name="options">The Table options.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableCell>> cells, 
			params GUITableOption[] options)
		{
			return DrawTable (tableState, columns, cells, null, SetDemoVersionOption (options));
		}

		/// <summary>
		/// Full Version Only.
		/// Draw a table completely manually.
		/// Each cell has to be created and given as parameter in cells.
		/// The collectionProperty is only needed for reorderable tables.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="cells">The Cells as a list of rows.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="options">The Table options.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableCell>> cells, 
			SerializedProperty collectionProperty,
			params GUITableOption[] options)
		{
			GUITableEntry tableEntry = new GUITableEntry (options);

			if (tableState == null)
			{
				tableState = new GUITableState();
				tableState.CheckState(columns, tableEntry, float.MaxValue);
			}

			Rect position = GUILayoutUtility.GetRect(
				tableState.totalWidth, 
				(tableEntry.rowHeight + (tableEntry.reorderable ? 5 : 0)) * cells.Count + EditorGUIUtility.singleLineHeight * (tableEntry.reorderable ? 2 : 1));
			if (Event.current.type == EventType.Layout)
				return tableState;
			else
				return GUITable.DrawTable (position, tableState, columns, cells, collectionProperty, SetDemoVersionOption (options));
		}

		static GUITableOption[] SetDemoVersionOption (GUITableOption[] options)
		{
			if (!options.Any (o => o.type == GUITableOption.Type.DemoVersion))
				return options.Concat (new GUITableOption[] { GUITableOption.DemoVersion () }).ToArray ();
			else
				return options;
		}

		#endregion



	}

}

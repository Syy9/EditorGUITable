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
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			params GUITableOption[] options) 
		{
			string firstElementPath = "Array.data[0]";
			List<string> properties = new List<string>();
			SerializedProperty firstElement = collectionProperty.FindPropertyRelative (firstElementPath);
			if (firstElement.propertyType == SerializedPropertyType.ObjectReference)
			{
				SerializedProperty sp = new SerializedObject (firstElement.objectReferenceValue).GetIterator ();
				sp.Next (true);
				while (sp.NextVisible(false))
				{
					if (!sp.propertyPath.Contains(".") && sp.name != "m_Script")
					{
						properties.Add (sp.propertyPath);
					}
				}
			}
			else
			{
				foreach (SerializedProperty prop in firstElement)
				{
					string subPropName = prop.propertyPath.Substring(firstElementPath.Length + 1);
					// Avoid drawing properties more than 1 level deep
					if (!subPropName.Contains("."))
						properties.Add (subPropName);
				}
			}
			return DrawTable (tableState, collectionProperty, properties, options);
		}

		/// <summary>
		/// Draw a table using just the paths of the properties to display.
		/// This will create columns automatically using the property name as title, and will create
		/// PropertyCell instances for each element.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="properties">The paths (names) of the properties to display.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<string> properties, 
			params GUITableOption[] options) 
		{
			List<SelectorColumn> columns = properties.Select(prop => (SelectorColumn) new SelectFromPropertyNameColumn(
				prop, ObjectNames.NicifyVariableName (prop))).ToList();

			return DrawTable (tableState, collectionProperty, columns, options);
		}

		/// <summary>
		/// Draw a table from the columns' settings, the path for the corresponding properties and a selector function
		/// that takes a SerializedProperty and returns the TableCell to put in the corresponding cell.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="columns">The Selector Columns.</param>
		/// <param name="options">The table options.</param>
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
				SerializedProperty sp = collectionProperty.FindPropertyRelative (string.Format ("Array.data[{0}]", i));
				if (tableEntry.filter != null && !tableEntry.filter (sp))
					continue;
				List <TableCell> row = new List<TableCell>();
				foreach (SelectorColumn col in columns)
				{
					row.Add ( col.GetCell (sp));
				}
				rows.Add(row);
			}
			return DrawTable (tableState, columns.Select((col) => (TableColumn) col).ToList(), rows, collectionProperty, options);
		}

		/// <summary>
		/// Draw a table completely manually.
		/// Each cell has to be created and given as parameter in cells.
		/// A collectionProperty is needed for reorderable tables. Use an overload with a collectionProperty.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="cells">The Cells as a list of rows.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableCell>> cells, 
			params GUITableOption[] options)
		{
			return DrawTable (tableState, columns, cells, null, options);
		}

		/// <summary>
		/// Draw a table completely manually.
		/// Each cell has to be created and given as parameter in cells.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="cells">The Cells as a list of rows.</param>
		/// <param name="collectionProperty">The SerializeProperty of the collection. This is useful for reorderable tables.</param>
		/// <param name="options">The table options.</param>
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
				return GUITable.DrawTable (position, tableState, columns, cells, collectionProperty, options);
		}

	}
}

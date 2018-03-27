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
			return DrawTable (tableState, collectionProperty, properties, options);
		}

		/// <summary>
		/// Draw a table using just the paths of the properties to display.
		/// This will create columns automatically using the property name as title, and will create
		/// PropertyEntry instances for each element.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="properties">The paths (names) of the properties to display.</param>
		/// <param name="tableState">The Table state.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<string> properties, 
			params GUITableOption[] options) 
		{
			List<PropertyColumn> columns = properties.Select(prop => new PropertyColumn(
				prop, ObjectNames.NicifyVariableName (prop))).ToList();

			return DrawTable (tableState, collectionProperty, columns, options);
		}

		/// <summary>
		/// Draw a table by defining the columns's settings and the path of the corresponding properties.
		/// This will automatically create Property Entries using these paths.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="propertyColumns">The Property columns, that contain the columns properties and the corresponding property path.</param>
		/// <param name="tableState">The Table state.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<PropertyColumn> propertyColumns, 
			params GUITableOption[] options) 
		{
			return DrawTable (
				tableState, 
				collectionProperty,
				propertyColumns.Select ((col) => new SelectorColumn (sp => new PropertyEntry (sp.FindPropertyRelative (col.propertyName)), col.propertyName)).ToList (), 
				options);
		}

		/// <summary>
		/// Draw a table from the columns' settings, the path for the corresponding properties and a selector function
		/// that takes a SerializedProperty and returns the TableEntry to put in the corresponding cell.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="columns">The Selector Columns.</param>
		/// <param name="tableState">The Table state.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<SelectorColumn> columns, 
			params GUITableOption[] options) 
		{
			GUITableEntry tableEntry = new GUITableEntry (options);
			List<List<TableEntry>> rows = new List<List<TableEntry>>();
			for (int i = 0 ; i < collectionProperty.arraySize ; i++)
			{
				SerializedProperty sp = collectionProperty.serializedObject.FindProperty (string.Format ("{0}.Array.data[{1}]", collectionProperty.propertyPath, i));
				if (tableEntry.filter != null && !tableEntry.filter (sp))
					continue;
				List <TableEntry> row = new List<TableEntry>();
				foreach (SelectorColumn col in columns)
				{
					row.Add ( col.selector.Invoke (sp));
				}
				rows.Add(row);
			}
			return DrawTable (tableState, columns.Select((col) => (TableColumn) col).ToList(), rows, collectionProperty, options);
		}

		/// <summary>
		/// Draw a table completely manually.
		/// Each entry has to be created and given as parameter in entries.
		/// A collectionProperty is needed for reorderable tables. Use an overload with a collectionProperty.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="entries">The Entries as a list of rows.</param>
		/// <param name="tableState">The Table state.</param>
		public static GUITableState DrawTable ( 
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableEntry>> entries, 
			params GUITableOption[] options)
		{
			return DrawTable (tableState, columns, entries, null, options);
		}

		public static GUITableState DrawTable ( 
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableEntry>> entries, 
			SerializedProperty collectionProperty,
			params GUITableOption[] options)
		{
			GUITableEntry tableEntry = new GUITableEntry (options);
			Rect position = GUILayoutUtility.GetRect(
				tableState.totalWidth, 
				(EditorGUIUtility.singleLineHeight + (tableEntry.reorderable ? 5 : 0)) * (entries.Count + (tableEntry.reorderable ? 2 : 1)));
			if (Event.current.type == EventType.Layout)
				return tableState;
			else
				return GUITable.DrawTable (position, tableState, columns, entries, collectionProperty, options);
		}

	}
}

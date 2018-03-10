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
	/// to the most customizable.
	/// </summary>
	public static class GUITable
	{

		/// <summary>
		/// Draw a table just from the collection's property.
		/// This will create columns for all the visible members in the elements' class,
		/// similar to what Unity would show in the classic vertical collection display, but as a table instead.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
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
			return DrawTable (rect, tableState, collectionProperty, properties, options);
		}

		/// <summary>
		/// Draw a table using just the paths of the properties to display.
		/// This will create columns automatically using the property name as title, and will create
		/// PropertyEntry instances for each element.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="properties">The paths (names) of the properties to display.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<string> properties, 
			params GUITableOption[] options) 
		{
			List<PropertyColumn> columns = properties.Select(prop => new PropertyColumn(
				prop, ObjectNames.NicifyVariableName (prop))).ToList();

			return DrawTable (rect, tableState, collectionProperty, columns, options);
		}

		/// <summary>
		/// Draw a table by defining the columns's settings and the path of the corresponding properties.
		/// This will automatically create Property Entries using these paths.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="propertyColumns">The Property columns, that contain the columns properties and the corresponding property path.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<PropertyColumn> propertyColumns, 
			params GUITableOption[] options) 
		{
			
			List<List<TableEntry>> rows = new List<List<TableEntry>>();

			for (int i = 0 ; i < collectionProperty.arraySize ; i++)
			{
				List<TableEntry> row = new List<TableEntry>();
				foreach (PropertyColumn col in propertyColumns)
				{
					row.Add (new PropertyEntry (
						collectionProperty.serializedObject, 
						string.Format("{0}.Array.data[{1}].{2}", collectionProperty.propertyPath, i, col.propertyName)));
				}
				rows.Add(row);
			}
			return DrawTable (rect, tableState, propertyColumns.Select((col) => (TableColumn) col).ToList(), rows, collectionProperty, options);
		}

		/// <summary>
		/// Draw a table from the columns' settings, the path for the corresponding properties and a selector function
		/// that takes a SerializedProperty and returns the TableEntry to put in the corresponding cell.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="collectionProperty">The serialized property of the collection.</param>
		/// <param name="columns">The Selector Columns.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
			GUITableState tableState,
			SerializedProperty collectionProperty, 
			List<SelectorColumn> columns, 
			params GUITableOption[] options) 
		{
			
			List<List<TableEntry>> rows = new List<List<TableEntry>>();
			for (int i = 0 ; i < collectionProperty.arraySize ; i++)
			{
				List<TableEntry> row = new List<TableEntry>();
				foreach (SelectorColumn col in columns)
				{
					row.Add ( col.selector.Invoke ( collectionProperty.serializedObject.FindProperty( string.Format("{0}.Array.data[{1}].{2}", collectionProperty.propertyPath, i, col.propertyName))));
				}
				rows.Add(row);
			}

			return DrawTable (rect, tableState, columns.Select((col) => (TableColumn) col).ToList(), rows, collectionProperty, options);
		}

		/// <summary>
		/// Draw a table completely manually.
		/// Each entry has to be created and given as parameter in entries.
		/// A collectionProperty is needed for reorderable tables. Use an overload with a collectionProperty.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="entries">The Entries as a list of rows.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableEntry>> entries, 
			params GUITableOption[] options)
		{
			return DrawTable(rect, tableState, columns, entries, null, options);
		}

		// Used for ReorderableList's callbacks access
		static List<List<TableEntry>> orderedRows;
		static List<List<TableEntry>> staticEntries;

		/// <summary>
		/// Draw a table completely manually.
		/// Each entry has to be created and given as parameter in entries.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="entries">The Entries as a list of rows.</param>
		/// <param name="collectionProperty">The SerializeProperty of the collection. This is useful for reorderable tables.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableEntry>> entries, 
			SerializedProperty collectionProperty,
			params GUITableOption[] options)
		{
			
			GUITableEntry tableEntry = new GUITableEntry (options);

			if (tableState == null)
				tableState = new GUITableState();
			
			if (tableEntry.reorderable)
			{
				if (collectionProperty == null)
				{
					Debug.LogError ("The collection's serialized property is needed to draw a reorderable table.");
					return tableState;
				}

				staticEntries = entries;
			
				if (tableState.reorderableList == null)
				{
					ReorderableList list = new ReorderableList (
						collectionProperty.serializedObject, 
						collectionProperty,
						true, true, true, true);

					list.drawElementCallback = (Rect r, int index, bool isActive, bool isFocused) => {
						DrawLine (tableState, columns, orderedRows[index], r.xMin + (list.draggable ? 0 : 14), r.yMin, tableEntry.rowHeight);
					};

					list.drawHeaderCallback = (Rect r) => { 
						DrawHeaders(r, tableState, columns, r.xMin + 12, r.yMin); 
					};

					list.onRemoveCallback = (l) => 
					{
						l.serializedProperty.DeleteArrayElementAtIndex (staticEntries.IndexOf (orderedRows[l.index]));
					};

					tableState.SetReorderableList (list);
				}
			}
			
			tableState.CheckState(columns, tableEntry, rect.width);

			orderedRows = entries;
			if (tableState.sortByColumnIndex >= 0)
			{
				if (tableState.sortIncreasing)
					orderedRows = entries.OrderBy (row => row [tableState.sortByColumnIndex]).ToList();
				else
					orderedRows = entries.OrderByDescending (row => row [tableState.sortByColumnIndex]).ToList();
			}

			if (tableEntry.reorderable)
			{
				collectionProperty.serializedObject.Update();
				tableState.reorderableList.DoList(rect);
				collectionProperty.serializedObject.ApplyModifiedProperties();
				return tableState;
			}


			float rowHeight = tableEntry.rowHeight;

			float currentX = rect.x;
			float currentY = rect.y;

			bool displayScrollView = tableState.totalWidth > rect.width && tableEntry.allowScrollView;
			if (displayScrollView)
			{
				tableState.scrollPosHoriz = GUI.BeginScrollView (rect, tableState.scrollPosHoriz, new Rect(0f, 0f, tableState.totalWidth, rect.height));
				currentX = 0f;
				currentY = 0f;
			}

			tableState.RightClickMenu (columns, rect);

			DrawHeaders(rect, tableState, columns, currentX, currentY);

			GUI.enabled = true;

			currentY += rowHeight;

			foreach (List<TableEntry> row in orderedRows)
			{
				currentX = rect.x;
				DrawLine (tableState, columns, row, currentX, currentY, rowHeight);
				currentY += rowHeight;
			}

			GUI.enabled = true;

			if (displayScrollView)
			{
				GUI.EndScrollView ();
			}

			tableState.Save();

			return tableState;
		}

	

		public static void DrawHeaders (
			Rect rect,
			GUITableState tableState,
			List<TableColumn> columns,
			float currentX,
			float currentY)
		{
			for (int i = 0 ; i < columns.Count ; i++)
			{
				TableColumn column = columns[i];
				if (!tableState.columnVisible [i])
					continue;
				string columnName = column.title;
				if (tableState.sortByColumnIndex == i)
				{
					if (tableState.sortIncreasing)
						columnName += " " + '\u25B2'.ToString();
					else
						columnName += " " + '\u25BC'.ToString();
				}

				tableState.ResizeColumn (i, currentX, rect);

				GUI.enabled = column.entry.enabledTitle;

				if (GUI.Button(new Rect(currentX, currentY, tableState.columnSizes[i]+4, EditorGUIUtility.singleLineHeight), columnName, EditorStyles.miniButtonMid) && column.entry.isSortable)
				{
					if (tableState.sortByColumnIndex == i && tableState.sortIncreasing)
					{
						tableState.sortIncreasing = false;
					}
					else if (tableState.sortByColumnIndex == i && !tableState.sortIncreasing)
					{
						tableState.sortByColumnIndex = -1;
					}
					else
					{
						tableState.sortByColumnIndex = i;
						tableState.sortIncreasing = true;
					}
				}

				currentX += tableState.columnSizes[i] + 4f;
			}
		}

		public static void DrawLine (
			GUITableState tableState,
			List<TableColumn> columns,
			List<TableEntry> row, 
			float currentX,
			float currentY,
			float rowHeight)
		{

			for (int i = 0 ; i < row.Count ; i++)
			{
				if (i >= columns.Count)
				{
					Debug.LogWarning ("The number of entries in this row is more than the number of columns");
					continue;
				}
				if (!tableState.columnVisible [i])
					continue;
				TableColumn column = columns [i];
				TableEntry property = row[i];
				GUI.enabled = column.entry.enabledEntries;
				property.DrawEntry (new Rect(currentX, currentY, tableState.columnSizes[i], rowHeight));
				currentX += tableState.columnSizes[i] + 4f;
			}
		}

	}
}

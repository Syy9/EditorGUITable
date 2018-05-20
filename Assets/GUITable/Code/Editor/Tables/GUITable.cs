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

		static readonly Color TABLE_BG_COLOR = new Color (0.3f, 0.3f, 0.3f);

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

			List <string> properties = SerializationHelpers.GetElementsSerializedFields (collectionProperty);
			if (properties == null && collectionProperty.arraySize == 0)
			{
				DrawTable (
					rect,
					null, 
					new List<TableColumn> () 
				{
					new TableColumn (collectionProperty.displayName + "(properties unknown, add at least 1 element)", TableColumn.Sortable (false), TableColumn.Resizeable (false))
				}, 
					new List <List <TableCell>> (),
					collectionProperty, 
					options);
				return tableState;
			}
			return DrawTable (rect, tableState, collectionProperty, properties, options);
		}

		/// <summary>
		/// Draw a table using just the paths of the properties to display.
		/// This will create columns automatically using the property name as title, and will create
		/// PropertyCell instances for each element.
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
			List<SelectorColumn> columns = properties.Select(prop => (SelectorColumn)new SelectFromPropertyNameColumn(
				prop, ObjectNames.NicifyVariableName (prop))).ToList();

			return DrawTable (rect, tableState, collectionProperty, columns, options);
		}

		/// <summary>
		/// Draw a table from the columns' settings, the path for the corresponding properties and a selector function
		/// that takes a SerializedProperty and returns the TableCell to put in the corresponding cell.
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
			GUITableEntry tableEntry = new GUITableEntry (options);
			List<List<TableCell>> rows = new List<List<TableCell>>();
			for (int i = 0 ; i < collectionProperty.arraySize ; i++)
			{
				SerializedProperty sp = collectionProperty.FindPropertyRelative (SerializationHelpers.GetElementAtIndexRelativePath (i));
				if (tableEntry.filter != null && !tableEntry.filter (sp))
					continue;
				List<TableCell> row = new List<TableCell>();
				foreach (SelectorColumn col in columns)
				{
					row.Add ( col.GetCell (sp));
				}
				rows.Add(row);
			}

			return DrawTable (rect, tableState, columns.Select((col) => (TableColumn) col).ToList(), rows, collectionProperty, options);
		}

		/// <summary>
		/// Draw a table completely manually.
		/// Each cell has to be created and given as parameter in cells.
		/// A collectionProperty is needed for reorderable tables. Use an overload with a collectionProperty.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="cells">The Cells as a list of rows.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableCell>> cells, 
			params GUITableOption[] options)
		{
			return DrawTable(rect, tableState, columns, cells, null, options);
		}

		// Used for ReorderableList's callbacks access
		static List<List<TableCell>> orderedRows;
		static List<List<TableCell>> staticCells;

		/// <summary>
		/// Draw a table completely manually.
		/// Each cell has to be created and given as parameter in cells.
		/// </summary>
		/// <returns>The updated table state.</returns>
		/// <param name="rect">The table's containing rectangle.</param>
		/// <param name="tableState">The Table state.</param>
		/// <param name="columns">The Columns of the table.</param>
		/// <param name="cells">The Cells as a list of rows.</param>
		/// <param name="collectionProperty">The SerializeProperty of the collection. This is useful for reorderable tables.</param>
		/// <param name="options">The table options.</param>
		public static GUITableState DrawTable (
			Rect rect,
			GUITableState tableState,
			List<TableColumn> columns, 
			List<List<TableCell>> cells, 
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

				staticCells = cells;
			
				if (tableState.reorderableList == null)
				{
					ReorderableList list = new ReorderableList (
						collectionProperty.serializedObject, 
						collectionProperty,
						true, true, true, true);

					list.drawElementCallback = (Rect r, int index, bool isActive, bool isFocused) => {
						DrawLine (tableState, columns, orderedRows[index], r.xMin + (list.draggable ? 0 : 14), r.yMin, tableEntry.rowHeight);
					};

					list.elementHeight = tableEntry.rowHeight;

					list.drawHeaderCallback = (Rect r) => { 
						DrawHeaders(r, tableState, columns, r.xMin + 12, r.yMin); 
					};

					list.onRemoveCallback = (l) => 
					{
						l.serializedProperty.DeleteArrayElementAtIndex (staticCells.IndexOf (orderedRows[l.index]));
					};

					tableState.SetReorderableList (list);
				}
			}
			
			tableState.CheckState(columns, tableEntry, rect.width);

			orderedRows = cells;
			if (tableState.sortByColumnIndex >= 0)
			{
				if (tableState.sortIncreasing)
					orderedRows = cells.OrderBy (row => row [tableState.sortByColumnIndex]).ToList();
				else
					orderedRows = cells.OrderByDescending (row => row [tableState.sortByColumnIndex]).ToList();
			}

			if (tableEntry.reorderable)
			{
				tableState.reorderableList.DoList(new Rect (rect.x, rect.y, tableState.totalWidth + 23f, rect.height));
				collectionProperty.serializedObject.ApplyModifiedProperties ();
				return tableState;
			}


			float rowHeight = tableEntry.rowHeight;

			float currentX = rect.x;
			float currentY = rect.y + 5;

			bool displayScrollView = tableState.totalWidth > rect.width && tableEntry.allowScrollView;

			tableState.RightClickMenu (columns, rect);

			DrawHeaders(rect, tableState, columns, currentX - tableState.scrollPos.x, currentY);

			GUI.enabled = true;

			currentY += EditorGUIUtility.singleLineHeight;

			if (tableEntry.allowScrollView)
			{
				tableState.scrollPos = GUI.BeginScrollView (
					new Rect (currentX, currentY, rect.width, TableHeight (tableEntry, cells.Count)),
					tableState.scrollPos, 
					new Rect(0f, 0f, tableState.totalWidth, tableEntry.rowHeight * cells.Count));
				currentX = 0f;
				currentY = 0f;
			}

			if (orderedRows.Count == 0)
			{
				currentX = tableEntry.allowScrollView ? 0 : rect.x;
				GUIHelpers.DrawRect (new Rect (currentX, currentY, tableState.totalWidth, rowHeight), TABLE_BG_COLOR);
				GUI.Label (new Rect (currentX + 5, currentY, rect.width, rowHeight), "Collection is empty");
			}
			else
			{
				foreach (List<TableCell> row in orderedRows)
				{
					currentX = tableEntry.allowScrollView ? 0 : rect.x;
					DrawLine (tableState, columns, row, currentX, currentY, rowHeight);
					currentY += rowHeight;
				}
			}

			GUI.enabled = true;

			if (tableEntry.allowScrollView)
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

				GUI.enabled = true;

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
			List<TableCell> row, 
			float currentX,
			float currentY,
			float rowHeight)
		{

			for (int i = 0 ; i < row.Count ; i++)
			{
				if (i >= columns.Count)
				{
					Debug.LogWarning ("The number of cells in this row is more than the number of columns");
					continue;
				}
				if (!tableState.columnVisible [i])
					continue;
				TableColumn column = columns [i];
				TableCell property = row[i];
				GUI.enabled = column.entry.enabledCells;
				property.DrawCell (new Rect(currentX, currentY, tableState.columnSizes[i], rowHeight));
				currentX += tableState.columnSizes[i] + 4f;
			}
		}

		public static float TableHeight (GUITableEntry tableEntry, int nbRows)
		{
			return (tableEntry.rowHeight + (tableEntry.reorderable ? 5 : 0)) * Mathf.Max(1, nbRows) 
			+ EditorGUIUtility.singleLineHeight * (tableEntry.reorderable ? 2 : 1);
		}

	}
}

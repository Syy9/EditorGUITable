using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIExtensions
{

	public static class GUITable
	{

	    public static GUITableState DrawTable (List<TableColumn> columns, List<List<TableEntry>> properties, GUITableState tableState)
	    {

			if (tableState == null)
				tableState = new GUITableState();
			
			CheckTableState (tableState, columns);

	        float rowHeight = EditorGUIUtility.singleLineHeight;

	        EditorGUILayout.BeginHorizontal ();
	        tableState.scrollPosHoriz = EditorGUILayout.BeginScrollView (tableState.scrollPosHoriz);

	        EditorGUILayout.BeginHorizontal ();
	        GUILayout.Space (2f);
			float currentX = 0f;

			RightClickMenu (tableState, columns);

	        for (int i = 0 ; i < columns.Count ; i++)
	        {
	            TableColumn column = columns[i];
				if (!tableState.columnVisible [i])
					continue;
	            string columnName = column.name;
	            if (tableState.sortByColumnIndex == i)
	            {
	                if (tableState.sortIncreasing)
	                    columnName += " " + '\u25B2'.ToString();
	                else
	                    columnName += " " + '\u25BC'.ToString();
	            }

				ResizeColumn (tableState, i, currentX);

				GUI.enabled = column.enabledTitle;

				if (GUILayout.Button(columnName, EditorStyles.miniButtonMid, GUILayout.Width (tableState.columnSizes[i]+4), GUILayout.Height (EditorGUIUtility.singleLineHeight)) && column.isSortable)
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
	        GUI.enabled = true;
	        EditorGUILayout.EndHorizontal ();


	        EditorGUILayout.BeginVertical ();
	        tableState.scrollPos = EditorGUILayout.BeginScrollView (tableState.scrollPos, GUIStyle.none, GUI.skin.verticalScrollbar);

	        List<List<TableEntry>> orderedRows = properties;
	        if (tableState.sortByColumnIndex >= 0)
	        {
				if (tableState.sortIncreasing)
					orderedRows = properties.OrderBy (row => row [tableState.sortByColumnIndex]).ToList();
				else
					orderedRows = properties.OrderByDescending (row => row [tableState.sortByColumnIndex]).ToList();
	        }

	        foreach (List<TableEntry> row in orderedRows)
	        {
	            EditorGUILayout.BeginHorizontal ();
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
	                GUI.enabled = column.enabledEntries;
					property.DrawEntry (tableState.columnSizes[i], rowHeight);
	            }
	            EditorGUILayout.EndHorizontal ();
	        }

	        GUI.enabled = true;

	        EditorGUILayout.EndScrollView ();
	        EditorGUILayout.EndVertical ();


	        EditorGUILayout.EndScrollView ();
	        EditorGUILayout.EndHorizontal ();

	        return tableState;
		}

		public static GUITableState DrawTable (List<PropertyColumn> propertyColumns, SerializedObject serializedObject, string collectionName, GUITableState tableState) 
		{

			List<List<TableEntry>> rows = new List<List<TableEntry>>();

			for (int i = 0 ; i < serializedObject.FindProperty(collectionName).arraySize ; i++)
			{
				List<TableEntry> row = new List<TableEntry>();
				foreach (string prop in propertyColumns.Select(col => col.propertyName))
				{
					row.Add (new PropertyEntry (serializedObject, string.Format("{0}.Array.data[{1}].{2}", collectionName, i, prop)));
				}
				rows.Add(row);
			}
			return DrawTable (propertyColumns.Select((col) => (TableColumn) col).ToList(), rows, tableState);
		}

		public static GUITableState DrawTable (List<SelectorColumn> columns, SerializedObject serializedObject, string collectionName, GUITableState tableState) 
		{

			List<List<TableEntry>> rows = new List<List<TableEntry>>();

			for (int i = 0 ; i < serializedObject.FindProperty(collectionName).arraySize ; i++)
			{
				List<TableEntry> row = new List<TableEntry>();
				foreach (SelectorColumn col in columns)
				{
					row.Add ( col.selector.Invoke ( serializedObject.FindProperty( string.Format("{0}.Array.data[{1}].{2}", collectionName, i, col.propertyName))));
				}
				rows.Add(row);
			}
			return DrawTable (columns.Select((col) => (TableColumn) col).ToList(), rows, tableState);
		}


		public static GUITableState DrawTable (SerializedObject serializedObject, string collectionName, List<string> properties, GUITableState tableState) 
		{
			List<PropertyColumn> columns = properties.Select(prop => new PropertyColumn(
				prop, ObjectNames.NicifyVariableName (prop), 100f)).ToList();

			return DrawTable (columns, serializedObject, collectionName, tableState);
		}

		public static GUITableState DrawTable (SerializedObject serializedObject, string collectionName, GUITableState tableState) 
		{
			List<string> properties = new List<string>();
			string firstElementPath = collectionName + ".Array.data[0]";
			foreach (SerializedProperty prop in serializedObject.FindProperty(firstElementPath))
				properties.Add (prop.propertyPath.Substring(firstElementPath.Length + 1));
			return DrawTable (serializedObject, collectionName, properties, tableState);
		}

		public static GUITableState DrawTable (SerializedProperty collectionProperty, GUITableState tableState) 
		{
			return DrawTable (collectionProperty.serializedObject, collectionProperty.propertyPath, tableState);
		}

		static void RightClickMenu (GUITableState tableState, List<TableColumn> columns)
		{
			Rect rect = new Rect(0, 0, tableState.columnSizes.Where((_, i) => tableState.columnVisible[i]).Sum(s => s + 4), EditorGUIUtility.singleLineHeight);
			GUI.enabled = true;
			if (rect.Contains (Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				GenericMenu contextMenu = new GenericMenu();
				for(int i = 0 ; i < columns.Count ; i++)
				{
					TableColumn column = columns[i];
					if (column.optional)
					{
						int index = i;
						contextMenu.AddItem (new GUIContent (column.name), tableState.columnVisible [i], () => tableState.columnVisible [index] = !tableState.columnVisible [index]);
					}
				}
				contextMenu.ShowAsContext();
			}
		}

		static void ResizeColumn (GUITableState tableState, int indexColumn, float currentX)
		{
			int controlId = EditorGUIUtility.GetControlID(FocusType.Passive);
			Rect resizeRect = new Rect(currentX + tableState.columnSizes[indexColumn] + 2, 0, 10, EditorGUIUtility.singleLineHeight);
			EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeHorizontal, controlId);
			switch(Event.current.type)
			{
				case EventType.MouseDown:
					{
						if (resizeRect.Contains(Event.current.mousePosition))
						{
							GUIUtility.hotControl = controlId;
							Event.current.Use();
						}
						break;
					}
				case EventType.MouseDrag:
					{
						if (GUIUtility.hotControl == controlId)
						{
							tableState.columnSizes[indexColumn] = Event.current.mousePosition.x - currentX - 5;
							Event.current.Use();
						}
						break;
					}
				case EventType.MouseUp:
					{
						if (GUIUtility.hotControl == controlId)
						{
							GUIUtility.hotControl = 0;
							Event.current.Use();
						}
						break;
					}
			}
		}

		static void CheckTableState (GUITableState tableState, List<TableColumn> columns)
		{
			if (tableState.columnSizes == null || tableState.columnSizes.Count < columns.Count)
			{
				tableState.columnSizes = columns.Select ((column) => column.width).ToList ();
			}
			if (tableState.columnVisible == null || tableState.columnVisible.Count < columns.Count)
			{
				tableState.columnVisible = columns.Select ((column) => column.visibleByDefault).ToList ();
			}
		}

	}

}

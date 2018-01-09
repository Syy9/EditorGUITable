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

	    public static GUITableState DrawTable (List<GUITableColumn> columns, List<List<GUITableEntry>> properties, GUITableState tableState)
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
	            GUITableColumn column = columns[i];
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

	        List<List<GUITableEntry>> orderedRows = properties;
	        if (tableState.sortByColumnIndex >= 0)
	        {
				if (tableState.sortIncreasing)
					orderedRows = properties.OrderBy (row => row [tableState.sortByColumnIndex]).ToList();
				else
					orderedRows = properties.OrderByDescending (row => row [tableState.sortByColumnIndex]).ToList();
	        }

	        foreach (List<GUITableEntry> row in orderedRows)
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
	                GUITableColumn column = columns [i];
	                GUITableEntry property = row[i];
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

		public class PropertyColumn
		{
			public string propertyName;
			public GUITableColumn column;
			public PropertyColumn (string propertyName, GUITableColumn column)
			{
				this.propertyName = propertyName;
				this.column = column;
			}
		}

		public static GUITableState DrawTable (List<PropertyColumn> propertyColumns, SerializedObject serializedObject, string collectionName, GUITableState tableState) 
		{

			List<List<GUITableEntry>> rows = new List<List<GUITableEntry>>();

			for (int i = 0 ; i < serializedObject.FindProperty(collectionName).arraySize ; i++)
			{
				List<GUITableEntry> row = new List<GUITableEntry>();
				foreach (string prop in propertyColumns.Select(col => col.propertyName))
				{
					row.Add (new PropertyEntry (serializedObject, string.Format("{0}.Array.data[{1}].{2}", collectionName, i, prop)));
				}
				rows.Add(row);
			}
			return DrawTable (propertyColumns.Select(col => col.column).ToList(), rows, tableState);
		}


		public static GUITableState DrawTable (SerializedObject serializedObject, string collectionName, List<string> properties, GUITableState tableState) 
		{
			List<PropertyColumn> columns = properties.Select(prop => new PropertyColumn(
				prop, 
				new GUITableColumn(ObjectNames.NicifyVariableName (prop), 100f))).ToList();

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

		static void RightClickMenu (GUITableState tableState, List<GUITableColumn> columns)
		{
			Rect rect = new Rect(0, 0, tableState.columnSizes.Where((_, i) => tableState.columnVisible[i]).Sum(s => s + 4), EditorGUIUtility.singleLineHeight);
			GUI.enabled = true;
			if (rect.Contains (Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				GenericMenu contextMenu = new GenericMenu();
				for(int i = 0 ; i < columns.Count ; i++)
				{
					GUITableColumn column = columns[i];
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

		static void CheckTableState (GUITableState tableState, List<GUITableColumn> columns)
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

	public class GUITableColumn
	{
	    public string name { get; private set; }
	    public float width { get; private set; }
		public bool enabledEntries = true;
		public bool isSortable = true;
		public bool enabledTitle = true;
		public bool optional = false;
		public bool visibleByDefault = true;
		public GUITableColumn (string name, float width, bool enabled = true, bool isSortable = true, bool enabledTitle = true, bool optional = false, bool visibleByDefault = true)
	    {
	        this.name = name;
	        this.width = width;
	        this.enabledEntries = enabled;
	        this.isSortable = isSortable;
			this.enabledTitle = enabledTitle;
			this.optional = optional;
			this.visibleByDefault = visibleByDefault;
	    }
	}

	public abstract class GUITableEntry : System.IComparable
	{
	    public abstract void DrawEntry (float width, float height);

	    public abstract string comparingValue { get; }

		public virtual int CompareTo (object other) 
		{ 
			GUITableEntry otherEntry = (GUITableEntry) other;
			if (otherEntry == null)
				return 1;
			return comparingValue.CompareTo ( otherEntry.comparingValue );
		}

	}

	public class PropertyEntry : GUITableEntry
	{
	    SerializedObject so;
	    string propertyName;

	    public override void DrawEntry (float width, float height)
	    {
	        SerializedProperty sp = so.FindProperty (propertyName);
	        if (sp != null)
	        {
	            EditorGUILayout.PropertyField (sp, GUIContent.none, GUILayout.Width (width), GUILayout.Height (height));
	            so.ApplyModifiedProperties ();
	        }
	        else
	        {
				Debug.LogWarningFormat ("Property not found: {0} -> {1}", so.targetObject.name, propertyName);
	            GUILayout.Space (width + 4f);
	        }
	    }

	    public override string comparingValue
	    {
	        get
	        {
	            SerializedProperty sp = so.FindProperty (propertyName);
	            if (sp != null)
	            {
	                switch (sp.propertyType)
					{
						case SerializedPropertyType.String:
						case SerializedPropertyType.Character:
							return sp.stringValue.ToString ();
						case SerializedPropertyType.Float:
							return sp.doubleValue.ToString ();
						case SerializedPropertyType.Integer:
						case SerializedPropertyType.LayerMask:
						case SerializedPropertyType.ArraySize:
							return sp.intValue.ToString ();
						case SerializedPropertyType.Enum:
							return sp.enumValueIndex.ToString ();
						case SerializedPropertyType.Boolean:
							return sp.boolValue.ToString ();
						case SerializedPropertyType.ObjectReference:
							return sp.objectReferenceValue.name.ToString ();
						case SerializedPropertyType.ExposedReference:
							return sp.exposedReferenceValue.name.ToString ();
	                }
	            }
	            return "";
	        }
	    }

		public override int CompareTo (object other)
		{
			GUITableEntry otherEntry = (GUITableEntry) other;
			if (otherEntry == null)
				throw new ArgumentException ("Object is not a GUITableEntry");

			PropertyEntry otherPropEntry = (PropertyEntry) other;
			if (otherPropEntry == null)
				return base.CompareTo(other);

			SerializedProperty sp = so.FindProperty (propertyName);
			SerializedProperty otherSp = otherPropEntry.so.FindProperty (otherPropEntry.propertyName);

			if (sp.propertyType != otherSp.propertyType)
				return base.CompareTo(other);
			
			if (sp != null)
			{

				switch (sp.propertyType)
				{
					case SerializedPropertyType.String:
					case SerializedPropertyType.Character:
						return sp.stringValue.CompareTo (otherSp.stringValue);
					case SerializedPropertyType.Float:
						return sp.doubleValue.CompareTo (otherSp.doubleValue);
					case SerializedPropertyType.Integer:
					case SerializedPropertyType.LayerMask:
					case SerializedPropertyType.ArraySize:
						return sp.intValue.CompareTo (otherSp.intValue);
					case SerializedPropertyType.Enum:
						return sp.enumValueIndex.CompareTo (otherSp.enumValueIndex);
					case SerializedPropertyType.Boolean:
						return sp.boolValue.CompareTo (otherSp.boolValue);
					case SerializedPropertyType.ObjectReference:
						return sp.objectReferenceValue.name.CompareTo (otherSp.objectReferenceValue.name);
					case SerializedPropertyType.ExposedReference:
						return sp.exposedReferenceValue.name.CompareTo (otherSp.exposedReferenceValue.name);
				}
			}
			return 0;
		}

	    public PropertyEntry (SerializedObject so, string propertyName)
	    {
	        this.so = so;
	        this.propertyName = propertyName;
	    }
	}

	public class ActionEntry : GUITableEntry
	{
	    string name;
	    System.Action action;
	    public override void DrawEntry (float width, float height)
	    {
	        if (GUILayout.Button (name, GUILayout.Width (width), GUILayout.Height (height)))
	        {
	            if (action != null)
	                action.Invoke ();
	        }
	    }

	    public override string comparingValue
	    {
	        get
	        {
	            return name;
	        }
	    }

	    public ActionEntry (string name, System.Action action)
	    {
	        this.name = name;
	        this.action = action;
	    }
	}
	public class LabelEntry : GUITableEntry
	{
	    
	    string value;

	    public override void DrawEntry (float width, float height)
	    {
	        EditorGUILayout.LabelField (value, GUILayout.Width (width), GUILayout.Height (height));
	    }

	    public override string comparingValue
	    {
	        get
	        {
	            return value;
	        }
	    }

	    public LabelEntry (string value)
	    {
	        this.value = value;
	    }
	}

	public class GUITableState
	{

	    public Vector2 scrollPos;

	    public Vector2 scrollPosHoriz;

	    public int sortByColumnIndex = -1;

	    public bool sortIncreasing;

		public List<float> columnSizes = new List<float> ();

		public List<bool> columnVisible = new List<bool> ();

	}

}

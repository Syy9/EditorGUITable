using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

namespace EditorGUITable
{

	/// <summary>
	/// The current state of the GUITable.
	/// This has to be used the same way state parameters are used in Unity GUI functions, like
	/// the scroll position in BeginScrollView.
	/// It has to be passed from one GUI frame to another by keeping a reference in your calling code.
	/// </summary>
	/// <example>
	/// <code>
	/// GUITableState tableState;
	/// void OnGUI ()
	/// {
	/// 	tableState = GUITable.DrawTable(collectionProperty, tableState);
	/// }
	/// </code>
	/// </example>
	public class GUITableState
	{

		public Vector2 scrollPos;

		public int sortByColumnIndex = -1;

		public bool sortIncreasing;

		public List<float> columnSizes = new List<float> ();

		public List<bool> columnVisible = new List<bool> ();

		string prefsKey;

		public ReorderableList reorderableList;

		public bool isBeingResized { get; private set; }

		public float totalWidth
		{
			get
			{
				return columnSizes.Where((_, i) => columnVisible[i]).Sum(s => s + 4);
			}
		}

		public GUITableState ()
		{
		}

		/// <summary>
		/// Initializes a <see cref="EditorGUITable.GUITableState"/> with a key to save it in EditorPrefs.
		/// This constructor can't be used in ScriptableObject's constructor or in the property's declaration,
		/// because it uses the EditorPrefs. Use it in OnEnable or Awake instead.
		/// </summary>
		/// <param name="prefsKey">Prefs key.</param>
		public GUITableState (string prefsKey)
		{
			this.prefsKey = prefsKey;
			GUITableState loadedState = Load(prefsKey);
			this.scrollPos = loadedState.scrollPos;
			this.sortByColumnIndex = loadedState.sortByColumnIndex;
			this.sortIncreasing = loadedState.sortIncreasing;
			this.columnSizes = loadedState.columnSizes;
			this.columnVisible = loadedState.columnVisible;
		}

		public void SetReorderableList (ReorderableList reorderableList)
		{
			this.reorderableList = reorderableList;
		}

		public void Save ()
		{
			if (!string.IsNullOrEmpty(prefsKey))
			EditorPrefs.SetString(prefsKey, JsonUtility.ToJson (this));
		}

		public static GUITableState Load (string prefsKey)
		{
			if (EditorPrefs.HasKey(prefsKey))
				return JsonUtility.FromJson<GUITableState> (EditorPrefs.GetString(prefsKey, string.Empty));
			else
				return new GUITableState();
		}

		public void CheckState (List<TableColumn> columns, GUITableEntry tableEntry, float availableWidth)
		{

			if (columnSizes == null || columnSizes.Count < columns.Count)
			{
				columnSizes = columns.Select ((column) => column.GetDefaultWidth()).ToList ();
			}
			if (columnVisible == null || columnVisible.Count < columns.Count)
			{
				columnVisible = columns.Select ((column) => column.entry.visibleByDefault).ToList ();
			}
			if (totalWidth < availableWidth && !isBeingResized)
			{
				List<int> expandableColumns = new List<int> ();
				for (int i = 0 ; i < columns.Count ; i++)
					if (columns[i].entry.expandWidth && columnSizes[i] < columns[i].entry.maxWidth)
						expandableColumns.Add (i);
				float addWidth = (availableWidth - totalWidth) / expandableColumns.Count;
				foreach (int i in expandableColumns)
					columnSizes[i] += addWidth;
			}
			for (int i = 0 ; i < columns.Count ; i++)
			{
				columnSizes[i] = Mathf.Clamp(columnSizes[i], columns[i].entry.minWidth, columns[i].entry.maxWidth);
			}
			if (reorderableList != null)
				reorderableList.draggable = sortByColumnIndex < 0;
		}

		public void RightClickMenu (List<TableColumn> columns, Rect? position = null)
		{
			Rect rect;
			if (position == null)
				rect = new Rect(0, 0, totalWidth, EditorGUIUtility.singleLineHeight);
			else
				rect = new Rect(position.Value.x, position.Value.y, totalWidth, EditorGUIUtility.singleLineHeight);
			GUI.enabled = true;
			if (rect.Contains (Event.current.mousePosition) && Event.current.type == EventType.MouseDown && Event.current.button == 1)
			{
				GenericMenu contextMenu = new GenericMenu();
				for(int i = 0 ; i < columns.Count ; i++)
				{
					TableColumn column = columns[i];
					if (column.entry.optional)
					{
						int index = i;
						contextMenu.AddItem (new GUIContent (column.title), columnVisible [i], () => columnVisible [index] = !columnVisible [index]);
					}
				}
				contextMenu.ShowAsContext();
			}
		}

		public void ResizeColumn (int indexColumn, float currentX, Rect? position = null)
		{
			int controlId = EditorGUIUtility.GetControlID(FocusType.Passive);
			Rect resizeRect;
			if (position == null)
				resizeRect = new Rect(currentX + columnSizes[indexColumn] + 2, 0, 10, EditorGUIUtility.singleLineHeight);
			else
				resizeRect = new Rect(currentX + columnSizes[indexColumn] + 2, position.Value.y, 10, EditorGUIUtility.singleLineHeight);
			EditorGUIUtility.AddCursorRect(resizeRect, MouseCursor.ResizeHorizontal, controlId);
			switch(Event.current.type)
			{
				case EventType.MouseDown:
					{
						if (resizeRect.Contains(Event.current.mousePosition))
						{
							GUIUtility.hotControl = controlId;
							Event.current.Use();
							isBeingResized = true;
						}
						break;
					}
				case EventType.MouseDrag:
					{
						if (GUIUtility.hotControl == controlId)
						{
							columnSizes[indexColumn] = Event.current.mousePosition.x - currentX - 5;
							Event.current.Use();
							isBeingResized = true;
						}
						break;
					}
				case EventType.MouseUp:
					{
						if (GUIUtility.hotControl == controlId)
						{
							GUIUtility.hotControl = 0;
							Event.current.Use();
							isBeingResized = false;
						}
						break;
					}
			}
		}

	}

}
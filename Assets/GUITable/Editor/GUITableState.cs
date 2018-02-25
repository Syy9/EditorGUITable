using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

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

		public Vector2 scrollPosHoriz;

		public int sortByColumnIndex = -1;

		public bool sortIncreasing;

		public List<float> columnSizes = new List<float> ();

		public List<bool> columnVisible = new List<bool> ();

		string prefsKey;

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
		/// Initializes a <see cref="GUIExtensions.GUITableState"/> with a key to save it in EditorPrefs.
		/// This constructor can't be used in ScriptableObject's constructor or in the property's declaration,
		/// because it uses the EditorPrefs. Use it in OnEnable or Awake instead.
		/// </summary>
		/// <param name="prefsKey">Prefs key.</param>
		public GUITableState (string prefsKey)
		{
			this.prefsKey = prefsKey;
			GUITableState loadedState = Load(prefsKey);
			this.scrollPos = loadedState.scrollPos;
			this.scrollPosHoriz = loadedState.scrollPosHoriz;
			this.sortByColumnIndex = loadedState.sortByColumnIndex;
			this.sortIncreasing = loadedState.sortIncreasing;
			this.columnSizes = loadedState.columnSizes;
			this.columnVisible = loadedState.columnVisible;
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

		public void CheckState (List<TableColumn> columns, GUITableEntry tableEntry, float availableWidth, bool isBeingResized)
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
		}

	}

}
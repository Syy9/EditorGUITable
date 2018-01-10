using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace GUIExtensions
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

	}

}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

	}

}
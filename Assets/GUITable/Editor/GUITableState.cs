using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GUIExtensions
{

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
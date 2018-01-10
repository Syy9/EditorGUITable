using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GUIExtensions
{

	/// <summary>
	/// Base class for all table entries.
	/// DrawEntry needs to be overriden to draw the entry for the cell.
	/// Use this to customize the table however needed.
	/// CompareTo can be overriden to customize the sorting.
	/// comparingValue is used as a fallback for sorting any types of entries, even different types.
	/// </summary>
	public abstract class TableEntry : System.IComparable
	{
		public abstract void DrawEntry (float width, float height);

		public abstract string comparingValue { get; }

		public virtual int CompareTo (object other) 
		{ 
			TableEntry otherEntry = (TableEntry) other;
			if (otherEntry == null)
				return 1;
			return comparingValue.CompareTo ( otherEntry.comparingValue );
		}

	}

}

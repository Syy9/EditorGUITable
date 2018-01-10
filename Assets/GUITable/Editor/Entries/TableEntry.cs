using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace GUIExtensions
{

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

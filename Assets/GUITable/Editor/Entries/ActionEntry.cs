using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{
	
	public class ActionEntry : TableEntry
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

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This entry class draws a button which, when clicked, will trigger the
	/// action given in the constructor.
	/// </summary>
	public class ActionEntry : TableEntry
	{
		string name;
		System.Action action;
		public override void DrawEntryLayout (float width, float height)
		{
			if (GUILayout.Button (name, GUILayout.Width (width), GUILayout.Height (height)))
			{
				if (action != null)
					action.Invoke ();
			}
		}

		public override void DrawEntry (Rect rect)
		{
			if (GUI.Button (rect, name))
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

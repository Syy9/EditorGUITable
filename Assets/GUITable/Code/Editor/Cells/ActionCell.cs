using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This cell class draws a button which, when clicked, will trigger the
	/// action given in the constructor.
	/// </summary>
	public class ActionCell : TableCell
	{
		string name;
		System.Action action;

		public override void DrawCell (Rect rect)
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

		public ActionCell (string name, System.Action action)
		{
			this.name = name;
			this.action = action;
		}
	}

}

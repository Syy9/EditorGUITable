using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace GUIExtensions
{

	public class LabelEntry : TableEntry
	{

		string value;

		public override void DrawEntry (float width, float height)
		{
			EditorGUILayout.LabelField (value, GUILayout.Width (width), GUILayout.Height (height));
		}

		public override string comparingValue
		{
			get
			{
				return value;
			}
		}

		public LabelEntry (string value)
		{
			this.value = value;
		}
	}

}

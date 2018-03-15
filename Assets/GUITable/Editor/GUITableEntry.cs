using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorGUITable
{

	public class GUITableEntry
	{

		public bool allowScrollView = true;
		public float rowHeight = EditorGUIUtility.singleLineHeight;
		public bool reorderable = false;
		public Func <SerializedProperty, bool> filter = null;

		public GUITableEntry (GUITableOption[] options)
		{
			ApplyOptions (options);
		}

		public virtual void ApplyOptions(GUITableOption[] options)
		{
			if (options == null)
				return;
			foreach (GUITableOption option in options)
			{
				switch (option.type)
				{
					case GUITableOption.Type.AllowScrollView:
						this.allowScrollView = (bool) option.value;
						break;
					case GUITableOption.Type.RowHeight:
						this.rowHeight = (float) option.value;
						break;
					case GUITableOption.Type.Reorderable:
						this.reorderable = (bool) option.value;
						break;
					case GUITableOption.Type.Filter:
						this.filter = (Func<SerializedProperty, bool>) option.value;
						break;
				}
			}
		}

	}

}
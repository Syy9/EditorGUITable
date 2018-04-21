using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace EditorGUITable
{

	public class GUITableOption
	{
		
		public enum Type
		{
			AllowScrollView,
			RowHeight,
			Reorderable,
			Filter,
		}

		public Type type;
		public object value;

		public GUITableOption(Type type, object value)
		{
			this.type = type;
			this.value = value;
		}

		public static GUITableOption AllowScrollView (bool enable)
		{
			return new GUITableOption (Type.AllowScrollView, enable);
		}

		public static GUITableOption RowHeight (float value)
		{
			return new GUITableOption (Type.RowHeight, value);
		}

		public static GUITableOption Reorderable (bool enable = true)
		{
			return new GUITableOption (Type.Reorderable, enable);
		}

		public static GUITableOption Filter (Func <SerializedProperty, bool> filter)
		{
			return new GUITableOption (Type.Filter, filter);
		}
	}

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

namespace EditorGUITable
{
	
	[CustomPropertyDrawer(typeof(ReorderableTableAttribute))]
	public class ReorderableTableDrawer : TableDrawer 
	{

		/// <summary>
		/// Full Version Only
		/// </summary>
		protected override GUITableState DrawTable (Rect rect, SerializedProperty collectionProperty, GUIContent label, TableAttribute tableAttribute)
		{
			return null;
		}
		

	}

}

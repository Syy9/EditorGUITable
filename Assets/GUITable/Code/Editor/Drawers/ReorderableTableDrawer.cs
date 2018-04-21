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

		protected override GUITableState DrawTable (Rect rect, SerializedProperty collectionProperty, GUIContent label, TableAttribute tableAttribute)
		{
			if (tableState != null)
				rect.width = Mathf.Min (rect.width, tableState.totalWidth + 20f);
			if (tableAttribute.properties == null && tableAttribute.widths == null)
				return GUITable.DrawTable(rect, tableState, collectionProperty, GUITableOption.AllowScrollView(false), GUITableOption.Reorderable());
			else if (tableAttribute.widths == null)
				return GUITable.DrawTable(rect, tableState, collectionProperty, tableAttribute.properties.ToList(), GUITableOption.AllowScrollView(false), GUITableOption.Reorderable());
			else
				return GUITable.DrawTable(rect, tableState, collectionProperty, GetPropertyColumns(tableAttribute), GUITableOption.AllowScrollView(false), GUITableOption.Reorderable());
		}

		protected override float GetRequiredAdditionalHeight ()
		{
			return 3f * EditorGUIUtility.singleLineHeight;;
		}

	}

}

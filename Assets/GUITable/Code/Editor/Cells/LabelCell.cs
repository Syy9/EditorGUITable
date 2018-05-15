using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


namespace EditorGUITable
{

	/// <summary>
	/// This cell class draws a string as a label.
	/// This is useful for properties you want to display in the table
	/// as read only, as the default PropertyField used in PropertyCell uses editable fields.
	/// </summary>
	public class LabelCell : TableCell
	{

		string value;

		public override void DrawCell (Rect rect)
		{
			GUI.Label(rect, value);
		}

		public override string comparingValue
		{
			get
			{
				return value;
			}
		}

		public LabelCell (string value)
		{
			this.value = value;
		}

		public LabelCell (SerializedProperty sp)
		{
			this.value = GetPropertyValueAsString (sp);
		}
	}

}

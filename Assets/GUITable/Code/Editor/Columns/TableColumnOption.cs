using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableColumnOption
{

	public enum Type
	{
		ExpandWidth,
		Width,
		MinWidth,
		MaxWidth,
		Resizeable,
		Sortable,
		EnabledCells,
		EnabledTitle,
		Optional,
		VisibleByDefault,
	}

	public Type type;
	public object value;

	public TableColumnOption(Type type, object value)
	{
		this.type = type;
		this.value = value;
	}
}

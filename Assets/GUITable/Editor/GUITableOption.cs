using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GUITableOption
{
	
	public enum Type
	{
		AllowScrollView,
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
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableColumnEntry
{

	public float defaultWidth = 0f;
	public bool expandWidth = false;
	public float maxWidth = float.MaxValue;
	public float minWidth = 20f;
	public bool resizeable = true;
	public bool sortable = true;
	/// <summary>
	/// Defines if the cells are enabled (interactable) or disabled (grayed out). Default: true.
	/// </summary>
	public bool enabledCells = true;
	/// <summary>
	/// Defines if the column is sortable.
	/// </summary>
	public bool isSortable = true;
	/// <summary>
	/// Defines if the title is enabled (interactable) or disabled (grayed out). Default: true.
	/// </summary>
	public bool enabledTitle = true;
	/// <summary>
	/// Defines if the column can be hidden by right-clicking the column titles bar. Default: false.
	/// </summary>
	public bool optional = false;
	/// <summary>
	/// Defines if the column is visible by default. If this is false, and optional is false too. The column can never be viewed. Default: true.
	/// </summary>
	public bool visibleByDefault = true;
	/// <summary>
	/// Defines if the column is expandable.
	/// </summary>

	public TableColumnEntry (TableColumnOption[] options)
	{
		ApplyOptions (options);
	}

	public virtual void ApplyOptions(TableColumnOption[] options)
	{
		if (options == null)
			return;
		foreach (TableColumnOption option in options)
		{
			switch (option.type)
			{
				case TableColumnOption.Type.ExpandWidth:
					this.expandWidth = (bool) option.value;
					break;
				case TableColumnOption.Type.MaxWidth:
					this.maxWidth = (float) option.value;
					break;
				case TableColumnOption.Type.MinWidth:
					this.minWidth = (float) option.value;
					break;
				case TableColumnOption.Type.Resizeable:
					this.resizeable = (bool) option.value;
					break;
				case TableColumnOption.Type.Sortable:
					this.sortable = (bool) option.value;
					break;
				case TableColumnOption.Type.Width:
					this.defaultWidth = (float) option.value;
					break;
				case TableColumnOption.Type.EnabledTitle:
					this.enabledTitle = (bool) option.value;
					break;
				case TableColumnOption.Type.EnabledCells:
					this.enabledCells = (bool) option.value;
					break;
				case TableColumnOption.Type.Optional:
					this.optional = (bool) option.value;
					break;
				case TableColumnOption.Type.VisibleByDefault:
					this.visibleByDefault = (bool) option.value;
					break;
			}
		}
	}

}

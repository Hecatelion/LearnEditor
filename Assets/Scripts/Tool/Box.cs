using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum BoxType
{
	HitBox,
	HurtBox
}

public class Box
{
	static readonly Color[] colors = {
			new Color(1, 0, 0, 0.6f),	// red
			new Color(0, 1, 0, 0.6f)	// green
		};

	public BoxType type;
	public Rect dimension;

	public bool IsClicked(int _mouseButton, Event _curEvent)
	{
		return dimension.IsClicked(_mouseButton, _curEvent);
	}
	public bool IsDragged(int _mouseButton, Event _curEvent)
	{
		return dimension.IsDragged(_mouseButton, _curEvent);
	}

	public void SetPosition(float _x, float _y)
	{
		dimension.Set(_x, _y, dimension.width, dimension.height);
	}

	public void Display()
	{
		EditorGUI.DrawRect(dimension, colors[(int)type]);
	}
}
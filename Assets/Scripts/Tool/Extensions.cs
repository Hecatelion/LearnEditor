﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


static class ExtensionMethod
{
	public static void SetAlphaToColor(this Texture2D texture, Color _color)
	{
		for (int i = 0; i < texture.width; i++)
		{
			for (int j = 0; j < texture.height; j++)
			{
				if (texture.GetPixel(i, j).a < 0.85f)
				{
					texture.SetPixel(i, j, _color);
				}
			}
		}

		texture.Apply();
	}

	private static bool IsReceivingEvent(this Rect rect, EventType _eventType, int _mouseButton, Event _curEvent)
	{

		if (_curEvent != null && _curEvent.type == _eventType && _curEvent.button == _mouseButton)
		{
			Vector2 mousePos = _curEvent.mousePosition;

			return mousePos.x < rect.x + rect.width && mousePos.x > rect.x
				&& mousePos.y < rect.y + rect.height && mousePos.y > rect.y;
		}

		return false;
	}

	public static bool IsDragged(this Rect rect, int _mouseButton, Event _curEvent)
	{
		return rect.IsReceivingEvent(EventType.MouseDrag, _mouseButton, _curEvent);
	}

	public static bool IsClicked(this Rect rect, int _mouseButton, Event _curEvent)
	{
		return rect.IsReceivingEvent(EventType.MouseDown, _mouseButton, _curEvent);
	}
}

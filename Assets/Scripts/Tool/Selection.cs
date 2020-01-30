using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Selection
{
	static readonly Color color = Color.yellow;
	const float thickness = 5f;

	public Box selectedBox;

	Rect topBar;
	Rect botBar;
	Rect leftBar;
	Rect rightBar;

	public Selection()
	{
		topBar.height = botBar.height = leftBar.width = rightBar.width = thickness;
	}

	// must be called in Tool's update or gui 
	public void Update(Event _curEvent)
	{
		if (selectedBox != null)
		{
			if (selectedBox.IsClicked(0, _curEvent))
			{
				Debug.Log("click");
			}
			else if (selectedBox.IsDragged(0, _curEvent))
			{
				Debug.Log("drag");
				MoveSelection(_curEvent);
			}
		}
	}

	public void Display()
	{
		EditorGUI.DrawRect(topBar, color);
		EditorGUI.DrawRect(botBar, color);
		EditorGUI.DrawRect(leftBar, color);
		EditorGUI.DrawRect(rightBar, color);
	}

	public void Select(Box _selectedBox)
	{
		selectedBox = _selectedBox;
		UpdateGraphics();
	}

	private void UpdateGraphics()
	{ 
		// size
		topBar.width = botBar.width = selectedBox.dimension.width + thickness;
		leftBar.height = rightBar.height = selectedBox.dimension.height;

		// pos
		topBar.x = leftBar.x = botBar.x = selectedBox.dimension.x;
		rightBar.x = selectedBox.dimension.x + selectedBox.dimension.width;
		
		topBar.y = leftBar.y = rightBar.y = selectedBox.dimension.y;
		botBar.y = selectedBox.dimension.y + selectedBox.dimension.height;
	}

	private void MoveSelection(Event _curEvent)
	{
		if (_curEvent.button == 0)
		{
			Vector2 mousePos = _curEvent.mousePosition;
			Vector2 movement = mousePos;// new Vector2(0, 0);
			selectedBox.SetPosition(movement.x, movement.y);
			
			UpdateGraphics();
		}
	}
	
	// drag move
	// drag resize
}
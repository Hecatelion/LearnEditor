using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public enum Mode
{
	None,
	Moving,
	Resizing
}

public enum BarType
{
	none = -1,
	top,
	bot,
	left,
	right
}

public class Selection
{
	const int windowOffset = 22;

	bool IsClicked
	{
		get => true;
	}

	Mode mode = Mode.None;

	// selection outline
	const float thickness = 5.0f;
	Rect[] bars = new Rect[4];
	Vector2 initialSidePos;
	BarType selectedSide = BarType.none;
	bool isResizing = false;

	// selected box
	public Box selectedBox;
	Vector2 holdOffset; 
	bool isMoving = false;


	public Selection()
	{
		bars[(int)BarType.top].height = bars[(int)BarType.bot].height = bars[(int)BarType.left].width = bars[(int)BarType.right].width = thickness;
	}

	// must be called in Tool's update or gui 
	public void Update(Event _curEvent)
	{
		if (selectedBox != null)
		{
			mode = Mode.None;

			// moving box mode
			if (selectedBox.IsClicked(0, _curEvent)) // add center rect as moving tool, such as bars, no selectBox.IsClicked anymore
			{
				holdOffset = _curEvent.mousePositionDrawableArea() - selectedBox.dimension.position;

				mode = Mode.Moving;
			}
			// resizing box mode
			else
			{
				for (int i = 0; i < 4; i++)
				{
					if (bars[i].IsClicked(0, _curEvent))
					{
						initialSidePos = _curEvent.mousePositionDrawableArea();

						selectedSide = (BarType)i;
						mode = Mode.Resizing;
					}
				} 
			}
			// no mode
			if(_curEvent.type == EventType.MouseUp && _curEvent.button == 0)
			{
				mode = Mode.None;
			}

			// mode actions
			switch (mode)
			{
				case Mode.Moving:		MoveSelection(_curEvent);		break;
				case Mode.Resizing:		ResizeSelection(_curEvent);		break;
			}
		}
	}

	public void Display()
	{
		if (selectedBox != null)
		{
			for (int i = 0; i < 4; i++)
			{
				EditorGUI.DrawRect(bars[i], (selectedSide == (BarType)i) ? Color.red : Color.yellow);
			}
		}
	}

	public bool IsReceivingEvent()
	{
		return isMoving || isResizing;
	}

	public void Select(Box _selectedBox)
	{
		selectedBox = _selectedBox;
		UpdateGraphics();
	}

	public void Unselect()
	{
		selectedSide = BarType.none;
		selectedBox = null;
	}

	private void UpdateGraphics()
	{ 
		// size
		bars[(int)BarType.top].width	 = bars[(int)BarType.bot].width	= selectedBox.dimension.width + thickness * 2;
		bars[(int)BarType.left].height	 = bars[(int)BarType.right].height = selectedBox.dimension.height + thickness * 2;

		// pos
		bars[(int)BarType.top].x = bars[(int)BarType.left].x = bars[(int)BarType.bot].x = selectedBox.dimension.x - thickness;
		bars[(int)BarType.right].x = selectedBox.dimension.x + selectedBox.dimension.width;
		
		bars[(int)BarType.top].y = bars[(int)BarType.left].y = bars[(int)BarType.right].y = selectedBox.dimension.y - thickness;
		bars[(int)BarType.bot].y = selectedBox.dimension.y + selectedBox.dimension.height;
	}

	private void MoveSelection(Event _curEvent)
	{
		if (_curEvent.button == 0)
		{
			selectedBox.SetPosition(_curEvent.mousePositionDrawableArea() - holdOffset);
			
			UpdateGraphics();
		}
	}

	private void ResizeSelection(Event _curEvent)
	{
		Vector2 movement = _curEvent.mousePositionDrawableArea() - initialSidePos;
		Debug.Log("movement : " + movement.y);

		switch (selectedSide)
		{
			case BarType.top:
				selectedBox.dimension.height -= movement.y;
				selectedBox.dimension.y = _curEvent.mousePositionDrawableArea().y;
				break;

			case BarType.bot:
				break;

			case BarType.left:
				break;

			case BarType.right:
				break;

			default:
				break;
		}

		UpdateGraphics();
	}
}
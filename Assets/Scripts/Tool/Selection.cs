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
	right,
	center
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
	Rect[] bars = new Rect[5];
	Vector2 initialSidePos;
	Rect initialBoxDimentions;
	BarType selectedSide = BarType.none;

	// selected box
	public Box selectedBox;
	Vector2 holdOffset; 


	public Selection()
	{
		bars[(int)BarType.top].height = thickness;
		bars[(int)BarType.bot].height = thickness;
		bars[(int)BarType.left].width = thickness;
		bars[(int)BarType.right].width = thickness;
		bars[(int)BarType.center].width = bars[(int)BarType.center].height = 5 * thickness;
	}

	// must be called in Tool's update or gui 
	public void Update(Event _curEvent)
	{
		if (selectedBox != null)
		{
			// moving box mode
			if (bars[(int)BarType.center].IsClicked(0, _curEvent))
			{
				holdOffset = _curEvent.mousePositionDrawableArea() - selectedBox.dimension.position;

				selectedSide = BarType.center;
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
						initialBoxDimentions = selectedBox.dimension;

						selectedSide = (BarType)i;
						mode = Mode.Resizing;
					}
				} 
			}

			// no mode
			if(_curEvent.type == EventType.MouseUp && _curEvent.button == 0)
			{
				mode = Mode.None;
				selectedSide = BarType.none;
			}

			// mode updates
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
			for (int i = 0; i < 5; i++)
			{
				EditorGUI.DrawRect(bars[i], (selectedSide == (BarType)i) ? Color.red : Color.yellow);
			}
		}
	}

	public bool IsReceivingEvent()
	{
		return mode != Mode.None;
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

		bars[(int)BarType.center].position = selectedBox.dimension.position + selectedBox.dimension.size / 2 - bars[(int)BarType.center].size / 2;
	}

	private void MoveSelection(Event _curEvent)
	{
		if (_curEvent.button == 0)
		{
			selectedBox.SetPosition(_curEvent.mousePositionDrawableArea() - selectedBox.dimension.size / 2);

			UpdateGraphics();
		}
	}

	private void ResizeSelection(Event _curEvent)
	{
		Vector2 movement = _curEvent.mousePositionDrawableArea() - initialSidePos;

		switch (selectedSide)
		{
			case BarType.top:
				selectedBox.dimension.height = initialBoxDimentions.height - movement.y;
				selectedBox.dimension.position = initialBoxDimentions.position + new Vector2(0, movement.y);
				break;

			case BarType.bot:
				selectedBox.dimension.height = initialBoxDimentions.height + movement.y;
				break;

			case BarType.left:
				selectedBox.dimension.width = initialBoxDimentions.width - movement.x;
				selectedBox.dimension.position = initialBoxDimentions.position + new Vector2(movement.x, 0);
				break;

			case BarType.right:
				selectedBox.dimension.width = initialBoxDimentions.width + movement.x;
				break;

			default:
				break;
		}

		UpdateGraphics();
	}
}
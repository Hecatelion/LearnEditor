using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MoveEditor : EditorWindow
{
	Move curMove = null;
	Step curStep = null;
	int curStepIndex = -1;
	List<Box> hitboxes = new List<Box>();
	Selection selection = new Selection();

	static readonly int c_margin = 20;
	static readonly Vector2 c_marginVec = new Vector2(c_margin, c_margin);
	static readonly Vector2 c_stepsButtonsPos = new Vector2(50, 75);
	static readonly Vector2 c_buttonSize = new Vector2(50, 50);
	static readonly Rect c_stepTextureRect = new Rect(110, 150, 350, 350);

	//Texture2D tex;
	//Texture2D curTex;
	//Texture2D texCustomAlpha;
	Texture2D[] stepsTextures;
	Texture2D toolTex;
	Sprite curSpr;

	Event curEvent;

	bool hasSelectedABoxThisFrame;

	// Colors
	Color hitBoxColor;
	Color defaultGUIColor;
	Color selectedGUIColor;

	[MenuItem("Custom/MoveEditor")]
	static void Init()
	{
		var window = GetWindow<MoveEditor>("Baba Tool", true);
		window.position = new Rect(50, 50, 900, 600);
		window.Show();
	}

	private void OnEnable()
	{
		hitBoxColor = Color.red;
		hitBoxColor.a = 0.6f;

		defaultGUIColor = GUI.backgroundColor;
		selectedGUIColor = Color.yellow;
	}

	void Update()
	{
		UpdateCurStepBoxes();

		// selection operations
		selection.Update(curEvent);

		hasSelectedABoxThisFrame = false;

		// should not try to select other box if the selected one is clicked
		if (selection.selectedBox != null && selection.selectedBox.IsClicked(0, curEvent))
		{
			hasSelectedABoxThisFrame = true;
		}
		// try select box
		else
		{
			foreach (var hitbox in hitboxes)
			{
				if (hitbox.IsClicked(0, curEvent))
				{
					selection.Select(hitbox);

					hasSelectedABoxThisFrame = true;

					// dont check all other boxes if one is clicked
					break;
				}
			}
		}

		// unselection
		if (!hasSelectedABoxThisFrame && selection.selectedBox != null &&
			curEvent.type == EventType.MouseDown && !selection.IsReceivingEvent())
		{
			selection.Unselect();
		}
	}

	private void OnGUI()
	{
		// event update
		curEvent = Event.current;

		// ----------------------------------------
		// move selection

		Move tempMove = curMove;

		GUI.Label(new Rect(50, 25, 50, 16), "move : ");
		curMove = (Move)EditorGUI.ObjectField(new Rect(150, 25, 300, 16),
			curMove, typeof(Move), true);

		// a new sprite has been selected
		if (tempMove != curMove)
		{
			UpdateStepsTextures();
		}

		// ----------------------------------------
		// step selection
		if (curMove != null)
		{
			for (int i = 0; i < curMove.steps.Count; ++i)
			{
				if (curMove.steps[i] == curStep)
				{
					GUI.backgroundColor = selectedGUIColor;
				}

				Sprite stepSpr = curMove.steps[i].sprite;

				if (stepSpr)
				{
					if (GUI.Button(new Rect(c_stepsButtonsPos + new Vector2(i * (c_margin + c_buttonSize.x), 0), c_buttonSize), stepsTextures[i]))
					{
						SwitchToStep(i);
					}
				}
				else
				{
					if (GUI.Button(new Rect(c_stepsButtonsPos + new Vector2(i * (c_margin + c_buttonSize.x), 0), c_buttonSize), "empty"))
					{
						SwitchToStep(i);
					}
				}

				if (curMove.steps[i] == curStep)
				{
					GUI.backgroundColor = defaultGUIColor;
				}
			}

			// highlight selected button
		}

		// ----------------------------------------
		// sprite selection
		if (curStep != null)
		{
			curSpr = curStep.sprite;

			GUI.Label(new Rect(50, 550, 50, 16), "sprite : ");
			curStep.sprite = (Sprite)EditorGUI.ObjectField(
				new Rect(150, 550, 300, 16),
				curStep.sprite,
				typeof(Sprite),
				true);

			// a new sprite has been selected
			if (curSpr != curStep.sprite)
			{
				UpdateStepsTextures(curStepIndex);
				UpdateToolTexture();
			}

			// ----------------------------------------
			// duration
			GUI.Label(new Rect(10, c_stepTextureRect.y, 50, 16), "duration : ");
			curStep.duration = EditorGUI.IntField(
				new Rect(60, c_stepTextureRect.y, 50, 16),
				curStep.duration);
		}

		if (toolTex)
		{
			// ----------------------------------------
			// add boxes
			if (GUI.Button(new Rect(40, 200, c_buttonSize.x, c_buttonSize.y), "ADD"))
			{
				AddHitbox();
			}

			// ----------------------------------------
			// remove boxes
			if (GUI.Button(new Rect(40, 200 + c_buttonSize.y + c_margin, c_buttonSize.x, c_buttonSize.y), "REM"))
			{
				RemoveHitbox(ref selection.selectedBox);
				selection.Unselect();
			}
		}

		// ----------------------------------------
		// display texture and boxes

		// sprite open in tool
		if (toolTex)
		{
			// draw sprite 
			if (toolTex != null)
			{
				EditorGUI.DrawPreviewTexture(c_stepTextureRect, toolTex);
			}

			// draw hitboxes
			foreach (var box in hitboxes)
			{
				box.Display();
			}

			// draw selection
			if (selection.selectedBox != null) selection.Display();
		}

		Repaint();
	}

	void OpenStep(int _i)
	{
		curStepIndex = _i;
		curStep = curMove.steps[_i];
		selection.Unselect();
		SetToolBoxesToCurStep();

		UpdateToolTexture();

		/*
		if (curStep.sprite != null)
		{
			CopyTexture2D(ref toolTex, stepsTextures[_i]);
		}
		else
		{
			toolTex = null;
		}
		*/
	}

	void SetToolBoxesToCurStep()
	{
		foreach (var box in curStep.hitboxes)
		{
			var tempBox = new Box();
			tempBox.dimension = box;
			tempBox.type = BoxType.HitBox;

			hitboxes.Add(tempBox);
		}
	}

	void AddHitbox()
	{
		Box hitbox = new Box();
		hitbox.type = BoxType.HitBox;
		hitbox.dimension = new Rect(110, 110, 100, 80);

		hitboxes.Add(hitbox);
		curStep.hitboxes.Add(hitbox.dimension);
	}

	void RemoveHitbox(ref Box _hitbox)
	{
		hitboxes.Remove(_hitbox);
		curStep.hitboxes.Remove(_hitbox.dimension);
	}

	void SetRefTexFromSprite(ref Texture2D _tex, Sprite _spr)
	{
		Rect sprRect = _spr.rect;
		_tex = new Texture2D((int)sprRect.width, (int)sprRect.height);
		_tex.filterMode = FilterMode.Point;

		_tex.SetPixels(_spr.texture.GetPixels((int)sprRect.x, (int)sprRect.y, (int)sprRect.width, (int)sprRect.height));
		_tex.Apply();
	}

	public void CopyTexture2D(ref Texture2D dest, Texture2D src)
	{
		if (!dest || dest.width != src.width || dest.height != src.height)
		{
			dest = new Texture2D(src.width, src.height);
		}

		dest.filterMode = FilterMode.Point;

		dest.SetPixels(src.GetPixels());
		dest.Apply();
	}

	void UpdateCurStepBoxes()
	{
		if (hitboxes != null && curStep != null)
		{
			for (int i = 0; i < hitboxes.Count; ++i)
			{
				curStep.hitboxes[i] = hitboxes[i].dimension;
			}
		}
	}

	void SwitchToStep(int _i)
	{
		hitboxes.Clear();
		OpenStep(_i);
	}

	void UpdateStepsTextures(int _index = -1)
	{
		if (_index == -1)
		{
			int nbSteps = curMove.steps.Count;
			stepsTextures = new Texture2D[nbSteps];

			for (int i = 0; i < nbSteps; i++)
			{
				if (curMove.steps[i].sprite != null)
				{
					SetRefTexFromSprite(ref stepsTextures[i], curMove.steps[i].sprite);
				}
				else
				{
					stepsTextures[i] = null;
				}
			}
		}
		else
		{
			if (curMove.steps[_index].sprite != null)
			{
				SetRefTexFromSprite(ref stepsTextures[_index], curMove.steps[_index].sprite);
			}
			else
			{
				stepsTextures[_index] = null;
			}
		}
	}

	void UpdateToolTexture()
	{
		if (stepsTextures[curStepIndex] != null)
		{
			CopyTexture2D(ref toolTex, stepsTextures[curStepIndex]);
		}
		else
		{
			toolTex = null;
		}
	}
}

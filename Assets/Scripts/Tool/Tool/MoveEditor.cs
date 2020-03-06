using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class MoveEditor : EditorWindow
{
	static readonly int c_margin = 20;
	static readonly Vector2 c_marginVec = new Vector2(c_margin, c_margin);
	static readonly Vector2 c_stepsButtonsPos = new Vector2(50, 75);
	static readonly Vector2 c_buttonSize = new Vector2(50, 50);
	static readonly Rect c_stepTextureRect = new Rect(110, 150, 350, 350);

	Texture2D tex;
	Texture2D texCustomAlpha;

	Event curEvent;

	Move curMove;
	Step curStep;
	//MoveType moveType;
	//int nbStep;
	List<Box> hitboxes = new List<Box>();
	Selection selection = new Selection();
	bool hasSelectedABoxThisFrame;

	Color hitBoxColor;

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
	}

	void Update()
    {
		// selection operations
		selection.Update(curEvent);

		hasSelectedABoxThisFrame = false;

		// should not try to select other box if the selected one is clicked
		if (selection.selectedBox != null && selection.selectedBox.IsClicked(0, curEvent))
		{
			hasSelectedABoxThisFrame = true;
		}
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
		GUI.Label(new Rect(50, 25, 50, 16), "move : ");
		curMove = (Move)EditorGUI.ObjectField(
			new Rect(150, 25, 300, 16),
			curMove,
			typeof(Move),
			true);

		// ----------------------------------------
		// step selection
		if (curMove != null)
		{
			for (int i = 0; i < curMove.steps.Count; ++i)
			{
				Texture2D stepTex = curMove.steps[i].texture;
				if (stepTex)
				{
					if (GUI.Button(new Rect(c_stepsButtonsPos, (c_buttonSize + new Vector2(i * c_margin, 0))), stepTex))
					{
						OpenStep(i);
					}
				}
				else
				{
					if (GUI.Button(new Rect(c_stepsButtonsPos, (c_buttonSize + new Vector2(i * c_margin, 0))), "empty"))
					{
						OpenStep(i);
					}
				}
			}
		}

		// ----------------------------------------
		// texture selection
		if (curStep != null)
		{
			bool texWasNull = !tex;

			// Texture 2D field
			GUI.Label(new Rect(50, 550, 50, 16), "texture : ");
			tex = (Texture2D)EditorGUI.ObjectField(
				new Rect(150, 550, 300, 16),
				tex,
				typeof(Texture2D),
				true);

			bool texSelectedThisFrame = texWasNull && tex != null;

			if (texSelectedThisFrame)
			{
				OpenTexture();
			}
		}

		if (texCustomAlpha)
		{
			// ----------------------------------------
			// add boxes
			if (GUI.Button(new Rect(40, 150, c_buttonSize.x, c_buttonSize.y), "ADD"))
			{
				AddHitbox();
			}

			// ----------------------------------------
			// remove boxes
			if (GUI.Button(new Rect(40, 150 + c_buttonSize.y + c_margin, c_buttonSize.x, c_buttonSize.y), "REM"))
			{
				RemoveHitbox(ref selection.selectedBox);
				selection.Unselect();
			}
		}

		// ----------------------------------------
		// display

		// sprite open in tool
		if (texCustomAlpha)
		{
			// draw sprite 
			EditorGUI.DrawPreviewTexture(c_stepTextureRect, texCustomAlpha);

			// draw hitboxes
			foreach (var box in hitboxes)
			{
				box.Display();
			}

			// draw selection
			if (selection.selectedBox != null)		selection.Display();
		}

		Repaint();
	}

	void OpenStep(int _i)
	{
		curStep = curMove.steps[_i];
		if (curStep.texture != null)
		{
			tex = curStep.texture;
			OpenTexture();
		}
	}

	void AddHitbox()
	{
		Box hitbox = new Box();
		hitbox.type = BoxType.HitBox;
		hitbox.dimension = new Rect(110, 110, 100, 80);

		hitboxes.Add(hitbox);
	}

	void RemoveHitbox(ref Box _hitbox)
	{
		hitboxes.Remove(_hitbox);
	}

	void OpenTexture()
	{
		CopyTexture2D(ref texCustomAlpha, tex);
		texCustomAlpha.SetAlphaToColor(Color.cyan);
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
}

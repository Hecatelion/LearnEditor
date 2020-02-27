using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Tool : EditorWindow
{
	Texture2D tex;
	Texture2D texCustomAlpha;

	Event curEvent;
	List<Box> hitboxes = new List<Box>();
	Selection selection = new Selection();
	bool hasSelectedABoxThisFrame;

	Color hitBoxColor;

	[MenuItem("Custom/Tool")]
	static void Init()
	{
		var window = GetWindow<Tool>("Baba Tool", true);
		window.position = new Rect(100, 100, 500, 500);
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

		// if ((selection.selectedBox == null || selection.selectedBox.IsClicked(0, curEvent))) // should not try to select other box if the selected one is clicked
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
		// texture selection
		bool texWasNull = !tex;
 
		// Texture 2D field
		GUI.Label(new Rect(50, 5, 50, 16), "texture : ");
		tex = (Texture2D)EditorGUI.ObjectField(
			new Rect(150, 5, 350 - 50, 16),
			tex,
			typeof(Texture2D),
			true);

		bool texSelectedThisFrame = texWasNull && tex != null;

		if (texSelectedThisFrame)
		{
			CopyTexture2D(ref texCustomAlpha, tex);
			texCustomAlpha.SetAlphaToColor(Color.cyan);
		}

		// ----------------------------------------
		// add boxes
		if (GUI.Button(new Rect(40, 110, 50, 50), "ADD"))
		{
			AddHitbox();
		}

		// ----------------------------------------
		// remove boxes
		if (GUI.Button(new Rect(40, 180, 50, 50), "REM"))
		{
			RemoveHitbox(ref selection.selectedBox);
			selection.Unselect();
		}

		// ----------------------------------------
		// display

		// sprite open in tool
		if (texCustomAlpha)
		{
			// draw sprite 
			EditorGUI.DrawPreviewTexture(new Rect(110, 110, 350, 350), texCustomAlpha);

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

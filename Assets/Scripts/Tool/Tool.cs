using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class Tool : EditorWindow
{
	Texture2D tex;
	Texture2D texCustomAlpha;

	Event curEvent;
	Box hitbox = new Box();
	Selection selection = new Selection();

	Color hitBoxColor;

	[MenuItem("Custom/Tool")]
	static void Init()
	{
		var window = GetWindow<Tool>("Baba Tool", true);
		window.position = new Rect(200, 200, 500, 500);
		window.Show();
	}

	private void OnEnable()
	{
		hitBoxColor = Color.red;
		hitBoxColor.a = 0.6f;

		hitbox.type = BoxType.HitBox;
		hitbox.dimension = new Rect(138, 100, 130, 100);
	}

	void Update()
    {
		// selection operations
		selection.Update(curEvent);
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
		// update selection

		if (hitbox.IsClicked(0, curEvent))
		{
			selection.Select(hitbox);
			selection.Display();
		}

		// ----------------------------------------
		// display

		// sprite open in tool
		if (texCustomAlpha)
		{
			// draw sprite 
			EditorGUI.DrawPreviewTexture(new Rect(100, 100, 300, 300), texCustomAlpha);

			// draw hitboxes
			hitbox.Display();

			// draw selection
			if (selection.selectedBox != null)		selection.Display();
		}

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

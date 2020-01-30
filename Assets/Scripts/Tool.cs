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
	public BoxType type;
	public Rect dimension;

	public bool IsClicked()
	{
		Event e = Event.current;
		if (e.type == EventType.MouseDown && e.button == 0)
		{
			Vector2 mousePos = e.mousePosition;

			return mousePos.x < dimension.x + dimension.width && mousePos.x > dimension.x
				&& mousePos.y < dimension.y + dimension.height && mousePos.y > dimension.y;
		}

		return false;
	}
}

public class Tool : EditorWindow
{
	Texture2D tex;
	Texture2D texCustomAlpha;

	Box hitbox = new Box();
	Box selection;

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
    { }

	private void OnGUI()
	{
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

		if (texCustomAlpha)
		{
			EditorGUI.DrawPreviewTexture(new Rect(100, 100, 300, 300), texCustomAlpha);
			EditorGUI.DrawRect(new Rect(138, 100, 130, 100), hitBoxColor);
		}

		if (hitbox.IsClicked() && selection != hitbox)
		{
			selection = hitbox;
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
}

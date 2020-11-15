using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Step
{
	//public Texture2D texture;
	public Sprite sprite;

	public int durationInFrame;
	public float DurationInSeconds { get => durationInFrame / 60.0f; }

	[HideInInspector] public int damages;
	public List<Rect> hitboxes;

	[HideInInspector] public List<Rect> hurtboxes;
	[HideInInspector] public List<Rect> grabboxes;
}

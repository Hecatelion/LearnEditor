using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Step
{
	[HideInInspector] public int duration;
	[HideInInspector] public int damages;
	[HideInInspector] public List<Rect> hitboxes;
	[HideInInspector] public List<Rect> hurtboxes;
	[HideInInspector] public List<Rect> grabboxes;
	[HideInInspector] public Texture2D texture;
}

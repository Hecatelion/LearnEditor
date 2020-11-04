using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct MoveAndInput
{
	public Move moveData;
	public KeyCode key; // use complexe input
}

public class Character : MonoBehaviour
{
	[SerializeField] List<MoveAndInput> moveList;

	SpriteRenderer renderer;

	void Start() 
	{
		renderer = GetComponent<SpriteRenderer>();

		//Texture2D texture = moveList[0].moveData.steps[0].texture;
		//renderer.sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(0.5f, 0.5f));

		Sprite sprite = moveList[0].moveData.steps[0].sprite;
		renderer.sprite = sprite;

	} // delegate inscription

	void Update()
    {
		// check if a move input has been pressed
		foreach (var moveAndInput in moveList)
		{
			if (Input.GetKeyDown(moveAndInput.key))
			{
				//Use(moveAndInput.moveData);
				Debug.Log("KeyPressed");
				break;
			}
		}
    }

	void Use(Move _move)
	{
		//_move
	}
}

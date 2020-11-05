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
	List<BoxCollider> hitboxes = new List<BoxCollider>();

	SpriteRenderer spriteRenderer;

	void Start() 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		Sprite sprite = moveList[0].moveData.steps[0].sprite;
		spriteRenderer.sprite = sprite;
		
		SetHitboxes();

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

	void SetHitboxes()
	{
		BoxCollider newCol = gameObject.AddComponent<BoxCollider>();

		SetBoxColliderDimensionsFrom(ref newCol, moveList[0].moveData.steps[0].hitboxes[0]);
	}

	void SetBoxColliderDimensionsFrom(ref BoxCollider _col, Rect _hitbox)
	{
		Rect editorTexRect = MoveEditor.c_stepTextureRect;
		Rect sprRect = spriteRenderer.sprite.rect;

		Vector2 sprSize = new Vector2(sprRect.width / 100, sprRect.height / 100);

		_col.size = new Vector3(
			(_hitbox.width / editorTexRect.width) * sprSize.x, // if (transform.scale == 1,1,1 && sprite.size == 128px,128px) then (spriteDisplayed.scale = 1.28,1.28,1.28) 
			(_hitbox.height / editorTexRect.height) * sprSize.y,
			1);

		Vector2 hbPos = new Vector2(_hitbox.x - editorTexRect.x, _hitbox.y - editorTexRect.y); // box in tool texture referential
		Vector3 colPos = new Vector3((hbPos.x / editorTexRect.width) * sprSize.x, (-hbPos.y / editorTexRect.height) * sprSize.y, 0); // *-1 because y goes up in unity world whereas it goes down in Unity GUI
		colPos += new Vector3(_col.size.x / 2, -_col.size.y / 2, 0);

		_col.center = colPos;
	}
}

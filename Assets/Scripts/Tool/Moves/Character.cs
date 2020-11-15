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
	[SerializeField] Sprite idleSprite;
	[SerializeField] List<MoveAndInput> moveList;
	List<BoxCollider> hitboxesColliders = new List<BoxCollider>();

	SpriteRenderer spriteRenderer;

	void Start() 
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		SetCurStepIdle();

	}

	void Update()
    {
		foreach (var moveAndInput in moveList)
		{
			if (Input.GetKeyDown(moveAndInput.key))
			{
				StartCoroutine(Use(moveAndInput.moveData));
				break;
			}
		}
	}

	IEnumerator Use(Move _move)
	{
		foreach (Step step in _move.steps)
		{
			SetCurStep(step);
			yield return new WaitForSeconds(step.DurationInSeconds);
		}

		SetCurStepIdle();
		yield return null;
	}

	void SetCurStep(Step _step)
	{
		spriteRenderer.sprite = _step.sprite;
		SetHitboxes(_step);
	}

	void SetCurStepIdle()
	{
		spriteRenderer.sprite = idleSprite;
		FreeHitboxesColliders();
	}

	void SetHitboxes(Step _step)
	{
		FreeHitboxesColliders();

		foreach (var box in _step.hitboxes)
		{
			BoxCollider newCol = gameObject.AddComponent<BoxCollider>();
			SetBoxColliderDimensionsFrom(newCol, box);

			hitboxesColliders.Add(newCol);
		}
	}

	void FreeHitboxesColliders()
	{
		foreach (var col in hitboxesColliders)
		{
			Destroy(col);
		}

		hitboxesColliders.Clear();
	}

	void SetBoxColliderDimensionsFrom(BoxCollider _col, Rect _hitbox)
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

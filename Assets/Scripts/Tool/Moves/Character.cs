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
	Event evt;

	void Start() {} // delegate inscription

	void Update()
    {
		evt = Event.current;

		// check if a move input has been pressed
		foreach (var moveAndInput in moveList)
		{
			if (moveAndInput.key == evt.keyCode)
			{
				//Use(moveAndInput.moveData);
				break;
			}
		}
    }

	void Use(Move _move)
	{
		//_move
	}
}

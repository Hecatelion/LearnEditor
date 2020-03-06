using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	[SerializeField] List<MoveType> moveList;
	Event evt;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		evt = Event.current;

		foreach (var move in moveList)
		{
			//if (move.key == evt.keyCode)
			{
				//Use(move);
				break;
			}
		}
    }

	void Use(Move _move)
	{
		//_move
	}
}

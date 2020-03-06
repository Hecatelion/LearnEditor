using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
struct SerializableMove
{
	public MoveType type;
	public Move move;
}

[System.Serializable]
struct SerializableCharacter
{
	public CharacterType characterType;
	public List<SerializableMove> moveList;
}

public class OLD_MoveManager : MonoBehaviour
{
	static OLD_MoveManager instance;

	[SerializeField] List<SerializableCharacter> fieldCharacterMoves;
	Dictionary<CharacterType, Dictionary<MoveType, Move>> characterMoves;

    void Start()
    {
		if (instance == null)
		{
			instance = this;
			Init();
		}
		else
		{
			Destroy(this.gameObject);
		}
    }

	void Init()
	{
		if (fieldCharacterMoves != null)
		{
			characterMoves = new Dictionary<CharacterType, Dictionary<MoveType, Move>>();

			foreach (var fieldMoveCharMove in fieldCharacterMoves)
			{
				characterMoves.Add(fieldMoveCharMove.characterType, new Dictionary<MoveType, Move>());
				Dictionary<MoveType, Move> moves = characterMoves[fieldMoveCharMove.characterType];

				foreach (var fieldMove in fieldMoveCharMove.moveList)
				{
					moves.Add(fieldMove.type, fieldMove.move);
				}
			}
		}

		_ = 0;
	}

	public static Move Get(CharacterType _characterType, MoveType _moveType)
	{
		return instance.characterMoves[_characterType][_moveType];
	}
}

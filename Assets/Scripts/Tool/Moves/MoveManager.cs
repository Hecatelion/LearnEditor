using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveManager : MonoBehaviour
{
	[System.Serializable]
	internal struct MoveData
	{
		public MoveType type;
	}

	static MoveManager instance;

	[SerializeField] public List<Move> moveDatas;

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
	{ }

	/*public static Move Get(CharacterType _characterType, MoveType _moveType)
	{
		return (from moveData in instance.moveDatas where moveData.type == _moveType select moveData).First().data;
	}*/
}

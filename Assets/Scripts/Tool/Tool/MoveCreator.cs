using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class MoveCreator : EditorWindow
{
	Event curEvent;

	MoveType moveType;
	int nbStep;
	int nbStepsLastFrame;
	List<Texture2D> textures;

	Move move;

	[MenuItem("Custom/MoveCreator")]
	static void Init()
	{
		var window = GetWindow<MoveCreator>("Baba Tool", true);
		window.position = new Rect(100, 100, 500, 500);
		window.Show();
	}

	private void OnEnable()
	{ }

	void Update()
	{ }

	private void OnGUI()
	{
		/*
		// event update
		curEvent = Event.current;

		// ----------------------------------------
		// move selection
		GUI.Label(new Rect(25, 25, 100, 16), "Nb Steps: ");
		moveType = (MoveType)EditorGUI.EnumPopup(new Rect(100, 25, 100, 16), moveType);

		// nb steps
		if (moveType != MoveType.None)
		{
			nbStepsLastFrame = nbStep;

			GUI.Label(new Rect(250, 25, 100, 16), "Nb Steps: ");
			nbStep = EditorGUI.IntField(new Rect(350, 25, 50, 16), "", nbStep);

			// get dog amount in chenil and check if value changes
			int stepDelta = nbStep - nbStepsLastFrame;

			// on value change, reallocate tab
			if (stepDelta != 0)
			{
				if (stepDelta > 0)
				{
					for (int i = 0; i < stepDelta; i++)
					{
						.Add(new Chien());
					}
				}
				else if (dogDelta < 0)
				{
					chenil.chiens.RemoveRange(chenil.nbChiens - 1, -stepDelta);
				}
			}

			// build asset
			if (nbStep > 0)
			{
				for (int i = 0; i < nbStep; i++)
				{

				}

				if (GUI.Button(new Rect(450, 20, 60, 24), "Save"))
				{
					Move.CreateAsset(moveType.ToString());
				}
			}
		}

		// sprites
		EditorGUI.

		Repaint();
		*/
	}
}


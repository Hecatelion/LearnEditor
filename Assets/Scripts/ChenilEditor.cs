using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(Chenil))]
[CanEditMultipleObjects]
public class ChenilEditor : Editor
{
	private void OnEnable()
	{
		//FillNewChenil(target as Chenil);
	}

	// Start is called before the first frame update
	void Start()
	{
		
	}

	// Update is called once per frame
	void Update()
	{ }

	public override void OnInspectorGUI()
	{
		serializedObject.Update();
		var chenil = target as Chenil;

		// get dog amount in chenil and check if value changes
		int nbChiensLastFrame = chenil.nbChiens;
		chenil.nbChiens = EditorGUILayout.IntField("dog amount", chenil.nbChiens);
		int dogDelta = chenil.nbChiens - nbChiensLastFrame;

		// on value change, reallocate tab
		if (dogDelta != 0)
		{
			if (dogDelta > 0)
			{
				for (int i = 0; i < dogDelta; i++)
				{
					chenil.chiens.Add(new Chien());
				}
			}
			else if (dogDelta < 0)
			{
				chenil.chiens.RemoveRange(chenil.nbChiens - 1, -dogDelta);
			}
		}

		// foreach dog, get details
		for (int i = 0; i < chenil.nbChiens; i++)
		{
			Chien tempChien = chenil.chiens[i];

			GUILayout.Space(10);
			GUILayout.Label("Dog " + i);
			EditorGUI.indentLevel++;
			GUILayout.BeginVertical();

			// ears
			tempChien.hasEars = EditorGUILayout.Toggle("has ears ?", tempChien.hasEars);
			if (tempChien.hasEars)
			{
				tempChien.nbEars = EditorGUILayout.IntField("ears amount", tempChien.nbEars);
			}

			// foot
			tempChien.hasFoot = EditorGUILayout.Toggle("has foot ?", tempChien.hasFoot);
			if (tempChien.hasFoot)
			{
				tempChien.nbFoot = EditorGUILayout.IntField("foot amount", tempChien.nbFoot);
			}
			EditorGUI.indentLevel--;

			GUILayout.EndVertical();
		}

		serializedObject.ApplyModifiedProperties();
	}

	void FillNewChenil(Chenil _chenil)
	{
		_chenil.chiens = new List<Chien>(_chenil.nbChiens);
		for (int i = 0; i < _chenil.nbChiens; i++)
		{
			_chenil.chiens.Add(new Chien());
		}
	}
}

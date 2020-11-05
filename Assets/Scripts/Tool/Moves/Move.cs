using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu(fileName = "DefaultMove", menuName = "Move", order = 1)]
public class Move : ScriptableObject
{
	[SerializeField] public List<Step> steps;
}
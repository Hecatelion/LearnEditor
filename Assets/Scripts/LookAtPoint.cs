//C# Example (LookAtPoint.cs)
using UnityEngine;
using UnityEditor;


[ExecuteInEditMode]
public class LookAtPoint : MonoBehaviour
{
	public Vector3 lookAtPoint = Vector3.zero;

	public void Update()
	{
		transform.LookAt(lookAtPoint);
	}
}
using UnityEditor;
using UnityEngine;

public class LookAtPointWindow : EditorWindow
{
	bool groupEnabled;
	public float x = 0.0f;
	public float y = 0.0f;
	public float z = 0.0f;

	// Add menu item named "My Window" to the Window menu
	[MenuItem("Window/LookAtPointWindow")]
	static void Init()
	{
		// Get existing open window or if none, make a new one:
		LookAtPointWindow window = (LookAtPointWindow)EditorWindow.GetWindow(typeof(LookAtPointWindow));
		window.Show();
	}
	/*public static void ShowWindow()
	{
		//Show existing window instance. If one doesn't exist, make one.
		EditorWindow.GetWindow(typeof(LookAtPointWindow));
	}*/

	void OnGUI()
	{
		groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", groupEnabled);

		if (groupEnabled)
		{
			x = EditorGUILayout.Slider("x", x, -5, 5);
			y = EditorGUILayout.Slider("y", y, -5, 5);
			z = EditorGUILayout.Slider("z", z, -5, 5);
		}

		EditorGUILayout.EndToggleGroup();
	}
}
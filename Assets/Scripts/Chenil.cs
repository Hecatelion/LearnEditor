using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chenil : MonoBehaviour
{
	public int nbChiens;
	public List<Chien> chiens;

    // Start is called before the first frame update
    void Start()
    {
		foreach (var chien in chiens)
		{
			if (chien.nbFoot > 4)
			{
				Debug.Log("Gambade");
			}
			if (chien.nbEars > 2)
			{
				Debug.Log("Woof");
			}

			Debug.Log("-");
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

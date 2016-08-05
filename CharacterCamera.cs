using UnityEngine;
using System.Collections;
using KGF;

public class CharacterCamera : MonoBehaviour 
{
	public KGFOrbitCamSettings settings;
	//public KGFOrbitCam camera;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("CharacterCamera start, calling settings.Apply()");
	}

	// Update is called once per frame
	void Update () 
	{
		settings.Apply();
	}
}

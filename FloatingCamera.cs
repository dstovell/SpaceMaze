using UnityEngine;
using System.Collections;
using KGF;

public class FloatingCamera : MonoBehaviour 
{
	public KGFOrbitCamSettings settings;
	public PipeMazePlayerMachine player;
	//public KGFOrbitCam camera;

	// Use this for initialization
	void Start () 
	{
		Debug.Log("CharacterCamera start, calling settings.Apply()");
	}

	// Update is called once per frame
	void Update () 
	{
		if (!player.MaintainingGround())
		{
			settings.Apply();
		}
	}
}

using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using TouchScript;
using TouchScript.Gestures;

public class PipeMazePlayerInput : MonoBehaviour 
{
	public class PlayerInput
	{
		public bool Walk = false;
		public bool Run = false;
		public bool Left = false;
		public bool Right = false;
		public bool Jump = false;
		public bool Dive = false;

	    public Vector3 MoveInput;
	    public Vector2 MouseInput;
	    public bool JumpInput;
	}
    public PlayerInput Current;

	public PipeMazePlayerMachine Player;

	public PipeMazeMagnetic MagneticBoots;

	void Awake() 
	{
	}

	// Use this for initialization
	void Start() 
	{
		Screen.orientation = ScreenOrientation.Portrait;
        Current = new PlayerInput();
		Player = gameObject.GetComponent<PipeMazePlayerMachine>();
	}

	// Update is called once per frame
	void Update() 
	{
		this.Current.Dive = false;
		this.Current.Jump = false;
	}

	//private int lastChargeDir
	void OnGUI () 
	{
		GUILayout.BeginArea(new Rect(10, 10, 400, 600));

		if (this.Player != null)
		{
			GUILayout.Box("State: " + this.Player.currentState);
			//GUILayout.Box("GroundDist: " + this.Player.controller.currentGround.Distance);
			//GUILayout.Box("Speed: " + this.Player.rb.velocity.magnitude);
		}

		if (this.MagneticBoots != null)
		{
			Vector3 chargeDir = this.MagneticBoots.CalculateLocalChargeForce();
			GUILayout.Box("Charge: " + this.MagneticBoots.charge.ToString());
			GUILayout.Box("NearbyMagnetics: " + ((this.MagneticBoots.nearbyMagnetics.Count > 0) ? (this.MagneticBoots.nearbyMagnetics[0].name + "(" + this.MagneticBoots.nearbyMagnetics[0].charge.ToString() + ")") : "none"));
			GUILayout.Box("Charge Dir: (" + chargeDir.x.ToString("#.00") + "," + chargeDir.y.ToString("#.00") + "," + chargeDir.z.ToString("#.00") +")");
		}

        GUILayout.EndArea();
	}

	private void OnEnable()
    {
       	FlickGesture [] flicks = Camera.main.GetComponents<FlickGesture>();
		for (int i=0; i<flicks.Length; i++)
		{
			flicks[i].Flicked += this.FlickedHandler;
		}

		TapGesture [] taps = Camera.main.GetComponents<TapGesture>();
		for (int i=0; i<taps.Length; i++)
		{
			taps[i].Tapped += this.TapHandler;
		}
    }

    private void OnDisable()
    {
       	FlickGesture [] flicks = Camera.main.GetComponents<FlickGesture>();
		for (int i=0; i<flicks.Length; i++)
		{
			flicks[i].Flicked -= this.FlickedHandler;
		}

		TapGesture [] taps = Camera.main.GetComponents<TapGesture>();
		for (int i=0; i<taps.Length; i++)
		{
			taps[i].Tapped -= this.TapHandler;
		}
    }

	private void FlickedHandler(object sender, EventArgs e)
	{
		Debug.LogError("FlickedHandler");
		FlickGesture gesture = sender as FlickGesture;
		if (gesture != null)
		{
			//Debug.LogError("FlickedHandler got FlickGesture Direction=" + gesture.Direction.ToString() + " ScreenFlickVector=" + gesture.ScreenFlickVector.x + "," + gesture.ScreenFlickVector.y);
			if (Mathf.Abs(gesture.ScreenFlickVector.x) >= Mathf.Abs(gesture.ScreenFlickVector.y))
			{
				if (gesture.ScreenFlickVector.x > 0)
				{
					//Right
					Debug.LogError("Right");
				}
				else
				{
					//Left
					Debug.LogError("Left");
				}
			}
			else
			{
				if (gesture.ScreenFlickVector.y > 0)
				{
					//Up
					Debug.LogError("Jump");
					this.Current.Jump = true;
				}
				else
				{
					//Down
					Debug.LogError("Dive");
					this.Current.Dive = true;
				}
			}
		}
	}

	private void TapHandler(object sender, EventArgs e)
	{
		TapGesture gesture = sender as TapGesture;
		if (gesture != null)
		{
			this.Current.Run = !this.Current.Run;
		}
	}
}

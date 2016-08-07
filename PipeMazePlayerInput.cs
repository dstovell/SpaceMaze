using UnityEngine;
using System.Collections;

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

	// Use this for initialization
	void Start() 
	{
        Current = new PlayerInput();
		Player = gameObject.GetComponent<PipeMazePlayerMachine>();
	}

	// Update is called once per frame
	void Update() 
	{
	}

	void OnGUI () 
	{
		GUILayout.BeginArea(new Rect(10, 10, 200, 600));


		GUILayout.Box("State: " + Player.currentState);
		//GUILayout.Box("GroundDist: " + Player.controller.currentGround.Distance);
		GUILayout.Box("Speed: " + Player.rb.velocity.magnitude);

		bool isMoving = this.Current.Run || this.Current.Walk;

		if (!isMoving)
		{
			if (GUILayout.Button("Run"))
			{
				this.Current.Run = true;
			}
			if (GUILayout.Button("Walk"))
			{
				this.Current.Walk = true;
			}
		}
		else
		{
			if (GUILayout.Button("Stop"))
			{
				this.Current.Run = false;
				this.Current.Walk = false;
			}
		}

		this.Current.Left = false;
		if (GUILayout.Button("Left"))
		{
			this.Current.Left = true;
		}

		this.Current.Right = false;
		if (GUILayout.Button("Right"))
		{
			this.Current.Right = true;
		}

		this.Current.Jump = false;
		if (GUILayout.Button("Jump"))
		{
			this.Current.Jump = true;
		}

		this.Current.Dive = false;
		if (GUILayout.Button("Dive"))
		{
			this.Current.Dive = true;
		}


        GUILayout.EndArea();
	}
}

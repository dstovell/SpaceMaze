using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PipeMazePipeComponent : PipeMazeComponent
{
	public void Awake()
	{
		//Debug.Log("PipeMazePipeComponent Awake " + this.name);

		this.Init();
	}
}


using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PipeMazeJunctionComponent : PipeMazeComponent
{
	public GameObject leftLink;
	public GameObject rightLink;
	public GameObject upLink;
	public GameObject downLink;

	private PipeMazeGoal goal = null;


	public void Awake()
	{
		//Debug.Log("PipeMazeJunctionComponent Awake " + this.name);

		this.Init();
		this.goal = this.gameObject.GetComponent<PipeMazeGoal>();
	}

	public void AddLink(GameObject anchorObject)
	{
		PipeMazeLink link = new PipeMazeLink(anchorObject, PipeMazeManager.Instance().PickRandomPipe(), PipeMazeManager.Instance().PickRandomJunction());
		this.links.Add(link);
	}

	public void Extend(int depth = 1)
    {
		//Debug.LogError("PipeMazeJunctionComponent Extend " + depth);

    	if (this.goal != null)
    	{
    		return;
    	}

		if (this.links == null) 
		{
			this.links = new List<PipeMazeLink>();

			if (this.forwardLink != null) AddLink(this.forwardLink);
			if (this.leftLink != null) AddLink(this.leftLink);
			if (this.rightLink != null) AddLink(this.rightLink);
			if (this.upLink != null) AddLink(this.upLink);
			if (this.downLink != null) AddLink(this.downLink);
		}

		if (depth > 1)
		{
			for (int i=0; i<this.links.Count; i++)
			{
				if (this.links[i].junctionComponent != null)
				{
					this.links[i].junctionComponent.Extend(depth-1);
				}
			}
		}
    }

	void OnTriggerEnter(Collider other)
	{
		//Debug.Log("PipeMazeJunctionComponent OnTriggerEnter " + other.name);

		PipeMazeActorComponent actor = other.gameObject.GetComponent<PipeMazeActorComponent>();
		if (actor != null)
		{
			actor.OnEnterJunction(this);
		}

		if (this.goal != null)
		{
			GameObject achiever = (other != null) ? other.gameObject : null;
			if (achiever != null)
			{
				this.goal.OnAchieved(achiever);
			}
		}
		else
		{
			Extend(2);
		}
	}

	void OnTriggerExit(Collider other)
	{
		PipeMazeActorComponent actor = other.gameObject.GetComponent<PipeMazeActorComponent>();
		if (actor != null)
		{
			actor.OnExitJunction(this);
		}
	}
}

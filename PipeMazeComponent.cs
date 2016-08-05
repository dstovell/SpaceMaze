using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PipeMazeGoal : MonoBehaviour
{
	virtual public void OnAchieved(GameObject achiever)
	{
	}
}

public class PipeMazeLink
{
	public PipeMazeLink(GameObject anchorObject, GameObject pipePrefab, GameObject junctionPrefab)
	{
		Quaternion rotation = Quaternion.identity; //Quaternion.LookRotation(anchorObject.transform.forward, anchorObject.transform.up);

		this.positon = this.CalculateAnchorPlacement(anchorObject, pipePrefab);
		this.pipe = GameObject.Instantiate(pipePrefab, this.positon, rotation) as GameObject;
		this.pipeComponent = this.pipe.GetComponentInChildren<PipeMazePipeComponent>();
		this.pipe.name = anchorObject.name + "_pipe";

		Vector3 junctionPos = this.CalculateAnchorPlacement(this.pipeComponent.inLink, junctionPrefab);
		//this.junction = GameObject.Instantiate(junctionPrefab, junctionPos, rotation) as GameObject;
		//this.junctionComponent = this.junction.GetComponentInChildren<PipeMazeJunctionComponent>();
	}

	public Vector3 CalculateAnchorPlacement(GameObject anchorObject, GameObject prefab)
	{
		Vector3 pos = Vector3.zero;
		PipeMazeComponent comp = prefab.GetComponentInChildren<PipeMazeComponent>();
		if (comp != null)
		{
			Vector3 offset = comp.GetAnchorOffset(comp.inLink);
			pos = anchorObject.transform.position + offset;
		}

		return pos;
	}

	public Vector3 positon;
	public GameObject pipe;
	public GameObject junction;
	public PipeMazePipeComponent pipeComponent;
	public PipeMazeJunctionComponent junctionComponent;
}

public class PipeMazeComponent : MonoBehaviour
{
	public GameObject forwardLink;

	public GameObject inLink;

	protected List<PipeMazeLink> links = null;

	protected void Init()
	{
	}

	public Vector3 GetAnchorOffset(GameObject anchorObject)
	{
		Vector3 offset = this.transform.position - anchorObject.transform.position;
		return offset;
	}
}

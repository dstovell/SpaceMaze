﻿using UnityEngine;
using System.Collections;

public class PipeGenerator : MonoBehaviour
{
	public GameObject [] panelPrefabs;
	public GameObject [] panelEdgePrefabs;

	public void Awake()
    {
    }

	public void OnDestroy() 
	{
    }

	public void Start()
    {
    }

	public void Update()
    {
    }

	void OnGUI () 
	{
		GUILayout.BeginArea(new Rect(10, 10, 200, 600));
		for (int i=0; i<panelPrefabs.Length; i++)
		{
			if (GUILayout.Button("Generate Pipe All " + panelPrefabs[i].name))
			{
				GeneratePipe(i);
			}
		}
        GUILayout.EndArea();
	}

	public GameObject PickRandomPanel()
    {
    	int panelCount = panelPrefabs.Length;
    	if (panelCount == 0)
    	{
    		return null;
    	}

		int randomIndex = Random.Range(0, panelCount);

    	return panelPrefabs[randomIndex];
    }

    public void GeneratePipe(int prefabIndex)
    {
		GameObject pipePrefab = new GameObject("PipePrefab");
		pipePrefab.transform.position = Vector3.zero;

		GameObject pipe = new GameObject("Pipe");
		pipe.transform.position = Vector3.zero;
		pipe.transform.parent = pipePrefab.transform;

		GameObject panelPrefab = this.panelPrefabs[prefabIndex];
		float panelSize = 1.5f;

		int circularSegmentCount = 8;
		int lengthSegmentCount = 20;
		float pipeRadius = 1.8f;

		float deltaAngle = 2.0f*Mathf.PI / (float)circularSegmentCount;
		for (int i=0; i<lengthSegmentCount; i++)
		{
			bool addEdge = ( (i == 0) || (i == (lengthSegmentCount-1)) );
			for (float angle=deltaAngle; angle<(2*Mathf.PI); angle+=deltaAngle)
			{
				float x = pipeRadius * Mathf.Cos(angle);
				float y = pipeRadius * Mathf.Sin(angle);
				float z = panelSize * (float)i;
				Vector3 position = new Vector3(x, y, z);
				Quaternion rotation = Quaternion.AngleAxis(Mathf.Rad2Deg * angle + 90, Vector3.forward);

				GameObject panel = GameObject.Instantiate(panelPrefab, position, rotation) as GameObject;
				panel.transform.parent = pipe.transform;

				if (addEdge)
				{
					float offsetScaler = 0.5f*panelSize;
					if (i == 0) 
					{
						offsetScaler *= -1.0f;
					}

					Vector3 edgePosition = position + offsetScaler*Vector3.forward;
					GameObject panelEdge = GameObject.Instantiate(panelEdgePrefabs[0], edgePosition, rotation) as GameObject;
					panelEdge.transform.parent = pipe.transform;
					if (i == 0) 
					{
						panelEdge.transform.Rotate(0f, 180f, 0f);
					}
				}
			}
		}


		GameObject inLink = new GameObject("inLink");
		inLink.transform.position = Vector3.zero;
		inLink.transform.parent = pipePrefab.transform;

		GameObject forwardLink = new GameObject("forwardLink");
		forwardLink.transform.position = new Vector3(0f, 0f, panelSize*(float)(lengthSegmentCount-1));
		forwardLink.transform.parent = pipePrefab.transform;

		PipeMazePipeComponent pipeComp = pipe.AddComponent<PipeMazePipeComponent>();
		pipeComp.inLink = inLink;
		pipeComp.forwardLink = forwardLink;

		//Move it so its easy to see
		pipePrefab.transform.position = new Vector3(0,10,0);
    }
}


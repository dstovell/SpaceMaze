using UnityEngine;
using System.Collections;

public class PipeMaze
{
	private GameObject rootObject = null;
	private PipeMazeComponent rootJunction = null;

	public PipeMaze(GameObject rootPrefab)
	{
		if (rootPrefab != null)
		{
			this.rootObject = GameObject.Instantiate(rootPrefab, Vector3.zero, Quaternion.identity) as GameObject;
			this.rootJunction = rootObject.GetComponent<PipeMazeComponent>();
		}
	}

	public void DestroyMaze()
	{
		GameObject.Destroy(this.rootObject);
		this.rootObject = null;
	}

	public PipeMazeComponent GetRootComponent() { return this.rootJunction; }
}

public class PipeMazeManager : MonoBehaviour
{
	public GameObject [] junctionPrefabs;
	public GameObject [] pipePrefabs;

	private PipeMaze currentMaze = null; 

	private static PipeMazeManager instance = null;
	public static PipeMazeManager Instance()
    {
    	return instance;
    }

	public void Awake()
    {
		instance = this;
    }

	public void OnDestroy() 
	{
		if (this.currentMaze != null)
		{
			this.currentMaze.DestroyMaze();
			this.currentMaze = null;
		}

		instance = null;
    }

	public void Start()
    {
		InitMaze();
    }

	public void Update()
    {
    }

	public GameObject PickRandomJunction()
    {
    	int junctionCount = junctionPrefabs.Length;
    	if (junctionCount == 0)
    	{
    		return null;
    	}

		int randomIndex = Random.Range(0, junctionCount);

    	return junctionPrefabs[randomIndex];
    }

	public GameObject PickRandomPipe()
    {
		int pipeCount = pipePrefabs.Length;
		if (pipeCount == 0)
    	{
    		return null;
    	}

		int randomIndex = Random.Range(0, pipeCount);

		return pipePrefabs[randomIndex];
    }

    public void InitMaze()
    {
		this.currentMaze = new PipeMaze(this.PickRandomJunction());
    }

    public PipeMaze GetMaze()
    {
    	return this.currentMaze;
    }
}


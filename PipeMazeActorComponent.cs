using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class PipeMazeActorComponent : MonoBehaviour
{
	public enum Type
	{
		Player,
		Ally,
		Enemy
	}

	public Type type = Type.Player;

	public void Awake()
	{
	}

	public void OnEnterJunction(PipeMazeJunctionComponent junction)
	{
		Debug.LogError("OnEnterJunction junction " + junction.gameObject.name);
	}

	public void OnExitJunction(PipeMazeJunctionComponent junction)
	{
		Debug.LogError("OnExitJunction junction " + junction.gameObject.name);
	}
}

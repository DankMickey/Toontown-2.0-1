using System;
using UnityEngine;

[Serializable]
public class WorldEdges : MonoBehaviour
{
	public int edgeHeight;

	public int edgeDepth;

	public float worldSize;

	public GameObject up;

	public GameObject down;

	public GameObject left;

	public GameObject right;

	public WorldEdges()
	{
		this.edgeHeight = 50;
		this.edgeDepth = 5;
		this.worldSize = 128f;
	}

	public override void Start()
	{
	}

	public override void Main()
	{
	}
}

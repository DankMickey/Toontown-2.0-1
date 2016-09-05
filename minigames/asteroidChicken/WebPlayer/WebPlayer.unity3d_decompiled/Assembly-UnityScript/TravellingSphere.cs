using System;
using UnityEngine;

[Serializable]
public class TravellingSphere : MonoBehaviour
{
	private Vector3 vel;

	public override void Start()
	{
		this.vel = new Vector3((float)1, (float)0, (float)0);
		MonoBehaviour.print("START");
	}

	public override void FixedUpdate()
	{
		MonoBehaviour.print("FA");
		this.get_rigidbody().AddForce(0.1f * this.vel, 1);
	}

	public override void Main()
	{
	}
}

using System;
using UnityEngine;

[Serializable]
public class Bouncer : MonoBehaviour
{
	public float jumpSpeed;

	public Bouncer()
	{
		this.jumpSpeed = 1f;
	}

	public override void OnCollisionEnter(Collision col)
	{
		col.get_rigidbody().AddForce(this.jumpSpeed * Vector3.get_up(), 2);
	}

	public override void Main()
	{
	}
}

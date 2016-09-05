using Boo.Lang.Runtime;
using System;
using UnityEngine;

[Serializable]
public class PlayerCollisions : MonoBehaviour
{
	public int jumpSpeed;

	public override void OnCollisionStay(Collision collision)
	{
		if (RuntimeServices.ToBool(RuntimeServices.Invoke(collision.get_gameObject(), "CompareTag", new object[]
		{
			"Bounce"
		})))
		{
			Vector3 vector = collision.get_transform().TransformDirection(Vector3.get_up());
			this.get_rigidbody().set_velocity(vector * (float)this.jumpSpeed);
		}
	}

	public override void Main()
	{
	}
}

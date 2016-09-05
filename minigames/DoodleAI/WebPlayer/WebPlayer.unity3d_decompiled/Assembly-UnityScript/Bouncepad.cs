using Boo.Lang.Runtime;
using System;
using UnityEngine;

[AddComponentMenu("Third Person Props/Jump pad"), RequireComponent(typeof(BoxCollider))]
[Serializable]
public class Bouncepad : MonoBehaviour
{
	public float jumpHeight;

	public Bouncepad()
	{
		this.jumpHeight = 5f;
	}

	public override void OnTriggerEnter(Collider col)
	{
		if (col.get_gameObject().get_layer() == 8)
		{
			Vector3 position = this.get_transform().get_position();
			MonoBehaviour.print("Trigger!!!!!!!!!!!!!!!!!!!!!!!!");
			col.get_rigidbody().set_isKinematic(false);
			MonoBehaviour.print("Is KineMatic Is Off!!!!!!!!!!!!!");
			col.get_rigidbody().AddForce(Vector3.get_up() * (float)1000, 5);
			MonoBehaviour.print("BOING!!!!!!!!!!!!!!!!!!!!!!!!!!!");
		}
	}

	public override void Reset()
	{
		if (RuntimeServices.EqualityOperator(this.get_collider(), null))
		{
			this.get_gameObject().AddComponent(typeof(BoxCollider));
		}
		RuntimeServices.SetProperty(this.get_collider(), "isTrigger", true);
	}

	public override void Main()
	{
	}
}

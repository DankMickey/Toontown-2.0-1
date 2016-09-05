using System;
using UnityEngine;

[Serializable]
public class MissileLauncher : MonoBehaviour
{
	public Rigidbody projectile;

	public int speed;

	public MissileLauncher()
	{
		this.speed = 20;
	}

	public override void Update()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			Rigidbody rigidbody = (Rigidbody)Object.Instantiate(this.projectile, this.get_transform().get_position(), this.get_transform().get_rotation());
			rigidbody.set_velocity(this.get_transform().TransformDirection(new Vector3((float)0, (float)0, (float)this.speed)));
		}
	}

	public override void Main()
	{
	}
}

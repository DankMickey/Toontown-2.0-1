using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[RequireComponent(typeof(Rigidbody))]
[Serializable]
public class Rocket : MonoBehaviour
{
	public GameObject explosion;

	public float timeOut;

	public int bounceCount;

	public int numOfBounces;

	public Rocket()
	{
		this.timeOut = 3f;
		this.numOfBounces = 5;
	}

	public override void Start()
	{
	}

	public override void OnCollisionEnter(Collision collision)
	{
		checked
		{
			this.bounceCount++;
			if (RuntimeServices.EqualityOperator(UnityRuntimeServices.GetProperty(collision.get_gameObject(), "tag"), "notTray"))
			{
				this.Kill();
			}
			if (this.bounceCount > this.numOfBounces)
			{
				ContactPoint contactPoint = collision.get_contacts()[0];
				Quaternion quaternion = Quaternion.FromToRotation(Vector3.get_up(), contactPoint.get_normal());
				Object.Instantiate(this.explosion, contactPoint.get_point(), quaternion);
				this.Kill();
			}
		}
	}

	public override void Kill()
	{
		ParticleEmitter particleEmitter = (ParticleEmitter)this.GetComponentInChildren(typeof(ParticleEmitter));
		if (particleEmitter)
		{
			particleEmitter.set_emit(false);
		}
		Object.Destroy(this.get_gameObject());
	}

	public override void Main()
	{
	}
}

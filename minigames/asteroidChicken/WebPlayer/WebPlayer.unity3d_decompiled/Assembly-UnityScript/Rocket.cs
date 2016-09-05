using System;
using UnityEngine;

[Serializable]
public class Rocket : MonoBehaviour
{
	public static float Speed = 12f;

	private float searchRadius;

	private GameObject enemySearchedLocation;

	private float turnDamping;

	public Rocket()
	{
		this.searchRadius = 45f;
		this.turnDamping = 0.1f;
	}

	public override void Start()
	{
		Collider[] array = Physics.OverlapSphere(this.get_transform().get_position(), this.searchRadius);
		int i = 0;
		Collider[] array2 = array;
		int length = array2.get_Length();
		checked
		{
			while (i < length)
			{
				if (array2[i].get_tag() == "Enemy")
				{
					this.enemySearchedLocation = array2[i].get_gameObject();
					break;
				}
				i++;
			}
			this.Invoke("Kill", 8f);
		}
	}

	public override void Update()
	{
		if (this.enemySearchedLocation)
		{
			Vector3 vector = this.enemySearchedLocation.get_transform().get_position() - this.get_transform().get_position();
			vector.Normalize();
			this.get_transform().set_forward(Vector3.Lerp(this.get_transform().get_forward(), vector, this.turnDamping));
			this.get_rigidbody().set_velocity(this.get_transform().get_forward() * Rocket.Speed);
			this.get_transform().LookAt(this.get_transform().get_position() + this.get_transform().get_forward());
		}
		else
		{
			this.get_rigidbody().set_velocity(this.get_transform().get_forward() * Rocket.Speed);
			this.get_transform().LookAt(this.get_transform().get_position() + this.get_transform().get_forward());
		}
	}

	public override void OnTriggerEnter(Collider other)
	{
		Asteroid asteroid = (Asteroid)other.get_transform().GetComponent(typeof(Asteroid));
		if (asteroid != null)
		{
			asteroid.MissileHit();
			this.Kill();
		}
	}

	public override void Kill()
	{
		ParticleEmitter particleEmitter = (ParticleEmitter)this.GetComponentInChildren(typeof(ParticleEmitter));
		if (particleEmitter)
		{
			particleEmitter.set_emit(false);
		}
		this.get_transform().DetachChildren();
		Object.Destroy(this.get_gameObject());
	}

	public override void Main()
	{
	}
}

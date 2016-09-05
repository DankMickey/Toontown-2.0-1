using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class JellbeanCapsule : MonoBehaviour
{
	public float maxAsteroidVelocity;

	public float minAsteroidVelocity;

	private Vector3 randomDirection;

	private int asteroidSize;

	public GameObject explosionPrefab;

	public AudioClip explosionSound;

	private Vector3 velocity;

	private int asteroidHitPoints;

	public bool happened;

	public JellbeanCapsule()
	{
		this.asteroidHitPoints = 4;
	}

	public override void Setup(int size, float velocityScale)
	{
		this.asteroidSize = size;
		this.get_transform().set_localScale(this.get_transform().get_localScale() * (float)size);
		float num = Random.Range(-1f, 1f);
		float num2 = Random.Range(-1f, 1f);
		float num3 = Random.Range(-1f, 1f);
		this.randomDirection = new Vector3(num, num2, num3);
		float num4 = Mathf.Lerp(this.minAsteroidVelocity, this.maxAsteroidVelocity, Random.get_value());
		num4 *= velocityScale;
		this.velocity = this.randomDirection * num4;
	}

	public override void FixedUpdate()
	{
		if (!this.happened)
		{
			this.happened = true;
			this.get_rigidbody().AddForce(this.velocity, 1);
		}
	}

	public override void Hit()
	{
		checked
		{
			this.asteroidHitPoints--;
			if (this.asteroidHitPoints <= 0)
			{
				this.Explode();
			}
		}
	}

	public override void MissileHit()
	{
		this.asteroidHitPoints = 0;
		this.Explode();
	}

	public override void Explode()
	{
		object target = Object.Instantiate(this.explosionPrefab, this.get_transform().get_position(), this.get_transform().get_rotation());
		object target2 = UnityRuntimeServices.Invoke(target, "GetComponent", new object[]
		{
			"Detonator"
		}, typeof(MonoBehaviour));
		RuntimeServices.SetProperty(target2, "size", 10);
		AudioSource.PlayClipAtPoint(this.explosionSound, this.get_transform().get_position());
		checked
		{
			GameController.instance.gameEnemies = GameController.instance.gameEnemies - 1;
			GameController.instance.gameScore = GameController.instance.gameScore + 10;
			Object.Destroy(this.get_gameObject());
		}
	}

	public override void Main()
	{
	}
}

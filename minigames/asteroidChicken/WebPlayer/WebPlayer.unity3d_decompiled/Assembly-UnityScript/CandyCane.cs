using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class CandyCane : MonoBehaviour
{
	public float maxAsteroidVelocity;

	public float minAsteroidVelocity;

	private Vector3 randomDirection;

	private int asteroidSize;

	public GameObject explosionPrefab;

	public AudioClip explosionSound;

	public GameObject wrapperSparkle;

	private Vector3 velocity;

	private int asteroidHitPoints;

	public bool happened;

	private float startDelay;

	public CandyCane()
	{
		this.asteroidHitPoints = 1;
	}

	public override void Start()
	{
		this.Setup(1, 0.1f);
	}

	public override void Setup(int size, float velocityScale)
	{
		this.asteroidSize = size;
		this.get_transform().set_localScale(this.get_transform().get_localScale() * (float)size);
		float num = Random.Range(-1f, 1f);
		float num2 = Random.Range(-1f, 1f);
		float num3 = Random.Range(-1f, 1f);
		this.randomDirection = new Vector3(num, num2, num3);
		float num4 = Random.Range(this.minAsteroidVelocity, this.maxAsteroidVelocity);
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

	public override int Hit()
	{
		checked
		{
			this.asteroidHitPoints--;
			MonoBehaviour.print("YHC: asteroidHitPoints = " + this.asteroidHitPoints);
			int arg_41_0;
			if (this.asteroidHitPoints < 1)
			{
				this.Explode();
				arg_41_0 = 1;
			}
			else
			{
				arg_41_0 = 0;
			}
			return arg_41_0;
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

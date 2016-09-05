using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class BlueWhistle : MonoBehaviour
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

	public float minCandySize;

	public float maxCandySize;

	public AudioClip biteSound;

	private float startDelay;

	public BlueWhistle()
	{
		this.asteroidHitPoints = 3;
		this.minCandySize = (float)2;
		this.maxCandySize = (float)4;
	}

	public override void Start()
	{
	}

	public override void Setup(float size, float velocityScale)
	{
		float num = 0.25f * Random.Range(this.minCandySize, this.maxCandySize);
		this.get_transform().set_localScale(this.get_transform().get_localScale() * size * num);
		float num2 = Random.Range(-0.5f, 0.5f);
		float num3 = Random.Range(-0.5f, 0.5f);
		float num4 = Random.Range(-1f, (float)0);
		this.randomDirection = new Vector3(num2, num3, num4);
		float num5 = Mathf.Lerp(this.minAsteroidVelocity, this.maxAsteroidVelocity, Random.get_value());
		num5 *= velocityScale;
		this.velocity = this.randomDirection * num5;
	}

	public override void FixedUpdate()
	{
		if (!this.happened)
		{
			this.happened = true;
			this.get_rigidbody().AddForce(this.velocity, 0);
		}
	}

	public override int Hit()
	{
		checked
		{
			this.asteroidHitPoints--;
			MonoBehaviour.print("YHC: asteroidHitPoints = " + this.asteroidHitPoints);
			int arg_93_0;
			if (this.asteroidHitPoints == 3)
			{
				AudioSource.PlayClipAtPoint(this.biteSound, this.get_transform().get_position());
				arg_93_0 = 0;
			}
			else if (this.asteroidHitPoints == 2)
			{
				AudioSource.PlayClipAtPoint(this.biteSound, this.get_transform().get_position());
				arg_93_0 = 0;
			}
			else if (this.asteroidHitPoints <= 1)
			{
				this.Explode();
				arg_93_0 = 1;
			}
			else
			{
				arg_93_0 = 0;
			}
			return arg_93_0;
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
		RuntimeServices.SetProperty(target2, "size", 7);
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

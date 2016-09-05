using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class YellowHardCandy : MonoBehaviour
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

	public GameObject candyWrapperOpen;

	private float startDelay;

	public YellowHardCandy()
	{
		this.asteroidHitPoints = 2;
		this.minCandySize = (float)1;
		this.maxCandySize = (float)4;
	}

	public override void Start()
	{
	}

	public override void Setup(float size, float velocityScale)
	{
		float num = Random.Range(this.minCandySize, this.maxCandySize);
		this.get_transform().set_localScale(this.get_transform().get_localScale() * size * num);
		if (this.get_transform().get_localScale().x > (float)15)
		{
			this.get_transform().set_localScale(new Vector3((float)15, (float)15, (float)15));
		}
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
			this.get_rigidbody().AddForce(this.velocity, 1);
		}
	}

	public override int Hit()
	{
		checked
		{
			this.asteroidHitPoints--;
		}
		int arg_10E_0;
		if (this.asteroidHitPoints == 1)
		{
			object target = Object.Instantiate(this.candyWrapperOpen, this.get_transform().get_position(), Quaternion.get_identity());
			Vector3 localScale = this.get_transform().get_localScale();
			object property = UnityRuntimeServices.GetProperty(target, "transform");
			RuntimeServices.SetProperty(property, "localScale", localScale);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(target, "transform", property)
			});
			Object.Instantiate(this.wrapperSparkle, this.get_transform().get_position(), Quaternion.get_identity());
			Transform transform = this.get_transform().Find("pink_candy/PCANDY:wrapper");
			if (transform)
			{
				transform.get_renderer().set_enabled(false);
			}
			else
			{
				MonoBehaviour.print("did not find child wrapper");
			}
			this.startDelay = Time.get_time();
			arg_10E_0 = 0;
		}
		else if (this.asteroidHitPoints <= 0 && Time.get_time() - this.startDelay > (float)1)
		{
			this.Explode();
			arg_10E_0 = 1;
		}
		else
		{
			arg_10E_0 = 0;
		}
		return arg_10E_0;
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
		RuntimeServices.SetProperty(target2, "size", 1.7f);
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

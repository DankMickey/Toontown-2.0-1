using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Asterpinata : MonoBehaviour
{
	public float maxAsteroidVelocity;

	public float minAsteroidVelocity;

	private Vector3 randomDirection;

	private float asteroidSize;

	public GameObject explosionPrefab;

	public AudioClip explosionSound;

	public GameObject jellybeanPrefab;

	public GameObject spacelipsPrefab;

	public GameObject whistlePrefab;

	public GameObject lollyPrefab;

	private Vector3 velocity;

	private int asteroidHitPoints;

	public bool happened;

	public int beanCount;

	public float beanWorldSize;

	private float randX;

	private float randY;

	private float randZ;

	private float randTorqueX;

	private float randTorqueY;

	private float randTorqueZ;

	private int randTorqueScale;

	private float velScale;

	private GameObject target;

	private int randInt;

	private bool passedThruWindshield;

	public object astroScript;

	public GameObject confettiEmitter;

	public GameObject confettiEmitterOrange;

	public Asterpinata()
	{
		this.asteroidHitPoints = 4;
		this.beanCount = 5;
		this.beanWorldSize = (float)40;
	}

	public override void Start()
	{
		this.target = GameObject.Find("Ship");
		this.astroScript = this.target.GetComponentInChildren(typeof(AstronautBehavior));
		this.passedThruWindshield = false;
		this.randInt = Random.Range(0, 5);
	}

	public override void Setup(float size, float velocityScale)
	{
		this.velScale = velocityScale;
		this.asteroidSize = size;
		Vector3 localScale = this.get_transform().get_localScale();
		localScale.x = size;
		Vector3 vector;
		this.get_transform().set_localScale(vector = localScale);
		Vector3 localScale2 = this.get_transform().get_localScale();
		localScale2.y = size;
		Vector3 vector2;
		this.get_transform().set_localScale(vector2 = localScale2);
		Vector3 localScale3 = this.get_transform().get_localScale();
		localScale3.z = size;
		Vector3 vector3;
		this.get_transform().set_localScale(vector3 = localScale3);
		this.get_transform().set_rotation(Random.get_rotation());
		if (this.get_transform().get_position().x >= (float)0)
		{
			this.randX = Random.Range(-0.25f, (float)0);
		}
		else
		{
			this.randX = Random.Range((float)0, 0.25f);
		}
		if (this.get_transform().get_position().y >= (float)0)
		{
			this.randY = Random.Range(-0.25f, (float)0);
		}
		else
		{
			this.randY = Random.Range((float)0, 0.25f);
		}
		this.randZ = Random.Range(-1f, (float)0);
		this.randomDirection = new Vector3(this.randX, this.randY, this.randZ);
		float num = Random.Range(this.minAsteroidVelocity, this.maxAsteroidVelocity);
		num *= velocityScale;
		this.randTorqueX = Random.Range((float)0, 20f);
		this.randTorqueY = Random.Range((float)0, 20f);
		this.randTorqueZ = Random.Range((float)0, 20f);
		int num2 = Random.Range(0, 20);
		int num3 = Random.Range(-2, 2);
		checked
		{
			if (num2 < 2)
			{
				this.randTorqueScale = (int)Random.Range((float)0, 2f);
			}
			else if (num2 >= 2 && num2 < 8)
			{
				this.randTorqueScale = (int)Random.Range((float)0, 1f);
			}
			else
			{
				this.randTorqueScale = (int)Random.Range((float)0, 0.5f);
			}
			this.randTorqueScale = (int)(unchecked((float)(checked(num3 * this.randTorqueScale)) * velocityScale * (float)8));
		}
		this.get_rigidbody().AddTorque((float)this.randTorqueScale * this.randTorqueX, (float)this.randTorqueScale * this.randTorqueY, (float)this.randTorqueScale * this.randTorqueZ);
		this.velocity = this.randomDirection * num;
	}

	public override void Update()
	{
		this.get_rigidbody().AddTorque((float)this.randTorqueScale * this.randTorqueX, (float)this.randTorqueScale * this.randTorqueY, (float)this.randTorqueScale * this.randTorqueZ);
	}

	public override void FixedUpdate()
	{
		if (!this.happened)
		{
			this.happened = true;
			this.get_rigidbody().AddForce(this.velocity, 1);
		}
		else if (this.randInt == 0 && this.get_transform().get_position().z - this.target.get_transform().get_position().z > (float)0)
		{
			(this.target.get_transform().get_position() - this.get_transform().get_position()).Normalize();
			this.get_transform().LookAt(this.target.get_transform());
			this.get_rigidbody().AddForce(this.get_transform().get_forward() * (float)5);
		}
		else
		{
			this.get_rigidbody().AddForce(this.velocity, 0);
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
		object obj = Object.Instantiate(this.confettiEmitter, this.get_transform().get_position(), this.get_transform().get_rotation());
		Vector3 localScale = this.get_transform().get_localScale();
		object property = UnityRuntimeServices.GetProperty(obj, "transform");
		RuntimeServices.SetProperty(property, "localScale", localScale);
		UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
		{
			new UnityRuntimeServices.MemberValueTypeChange(obj, "transform", property)
		});
		Vector3 vector = new Vector3((float)10, (float)10, (float)10);
		object property2 = UnityRuntimeServices.GetProperty(obj, "transform");
		RuntimeServices.SetProperty(property2, "localScale", vector);
		UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
		{
			new UnityRuntimeServices.MemberValueTypeChange(obj, "transform", property2)
		});
		obj = Object.Instantiate(this.confettiEmitterOrange, this.get_transform().get_position(), this.get_transform().get_rotation());
		Vector3 localScale2 = this.get_transform().get_localScale();
		object property3 = UnityRuntimeServices.GetProperty(obj, "transform");
		RuntimeServices.SetProperty(property3, "localScale", localScale2);
		UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
		{
			new UnityRuntimeServices.MemberValueTypeChange(obj, "transform", property3)
		});
		Vector3 vector2 = new Vector3((float)10, (float)10, (float)10);
		object property4 = UnityRuntimeServices.GetProperty(obj, "transform");
		RuntimeServices.SetProperty(property4, "localScale", vector2);
		UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
		{
			new UnityRuntimeServices.MemberValueTypeChange(obj, "transform", property4)
		});
		object obj2 = Object.Instantiate(this.explosionPrefab, this.get_transform().get_position(), this.get_transform().get_rotation());
		object obj3 = UnityRuntimeServices.Invoke(obj2, "GetComponent", new object[]
		{
			"Detonator"
		}, typeof(MonoBehaviour));
		RuntimeServices.SetProperty(obj3, "size", 1.5f);
		AudioSource.PlayClipAtPoint(this.explosionSound, this.get_transform().get_position(), (float)1);
		this.SpewBeans();
		UnityRuntimeServices.Invoke(this.astroScript, "PlayCheer", new object[0], typeof(MonoBehaviour));
		checked
		{
			GameController.instance.gameEnemies = GameController.instance.gameEnemies - 1;
			GameController.instance.gameScore = GameController.instance.gameScore + 10;
			Object.Destroy(this.get_gameObject());
		}
	}

	public override void SpewBeans()
	{
		for (int i = 0; i < this.beanCount; i = checked(i + 1))
		{
			float num = this.get_transform().get_localScale().x * 0.5f;
			float num2 = this.get_transform().get_position().x + Random.Range(-num, num);
			float num3 = this.get_transform().get_position().y + Random.Range(-num, num);
			float num4 = this.get_transform().get_position().z + Random.Range(-num, num);
			int num5 = Random.Range(0, 4);
			int num6 = num5;
			if (num6 == 0)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.jellybeanPrefab, new Vector3(num2, num3, num4), this.get_transform().get_rotation());
				UnityRuntimeServices.Invoke(gameObject.GetComponent(typeof(YellowHardCandy)), "Setup", new object[]
				{
					this.asteroidSize,
					this.velScale
				}, typeof(MonoBehaviour));
			}
			else if (num6 == 1)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.spacelipsPrefab, new Vector3(num2, num3, num4), this.get_transform().get_rotation());
				UnityRuntimeServices.Invoke(gameObject.GetComponent(typeof(YellowHardCandy)), "Setup", new object[]
				{
					this.asteroidSize,
					this.velScale
				}, typeof(MonoBehaviour));
			}
			else if (num6 == 2)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.lollyPrefab, new Vector3(num2, num3, num4), this.get_transform().get_rotation());
				UnityRuntimeServices.Invoke(gameObject.GetComponent(typeof(Lolly)), "Setup", new object[]
				{
					this.asteroidSize,
					this.velScale
				}, typeof(MonoBehaviour));
			}
			else if (num6 == 3)
			{
				GameObject gameObject = (GameObject)Object.Instantiate(this.whistlePrefab, new Vector3(num2, num3, num4), this.get_transform().get_rotation());
				UnityRuntimeServices.Invoke(gameObject.GetComponent(typeof(BlueWhistle)), "Setup", new object[]
				{
					this.asteroidSize,
					this.velScale
				}, typeof(MonoBehaviour));
			}
		}
	}

	public override void Main()
	{
	}
}

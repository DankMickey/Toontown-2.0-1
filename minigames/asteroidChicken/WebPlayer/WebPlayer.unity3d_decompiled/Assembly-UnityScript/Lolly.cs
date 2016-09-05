using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class Lolly : MonoBehaviour
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

	public Lolly()
	{
		this.asteroidHitPoints = 4;
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
		float num2 = Random.Range(-1f, 1f);
		float num3 = Random.Range(-1f, 1f);
		float num4 = Random.Range(-1f, 1f);
		this.randomDirection = new Vector3(num2, num3, num4);
		float num5 = Mathf.Lerp(this.minAsteroidVelocity, this.maxAsteroidVelocity, Random.get_value());
		num5 *= velocityScale;
		this.velocity = this.randomDirection * num5 * 0.1f;
	}

	public override void FixedUpdate()
	{
	}

	public override int Hit()
	{
		checked
		{
			this.asteroidHitPoints--;
			int arg_1F6_0;
			if (this.asteroidHitPoints == 3)
			{
				object obj = this.get_transform().Find("t2_m_prp_ext_lollipop/lollipop1");
				if (RuntimeServices.ToBool(obj))
				{
					bool flag = false;
					object property = UnityRuntimeServices.GetProperty(obj, "renderer");
					RuntimeServices.SetProperty(property, "enabled", flag);
					UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
					{
						new UnityRuntimeServices.MemberValueTypeChange(obj, "renderer", property)
					});
					obj = this.get_transform().Find("t2_m_prp_ext_lollipop/lollipop2");
					bool flag2 = true;
					object property2 = UnityRuntimeServices.GetProperty(obj, "renderer");
					RuntimeServices.SetProperty(property2, "enabled", flag2);
					UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
					{
						new UnityRuntimeServices.MemberValueTypeChange(obj, "renderer", property2)
					});
					AudioSource.PlayClipAtPoint(this.biteSound, this.get_transform().get_position());
				}
				else
				{
					MonoBehaviour.print("couldn't find lollymesh");
				}
				arg_1F6_0 = 0;
			}
			else if (this.asteroidHitPoints == 2)
			{
				object obj = this.get_transform().Find("t2_m_prp_ext_lollipop/lollipop2");
				if (RuntimeServices.ToBool(obj))
				{
					bool flag3 = false;
					object property3 = UnityRuntimeServices.GetProperty(obj, "renderer");
					RuntimeServices.SetProperty(property3, "enabled", flag3);
					UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
					{
						new UnityRuntimeServices.MemberValueTypeChange(obj, "renderer", property3)
					});
					obj = this.get_transform().Find("t2_m_prp_ext_lollipop/lollipop3");
					bool flag4 = true;
					object property4 = UnityRuntimeServices.GetProperty(obj, "renderer");
					RuntimeServices.SetProperty(property4, "enabled", flag4);
					UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
					{
						new UnityRuntimeServices.MemberValueTypeChange(obj, "renderer", property4)
					});
					AudioSource.PlayClipAtPoint(this.biteSound, this.get_transform().get_position());
				}
				else
				{
					MonoBehaviour.print("couldn't find lollymesh");
				}
				arg_1F6_0 = 0;
			}
			else if (this.asteroidHitPoints <= 1)
			{
				this.Explode();
				arg_1F6_0 = 1;
			}
			else
			{
				arg_1F6_0 = 0;
			}
			return arg_1F6_0;
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
		RuntimeServices.SetProperty(target2, "size", 2);
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

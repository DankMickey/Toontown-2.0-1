using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class EnemyBot : MonoBehaviour
{
	public float attackDistance;

	public Transform player;

	public Transform hand;

	public GameObject fireBall;

	public float fireRate;

	private float nextFire;

	private EnemyAIStatus status;

	public CharacterController controller;

	public EnemyBot()
	{
		this.attackDistance = 10f;
		this.fireRate = 1.4f;
		this.status = EnemyAIStatus.Idle;
	}

	public override void Awake()
	{
		this.controller = (CharacterController)this.GetComponent(typeof(CharacterController));
	}

	public override void Update()
	{
		this.CheckStatus();
		EnemyAIStatus enemyAIStatus = this.status;
		if (enemyAIStatus == EnemyAIStatus.Idle)
		{
			this.Idle();
		}
		else if (enemyAIStatus == EnemyAIStatus.Attack)
		{
			this.Attack();
		}
	}

	public override void Idle()
	{
		this.get_animation().CrossFade("idle");
	}

	public override void Attack()
	{
		this.get_transform().LookAt(this.player.get_transform());
		this.get_animation().CrossFade("attack");
		if (Time.get_time() > this.nextFire)
		{
			this.nextFire = Time.get_time() + this.fireRate;
			object target = Object.Instantiate(this.fireBall, this.hand.get_position(), this.hand.get_rotation());
			Physics.IgnoreCollision((Collider)RuntimeServices.Coerce(UnityRuntimeServices.GetProperty(target, "collider"), typeof(Collider)), (Collider)RuntimeServices.Coerce(this.get_collider(), typeof(Collider)));
			Vector3 vector = this.get_transform().TransformDirection(Vector3.get_forward() * (float)20);
			object property = UnityRuntimeServices.GetProperty(target, "rigidbody");
			RuntimeServices.SetProperty(property, "velocity", vector);
			UnityRuntimeServices.PropagateValueTypeChanges(new UnityRuntimeServices.ValueTypeChange[]
			{
				new UnityRuntimeServices.MemberValueTypeChange(target, "rigidbody", property)
			});
			object target2 = UnityRuntimeServices.Invoke(UnityRuntimeServices.GetProperty(target, "gameObject"), "AddComponent", new object[]
			{
				"BulletScript"
			}, typeof(MonoBehaviour));
			RuntimeServices.SetProperty(target2, "playerShooting", this.get_gameObject());
		}
	}

	public override void CheckStatus()
	{
		float magnitude = (this.player.get_position() - this.get_transform().get_position()).get_magnitude();
		if (magnitude < this.attackDistance)
		{
			this.status = EnemyAIStatus.Attack;
		}
		else if (magnitude > this.attackDistance)
		{
			this.status = EnemyAIStatus.Idle;
		}
	}

	public override void Main()
	{
	}
}

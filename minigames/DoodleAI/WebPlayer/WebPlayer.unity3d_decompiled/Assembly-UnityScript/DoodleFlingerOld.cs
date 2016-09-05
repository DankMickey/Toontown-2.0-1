using Boo.Lang.Runtime;
using System;
using UnityEngine;
using UnityScript.Lang;

[Serializable]
public class DoodleFlingerOld : MonoBehaviour
{
	public AnimationClip doodleIdle;

	public float awareDistance;

	public float scaredDistance;

	public Transform player;

	public float runSpeed;

	public float turn;

	public bool bounce;

	public float boing;

	public float gravity;

	public float speed;

	public float jumpSpeed;

	private bool grounded;

	private int state;

	private AIoldStatus status;

	private Vector3 moveDirection;

	public DoodleFlingerOld()
	{
		this.awareDistance = 15f;
		this.scaredDistance = 10f;
		this.runSpeed = 500f;
		this.turn = 1f;
		this.boing = 25f;
		this.gravity = 20f;
		this.speed = 6f;
		this.jumpSpeed = 8f;
		this.status = AIoldStatus.IdleDoodle;
		this.moveDirection = Vector3.get_zero();
	}

	public override void Awake()
	{
		this.player = GameObject.Find("player").get_transform();
	}

	public override void Update()
	{
		this.CheckStatus();
		AIoldStatus aIoldStatus = this.status;
		if (aIoldStatus == AIoldStatus.IdleDoodle)
		{
			this.Idle();
		}
		else if (aIoldStatus == AIoldStatus.ScaredDoodle)
		{
			this.RunAway();
		}
	}

	public override void Idle()
	{
		this.get_animation().CrossFade(this.doodleIdle.get_name());
	}

	public override void RunAway()
	{
		this.get_transform().LookAt(this.player.get_transform());
		float y = this.get_transform().get_eulerAngles().y + (float)-180 * this.turn;
		Vector3 eulerAngles = this.get_transform().get_eulerAngles();
		float num = eulerAngles.y = y;
		Vector3 vector;
		this.get_transform().set_eulerAngles(vector = eulerAngles);
		this.moveDirection = new Vector3((float)0, (float)0, (float)40);
		this.moveDirection = this.get_transform().TransformDirection(this.moveDirection);
		this.moveDirection *= this.runSpeed;
		this.get_rigidbody().AddForce(this.moveDirection * Time.get_deltaTime());
		this.get_animation().CrossFade("run");
	}

	public override void CheckStatus()
	{
		float magnitude = (this.player.get_position() - this.get_transform().get_position()).get_magnitude();
		if (magnitude < this.scaredDistance)
		{
			this.status = AIoldStatus.ScaredDoodle;
		}
		else if (magnitude > this.awareDistance)
		{
			this.status = AIoldStatus.IdleDoodle;
		}
	}

	public override void OnCollisionEnter(Collision collision)
	{
		checked
		{
			this.state++;
			if (this.state > 0)
			{
				this.grounded = true;
			}
			if (RuntimeServices.EqualityOperator(UnityRuntimeServices.GetProperty(collision.get_gameObject(), "tag"), "notTray"))
			{
				Object.Destroy(this.get_gameObject());
			}
		}
	}

	public override void OnCollisionExit()
	{
		checked
		{
			this.state--;
			if (this.state < 1)
			{
				this.grounded = false;
				this.state = 0;
			}
		}
	}

	public override void Main()
	{
	}
}

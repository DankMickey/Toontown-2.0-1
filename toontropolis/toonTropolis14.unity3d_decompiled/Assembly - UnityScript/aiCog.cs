using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
[Serializable]
public class aiCog : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class NextWaypoint$41 : GenericGenerator<WaitForSeconds>
	{
		internal aiCog $self_166;

		public NextWaypoint$41(aiCog self_)
		{
			this.$self_166 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new aiCog.NextWaypoint$41.$(this.$self_166);
		}
	}

	public bool enemyActive;

	public int hitpoints;

	public int runSpeed;

	public float rotateSpeed;

	public GameObject cameraMain;

	private CharacterController characterController;

	public int aiState;

	public GameObject playerChar;

	public GameObject grpWayPonints;

	public GameObject[] WayPointArray;

	public int wayPointInt;

	public GameObject activeWaypoint;

	private bool wayPointSwitch;

	public bool wayPointLoop;

	public bool wayPointReverseOrder;

	public float waypointWaitTime;

	public AnimationClip animWalk;

	public AnimationClip animIdle;

	public AnimationClip animIdleBattle;

	public aiCog()
	{
		this.enemyActive = true;
		this.hitpoints = 100;
		this.runSpeed = 10;
		this.rotateSpeed = 0.7f;
		this.aiState = 1;
		this.wayPointInt = 0;
		this.wayPointSwitch = true;
		this.wayPointLoop = true;
		this.wayPointReverseOrder = false;
		this.waypointWaitTime = 0.1f;
	}

	public void Start()
	{
		this.activeWaypoint = this.WayPointArray[0];
		this.characterController = (CharacterController)this.GetComponent(typeof(CharacterController));
		if (!this.playerChar)
		{
			this.playerChar = GameObject.FindWithTag("Player");
		}
		if (!this.cameraMain)
		{
			this.cameraMain = GameObject.FindWithTag("MainCamera");
		}
		this.animWalk.set_wrapMode(2);
		this.animIdle.set_wrapMode(2);
		this.animIdleBattle.set_wrapMode(2);
	}

	public void Update()
	{
		if (this.aiState == 2 && this.enemyActive)
		{
			if ((this.get_gameObject().get_transform().get_position() - this.activeWaypoint.get_transform().get_position()).get_magnitude() >= (float)1)
			{
				this.MoveCharacter();
				this.get_animation().Play(this.animWalk.get_name());
			}
			else
			{
				this.aiState = 1;
			}
		}
		if (this.aiState == 1 && this.enemyActive)
		{
			this.Idle();
		}
	}

	public void Idle()
	{
		if (this.wayPointSwitch)
		{
			this.StartCoroutine_Auto(this.NextWaypoint());
		}
	}

	public void MoveCharacter()
	{
		float num = this.RotateTowardsPosition(this.activeWaypoint.get_transform().get_position(), this.rotateSpeed);
		Vector3 vector = this.get_transform().TransformDirection(Vector3.get_forward() * (float)this.runSpeed);
		this.characterController.SimpleMove(vector);
	}

	public float RotateTowardsPosition(Vector3 targetPos, float rotateSpeed)
	{
		Vector3 vector = this.get_transform().InverseTransformPoint(targetPos);
		float num = Mathf.Atan2(vector.x, vector.z) * 57.29578f;
		float num2 = rotateSpeed * Time.get_deltaTime();
		float num3 = Mathf.Clamp(num, num2 * (float)-1, num2);
		this.get_transform().Rotate((float)0, num3, (float)0);
		return num;
	}

	public IEnumerator NextWaypoint()
	{
		return new aiCog.NextWaypoint$41(this).GetEnumerator();
	}

	public void stopEnemy()
	{
		this.enemyActive = false;
	}

	public void startEnemy()
	{
		this.enemyActive = true;
	}

	public void Main()
	{
	}
}

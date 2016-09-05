using System;
using UnityEngine;

[AddComponentMenu("ControlPrototype/Player Animation")]
[Serializable]
public class PlayerAnimation : MonoBehaviour
{
	public AnimationClip animIdle;

	public AnimationClip animIdleTap;

	public AnimationClip animWalk;

	public AnimationClip animRun;

	public AnimationClip animJumpTakeoff;

	public AnimationClip animJumpCycle;

	public AnimationClip animJumpLand;

	private bool flagIdle;

	private float nextLoad;

	private float rate;

	public PlayerAnimation()
	{
		this.rate = (float)6;
	}

	public override void Start()
	{
		this.get_animation().set_wrapMode(2);
		this.get_animation().get_Item(this.animWalk.get_name()).set_layer(-1);
		this.get_animation().get_Item(this.animRun.get_name()).set_layer(-1);
		this.get_animation().get_Item(this.animIdle.get_name()).set_layer(-1);
		this.get_animation().get_Item(this.animJumpCycle.get_name()).set_layer(-1);
		this.get_animation().SyncLayer(-1);
		this.get_animation().Stop();
		this.get_animation().Play(this.animIdle.get_name());
	}

	public override void Update()
	{
		CharacterController characterController = (CharacterController)this.get_transform().GetComponent(typeof(CharacterController));
		Vector3 velocity = characterController.get_velocity();
		velocity.y = (float)0;
		float magnitude = velocity.get_magnitude();
		if (magnitude > (float)5)
		{
			this.get_animation().CrossFade(this.animRun.get_name());
		}
		else if (magnitude > 0.1f)
		{
			this.get_animation().CrossFade(this.animWalk.get_name());
		}
		else if (Input.GetButton("Jump"))
		{
			this.get_animation().CrossFade(this.animJumpCycle.get_name());
		}
		else
		{
			this.get_animation().CrossFade(this.animIdle.get_name());
		}
	}

	public override void Main()
	{
	}
}

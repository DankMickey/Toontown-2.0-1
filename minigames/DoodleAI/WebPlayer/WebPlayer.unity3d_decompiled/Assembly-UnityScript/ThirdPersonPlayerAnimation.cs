using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[AddComponentMenu("Third Person Player/Third Person Player Animation")]
[Serializable]
public class ThirdPersonPlayerAnimation : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $Slam$62 : GenericGenerator<object>
	{
		internal ThirdPersonPlayerAnimation $self_$65;

		public $Slam$62(ThirdPersonPlayerAnimation self_)
		{
			this.$self_$65 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new ThirdPersonPlayerAnimation.$Slam$62.$(this.$self_$65);
		}
	}

	public float runSpeedScale;

	public float walkSpeedScale;

	public ThirdPersonPlayerAnimation()
	{
		this.runSpeedScale = 1f;
		this.walkSpeedScale = 1f;
	}

	public override void Start()
	{
		this.get_animation().set_wrapMode(2);
		this.get_animation().get_Item("run").set_layer(-1);
		this.get_animation().get_Item("walk").set_layer(-1);
		this.get_animation().get_Item("idle").set_layer(-2);
		this.get_animation().SyncLayer(-1);
		this.get_animation().get_Item("ledgefall").set_layer(9);
		this.get_animation().get_Item("ledgefall").set_wrapMode(2);
		this.get_animation().get_Item("jump").set_layer(10);
		this.get_animation().get_Item("jump").set_wrapMode(8);
		this.get_animation().get_Item("jumpfall").set_layer(10);
		this.get_animation().get_Item("jumpfall").set_wrapMode(8);
		this.get_animation().get_Item("jetpackjump").set_layer(10);
		this.get_animation().get_Item("jetpackjump").set_wrapMode(8);
		this.get_animation().get_Item("jumpland").set_layer(10);
		this.get_animation().get_Item("jumpland").set_wrapMode(1);
		this.get_animation().get_Item("walljump").set_layer(11);
		this.get_animation().get_Item("walljump").set_wrapMode(1);
		this.get_animation().get_Item("buttstomp").set_speed(0.15f);
		this.get_animation().get_Item("buttstomp").set_layer(20);
		this.get_animation().get_Item("buttstomp").set_wrapMode(1);
		AnimationState animationState = this.get_animation().get_Item("punch");
		animationState.set_wrapMode(1);
		this.get_animation().Stop();
		this.get_animation().Play("idle");
	}

	public override void Update()
	{
		ThirdPersonController thirdPersonController = (ThirdPersonController)this.GetComponent(typeof(ThirdPersonController));
		float speed = thirdPersonController.GetSpeed();
		if (speed > thirdPersonController.walkSpeed)
		{
			this.get_animation().CrossFade("run");
			this.get_animation().Blend("jumpland", (float)0);
		}
		else if (speed > 0.1f)
		{
			this.get_animation().CrossFade("walk");
			this.get_animation().Blend("jumpland", (float)0);
		}
		else
		{
			this.get_animation().Blend("walk", (float)0, 0.3f);
			this.get_animation().Blend("run", (float)0, 0.3f);
			this.get_animation().Blend("run", (float)0, 0.3f);
		}
		this.get_animation().get_Item("run").set_normalizedSpeed(this.runSpeedScale);
		this.get_animation().get_Item("walk").set_normalizedSpeed(this.walkSpeedScale);
		if (thirdPersonController.IsJumping())
		{
			if (thirdPersonController.IsControlledDescent())
			{
				this.get_animation().CrossFade("jetpackjump", 0.2f);
			}
			else if (thirdPersonController.HasJumpReachedApex())
			{
				this.get_animation().CrossFade("jumpfall", 0.2f);
			}
			else
			{
				this.get_animation().CrossFade("jump", 0.2f);
			}
		}
		else if (!thirdPersonController.IsGroundedWithTimeout())
		{
			this.get_animation().CrossFade("ledgefall", 0.2f);
		}
		else
		{
			this.get_animation().Blend("ledgefall", (float)0, 0.2f);
		}
	}

	public override void DidLand()
	{
		this.get_animation().Play("jumpland");
	}

	public override void DidButtStomp()
	{
		this.get_animation().CrossFade("buttstomp", 0.1f);
		this.get_animation().CrossFadeQueued("jumpland", 0.2f);
	}

	public override IEnumerator Slam()
	{
		return new ThirdPersonPlayerAnimation.$Slam$62(this).GetEnumerator();
	}

	public override void DidWallJump()
	{
		this.get_animation().Play("walljump");
	}

	public override void Main()
	{
	}
}

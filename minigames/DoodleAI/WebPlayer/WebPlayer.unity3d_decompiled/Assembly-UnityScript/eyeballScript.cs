using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class eyeballScript : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class $Start$78 : GenericGenerator<WaitForSeconds>
	{
		internal eyeballScript $self_$81;

		public $Start$78(eyeballScript self_)
		{
			this.$self_$81 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new eyeballScript.$Start$78.$(this.$self_$81);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class $BlinkReset$82 : GenericGenerator<WaitForSeconds>
	{
		internal eyeballScript $self_$84;

		public $BlinkReset$82(eyeballScript self_)
		{
			this.$self_$84 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new eyeballScript.$BlinkReset$82.$(this.$self_$84);
		}
	}

	public AnimationClip animBlink;

	public float BlinkSpaceTime;

	public bool secondEye;

	public GameObject lookTarget;

	public GameObject aimer;

	public eyeballScript()
	{
		this.BlinkSpaceTime = 5f;
	}

	public override IEnumerator Start()
	{
		return new eyeballScript.$Start$78(this).GetEnumerator();
	}

	public override void Blink()
	{
		this.get_animation().Play(this.animBlink.get_name());
		this.StartCoroutine_Auto(this.BlinkReset());
	}

	public override IEnumerator BlinkReset()
	{
		return new eyeballScript.$BlinkReset$82(this).GetEnumerator();
	}

	public override void Update()
	{
	}

	public override void Main()
	{
	}
}

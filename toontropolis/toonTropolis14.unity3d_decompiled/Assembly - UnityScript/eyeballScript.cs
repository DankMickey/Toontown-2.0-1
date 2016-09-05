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
	internal sealed class Start$76 : GenericGenerator<WaitForSeconds>
	{
		internal eyeballScript $self_274;

		public Start$76(eyeballScript self_)
		{
			this.$self_274 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new eyeballScript.Start$76.$(this.$self_274);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class BlinkReset$77 : GenericGenerator<WaitForSeconds>
	{
		internal eyeballScript $self_276;

		public BlinkReset$77(eyeballScript self_)
		{
			this.$self_276 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new eyeballScript.BlinkReset$77.$(this.$self_276);
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
		this.secondEye = false;
	}

	public IEnumerator Start()
	{
		return new eyeballScript.Start$76(this).GetEnumerator();
	}

	public void Blink()
	{
		this.get_animation().Play(this.animBlink.get_name());
		this.StartCoroutine_Auto(this.BlinkReset());
	}

	public IEnumerator BlinkReset()
	{
		return new eyeballScript.BlinkReset$77(this).GetEnumerator();
	}

	public void Update()
	{
	}

	public void Main()
	{
	}
}

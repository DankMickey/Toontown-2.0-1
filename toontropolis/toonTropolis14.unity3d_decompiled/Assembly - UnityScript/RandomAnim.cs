using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class RandomAnim : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class WaitForSomething$73 : GenericGenerator<WaitForSeconds>
	{
		internal RandomAnim $self_265;

		public WaitForSomething$73(RandomAnim self_)
		{
			this.$self_265 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new RandomAnim.WaitForSomething$73.$(this.$self_265);
		}
	}

	[CompilerGenerated]
	[Serializable]
	internal sealed class Idles$74 : GenericGenerator<object>
	{
		internal RandomAnim $self_263;

		public Idles$74(RandomAnim self_)
		{
			this.$self_263 = self_;
		}

		public override IEnumerator<object> GetEnumerator()
		{
			return new RandomAnim.Idles$74.$(this.$self_263);
		}
	}

	public bool canAnimate;

	public int choice;

	public int timer;

	public AnimationClip animA;

	public AnimationClip animB;

	public AnimationClip animC;

	public AnimationClip animD;

	public RandomAnim()
	{
		this.canAnimate = true;
		this.choice = 0;
		this.timer = 0;
	}

	public void Update()
	{
		this.StartCoroutine_Auto(this.Idles());
	}

	public IEnumerator Idles()
	{
		return new RandomAnim.Idles$74(this).GetEnumerator();
	}

	public IEnumerator WaitForSomething()
	{
		return new RandomAnim.WaitForSomething$73(this).GetEnumerator();
	}

	public void Playagain()
	{
		this.StartCoroutine_Auto(this.Idles());
	}

	public void Main()
	{
	}
}

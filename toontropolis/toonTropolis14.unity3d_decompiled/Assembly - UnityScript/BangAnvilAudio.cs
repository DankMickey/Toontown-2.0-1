using Boo.Lang;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class BangAnvilAudio : MonoBehaviour
{
	[CompilerGenerated]
	[Serializable]
	internal sealed class AnvilBang$37 : GenericGenerator<WaitForSeconds>
	{
		internal BangAnvilAudio $self_161;

		public AnvilBang$37(BangAnvilAudio self_)
		{
			this.$self_161 = self_;
		}

		public override IEnumerator<WaitForSeconds> GetEnumerator()
		{
			return new BangAnvilAudio.AnvilBang$37.$(this.$self_161);
		}
	}

	public IEnumerator AnvilBang(object delay)
	{
		return new BangAnvilAudio.AnvilBang$37(this).GetEnumerator();
	}

	public void AnvilDrop(object delay)
	{
	}

	public void Update()
	{
	}

	public void Main()
	{
	}
}
